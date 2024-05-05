using Meteors;
using Meteors.AspNetCore.Common.AuxiliaryImplemental.Classes;
using Meteors.AspNetCore.Service.BoundedContext;
using Meteors.OperationContext;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topup.BoundedContext.DataTransferObjects.Balance;
using Topup.Infrastructure.Databases.SqlServer;
using Topup.Infrastructure.Models;

namespace Balance.BoundedContext.Repositories
{
    //all logic here
    [AutoService]
    public class BalanceRepository : IBalanceRepository
    {
        private readonly BalanceDbContext Context;
        const int maxRetry = 3;

        public BalanceRepository(BalanceDbContext context)
        {
            Context = context;
        }


        public async Task<OperationResult<CurrentBalanceDto>> CurrentBalance(string userNO)
        {
            //Context.Users.
            //    AsNoTracking().Where(x => x.UserNO == userNO).Select(x => x.CurrentBalance);

            var one = await Context.Users.
                AsNoTracking().FirstOrDefaultAsync(x => x.UserNO == userNO);
            //==Context.Transactions.Sum(x=> status == Processing and status == Completed )

            if (one is null)
                return Statuses.NotExist; //need more hanlde with errors and message

            /*
             track fetch balance with lastdate accessable 
             */

            return (new CurrentBalanceDto { CurrentBalance = one.CurrentBalance, AvailableBalance = one.AvailableBalance }, null);
        }

        // separate credit and debit 


        public async Task<OperationResult<TransactionBalanceDto>> Credit(CreditDto dto)
        {
            /*
             insert trasn then check Concurrency on value / check > 0
            add both - will  assumed that Credit is complated directly as transaction
             */

            if (dto.Amount <= 0) //extra check
                return ("unvalid amount", Statuses.Failed);


            UserBalance? user = null;
          
            try
            {
                while (true)//credit inform to complated in our simple examble/real word can be bank/card transaction due other role
                {
                    try
                    {
                        using var transaction = await Context.Database.BeginTransactionAsync();
                        user = await Context.Users.FirstOrDefaultAsync(x => x.UserNO == dto.UserNO);//reload inside transcation insted of get orginal data 
                        
                        if (user == null)//maybe deleted
                            return ("invalid user", Statuses.Failed);

                        var transactionRecord = await setTransactionContext(dto.Amount,
                                TransactionStatus.Processing, user.Id);

                        //simple add in both available and current,  in real world more complecated .. avialble add then when success complated add to current
                        //we will apply this in debit
                        user.AvailableBalance = user.AvailableBalance + dto.Amount;
                        user.CurrentBalance = user.CurrentBalance + dto.Amount;

                        await Context.SaveChangesAsync();//conflict
                        await transaction.CommitAsync();

                        transactionRecord.Status = TransactionStatus.Completed;//set complated
                        transactionRecord.DateUpdated = DateTime.Now;
                        await Context.SaveChangesAsync();

                        return _Operation.SetSuccess(new TransactionBalanceDto
                        {
                            TransactionReference = transactionRecord.TransactionReference,
                            Status = transactionRecord.Status,
                            Amount = transactionRecord.Amount,
                            Date = transactionRecord.Date,
                            CurrentBalance = user.CurrentBalance,
                        });
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        // reset tracking that able to insert failed trancation
                        if (user is not null)
                            Context.Entry(user).State = EntityState.Detached;

                        var delay = Random.Shared.Next(1000,1500);
                        //dead lock fix by queue which not executed/ for now just random delay can be change to ppssible avoid but not currect solution
                        //to build queue driven for each concurrecny job user..  make this Assessment need more time, than 6h 
                        await Task.Delay(delay);
                    }
                }
            }
            catch { }

            if (user is not null)
            {
                await setTransactionContext(dto.Amount,
                   TransactionStatus.Failed, user.Id);

                await Context.SaveChangesAsync();
            }

            return ("Unexpected error", Statuses.Failed);
        }

        public async Task<OperationResult<TransactionBalanceDto>> Debit(DebitDto dto)
        {
            /*
             insert trasn then check Concurrency on value / check < 0
            sub Current Balance only as proccing ... wait until tranaction complated 
            update event to complated , comfirm by http Debit check 0
            - will  assumed that Debit is complated directly as transaction
             */
            dto.Amount = -dto.Amount;

            if (dto.Amount >= 0) //extra check
                return ("unvalid amount", Statuses.Failed);

            UserBalance? user = null;
            int retry = 0;
            try
            {
                var delay =  Task.Delay(1000);

                while (retry < maxRetry)
                {
                    try
                    {
                        using var transaction = await Context.Database.BeginTransactionAsync();
                        user = await Context.Users.FirstOrDefaultAsync(x => x.UserNO == dto.UserNO);//reload inside transcation insted of get orginal data 


                        if (user == null)//maybe deleted
                            return ("invalid user", Statuses.Failed);

                        if (user.CurrentBalance < Math.Abs(dto.Amount))
                            return ("not enough Balance", Statuses.Failed);//can save in trasnaction as Failed


                        var transactionRecord = await setTransactionContext(dto.Amount,
                            TransactionStatus.Pending, user.Id);

                        // Update users balance
                        user.CurrentBalance += dto.Amount;

                        await Context.SaveChangesAsync();//conflict
                        await transaction.CommitAsync();

                        transactionRecord.Status = TransactionStatus.Processing;
                        transactionRecord.DateUpdated = DateTime.Now;
                        await Context.SaveChangesAsync();

                        return _Operation.SetSuccess(new TransactionBalanceDto
                        {
                            TransactionReference = transactionRecord.TransactionReference,
                            Status = transactionRecord.Status,
                            Amount = transactionRecord.Amount,
                            Date = transactionRecord.Date,
                            CurrentBalance = user.CurrentBalance,
                        });

                    }
                    catch (DbUpdateConcurrencyException ex)
                    {

                        // reset tracking that able to insert failed trancation
                        if (user is not null)
                            Context.Entry(user).State = EntityState.Detached;

                        retry++;

                        if (retry >= maxRetry)
                        {
                            if (user is not null)
                            {
                                await setTransactionContext(dto.Amount,
                                   TransactionStatus.Failed, user.Id);

                                await Context.SaveChangesAsync();
                            }

                            return ("retry limit exceeded", Statuses.Failed);
                        }
                        else
                        {

                            await delay;
                        }
                    }
                    //catch (Exception ex) //handle other exceptions
                }

            }
            catch
            {
                //occurred
            }


            if (user is not null)
            {
                await setTransactionContext(dto.Amount,
                   TransactionStatus.Failed, user.Id);

                await Context.SaveChangesAsync();
            }

            return ("Unexpected error", Statuses.Failed);
        }


        public async Task<OperationResult<TransactionBalanceDto>> ConfirmDebitTransaction(ConfirmDebitTransactionDto dto)
        {
            /*
                    get trans , update status and if complated sub from Available Balance
                        if Failed  back to current Balance and 
             */
            dto.Amount = -dto.Amount;

            if (dto.Amount >= 0) //extra check
                return ("invalid", Statuses.Failed);


            UserBalance? user = null;

            try
            {
                while (true)//simple for now- reconciliation solve this and constraints outbox schdual job to fix
                {
                    try
                    {
                        using var transaction = await Context.Database.BeginTransactionAsync();
                        user = await Context.Users.FirstOrDefaultAsync(x => x.UserNO == dto.UserNO);//reload inside transcation insted of get orginal data 

                        if (user == null)//maybe deleted
                            return ("invalid user", Statuses.Failed);

                        //check tran payload
                        var balanceTrans = await Context.Transactions.
                            FirstOrDefaultAsync(x => x.TransactionReference == dto.TransactionReference &&
                            x.UserId == user.Id && x.Amount == dto.Amount &&
                            x.Status != TransactionStatus.Completed && x.Status != TransactionStatus.Failed);

                        if (balanceTrans is null)
                            return ("invalid", Statuses.Failed);



                        if (dto.Status == TransactionStatus.Completed)
                            user.AvailableBalance = user.AvailableBalance + dto.Amount;
                        else
                        //refund value
                        if (dto.Status == TransactionStatus.Failed) //status should be other than Failed as refound ..ect
                            user.CurrentBalance = user.CurrentBalance - dto.Amount;

                        await Context.SaveChangesAsync();//conflict
                        await transaction.CommitAsync();


                        //update status
                        balanceTrans.Status = dto.Status;
                        balanceTrans.DateUpdated = DateTime.Now;
                        await Context.SaveChangesAsync();

                        return _Operation.SetSuccess(new TransactionBalanceDto
                        {
                            TransactionReference = balanceTrans.TransactionReference,
                            Status = balanceTrans.Status,
                            Amount = balanceTrans.Amount,
                            Date = balanceTrans.Date,
                            CurrentBalance = user.CurrentBalance,
                        });
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        if (user is not null)
                            Context.Entry(user).State = EntityState.Detached;

                        var delay = Random.Shared.Next(1000, 1500);
                        //dead lock fix by queue which not executed/ for now just random delay can be change to ppssible avoid but not currect solution
                        //to build queue driven for each concurrecny job user..  make this Assessment need more time, than 6h 
                        await Task.Delay(delay);
                    }
                }

            }
            catch 
            {

            }

            return ("Unexpected error", Statuses.Failed);

        }





        private async Task<TransactionBalance> setTransactionContext(decimal amount, TransactionStatus status, Guid userId)
        {
            var transactionRecord = new TransactionBalance
            {
                TransactionReference = Generator.RandomString(10),
                Status = status,
                Amount = amount,
                Date = DateTime.Now,
                UserId = userId,
                DateCreated = DateTime.Now
            };

            await Context.AddAsync(transactionRecord);

            return transactionRecord;
        }


        //public async Task<OperationResult<TransactionBalanceDto>> DebitTest(DebitDto dto)
        //{
        //    /*
        //     insert trasn then check Concurrency on value / check < 0
        //    sub Current Balance only as proccing ... wait until tranaction complated 
        //    update event to complated , comfirm by http Debit check 0
        //    - will  assumed that Debit is complated directly as transaction
        //     */
        //    dto.Amount = -dto.Amount;

        //    if (dto.Amount >= 0) //extra check
        //        return ("unvalid amount", Statuses.Failed);

        //     using var transaction = await Context.Database.BeginTransactionAsync();
        //    try
        //    {
        //        var one = await Context.Users.FirstOrDefaultAsync(x => x.UserNO == dto.UserNO);

        //        if (one is null)
        //            return ("invalid", Statuses.Failed);

        //        if (one.CurrentBalance < Math.Abs(dto.Amount)) //can be add to history transaction
        //            return ("not enough Balance", Statuses.Failed);


        //        //set transaction Authorized
        //        var setTrans = new TransactionBalance()
        //        {
        //            TransactionReference = Generator.RandomString(10),
        //            Status = TransactionStatus.Pending,
        //            Amount = dto.Amount,
        //            Date = DateTime.Now,

        //            UserId = one.Id,

        //            DateCreated = DateTime.Now,
        //        };

        //        await Context.AddAsync(setTrans);

        //        //save transaction Processing put wait commit to confirm balance update

        //        await Context.SaveChangesAsync();


        //         await transaction.CreateSavepointAsync("IgnorBalance");

        //        //sub only from Current  -AvailableBalance need confirm
        //        one.CurrentBalance = one.CurrentBalance + dto.Amount;

        //        int iter = 3; //not excatly right best to do qdl quer domain lan in queue to insure one job access to balance and relase the lock
        //        while (iter-- > 0)
        //        {
        //            try
        //            {
        //                //try save Balance
        //                await Context.SaveChangesAsync();

        //                //update in same before commit
        //                setTrans.Status = TransactionStatus.Processing; //not complated until comfired from consumer
        //                setTrans.DateUpdated = DateTime.Now;

        //                await Context.SaveChangesAsync();

        //                //last save
        //                 await transaction.CommitAsync();


        //                return _Operation.SetSuccess(new TransactionBalanceDto()
        //                {
        //                    TransactionReference = setTrans.TransactionReference,
        //                    Status = setTrans.Status,
        //                    Amount = setTrans.Amount,
        //                    Date = setTrans.Date,
        //                });

        //                // break;
        //            }
        //            catch (DbUpdateConcurrencyException ex)
        //            {

        //                foreach (var entry in ex.Entries)
        //                {
        //                    if (entry.Entity is UserBalance)
        //                    {
        //                        var database = entry.GetDatabaseValues();

        //                        entry.OriginalValues.SetValues(database);

        //                        var databaseValue = (UserBalance)database.ToObject();

        //                        if (databaseValue.CurrentBalance < Math.Abs(dto.Amount))
        //                        {
        //                            //refuse tranaction
        //                            //no need to add to history in this simple

        //                            await transaction.RollbackAsync();
        //                            return ("not enough Balance", Statuses.Failed);
        //                        }

        //                        break;
        //                    }
        //                }
        //                // var entry = ex.Entries.Single();
        //                // var Current = (UserBalance)entry.Entity;
        //            }
        //        }


        //        //mean failed to credit balance ... modify trancation to failed the save and commit
        //        await transaction.RollbackToSavepointAsync("IgnorCurrent");

        //        setTrans.Status = TransactionStatus.Failed;
        //        setTrans.DateUpdated = DateTime.Now;
        //        await Context.SaveChangesAsync();
        //        await transaction.CommitAsync();

        //        //tranaction failed
        //        return _Operation.SetSuccess(new TransactionBalanceDto()
        //        {
        //            TransactionReference = setTrans.TransactionReference,
        //            Status = setTrans.Status,
        //            Amount = setTrans.Amount,
        //            Date = setTrans.Date,
        //        });
        //    }
        //    catch (Exception)
        //    {

        //        await transaction.RollbackAsync();
        //        return ("invalid", Statuses.Failed);
        //    }
        //}

    }
}

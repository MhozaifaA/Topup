# Topup 

To run projects publish then user fit docker images

or **Debug** by visutal studio

## Get Start:

1. Install VS Code /Visual Studio 17.x later
2. Ensure installed .Net 8.0 sdk.
3. Install RabbitMQ.
4. Ensure installed SqlServer.
5. Edit appsetsings as your environment(connection string, urls...).
6. Build Solution (MassTransit, Meteors, Microsoft, EFCore) have to installed.
7. Cong Startup projects `BalaceApi`/`TopupApi` .
8. Update-Database -Context BalanceDbContext
9. Update Database -Context TopupDbContext
10. run projects.
11. Balance Api isnot protected (quick to dev). 
12. Topup seed check ``src\Infrastructure\Databases\Topup.Infrastructure.Databases.SqlServer\Main\Seed``
13. usernmae:huzaifa, passsword:huzaifa, userNO:012345 /  username:test, password:test, userNO:543210



### Balance Api

_____________________________

<img src="/docs/balanceapiswagger.jpg">



**All Apis work Concurrency**

``` sql
--run 100 vertual user instert 20~100 same time 

select SUM(Amount) from Transactions where Status = 2 or Status = 3

--= CurrentBalance = SUM(Amount)  end of test sure

select CurrentBalance,AvailableBalance from Users  
```



> read comments inside project



<img src="/docs/rmq.jpg">

### Topup Api

<img src="/docs/topupapiswagger.jpg">

<img src="/docs/folder.jpg">





[^Close]: Read the comments


using Meteors;
using Meteors.OperationContext;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Text.Json;

namespace Integration.Balance.HttpService
{
    public class BalanceHttpService
    {
        private readonly HttpClient _httpClient;

        public BalanceHttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<OperationResult<CurrentBalanceResponse>> CurrentBalance(string userNO)
        {
            try
            {
                var obj = await _httpClient.GetFromJsonAsync<CurrentBalanceResponse>($"{Routes.CurrentBalanceUrl}/{userNO}");

                //response.EnsureSuccessStatusCode();//no need to pipe all status

                //var json = await response.Content.ReadAsStringAsync();
                //var obj = JsonSerializer.Deserialize<CurrentBalanceResponse>(json);

                return _Operation.SetSuccess(obj);
            }
            catch (HttpRequestException e) when (e.HttpRequestError == HttpRequestError.ConnectionError) //basic handle without log and tracking
            {
                return (e.Message + " Please run Balance Api", Statuses.Failed);
            }
            catch (Exception e)
            {
                return e;
            }
        }


        public async Task<OperationResult<TransactionResponse>> Debit(TransferRequest body)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{Routes.DebitUrl}", body);

                response.EnsureSuccessStatusCode();//no need to pipe all status

                var json = await response.Content.ReadAsStringAsync();
                var obj = JsonSerializer.Deserialize<TransactionResponse>(json);

                return _Operation.SetSuccess(obj);
            }
            catch (HttpRequestException e) when (e.HttpRequestError == HttpRequestError.ConnectionError) //basic handle without log and tracking
            {
                return (e.Message + " Please run Balance Api", Statuses.Failed);
            }
            catch (Exception e)
            {
                return e;
            }
        }


        public async Task<OperationResult<TransactionResponse>> ConfirmDebitTransaction(ConfirmDebitRequest body)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{Routes.ConfirmDebitTransactionUrl}", body);

              

                response.EnsureSuccessStatusCode();//no need to pipe all status

                var json = await response.Content.ReadAsStringAsync();
                var obj = JsonSerializer.Deserialize<TransactionResponse>(json);

                return _Operation.SetSuccess(obj);
            }
            catch (HttpRequestException e) when (e.HttpRequestError == HttpRequestError.ConnectionError) //basic handle without log and tracking
            {
                return (e.Message + " Please run Balance Api", Statuses.Failed);
            }
            catch (Exception e)
            {
                return e;
            }
        }
    }
}

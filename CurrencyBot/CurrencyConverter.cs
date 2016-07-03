using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Microsoft.Rest;

namespace CurrencyBot
{
    public class CurrencyConverter
    {
        public async Task<decimal> GetConvertedResult(decimal value, Currency currencyInput, Currency currencyOutput)
        {
           var response = await LoadExchangeRate(currencyInput);
           var conversationUnit = GetConversationUnit(response);
            return ConvertMoney(value, currencyOutput, conversationUnit);
        }

        private async Task<HttpResponseMessage> LoadExchangeRate(Currency currencyInput)
        {
            var currencyInputString = ConvertCurrencyName(currencyInput);
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://api.fixer.io/latest?base="+currencyInputString);
            HttpResponseMessage response = await client.GetAsync(client.BaseAddress);
            return response;
        }

        private ConversationUnit GetConversationUnit(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                JavaScriptSerializer json = new JavaScriptSerializer();
                return json.Deserialize<ConversationUnit>(response.Content.AsString());
            }
            return new ConversationUnit();
        }

        private string ConvertCurrencyName(Currency currency)
        {
            switch (currency)
            {
                case Currency.Dollar:
                    return "USD";
                case Currency.Euro:
                    return "EUR";
                case Currency.Ruble:
                    return "RUR";
                default:
                    return string.Empty;
            }
        }

        private decimal ConvertMoney(decimal value, Currency currencyOutput, ConversationUnit conversation)
        {
            if (value < 0) return 0;
            var currencyOutputString = ConvertCurrencyName(currencyOutput);
            var exchangeRate = conversation.Rates[currencyOutputString];
            if (exchangeRate <= 0)
            {
                return -1;
            }
            return value*exchangeRate;
        }
    }


    public class ConversationUnit
    {
        public string Base { get; set; }
        public DateTime Date { get; set; }
        public Dictionary<string,decimal> Rates { get; set; } 
        public ConversationUnit() { }
    }
    public enum Currency { Dollar ,Euro, Ruble}
}
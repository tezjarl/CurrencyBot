using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Utilities;
using Newtonsoft.Json;

namespace CurrencyBot
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<Message> Post([FromBody]Message message)
        {
            if (message.Type == "Message")
            {
                // calculate something for us to return
                if (message.Text == "help")
                    return HelpMessageReply(message);
                if (Regex.IsMatch(message.Text, "[(0-9]+/(eu|r){1}"))
                {
                    return ConvertedResultReply(message);
                }
                // return our reply to the user
                return message.CreateReplyMessage("I don't understood your message");
            }
            return HandleSystemMessage(message);
        }
        //TODO: Replace this one to LUIS interaction
        private Message ConvertedResultReply(Message message)
        {
            string[] inputValues=message.Text.Split('/');
            if (inputValues.Length != 2) message.CreateReplyMessage("Cannot convert", "en");
            CurrencyConverter converter=new CurrencyConverter();
            decimal convertingValue = Convert.ToDecimal(inputValues[0]);
            switch (inputValues[1])
            {
                case "eu":
                    return
                        message.CreateReplyMessage(
                            converter.GetConvertedResult(convertingValue, Currency.Dollar, Currency.Euro).Result.ToString());
                case "r":
                    return
                  message.CreateReplyMessage(
                      converter.GetConvertedResult(convertingValue, Currency.Dollar, Currency.Ruble).Result.ToString());
                default:
                    return message.CreateReplyMessage("Conversation for this currency not yet supported");
            }
        }

        private Message HelpMessageReply(Message message)
        {
            StringBuilder builder=new StringBuilder();

            builder.AppendLine("The request should looks like the following");
            builder.AppendLine("100/r");
            builder.AppendLine("You can use any number you want, 100 is just an example");
            builder.AppendLine("Supported currencies and their symbols: \n");
            builder.AppendLine("eu-euros, \n");
            builder.AppendLine("r-rubles RUS, \n");
            builder.AppendLine("IMPORTANT:Current version converts only from dollars US");

            return  message.CreateReplyMessage(builder.ToString(),"en");
        }

        private Message HandleSystemMessage(Message message)
        {
            if (message.Type == "Ping")
            {
                Message reply = message.CreateReplyMessage();
                reply.Type = "Ping";
                return reply;
            }
            if (message.Type == "DeleteUserData")
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            if (message.Type == "BotAddedToConversation")
            {
                return message.CreateReplyMessage("Hello I am currency bot!");
            }
            if (message.Type == "BotRemovedFromConversation")
            {
            }
            if (message.Type == "UserAddedToConversation")
            {
            }
            if (message.Type == "UserRemovedFromConversation")
            {
            }
            if (message.Type == "EndOfConversation")
            {
            }

            return null;
        }
    }
}
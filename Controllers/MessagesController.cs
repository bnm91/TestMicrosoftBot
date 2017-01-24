using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Bot_Application1
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                // calculate something for us to return
                int length = (activity.Text ?? string.Empty).Length;

                Activity reply = null;
                // return our reply to the user
                if (activity.Text == "!notarapper.gif")
                {
                    reply = activity.CreateReply();
                    Attachment attachment = new Attachment()
                        {
                            ContentUrl = "http://i.imgur.com/xgz9nkR.gif",
                            ContentType = "image/gif",
                            Name = "notarapper.gif"
                        };
                    reply.Attachments = new List<Attachment>();
                    reply.Attachments.Add(attachment);
                    }
                else if (activity.Text.StartsWith("!d"))
                {
                    int sides = Int32.Parse(activity.Text.Substring(2, length - 2));
                    Random rnd = new Random();
                    reply = activity.CreateReply(rnd.Next(sides + 1).ToString());
                }
                else
                {
                    reply = activity.CreateReply($"You sent {activity.Text} which was {length} characters");
                }
                await connector.Conversations.ReplyToActivityAsync(reply);
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}
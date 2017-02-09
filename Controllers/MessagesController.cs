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
using System.Text.RegularExpressions;
using Bot_Application1.TeamServicesAPI;
using Newtonsoft.Json.Linq;

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
                    reply = activity.CreateReply("http://i.imgur.com/xgz9nkR.gif");
                    //Attachment attachment = new Attachment()
                    //    {
                    //        ContentUrl = "http://i.imgur.com/xgz9nkR.gif",
                    //        ContentType = "image/gif",
                    //        Name = "notarapper.gif"
                    //    };
                    //reply.Attachments = new List<Attachment>();
                    //reply.Attachments.Add(attachment);
                }
                else if (activity.Text.Contains("!d"))// && Int32.TryParse(activity.Text.Substring(2, length -2))
                {
                    int sides = Int32.Parse(activity.Text.Substring(2, length - 2));
                    Random rnd = new Random();
                    reply = activity.CreateReply(rnd.Next(sides + 1).ToString());
                }
                else if (Regex.IsMatch(activity.Text, @".*?\[.*?\].*?"))
                {
                    int num;
                    string input = Regex.Match(activity.Text, @"\[(.*?)\]").Groups[1].Value;
                    bool result = Int32.TryParse(input, out num);
                    if (result)
                    {
                        WorkItemsAPI api = new WorkItemsAPI();
                        string r = api.GetWorkItem(num.ToString());
                        JObject json = JObject.Parse(r);
                        JToken f = json["fields"];
                        JToken systemAreaPath = f["System.AreaPath"];
                        JToken systemTitle = f["System.Title"];
                        reply = activity.CreateReply(systemAreaPath.ToString() + ": " + systemTitle.ToString());
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                //else
                //{
                //    reply = activity.CreateReply($"You sent {activity.Text} which was {length} characters");
                //}
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
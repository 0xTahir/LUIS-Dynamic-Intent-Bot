using System;
using System.Configuration;
using System.Threading.Tasks;

using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;

using System.Security;
using Microsoft.SharePoint.Client;

namespace Microsoft.Bot.Sample.LuisBot
{
    // For more information about this template visit http://aka.ms/azurebots-csharp-luis
    [Serializable]
    public class BasicLuisDialog : LuisDialog<object>
    {
        public BasicLuisDialog() : base(new LuisService(new LuisModelAttribute(
            ConfigurationManager.AppSettings["LuisAppId"], 
            ConfigurationManager.AppSettings["LuisAPIKey"], 
            domain: ConfigurationManager.AppSettings["LuisAPIHostName"])))
        {
        }

        [LuisIntent("None")]
        public async Task NoneIntent(IDialogContext context, LuisResult result)
        {

	/*
	
	Once LUIS returns no intent then we will goto SharePoint Online to see the intents stored in a list.

	*/

            string x = GetIntentionsFromSPOList();

            context.PostAsync($"SPO Site Intentions:  {x}");

            await this.ShowLuisResult(context, result);
        }

        public string GetIntentionsFromSPOList()
        {
        
            /* https://blogs.msdn.microsoft.com/kaevans/2014/02/23/call-o365-using-csom-with-a-console-application/ */
            /* https://msdn.microsoft.com/en-us/library/office/ee534956(v=office.14).aspx */

    
            string webUrl = "https://tenantname.sharepoint.com/sites/MyTeam1";
            string userName = "admin@tenantname.onmicrosoft.com";

            var secure = new SecureString();
            foreach (char c in "SecretPassword")
            {
                secure.AppendChar(c);
            }

            using (var clientContext = new ClientContext(webUrl))
            {
                clientContext.Credentials = new SharePointOnlineCredentials(userName, secure);

                List oList = clientContext.Web.Lists.GetByTitle("LUIS");

                CamlQuery camlQuery = new CamlQuery();
                camlQuery.ViewXml = "<View><Query><Where><Geq><FieldRef Name='ID'/>" +
                    "<Value Type='Number'>1</Value></Geq></Where></Query><RowLimit>100</RowLimit></View>";
                ListItemCollection collListItem = oList.GetItems(camlQuery);

                clientContext.Load(collListItem);

                clientContext.ExecuteQuery();

                string temp = "";

                foreach (ListItem oListItem in collListItem)
                {
                    //Console.WriteLine("ID: {0} \nTitle: {1} \nBody: {2}", oListItem.Id, oListItem["Title"], oListItem["Body"]);
                    temp = temp + oListItem["Title"].ToString()+ ", ";
                }

                return temp.TrimEnd(' ').TrimEnd(',');

            }
        }


        // Go to https://luis.ai and create a new intent, then train/publish your luis app.
        // Finally replace "Gretting" with the name of your newly created intent in the following handler
        [LuisIntent("Greeting")]
        public async Task GreetingIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        [LuisIntent("Cancel")]
        public async Task CancelIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        [LuisIntent("Help")]
        public async Task HelpIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        private async Task ShowLuisResult(IDialogContext context, LuisResult result) 
        {
            await context.PostAsync($"You have reached {result.Intents[0].Intent}. You said: {result.Query}");
            context.Wait(MessageReceived);
        }
    }
}
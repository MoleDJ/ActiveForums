using System;
using ActiveForumsTapatalk.Implementation;


namespace ActiveForumsTapatalk.Push
{
    public class Push : TapatalkBase.Push
    {
        public string GetTapatalkApiKey()
        {
            return "";
        }
        public string GetForumUrl()
        {
            return "";
        }
        public override void InitPush(System.Web.HttpApplication context)
        {
            base.InitPush(context);
            context.BeginRequest += new EventHandler(context_BeginRequest);
        }

        private void context_BeginRequest(object sender, EventArgs e)
        {
         
        }

    }
}

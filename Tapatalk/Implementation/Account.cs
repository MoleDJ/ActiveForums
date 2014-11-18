using System;
using DotNetNuke.Common.Utilities;
using Newtonsoft.Json.Linq;
using Tapatalk.Interfaces;
using Tapatalk_BasePlugin.Interfaces;
using Tapatalk_Plugin.Structures;

namespace ActiveForumsTapatalk.Implementation
{
    public partial class ActiveForumsTapatalk : IAccount
    {

        public override bool _check_emailforuser_exists(ITapatalkModuleContext context, string email)
        {
            throw new NotImplementedException();
        }

        public override bool _check_username_exists(ITapatalkModuleContext context, string username)
        {
            throw new NotImplementedException();
        }

        public override sign_in_response _sign_in_create_new_account(ITapatalkModuleContext context, string email, string username, string password, JObject tapatalkIDProfile)
        {
            throw new NotImplementedException();
        }

        public override sign_in_response _sign_in_using_email(ITapatalkModuleContext context, string email)
        {
            throw new NotImplementedException();
        }

        public override sign_in_response _sign_in_using_username(ITapatalkModuleContext context, string username)
        {
            throw new NotImplementedException();
        }

        public override forget_password_response _forget_password(ITapatalkModuleContext context, string username, string token, string code)
        {
            throw new NotImplementedException();
        }

        public override update_password_response _update_password(ITapatalkModuleContext context, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override update_password_response _update_password(ITapatalkModuleContext context, string newPassword, string token, string code)
        {
            throw new NotImplementedException();
        }

        public override update_email_response _update_email(ITapatalkModuleContext context, string password, string newEmail)
        {
            throw new NotImplementedException();
        }

        public override register_response _register(ITapatalkModuleContext context, string username, string password, string email, string token, string code)
        {
            throw new NotImplementedException();
        }

        public override prefetch_account_response _prefetch_account(ITapatalkModuleContext context, string email)
        {
            throw new NotImplementedException();
        }
    }
}

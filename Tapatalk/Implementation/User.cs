using System;

using System.Linq;
using System.Web;
using System.Web.Security;
using DotNetNuke.Modules.ActiveForums;
using DotNetNuke.Modules.ActiveForums.Tapatalk.Classes;
using DotNetNuke.Security.Membership;
using Tapatalk.Extensions;
using Tapatalk.Interfaces;
using Tapatalk_BasePlugin.Interfaces;
using Tapatalk_Plugin.Structures;


namespace ActiveForumsTapatalk.Implementation
{
    public partial class ActiveForumsTapatalk : IUser
    {
        /// <summary>
        /// https://tapatalk.com/api/api_section.php?id=2#login
        /// </summary>
        /// <param name="context"></param>
        /// <param name="login_name"></param>
        /// <param name="password"></param>
        /// <param name="anonymous"></param>
        /// <returns></returns>
        public override login_response _login(ITapatalkModuleContext context, string loginName, string password, bool anonymous = false)
        {
            var aftContext = (ActiveForumsTapatalkModuleContext)context;
         
            var loginStatus = UserLoginStatus.LOGIN_FAILURE;

            DotNetNuke.Entities.Users.UserController.ValidateUser(aftContext.Portal.PortalID, loginName, password, string.Empty, aftContext.Portal.PortalName, Context.Request.UserHostAddress, ref loginStatus);

            var result = false;
            var resultText = string.Empty;

            switch (loginStatus)
            {
                case UserLoginStatus.LOGIN_SUCCESS:
                case UserLoginStatus.LOGIN_SUPERUSER:
                    result = true;
                    break;

                case UserLoginStatus.LOGIN_FAILURE:
                    resultText = "Invalid Login/Password Combination";
                    break;

                case UserLoginStatus.LOGIN_USERNOTAPPROVED:
                    resultText = "User Not Approved";
                    break;

                case UserLoginStatus.LOGIN_USERLOCKEDOUT:
                    resultText = "User Temporarily Locked Out";
                    break;

                default:
                    resultText = "Unknown Login Error";
                    break;
            }

            User forumUser = null;

            if (result)
            {
                // Get the User
                var userInfo = DotNetNuke.Entities.Users.UserController.GetUserByName(aftContext.Module.PortalID, loginName);

                if (userInfo == null)
                {
                    result = false;
                    resultText = "Unknown Login Error";
                }
                else
                {
                    // Set Login Cookie
                    var expiration = DateTime.Now.Add(FormsAuthentication.Timeout);

                    var ticket = new FormsAuthenticationTicket(1, loginName, DateTime.Now, expiration, false, userInfo.UserID.ToString());
                    var authCookie = new HttpCookie(aftContext.AuthCookieName, FormsAuthentication.Encrypt(ticket))
                    {
                        Domain = FormsAuthentication.CookieDomain,
                        Path = FormsAuthentication.FormsCookiePath,
                    };


                    Context.Response.SetCookie(authCookie);

                    forumUser = new UserController().GetUser(aftContext.Module.PortalID, aftContext.ModuleSettings.ForumModuleId, userInfo.UserID);
                }
            }

            Context.Response.AddHeader("Mobiquo_is_login", result ? "true" : "false");

            var response = new login_response()
            {
                result = result,
                result_text = resultText,
                can_upload_avatar = false,
                can_pm = false
            };

            if (result && forumUser != null)
            {
                response.user_id = forumUser.UserId.ToString();
                response.username = forumUser.UserName;
                response.email = forumUser.Email;
                response.usergroup_id = new string[] { };
                response.post_count = forumUser.PostCount;
                response.icon_url = Helpers.GetAvatarUrl(forumUser.UserId);

            }
            return response;
        }

        /// <summary>
        ///  https://tapatalk.com/api/api_section.php?id=2#get_inbox_stat
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override get_inbox_stat_response _get_inbox_stat(ITapatalkModuleContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// https://tapatalk.com/api/api_section.php?id=2#logout_user
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override void _logout_user(ITapatalkModuleContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// https://tapatalk.com/api/api_section.php?id=2#get_inbox_stat
        /// </summary>
        /// <param name="context"></param>
        /// <param name="page"></param>
        /// <param name="perPage"></param>
        /// <param name="id"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        public override get_online_users_response _get_online_users(ITapatalkModuleContext context, int page = 1, int perPage = 20, string id = null, string area = null)
        {
            throw new NotImplementedException();

        }

        /// <summary>
        /// https://tapatalk.com/api/api_section.php?id=2#get_user_info
        /// </summary>
        /// <param name="context"></param>
        /// <param name="username"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public override get_user_info_response _get_user_info(ITapatalkModuleContext context, string username, string userId = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// https://tapatalk.com/api/api_section.php?id=2#get_user_topic
        /// </summary>
        /// <param name="context"></param>
        /// <param name="username"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public override get_user_topic_response[] _get_user_topic(ITapatalkModuleContext context, string username, string userId = null)
        {

            throw new NotImplementedException();
        }

        /// <summary>
        /// https://tapatalk.com/api/api_section.php?id=2#get_user_reply_post
        /// </summary>
        /// <param name="context"></param>
        /// <param name="username"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public override get_user_reply_post_response[] _get_user_reply_post(ITapatalkModuleContext context, string username, string userId = null)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// https://tapatalk.com/api/api_section.php?id=2#get_recomended_user
        /// </summary>
        /// <param name="context"></param>
        /// <param name="page"></param>
        /// <param name="perPage"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public override get_recommended_user_response _get_recomended_user(ITapatalkModuleContext context, int page = 1, int perPage = 20, int mode = 1)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// https://tapatalk.com/api/api_section.php?id=2#search_user
        /// </summary>
        /// <param name="context"></param>
        /// <param name="keywords"></param>
        /// <param name="page"></param>
        /// <param name="perPage"></param>
        /// <returns></returns>
        public override search_user_response _search_user(ITapatalkModuleContext context, string keywords, int page = 1, int perPage = 20)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// https://tapatalk.com/api/api_section.php?id=2#ignore_user
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userId"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public override ignore_user_response _ignore_user(ITapatalkModuleContext context, string userId, int mode = 1)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// https://tapatalk.com/api/api_section.php?id=2#get_contact
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public override get_contact_response _get_contact(ITapatalkModuleContext context, string userId)
        {
            throw new NotImplementedException();
        }
    }
}

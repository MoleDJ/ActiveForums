using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using DotNetNuke.Modules.ActiveForums;
using DotNetNuke.Modules.ActiveForums.Tapatalk.Classes;
using Tapatalk.Interfaces;
using Tapatalk_BasePlugin.Interfaces;
using Tapatalk_Plugin.Structures;

namespace ActiveForumsTapatalk.Implementation
{
    public partial class ActiveForumsTapatalk : ITopic
    {
        /// <summary>
        /// https://tapatalk.com/api/api_section.php?id=3#mark_topic_read
        /// </summary>
        /// <param name="context"></param>
        /// <param name="topicIds"></param>
        /// <returns></returns>
        public override mark_topic_read_response _mark_topic_read(ITapatalkModuleContext context, List<string> topicIds)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// https://tapatalk.com/api/api_section.php?id=3#get_topic_status
        /// </summary>
        /// <param name="context"></param>
        /// <param name="topicIds"></param>
        /// <returns></returns>
        public override get_topic_status_response _get_topic_status(ITapatalkModuleContext context, List<string> topicIds)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// https://tapatalk.com/api/api_section.php?id=3#new_topic
        /// </summary>
        /// <param name="context"></param>
        /// <param name="forumId"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="prefixId"></param>
        /// <param name="attachmentIds"></param>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public override new_topic_response _new_topic(ITapatalkModuleContext context, string forumId, string subject, string body, string prefixId, List<string> attachmentIds = null, string groupId = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// https://tapatalk.com/api/api_section.php?id=3#get_topic
        /// </summary>
        /// <param name="context"></param>
        /// <param name="forumId"></param>
        /// <param name="page"></param>
        /// <param name="perPage"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public override get_topic_response _get_topic(ITapatalkModuleContext context, string forumId, int page, int perPage, string mode)
        {
            var aftContext = (ActiveForumsTapatalkModuleContext)context;
            if (mode == "ANN" || mode == "TOP")
            {
                return new get_topic_response();
            }
            int rowIndex = 0;
            if (page != 1)
            {
                rowIndex = ((page * perPage) - perPage);
            }
            string sort = SortColumns.ReplyCreated;
            var result = new get_topic_response();
            DataSet ds = DataProvider.Instance().UI_TopicsView(aftContext.Module.PortalID, aftContext.Module.ModuleID, Convert.ToInt32(forumId), aftContext.UserId, rowIndex, perPage, aftContext.ForumUser.IsSuperUser, sort);
            if (ds.Tables.Count > 0)
            {
                var drForum = ds.Tables[0].Rows[0];
                var drSecurity = ds.Tables[1].Rows[0];
                var dtSubForums = ds.Tables[2];
                var dtTopics = ds.Tables[3];
                if (page == 1)
                {
                    var dtAnnounce = ds.Tables[4];
                }
                
                var bView = Permissions.HasPerm(drSecurity["CanView"].ToString(), aftContext.ForumUser.UserRoles);
                var bRead = Permissions.HasPerm(drSecurity["CanRead"].ToString(), aftContext.ForumUser.UserRoles);
                var bCreate = Permissions.HasPerm(drSecurity["CanCreate"].ToString(), aftContext.ForumUser.UserRoles);
                var bEdit = Permissions.HasPerm(drSecurity["CanEdit"].ToString(), aftContext.ForumUser.UserRoles);
                var bDelete = Permissions.HasPerm(drSecurity["CanDelete"].ToString(), aftContext.ForumUser.UserRoles);
                //bReply = Permissions.HasPerm(drSecurity["CanReply"].ToString(), aftContext.ForumUser.UserRoles);
                var bPoll = Permissions.HasPerm(drSecurity["CanPoll"].ToString(), aftContext.ForumUser.UserRoles);

                var bSubscribe = Permissions.HasPerm(drSecurity["CanSubscribe"].ToString(), aftContext.ForumUser.UserRoles);
                var bModMove = Permissions.HasPerm(drSecurity["CanModMove"].ToString(), aftContext.ForumUser.UserRoles);
                var bModSplit = Permissions.HasPerm(drSecurity["CanModSplit"].ToString(), aftContext.ForumUser.UserRoles);
                var bModDelete = Permissions.HasPerm(drSecurity["CanModDelete"].ToString(), aftContext.ForumUser.UserRoles);
                var bModApprove = Permissions.HasPerm(drSecurity["CanModApprove"].ToString(), aftContext.ForumUser.UserRoles);
                var bModEdit = Permissions.HasPerm(drSecurity["CanModEdit"].ToString(), aftContext.ForumUser.UserRoles);
                var bModPin = Permissions.HasPerm(drSecurity["CanModPin"].ToString(), aftContext.ForumUser.UserRoles);
                var bModLock = Permissions.HasPerm(drSecurity["CanModLock"].ToString(), aftContext.ForumUser.UserRoles);
                bModApprove = Permissions.HasPerm(drSecurity["CanModApprove"].ToString(), aftContext.ForumUser.UserRoles);

               
                if (bView)
                {
                    result.forum_id = forumId;
                    result.forum_name = drForum["ForumName"].ToString();
                    result.can_post = bCreate;
                    //GroupName = drForum["GroupName"].ToString();
                    //ForumGroupId = Convert.ToInt32(drForum["ForumGroupId"]);
                    
                    result.can_subscribe = bSubscribe;
                    if (aftContext.UserId > 0)
                    {
                        result.is_subscribed = Convert.ToBoolean(((Convert.ToInt32(drForum["IsSubscribedForum"]) > 0) ? true : false));
                    }

                    result.total_topic_num = Convert.ToInt32(drForum["TopicRowCount"]);
                    result.topics = (from r in dtTopics.Rows.Cast<DataRow>() 
                                     select new get_topic_response.topic()
                                     {
                                         forum_id = r.GetString("ForumId"),
                                         topic_id = r.GetString("TopicId"),
                                         topic_title = HttpUtility.HtmlDecode(r.GetString("Subject")),
                                         topic_author_id = r.GetString("AuthorId"),
                                         topic_author_name = r.GetString("AuthorName"),
                                         is_subscribed = false, 
                                         can_subscribe = false,
                                         is_closed = r.GetBoolean("IsLocked"),
                                         icon_url = Helpers.GetAvatarUrl(r.GetInt("AuthorId")),
                                         last_reply_time = r.GetDateTime("LastReplyDate"),
                                         reply_number = r.GetInt("ReplyCount"),
                                         new_post = false,
                                         view_number = r.GetInt("ViewCount"),
                                         short_content = r.GetString("Body"),
                                         can_approve = false,
                                         can_merge = false,
                                         can_ban = false,
                                         can_close = false,
                                         can_delete = false,
                                         can_move = false,
                                         can_rename = false,
                                         can_stick = false,
                                         is_approved = true,
                                         is_ban = false,
                                         is_deleted = false,
                                         is_sticky = r.GetBoolean("IsPinned")
                                     }).ToArray();
                }
            }
           
           return result;
        }

        /// <summary>
        /// https://tapatalk.com/api/api_section.php?id=3#get_unread_topic
        /// </summary>
        /// <param name="context"></param>
        /// <param name="page"></param>
        /// <param name="perPage"></param>
        /// <param name="searchId"></param>
        /// <param name="filters"></param>
        /// <returns></returns>
        public override get_unread_topic_response _get_unread_topic(ITapatalkModuleContext context, int page, int perPage, string searchId = null, object filters = null)
        {
            // If user is not signed in, don't return any unread topics
            if (context.IsLogged() == false)
            {
                return new get_unread_topic_response
                {
                    total_topic_num = 0
                };
            }

            throw new NotImplementedException();
        }

        /// <summary>
        /// https://tapatalk.com/api/api_section.php?id=3#get_participated_topic
        /// </summary>
        /// <param name="context"></param>
        /// <param name="username"></param>
        /// <param name="page"></param>
        /// <param name="perPage"></param>
        /// <param name="searchId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public override get_participated_topic_response _get_participated_topic(ITapatalkModuleContext context, string username, int page, int perPage, string searchId, string userId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// https://tapatalk.com/api/api_section.php?id=3#get_latest_topic
        /// </summary>
        /// <param name="context"></param>
        /// <param name="page"></param>
        /// <param name="perPage"></param>
        /// <param name="searchId"></param>
        /// <param name="filters"></param>
        /// <returns></returns>
        public override get_latest_topic_response _get_latest_topic(ITapatalkModuleContext context, int page, int perPage, string searchId = null, object filters = null)
        {
            throw new NotImplementedException();
        }
    }
}

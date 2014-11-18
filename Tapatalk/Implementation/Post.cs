using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using DotNetNuke.Modules.ActiveForums;
using DotNetNuke.Modules.ActiveForums.Data;
using DotNetNuke.Modules.ActiveForums.Tapatalk.Classes;
using Tapatalk.Interfaces;
using Tapatalk_BasePlugin.Interfaces;
using Tapatalk_Plugin.Structures;

namespace ActiveForumsTapatalk.Implementation
{
    public partial class ActiveForumsTapatalk : IPost
    {
        /// <summary>
        /// https://tapatalk.com/api/api_section.php?id=4#report_post
        /// </summary>
        /// <param name="context"></param>
        /// <param name="postId"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        public override report_post_response _report_post(ITapatalkModuleContext context, string postId, string reason)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// https://tapatalk.com/api/api_section.php?id=4#reply_post
        /// </summary>
        /// <param name="context"></param>
        /// <param name="forumId"></param>
        /// <param name="topicId"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="attachmentIds"></param>
        /// <param name="groupId"></param>
        /// <param name="returnHtml"></param>
        /// <returns></returns>
        public override reply_post_response _reply_post(ITapatalkModuleContext context, string forumId, string topicId, string subject, string body, List<string> attachmentIds, string groupId, bool returnHtml)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// https://tapatalk.com/api/api_section.php?id=4#get_quote_post
        /// </summary>
        /// <param name="context"></param>
        /// <param name="postId"></param>
        /// <returns></returns>
        public override get_quote_post_response _get_quote_post(ITapatalkModuleContext context, string postId)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// https://tapatalk.com/api/api_section.php?id=4#get_quote_post
        /// </summary>
        /// <param name="context"></param>
        /// <param name="postId"></param>
        /// <returns></returns>
        public override get_quote_post_response _get_quote_post(ITapatalkModuleContext context, List<string> postId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// https://tapatalk.com/api/api_section.php?id=4#get_raw_post
        /// </summary>
        /// <param name="context"></param>
        /// <param name="postId"></param>
        /// <returns></returns>
        public override get_raw_post_response _get_raw_post(ITapatalkModuleContext context, string postId)
        {
            var aftContext = (ActiveForumsTapatalkModuleContext)context;
            var result = new get_raw_post_response();
            return result;
        }

        /// <summary>
        /// https://tapatalk.com/api/api_section.php?id=4#save_raw_post
        /// </summary>
        /// <param name="context"></param>
        /// <param name="postId"></param>
        /// <param name="postTitle"></param>
        /// <param name="postContent"></param>
        /// <param name="returnHtml"></param>
        /// <param name="attachmentIds"></param>
        /// <param name="groupId"></param>
        /// <param name="reason"></param>
        /// <returns></returns>
        public override save_raw_post_response _save_raw_post(ITapatalkModuleContext context, string postId, string postTitle, string postContent, bool returnHtml, List<string> attachmentIds, string groupId, string reason)
        {
            throw new NotImplementedException();
        }

  
        /// <summary>
        /// https://tapatalk.com/api/api_section.php?id=4#get_thread
        /// </summary>
        /// <param name="context"></param>
        /// <param name="topicId"></param>
        /// <param name="page"></param>
        /// <param name="perPage"></param>
        /// <param name="returnHtml"></param>
        /// <returns></returns>
        public override get_thread_response _get_thread(ITapatalkModuleContext context, string topicId, int page, int perPage, bool returnHtml)
        {
            var aftContext = (ActiveForumsTapatalkModuleContext)context;
            var result = new get_thread_response();
            int rowIndex = 0;
            if (page != 1)
            {
                rowIndex = ((page * perPage) - perPage);
            }
            var dbForums = new ForumsDB();
            var forumId = dbForums.Forum_GetByTopicId(Convert.ToInt32(topicId));
            var ds = DataProvider.Instance().UI_TopicView(aftContext.Module.PortalID, aftContext.Module.ModuleID, forumId, Convert.ToInt32(topicId), aftContext.UserId, rowIndex, perPage, aftContext.ForumUser.IsSuperUser, "");

            // Load our values
            var drForum = ds.Tables[0].Rows[0];
            var drSecurity = ds.Tables[1].Rows[0];
            var dtTopic = ds.Tables[2];
            var dtAttach = ds.Tables[3];

            var breadCrumbs = new List<get_thread_response.breadcrumbItem>();
            var fc = new ForumController();
            var forumIds = fc.GetForumsForUser(aftContext.ForumUser.UserRoles, aftContext.Module.PortalID, aftContext.ModuleId, "CanRead");
            var forumTable = fc.GetForumView(aftContext.Module.PortalID, aftContext.ModuleId, aftContext.UserId, aftContext.ForumUser.IsSuperUser, forumIds);

            DataRow currentBreadCrumb = (from bf in forumTable.Rows.Cast<DataRow>()
                                    where bf.GetInt("ForumId") == forumId
                                    select bf).SingleOrDefault();
            while (currentBreadCrumb != null)
            {
                breadCrumbs.Add(new get_thread_response.breadcrumbItem
                {
                    forum_id = currentBreadCrumb.GetString("ForumId"),
                    sub_only = false,
                    forum_name = currentBreadCrumb.GetString("ForumName")
                });
                currentBreadCrumb = (from bf in forumTable.Rows.Cast<DataRow>()
                                     where bf.GetString("ForumId") == currentBreadCrumb.GetString("ParentForumId")
                                     select bf).SingleOrDefault();
            }
            breadCrumbs.Add(new get_thread_response.breadcrumbItem
            {
                forum_id = "G" + currentBreadCrumb.GetString("ForumGroupId"),
                sub_only = true,
                forum_name = currentBreadCrumb.GetString("GroupName")
            });


            // If we're in a sub forum, add the parent to the breadcrumb
            //if (drForum.GetInt("ParentForumId") > 0)
            //    breadCrumbs.Add(new get_thread_response.breadcrumbItem
            //    {
            //        ForumId = drForum.GetString("ParentForumId.ToString(),
            //        IsCategory = false,
            //        Name = forumPostSummary.ParentForumName.ToBytes()
            //    });

            //breadCrumbs.Add(new BreadcrumbStructure
            //{
            //    ForumId = forumId.ToString(),
            //    IsCategory = false,
            //    Name = forumPostSummary.ForumName.ToBytes()
            //});

            // first make sure we have read permissions, otherwise we need to redirect
            var bRead = Permissions.HasPerm(drSecurity["CanRead"].ToString(), aftContext.ForumUser.UserRoles);

            if (!bRead)
            {
                return result;
            }


            //bCreate = Permissions.HasPerm(drSecurity["CanCreate"].ToString(), ForumUser.UserRoles);
            var bEdit = Permissions.HasPerm(drSecurity["CanEdit"].ToString(), aftContext.ForumUser.UserRoles);
            var bDelete = Permissions.HasPerm(drSecurity["CanDelete"].ToString(), aftContext.ForumUser.UserRoles);
            //bReply = Permissions.HasPerm(drSecurity["CanReply"].ToString(), ForumUser.UserRoles);
            //bPoll = Permissions.HasPerm(drSecurity["CanPoll"].ToString(), ForumUser.UserRoles);
            var bAttach = Permissions.HasPerm(drSecurity["CanAttach"].ToString(), aftContext.ForumUser.UserRoles);
            var bSubscribe = Permissions.HasPerm(drSecurity["CanSubscribe"].ToString(), aftContext.ForumUser.UserRoles);
            // bModMove = Permissions.HasPerm(drSecurity["CanModMove"].ToString(), ForumUser.UserRoles);
            var bModSplit = Permissions.HasPerm(drSecurity["CanModSplit"].ToString(), aftContext.ForumUser.UserRoles);
            var bModDelete = Permissions.HasPerm(drSecurity["CanModDelete"].ToString(), aftContext.ForumUser.UserRoles);
            var bModApprove = Permissions.HasPerm(drSecurity["CanModApprove"].ToString(), aftContext.ForumUser.UserRoles);
            var bTrust = Permissions.HasPerm(drSecurity["CanTrust"].ToString(), aftContext.ForumUser.UserRoles);
            var bModEdit = Permissions.HasPerm(drSecurity["CanModEdit"].ToString(), aftContext.ForumUser.UserRoles);

          //  var isTrusted = Utilities.IsTrusted((int)ForumInfo.DefaultTrustValue, aftContext.ForumUser.TrustLevel, Permissions.HasPerm(ForumInfo.Security.Trust, aftContext.ForumUser.UserRoles));
            result.forum_id = drForum.GetString("ForumId");
            result.forum_name = drForum.GetString("ForumName");
            result.is_closed = Utilities.SafeConvertBool(drForum["IsLocked"]);
            result.topic_id = topicId;
            result.topic_title = drForum.GetString("Subject");
            result.total_post_num = Utilities.SafeConvertInt(drForum["ReplyCount"]) + 1;
            result.is_subscribed = aftContext.UserId > 0 && Utilities.SafeConvertInt(drForum["IsSubscribedTopic"]) > 0;
            result.breadcrumb = breadCrumbs.ToArray();

            result.posts = (from r in dtTopic.Rows.Cast<DataRow>()
                            select new get_thread_response.post()
                            {
                                post_id = r.GetString("ReplyId"),
                                icon_url = Helpers.GetAvatarUrl(r.GetInt("AuthorId")),
                                post_author_name = r.GetString("AuthorName"),
                                post_author_id = r.GetString("AuthorId"),
                                post_content = Helpers.HtmlToTapatalk(r.GetString("Body"), returnHtml),
                                can_edit = bEdit, // TODO: Fix this
                                is_online = r.GetBoolean("IsUserOnline"),
                                post_time = r.GetDateTime("DateCreated").ToUniversalTime(),
                                post_title = r.GetString("Subject"),
                                attachments = (from a in dtAttach.Rows.Cast<DataRow>()
                                               select new  get_thread_response.post.attachment  {
                                                    content_type = a.GetString("ContentType"),
                                                    filename = a.GetString("FileName"),
                                                    filesize = a.GetInt("FileSize"),
                                                 //   url = Helpers.GetAttachmentUrl(index.ToString() + ";" + p.MessageID.ToString(), Context),
                                                  //  thumbnail_url = GetAttachmentThumbnailUrl(value.SavedAsName, p.MemberID, Context)
                                }).ToArray()
                            }).ToArray();
            return result;
        }

        /// <summary>
        /// https://tapatalk.com/api/api_section.php?id=4#get_thread_by_unread
        /// </summary>
        /// <param name="context"></param>
        /// <param name="topicId"></param>
        /// <param name="perPage"></param>
        /// <param name="returnHtml"></param>
        /// <returns></returns>
        public override get_thread_by_unread_response _get_thread_by_unread(ITapatalkModuleContext context, string topicId, int perPage, bool returnHtml)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// https://tapatalk.com/api/api_section.php?id=4#get_thread_by_post
        /// </summary>
        /// <param name="context"></param>
        /// <param name="postId"></param>
        /// <param name="perPage"></param>
        /// <param name="returnHtml"></param>
        /// <returns></returns>
        public override get_thread_by_post_response _get_thread_by_post(ITapatalkModuleContext context, string postId, int perPage, bool returnHtml)
        {
            throw new NotImplementedException();
        }
    }
}

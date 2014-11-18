using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DotNetNuke.Modules.ActiveForums;
using Tapatalk.Extensions;
using Tapatalk.Interfaces;
using Tapatalk_BasePlugin.Interfaces;
using Tapatalk_Plugin.Structures;

namespace ActiveForumsTapatalk.Implementation
{
    public partial class ActiveForumsTapatalk : IForum
    {
        public override get_config_response _get_config(ITapatalkModuleContext context)
        {
            return new get_config_response
            {
                sys_version = "1",
                version = "AFD_0.0.1", 
                is_open =  true, 
                api_level =  "3",
                guest_okay =  true,
                min_search_length =  3,
                subscribe_forum =  "1",
                goto_post =  "0",
                goto_unread = "0",
                announcement = "0",
                no_refresh_on_post =  "1",
                subscribe_load =  "1",
                user_id =  "1",
                avatar =  "0",
                get_latest_topic =  "1",
                mark_read =  "1",
                mark_forum =  "1",
                mark_topic_read =  "1",
                get_forum =  "1",
                guest_search =  "1",
                advanced_search =  "0",
                can_unread =  "1",
                searchid = "0",
                get_participated_forum = "0",
                get_topic_status = "0",
                get_forum_status = "0",
                multi_quote = "0",
                conversation = "0",
                inbox_stat = "0",
                push = "1",
                push_type = "pm,sub,like,quote,newtopic,tag",
                report_post = "0",
                report_pm = "0",
                get_id_by_url =  "0",
                delete_reason = "0",
                m_approve = "0",
                m_delete = "0",
                m_report = "0",
                pm_load = "1",              
                mass_subscribe = "0",
                emoji_support = "0",
                get_smilies = "0",
                advanced_online_users = "0",
                search_user = "0",
                ignore_user = "0",
                //user_recommended = "0",
                mark_pm_unread = "1",
                get_activity = "0",
                alert = "0",
                advanced_delete = "0",
                default_smilies = "0",
                disable_bbcode = "0",
                sso_login = "1",
                sso_signin = "1",
                sso_register = "1",
                inappreg = "1",
                sign_in = "1",
                json_support = "1"
            };
           
        }

        /// <summary>
        /// https://tapatalk.com/api/api_section.php?id=1#get_forum
        /// </summary>
        /// <param name="context"></param>
        /// <param name="returnDescription"></param>
        /// <param name="forumId"></param>
        /// <returns></returns>
        public override get_forum_response[] _get_forum(ITapatalkModuleContext context, bool returnDescription = false, string forumId = null)
        {
            var aftContext = (ActiveForumsTapatalkModuleContext)context;
            var fc = new ForumController();
            var forumIds = fc.GetForumsForUser(aftContext.ForumUser.UserRoles, aftContext.Module.PortalID, aftContext.ModuleId, "CanRead");
            var forumTable = fc.GetForumView(aftContext.Module.PortalID, aftContext.ModuleId, aftContext.UserId, aftContext.ForumUser.IsSuperUser, forumIds);
            //var forumSubscriptions = fc.GetSubscriptionsForUser(aftContext.ModuleSettings.ForumModuleId, aftContext.UserId, null, 0).ToList();

            var result = new List<get_forum_response>();

            // Note that all the fields in the DataTable are strings if they come back from the cache, so they have to be converted appropriately.

            // Get the distict list of groups
            var groups = forumTable.AsEnumerable()
                .Select(r => new
                {
                    ID = Convert.ToInt32(r["ForumGroupId"]),
                    Name = r["GroupName"].ToString(),
                    SortOrder = Convert.ToInt32(r["GroupSort"]),
                    Active = Convert.ToBoolean(r["GroupActive"])
                }).Distinct().Where(o => o.Active).OrderBy(o => o.SortOrder);

            // Get all forums the user can read
            var visibleForums = forumTable.AsEnumerable()
                .Select(f => new
                {
                    ID = Convert.ToInt32(f["ForumId"]),
                    ForumGroupId = Convert.ToInt32(f["ForumGroupId"]),
                    Name = f["ForumName"].ToString(),
                    Description = f["ForumDesc"].ToString(),
                    ParentForumId = Convert.ToInt32(f["ParentForumId"]),
                    ReadRoles = f["CanRead"].ToString(),
                    SubscribeRoles = f["CanSubscribe"].ToString(),
                    LastRead = Convert.ToDateTime(f["LastRead"]),
                    LastPostDate = Convert.ToDateTime(f["LastPostDate"]),
                    SortOrder = Convert.ToInt32(f["ForumSort"]),
                    Active = Convert.ToBoolean(f["ForumActive"])
                })
                .Where(o => o.Active && Permissions.HasPerm(o.ReadRoles, aftContext.ForumUser.UserRoles))
                .OrderBy(o => o.SortOrder).ToList();

            foreach (var group in groups)
            {
                // Find any root level forums for this group
                var groupForums = visibleForums.Where(vf => vf.ParentForumId == 0 && vf.ForumGroupId == group.ID).ToList();

                if (!groupForums.Any())
                    continue;

                // Create the structure to represent the group
                var groupStructure = new get_forum_response()
                {
                    forum_id = "G" + group.ID.ToString(), // Append G to distinguish between forums and groups with the same id.
                    forum_name = group.Name,
                    description = null,
                    parent_id = "-1",
                    logo_url = null,
                    new_post = false,
                    is_protected = false,
                    is_subscribed = false,
                    can_subscribe = false,
                    url = null,
                    sub_only = true,
                };

                // Add the Child Forums
                var groupChildren = new List<get_forum_response>();
                foreach (var groupForum in groupForums)
                {
                    var forumStructure = new get_forum_response
                    {
                        forum_id = groupForum.ID.ToString(),
                        forum_name = Utilities.StripHTMLTag(groupForum.Name),
                        description = returnDescription ? Utilities.StripHTMLTag(groupForum.Description) : string.Empty,
                        parent_id = 'G' + group.ID.ToString(),
                        logo_url = null,
                        new_post = aftContext.UserId > 0 && groupForum.LastPostDate > groupForum.LastRead,
                        is_protected = false,
                        is_subscribed = false, //forumSubscriptions.Any(fs => fs.ForumId == groupForum.ID),
                        can_subscribe = Permissions.HasPerm(groupForum.SubscribeRoles, aftContext.ForumUser.UserRoles),
                        url = null,
                        sub_only = false
                    };

                    // Add any sub-forums

                    var subForums = visibleForums.Where(vf => vf.ParentForumId == groupForum.ID).ToList();

                    if (subForums.Any())
                    {
                        var forumChildren = new List<get_forum_response>();

                        foreach (var subForum in subForums)
                        {
                            forumChildren.Add(new get_forum_response
                            {
                                forum_id = subForum.ID.ToString(),
                                forum_name = Utilities.StripHTMLTag(subForum.Name),
                                description = returnDescription ? Utilities.StripHTMLTag(subForum.Description) : String.Empty,
                                parent_id = groupForum.ID.ToString(),
                                logo_url = null,
                                new_post = aftContext.UserId > 0 && subForum.LastPostDate > subForum.LastRead,
                                is_protected = false,
                                is_subscribed = false,//forumSubscriptions.Any(fs => fs.ForumId == subForum.ID),
                                can_subscribe = Permissions.HasPerm(subForum.SubscribeRoles, aftContext.ForumUser.UserRoles),
                                url = null,
                                sub_only = false
                            });
                        }

                        forumStructure.child = forumChildren.ToArray();
                    }

                    groupChildren.Add(forumStructure);
                }

                groupStructure.child = groupChildren.ToArray();

                result.Add(groupStructure);
            }

            return result.ToArray();
        }

        /// <summary>
        /// https://tapatalk.com/api/api_section.php?id=1#get_participated_forum
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override get_participated_forum_response _get_participated_forum(ITapatalkModuleContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// https://tapatalk.com/api/api_section.php?id=1#mark_all_as_read
        /// </summary>
        /// <param name="context"></param>
        /// <param name="forumId"></param>
        /// <returns></returns>
        public override mark_all_as_read_response _mark_all_as_read(ITapatalkModuleContext context, string forumId = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// https://tapatalk.com/api/api_section.php?id=1#login_forum
        /// </summary>
        /// <param name="context"></param>
        /// <param name="forumId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public override login_forum_response _login_forum(ITapatalkModuleContext context, string forumId, string password)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// https://tapatalk.com/api/api_section.php?id=1#get_id_by_url
        /// </summary>
        /// <param name="context"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public override get_id_by_url_response _get_id_by_url(ITapatalkModuleContext context, string url)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// https://tapatalk.com/api/api_section.php?id=1#get_board_stat
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override get_board_stat_response _get_board_stat(ITapatalkModuleContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///  https://tapatalk.com/api/api_section.php?id=1#get_forum_status
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override get_forum_status_response _get_forum_status(ITapatalkModuleContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// https://tapatalk.com/api/api_section.php?id=1#get_smilies
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override get_smilies_response _get_smilies(ITapatalkModuleContext context)
        {
            throw new NotImplementedException();
        }
        
        #region Helper Methods
        
        #endregion

    }
}

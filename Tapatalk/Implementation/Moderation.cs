using System;
using System.Collections.Generic;
using Tapatalk.Classes;
using Tapatalk.Interfaces;
using Tapatalk.XmlRpc;
using Tapatalk_BasePlugin.Interfaces;
using Tapatalk_Plugin.Structures;

namespace ActiveForumsTapatalk.Implementation
{
    public partial class ActiveForumsTapatalk : IModeration
    {

        public override login_mod_response _login_mod(ITapatalkModuleContext context, string username, string password)
        {
            throw new NotImplementedException();
        }

        public override m_stick_topic_response _m_stick_topic(ITapatalkModuleContext context, string topicId, int mode)
        {
            throw new NotImplementedException();
        }

        public override m_close_topic_response _m_close_topic(ITapatalkModuleContext context, string topicId, int mode)
        {
            throw new NotImplementedException();
        }

        public override m_delete_topic_response _m_delete_topic(ITapatalkModuleContext context, string topicId, int mode, string reason)
        {
            throw new NotImplementedException();
        }

        public override m_delete_post_response _m_delete_post(ITapatalkModuleContext context, string postId, int mode, string reason)
        {
            throw new NotImplementedException();
        }

        public override m_undelete_topic_response _m_undelete_topic(ITapatalkModuleContext context, string topicId, string reason)
        {
            throw new NotImplementedException();
        }

        public override m_undelete_post_response _m_undelete_post(ITapatalkModuleContext context, string postId, string reason)
        {
            throw new NotImplementedException();
        }

        public override m_move_topic_response _m_move_topic(ITapatalkModuleContext context, string topicId, string forumId, bool redirect)
        {
            throw new NotImplementedException();
        }

        public override m_rename_topic_response _m_rename_topic(ITapatalkModuleContext context, string topicId, string title, string prefixId)
        {
            throw new NotImplementedException();
        }

        public override m_move_post_response _m_move_post(ITapatalkModuleContext context, string postId, string topicId, string topicTitle, string forumId)
        {
            throw new NotImplementedException();
        }

        public override m_merge_topic_response _m_merge_topic(ITapatalkModuleContext context, string topicId1, string topicId2, bool redirect)
        {
            throw new NotImplementedException();
        }

        public override m_get_moderate_topic_response _m_get_moderate_topic(ITapatalkModuleContext context, int page, int perPage)
        {
            throw new NotImplementedException();
        }

        public override m_get_moderate_post_response _m_get_moderate_post(ITapatalkModuleContext context, int page, int perPage)
        {
            throw new NotImplementedException();
        }

        public override m_approve_topic_response _m_approve_topic(ITapatalkModuleContext context, string topicId, int mode)
        {
            throw new NotImplementedException();
        }

        public override m_approve_post_response _m_approve_post(ITapatalkModuleContext context, string postId, int mode)
        {
            throw new NotImplementedException();
        }

        public override m_ban_user_response _m_ban_user(ITapatalkModuleContext context, string username, int mode, string reason, int expires)
        {
            throw new NotImplementedException();
        }

        public override m_get_delete_topic_response _m_get_delete_topic(ITapatalkModuleContext context, int page, int perPage)
        {
            throw new NotImplementedException();
        }

        public override m_get_delete_post_response _m_get_delete_post(ITapatalkModuleContext context, int page, int perPage)
        {
            throw new NotImplementedException();
        }

        public override m_get_report_post_response _m_get_report_post(ITapatalkModuleContext context, int page, int perPage)
        {
            throw new NotImplementedException();
        }

        public override m_mark_as_spam_response _m_mark_as_spam(ITapatalkModuleContext context, string userId)
        {
            throw new NotImplementedException();
        }

        public override m_merge_post_response _m_merge_post(ITapatalkModuleContext context, List<string> postIds, string postId)
        {
            throw new NotImplementedException();
        }

        public override m_unban_user_response _m_unban_user(ITapatalkModuleContext context, string userId)
        {
            throw new NotImplementedException();
        }

        public override m_close_report_response _m_close_report(ITapatalkModuleContext context, string reportId)
        {
            throw new NotImplementedException();
        }
    }
}

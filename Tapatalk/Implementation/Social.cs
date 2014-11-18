using System;
using Tapatalk.Classes;
using Tapatalk.Interfaces;
using Tapatalk.XmlRpc;
using Tapatalk_BasePlugin.Interfaces;
using Tapatalk_Plugin.Structures;

namespace ActiveForumsTapatalk.Implementation
{
    public partial class ActiveForumsTapatalk : ISocial
    {

        public override thank_post_response _thank_post(ITapatalkModuleContext context, string postId)
        {
            throw new NotImplementedException();
        }

        public override follow_response _follow(ITapatalkModuleContext context, string userId)
        {
            throw new NotImplementedException();
        }

        public override unfollow_response _unfollow(ITapatalkModuleContext context, string userId)
        {
            throw new NotImplementedException();
        }

        public override like_post_response _like_post(ITapatalkModuleContext context, string postId)
        {
            throw new NotImplementedException();
        }

        public override unlike_post_response _unlike_post(ITapatalkModuleContext context, string postId)
        {
            throw new NotImplementedException();
        }

        public override get_following_response _get_following(ITapatalkModuleContext context)
        {
            throw new NotImplementedException();
        }

        public override get_follower_response _get_follower(ITapatalkModuleContext context)
        {
            throw new NotImplementedException();
        }

        public override set_reputation_response _set_reputation(ITapatalkModuleContext context, string userId, string mode)
        {
            throw new NotImplementedException();
        }

        public override get_alert_response _get_alert(ITapatalkModuleContext context, int page, int perpage)
        {
            throw new NotImplementedException();
        }

        public override get_activity_response _get_activity(ITapatalkModuleContext context, int page, int perpage)
        {
            throw new NotImplementedException();
        }
    }
}

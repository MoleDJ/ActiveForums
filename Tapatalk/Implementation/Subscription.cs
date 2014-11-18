using System;
using Tapatalk.Interfaces;
using Tapatalk_BasePlugin.Interfaces;
using Tapatalk_Plugin.Structures;

namespace ActiveForumsTapatalk.Implementation
{
    public partial class ActiveForumsTapatalk : ISubscription
    {
        /// <summary>
        /// https://tapatalk.com/api/api_section.php?id=6#get_subscribed_forum
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override get_subscribed_forum_response _get_subscribed_forum(ITapatalkModuleContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// https://tapatalk.com/api/api_section.php?id=6#subscribe_forum
        /// </summary>
        /// <param name="context"></param>
        /// <param name="forum_id"></param>
        /// <returns></returns>
        public override subscribe_forum_response _subscribe_forum(ITapatalkModuleContext context, string forumId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// https://tapatalk.com/api/api_section.php?id=6#unsubscribe_forum
        /// </summary>
        /// <param name="context"></param>
        /// <param name="forum_id"></param>
        /// <returns></returns>
        public override unsubscribe_forum_response _unsubscribe_forum(ITapatalkModuleContext context, string forumId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// https://tapatalk.com/api/api_section.php?id=6#get_subscribed_topic
        /// </summary>
        /// <param name="context"></param>
        /// <param name="start_num"></param>
        /// <param name="last_num"></param>
        /// <returns></returns>
        public override get_subscribed_topic_response _get_subscribed_topic(ITapatalkModuleContext context, int page, int perPage)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// https://tapatalk.com/api/api_section.php?id=6#subscribe_topic
        /// </summary>
        /// <param name="context"></param>
        /// <param name="topic_id"></param>
        /// <returns></returns>
        public override subscribe_topic_response _subscribe_topic(ITapatalkModuleContext context, string topicId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// https://tapatalk.com/api/api_section.php?id=6#unsubscribe_topic
        /// </summary>
        /// <param name="context"></param>
        /// <param name="topic_id"></param>
        /// <returns></returns>
        public override unsubscribe_topic_response _unsubscribe_topic(ITapatalkModuleContext context, string topicId)
        {
            throw new NotImplementedException();
        }
    }
}

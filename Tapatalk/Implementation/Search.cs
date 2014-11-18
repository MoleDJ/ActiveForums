using System;
using System.Collections.Generic;
using System.Linq;
using Tapatalk.Interfaces;
using Tapatalk_BasePlugin.Interfaces;
using Tapatalk_Plugin.Structures;

namespace ActiveForumsTapatalk.Implementation
{
    public partial class ActiveForumsTapatalk : ISearch
    {
        /// <summary>
        /// https://tapatalk.com/api/api_section.php?id=5#search_topic
        /// </summary>
        /// <param name="context"></param>
        /// <param name="searchFilter"></param>
        /// <param name="page"></param>
        /// <param name="perPage"></param>
        /// <param name="searchId"></param>
        /// <returns></returns>
        public override search_topic_response  _search_topic(ITapatalkModuleContext context, string searchFilter, int page, int perPage, string searchId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// https://tapatalk.com/api/api_section.php?id=5#search_post
        /// </summary>
        /// <param name="context"></param>
        /// <param name="searchFilter"></param>
        /// <param name="page"></param>
        /// <param name="perPage"></param>
        /// <param name="searchId"></param>
        /// <returns></returns>
        public override search_post_response _search_post(ITapatalkModuleContext context, string searchFilter, int page, int perPage, string searchId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// https://tapatalk.com/api/api_section.php?id=5#search
        /// </summary>
        /// <param name="context"></param>
        /// <param name="searchFilter"></param>
        /// <returns></returns>
        public override object _search(ITapatalkModuleContext context, object searchFilter)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace ActiveForumsTapatalk.SmartBanner
{
    public class SmartBanner : TapatalkBase.SmartBanner
    {
        public override string ApiKey
        {
            get { return ""; }
        }
        public override bool BannerEnabled
        {
            get
            {
                return true;
            }
        }
        public override bool WelcomeEnabled
        {
            get
            {
                return true;
            }
        }
        public override string AppLocationUrl
        {
            /*
             App Scheme Rules

            Format:
            scheme://url-to-forum-root/?user_id={user-id}&location={location}&fid={fid}&tid={tid}&pid={pid}&uid={uid}&mid={mid}

            URL:

            scheme: app scheme name, default as 'tapatalk'
            url-to-forum-root: used to search if the forum was in app account/history list. If not, search it in tapatalk/byo App network.
            Params: all params are optional

            user_id: Indicate app should open the content with which account. When there is no account for this forum, open content as guest. When the user_id was not in one of the accounts for this forum, app side decide open with which account.
            location: Valid value: index forum topic post profile message online search login. Default as index.
            fid: Forum board id. Required if location is forum topic post
            tid: Topic id. Required if location is topic post
            pid: Post id. Required if location is post
            uid: User id. Required if location is profile
            mid: PM id or Conversation id. Required if location is message
            page: Page number. Required if location is forumtopicpost
            perpage: Topic/Post number per-page. Required if location is forumtopic
             */
            get
            {
                var currentUri = CurrentHttpContext.Request.Url.AbsoluteUri;

                return currentUri.ToString();
            }
        }

        public override bool IsMobileSkin
        {
            get { return false; }
        }

        public override string PageType
        {
            get
            {
                return "index";
            }
        }
    }
}

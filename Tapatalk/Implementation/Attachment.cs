using System;
using Tapatalk.Interfaces;
using Tapatalk_BasePlugin.Interfaces;
using Tapatalk_Plugin.Structures;

namespace ActiveForumsTapatalk.Implementation
{
    public partial class ActiveForumsTapatalk : IAttachment
    {
        public override upload_response _upload(ITapatalkModuleContext context, string methodName, string forumId, string messageId, string groupId, string type, string filename, byte[] content)
        {
            var apgContext = (ActiveForumsTapatalkModuleContext)context;
            if (methodName == "upload_attach")
            {
                if (string.IsNullOrEmpty(groupId))
                {
                    groupId = Guid.NewGuid().ToString();
                }
            }
            throw new NotImplementedException();
        }
        public override remove_attachment_response _remove_attachment(ITapatalkModuleContext context, string attachmentId, string forumId, string groupId, string postId = null)
        {
            throw new NotImplementedException();
        }
    }
}

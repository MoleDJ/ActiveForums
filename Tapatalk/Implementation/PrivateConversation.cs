using System;
using System.Collections.Generic;
using Tapatalk.Classes;
using Tapatalk.Interfaces;
using Tapatalk.XmlRpc;
using Tapatalk_BasePlugin.Interfaces;
using Tapatalk_Plugin.Structures;

namespace ActiveForumsTapatalk.Implementation
{
    public partial class ActiveForumsTapatalk : IPrivateConversation
    {

        public override new_conversation_response _new_conversation(ITapatalkModuleContext context, List<string> userName, string subject, string body, List<string> attachmentIds, string groupId)
        {
            throw new NotImplementedException();
        }

        public override reply_conversation_response _reply_conversation(ITapatalkModuleContext context, string conversationId, string body, string subject, List<string> attachmentIds, string groupId)
        {
            throw new NotImplementedException();
        }

        public override invite_participant_response _invite_participant(ITapatalkModuleContext context, List<string> userName, string conversationId, string reason)
        {
            throw new NotImplementedException();
        }

        public override get_conversations_response _get_conversations(ITapatalkModuleContext context, int page, int perPage)
        {
            throw new NotImplementedException();
        }

        public override get_conversation_response _get_conversation(ITapatalkModuleContext context, string conversationId, int page, int perPage, bool returnHtml)
        {
            throw new NotImplementedException();
        }

        public override get_quote_conversation_response _get_quote_conversation(ITapatalkModuleContext context, string conversationId, string messageId)
        {
            throw new NotImplementedException();
        }

        public override delete_conversation_response _delete_conversation(ITapatalkModuleContext context, string conversationId, int mode)
        {
            throw new NotImplementedException();
        }

        public override mark_conversation_unread_response _mark_conversation_unread(ITapatalkModuleContext context, string conversationId)
        {
            throw new NotImplementedException();
        }

        public override mark_conversation_read_response _mark_conversation_read(ITapatalkModuleContext context, List<string> conversationIds)
        {
            throw new NotImplementedException();
        }
    }
}

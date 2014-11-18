using System;
using System.Collections.Generic;
using Tapatalk.Interfaces;
using Tapatalk_BasePlugin.Interfaces;
using Tapatalk_Plugin.Structures;

namespace ActiveForumsTapatalk.Implementation
{
    public partial class ActiveForumsTapatalk : IPrivateMessage
    {
        public override report_pm_response _report_pm(ITapatalkModuleContext context, string messageId, string reason)
        {
            throw new NotImplementedException();
        }

        public override create_message_response _create_message(ITapatalkModuleContext context, List<string> username, string subject, string body, int action, string messageId = null)
        {
            throw new NotImplementedException();
        }

        public override get_box_info_response _get_box_info(ITapatalkModuleContext context)
        {
            throw new NotImplementedException();

        }

        public override get_box_response _get_box(ITapatalkModuleContext context, string boxId, int page, int perPage)
        {
            throw new NotImplementedException();
        }

        public override get_message_response _get_message(ITapatalkModuleContext context, string messageId, string boxId, bool returnHtml = false)
        {
            throw new NotImplementedException();
        }

        public override get_quote_pm_response _get_quote_pm(ITapatalkModuleContext context, string messageId)
        {
            throw new NotImplementedException();
        }

        public override delete_message_response _delete_message(ITapatalkModuleContext context, string messageId, string boxId)
        {
            throw new NotImplementedException();
        }

        public override mark_pm_unread_response _mark_pm_unread(ITapatalkModuleContext context, string messageId)
        {
            throw new NotImplementedException();
        }

        public override mark_pm_read_response _mark_pm_read(ITapatalkModuleContext context, List<string> messageIds = null)
        {
            throw new NotImplementedException();
        }
    }
}
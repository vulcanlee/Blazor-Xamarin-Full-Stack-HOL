using System.Collections.Generic;
using Backend.Helpers;
using BAL.Helpers;

namespace Backend.Models
{
    public class MailQueueStatusCondition
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public static void Initialization(List<MailQueueStatusCondition> MailQueueStatusConditions)
        {
            MailQueueStatusConditions.Clear();
            MailQueueStatusConditions.Add(new MailQueueStatusCondition()
            {
                Id = -1,
                Title = "全部",
            });
            MailQueueStatusConditions.Add(new MailQueueStatusCondition()
            {
                Id = MagicHelper.MailStatus等待,
                Title = ToDescriptipnHelper.MailQueueStatusName(MagicHelper.MailStatus等待),
            });
            MailQueueStatusConditions.Add(new MailQueueStatusCondition()
            {
                Id = MagicHelper.MailStatus失敗,
                Title = ToDescriptipnHelper.MailQueueStatusName(MagicHelper.MailStatus失敗),
            });
            MailQueueStatusConditions.Add(new MailQueueStatusCondition()
            {
                Id = MagicHelper.MailStatus成功,
                Title = ToDescriptipnHelper.MailQueueStatusName(MagicHelper.MailStatus成功),
            });
        }

    }
}

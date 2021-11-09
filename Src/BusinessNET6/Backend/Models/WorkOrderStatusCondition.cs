using System.Collections.Generic;
using Backend.Helpers;

namespace Backend.Models
{
    public class WorkOrderStatusCondition
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public static void Initialization(List<WorkOrderStatusCondition> WorkOrderStatusConditions)
        {
            WorkOrderStatusConditions.Clear();
            int id = 0;
            for (id = -1; id <= 4; id++)
            {
                WorkOrderStatusConditions.Add(new WorkOrderStatusCondition()
                {
                    Id = id,
                    Title = id.GetWorkOrderStatus(),
                });
            }
            id = 99;
            WorkOrderStatusConditions.Add(new WorkOrderStatusCondition()
            {
                Id = id,
                Title = id.GetWorkOrderStatus(),
            });
        }

    }
}

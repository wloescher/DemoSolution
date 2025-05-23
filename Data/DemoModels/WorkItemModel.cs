﻿using static DemoModels.Enums;

namespace DemoModels
{
    public class WorkItemModel
    {
        public int WorkItemId { get; set; }
        public Guid WorkItemGuid { get; set; }
        public int ClientId { get; set; }
        public WorkItemType Type { get; set; }
        public WorkItemStatus Status { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string Title { get; set; } = string.Empty;
        public string SubTitle { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = new DateTime(1900, 1, 1);
        public string? CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; } = new DateTime(1900, 1, 1);
        public string? ModifiedBy { get; set; }
    }
}

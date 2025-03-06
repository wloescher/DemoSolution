using System.ComponentModel.DataAnnotations;

namespace DemoModels
{
    public static class Enums
    {
        public enum DataDictionaryGroup
        {
            AuditAction = 1,
            UserType = 2,
            ClientType = 3,
            WorkItemType = 4,
            WorkItemStatus = 5,
        }

        public enum AuditAction
        {
            Create = 1,
            Update = 2,
            Delete = 3,
            Read = 4,
            Login = 5,
            Logout = 6,
            Error = 7,
        }

        public enum UserType
        {
            Admin = 1,
            Sales = 2,
            Marketing = 3,
            Accounting = 4,
            Executive = 5,
            Client = 6,
        }

        public enum ClientType
        {
            Internal = 1,
            External = 2,
            Lead = 3,
        }

        public enum WorkItemType
        {
            Article = 1,
            [Display(Name = "Blog Post")]
            BlogPost = 2,
            Infographic = 3,
            [Display(Name = "eBook")]
            Video = 4,
            EBook = 5,
        }

        public enum WorkItemStatus
        {
            New = 1,
            [Display(Name = "In Planning")]
            InPlanning = 2,
            [Display(Name = "In Progress")]
            InProgress = 3,
            Approved = 4,
            Rejected = 5,
            Publishing = 6,
            Completed = 7,
            [Display(Name = "On Hold")]
            OnHold = 8,
        }
    }
}

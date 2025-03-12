using System.ComponentModel.DataAnnotations;

namespace DemoModels
{
    public static class Enums
    {
        public enum DataDictionaryGroup
        {
            [Display(Name = "Audit Action")]
            AuditAction = 1,

            [Display(Name = "User Type")]
            UserType = 2,

            [Display(Name = "Client Type")]
            ClientType = 3,

            [Display(Name = "Work Item Type")]
            WorkItemType = 4,

            [Display(Name = "Work Item Status")]
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
            Client = 2,
            Sales = 3,
            Marketing = 4,
            Accounting = 5,
            Executive = 6,
            Developer = 7,
        }

        public enum ClientType
        {
            Internal = 1,
            External = 2,
            Lead = 3,
        }

        public enum WorkItemType
        {
            [Display(Name = "User Story")]
            UserStory = 1,

            Task = 2,
            Bug = 3,
            Epic = 4,
            Feature = 5,
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
            Staged = 6,
            Completed = 7,

            [Display(Name = "On Hold")]
            OnHold = 8,
        }
    }
}

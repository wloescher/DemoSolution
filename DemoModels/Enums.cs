using System.ComponentModel.DataAnnotations;

namespace DemoModels
{
    public static class Enums
    {
        public enum AuditAction
        {
            Create,
            Update,
            Delete,
            Read,
            Login,
            Logout,
            Error,
        }

        public enum ClientType { 
            Internal,
            External,
            Lead,
        }

    public enum UserType
        {
            Admin,
		    Sales,
		    Marketing,
		    Accounting,
		    Executive,
            Client,
        }
    }

    public enum WorkItemType
    {
        Article,
        [Display(Name = "Blog Post")]
        BlogPost,
        Video,
        Infographic,
        [Display(Name = "eBook")]
        EBook,
    }

    public enum WorkItemStatus
    {
        New,
        [Display(Name = "In Planning")]
        InPlanning,
        [Display(Name = "In Progress")]
        InProgress,
        Approved,
        Rejected,
        Publishing,
        Completed,
        [Display(Name = "On Hold")]
        OnHold,
    }
}

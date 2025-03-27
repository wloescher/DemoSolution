CREATE VIEW WorkItemUserView AS
	
	SELECT WorkItemUserId
		, WorkItemId = WorkItemUserWorkItemId
		, UserId = WorkItemUserUserId
		, WorkItemTitle
		, UserFullName = UserFirstName + ' ' + UserLastName
		, UserEmailAddress

	FROM WorkItemUser
		LEFT JOIN WorkItem ON WorkItemUserWorkItemId = WorkItemId
			AND WorkItemIsActive = 1
			AND WorkItemIsDeleted = 0
		LEFT JOIN [User] ON WorkItemUserUserId = UserId
			AND UserIsActive = 1
			AND UserIsDeleted = 0
			
	WHERE WorkItemUserIsDeleted = 0;
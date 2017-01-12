ALTER TABLE prod.OrdersNotifications ADD
InsertedDate datetime NOT NULL DEFAULT getdate()
ALTER TABLE prod.OrdersNotificationsEmails ADD
InsertedDate datetime NOT NULL DEFAULT getdate()

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spTimesheet_UpdatePayStatus`(
    IN timesheetId int,
    IN timesheetPayStatus int,
    IN timesheetUpdatedBy varchar(28),
    IN timesheetUpdatedAt datetime
)
BEGIN
	UPDATE `myfinancedb`.`Timesheet`
	SET
		`PayStatus` = timesheetPayStatus,
		`UpdatedBy` = timesheetUpdatedBy,
		`UpdatedAt` = timesheetUpdatedAt
	WHERE `Id` = timesheetId;
END$$
DELIMITER ;

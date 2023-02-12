DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spTimesheet_UpdateStatus`(
    IN timesheetId int,
    IN timesheetIsActive bool,
    IN timesheetUpdatedBy varchar(28),
    IN timesheetUpdatedAt datetime
)
BEGIN
	UPDATE `myfinancedb`.`Timesheet`
	SET
		`IsActive` = timesheetIsActive,
		`UpdatedBy` = timesheetUpdatedBy,
		`UpdatedAt` = timesheetUpdatedAt
	WHERE `Id` = timesheetId;
END$$
DELIMITER ;

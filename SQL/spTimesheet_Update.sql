DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spTimesheet_Update`(
	IN timesheetId int,
    IN timesheetComments varchar(45),
    IN timesheetIsActive bool,
    IN timesheetUpdatedBy varchar(28),
    IN timesheetUpdatedAt datetime
)
BEGIN
	UPDATE `myfinancedb`.`Timesheet`
	SET
		`Comments` = timesheetComments,
		`IsActive` = timesheetIsActive,
		`UpdatedBy` = timesheetUpdatedBy,
		`UpdatedAt` = timesheetUpdatedAt
	WHERE (`Id` = timesheetId);
END$$
DELIMITER ;

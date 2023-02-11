DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spTimesheet_Archive`(
    IN timesheetId int,
    IN timesheetIsArchived bool,
    IN timesheetUpdatedBy varchar(28),
    IN timesheetUpdatedAt datetime
)
BEGIN
	UPDATE `myfinancedb`.`Timesheet`
	SET
		`IsArchived` = timesheetIsArchived,
		`UpdatedBy` = timesheetUpdatedBy,
		`UpdatedAt` = timesheetUpdatedAt
	WHERE `Id` = timesheetId;
END$$
DELIMITER ;

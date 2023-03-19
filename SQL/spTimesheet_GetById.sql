DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spTimesheet_GetById`(
	IN userId varchar(28),
	IN timesheetId int
)
BEGIN
	SELECT
		Id,
		CompanyId,
		TimeIn,
		TimeBreak,
		TimeOut,
		Comments,
		IsActive
	FROM Timesheet
	WHERE UpdatedBy = userId
		AND Id = timesheetId
		AND IsArchived = FALSE;
END$$
DELIMITER ;

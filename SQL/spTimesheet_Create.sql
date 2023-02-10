DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spTimesheet_Create`(
	IN timesheetCompanyId varchar(28),
	IN timesheetTimeIn datetime,
	IN timesheetTimeBreak int(2),
	IN timesheetTimeOut datetime,
	IN timesheetHoursWorked time,
    IN timesheetHourRate decimal(10,2),
    IN timesheetTotalAmount decimal(10,2),
    IN timesheetComments varchar(45),
    IN timesheetUpdatedBy varchar(28),
	IN timesheetCreatedAt datetime,
	IN timesheetUpdatedAt datetime
)
BEGIN
	INSERT INTO `myfinancedb`.`Timesheet` (
		`CompanyId`,
		`TimeIn`,
		`TimeBreak`,
		`TimeOut`,
        `HoursWorked`,
        `HourRate`,
        `TotalAmount`,
        `Comments`,
		`UpdatedBy`,
		`CreatedAt`,
		`UpdatedAt`
	)
	VALUES (
		timesheetCompanyId,
		timesheetTimeIn,
		timesheetTimeBreak,
		timesheetTimeOut,
		timesheetHoursWorked,
		timesheetHourRate,
		timesheetTotalAmount,
        timesheetComments,
        timesheetUpdatedBy,
        timesheetCreatedAt,
        timesheetUpdatedAt
	);
END$$
DELIMITER ;

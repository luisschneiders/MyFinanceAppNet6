DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spTimesheet_GetRecordsByDateRange`(
	IN userId varchar(28),
    IN startDate datetime,
    IN endDate datetime
)
BEGIN
	SELECT
		t.Id,
		c.Description,
		t.TimeIn,
		t.TimeBreak,
		t.TimeOut,
        t.PayStatus,
        t.HourRate,
        t.Comments,
        t.HoursWorked,
        t.TotalAmount,
		t.IsActive
	FROM Timesheet t
	JOIN Company c ON c.Id = t.CompanyId
	WHERE (
		t.UpdatedBy = userId AND
        t.IsArchived = FALSE AND
        (date(t.TimeIn) BETWEEN startDate AND endDate)
	)
	ORDER BY t.TimeIn DESC;
END$$
DELIMITER ;

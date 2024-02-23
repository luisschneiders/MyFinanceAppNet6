DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spTransaction_GetIOByDateRangeGroupByDayAndLabel`(
	IN userId varchar(28),
    IN startDate datetime,
    IN endDate datetime
)
BEGIN
	Select 
		t.Label,
		IFNULL(SUM(t.Amount), 0) as TotalAmount,
		day(t.TDate) AS DayNumber
	FROM myfinancedb.Transaction t
	WHERE t.UpdatedBy = userId
		AND t.IsActive = TRUE
		AND t.IsArchived = FALSE
		AND t.Label <> "T"
		AND (date(t.TDate) BETWEEN startDate AND endDate)
	GROUP BY day(t.TDate), t.Label
	ORDER BY t.Label, DayNumber ASC;
END$$
DELIMITER ;

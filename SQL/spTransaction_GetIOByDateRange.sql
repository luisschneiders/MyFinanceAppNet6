DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spTransaction_GetIOByDateRange`(
	IN userId varchar(28),
	IN startDate datetime,
	IN endDate datetime
)
BEGIN
	SELECT
		t.Label,
		IFNULL(SUM(t.Amount), 0) as TotalAmount,
		month(t.TDate) AS MonthNumber
	FROM Transaction t
	WHERE UpdatedBy = userId
		AND t.IsArchived = FALSE
		AND t.Label <> "T"
		AND (date(t.TDate) BETWEEN startDate AND endDate)
	GROUP BY month(t.TDate), t.Label;
END$$
DELIMITER ;

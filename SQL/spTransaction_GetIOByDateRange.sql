DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spTransaction_GetIOByDateRange`(
	IN userId varchar(28),
    IN startDate datetime,
    IN endDate datetime
)
BEGIN
	SELECT
        t.Label,
		SUM(t.Amount) as TotalAmount,
		month(t.TDate) AS Month
	FROM Transaction t
    WHERE UpdatedBy = userId
		AND t.IsArchived = FALSE
        AND t.Label <> "T"
		AND (date(t.TDate) BETWEEN startDate AND endDate)
	GROUP BY month(t.TDate), t.Label;
END$$
DELIMITER ;

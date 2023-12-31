DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spExpense_GetRecordsSumTop5ByDateRange`(
	IN userId varchar(28),
	IN startDate datetime,
	IN endDate datetime
)
BEGIN
	SELECT
		e.ECategoryId,
		ec.Description AS ECategoryDescription,
        ec.Color AS ECategoryColor,
        SUM(e.Amount) AS TotalAmount
	FROM Expense e
    JOIN ExpenseCategory ec ON ec.Id = e.ECategoryId
	WHERE e.UpdatedBy = userId
		AND (date(e.EDate) >= date(startDate) AND date(e.EDate) <= date(endDate))
		AND e.IsActive = TRUE
		AND e.IsArchived = FALSE
	GROUP BY e.ECategoryId
	ORDER BY TotalAmount DESC
	LIMIT 5;
END$$
DELIMITER ;

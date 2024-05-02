DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spExpense_GetRecordsSumByDateRangeGroupByMonthAndCategory`(
	IN userId varchar(28),
	IN startDate datetime,
	IN endDate datetime
)
BEGIN
	SELECT
		e.ECategoryId,
		ec.Description AS ECategoryDescription,
        ec.Color AS ECategoryColor,
		IFNULL(SUM(e.Amount), 0) as TotalAmount,
		month(e.EDate) AS MonthNumber
	FROM Expense e
    JOIN ExpenseCategory ec ON ec.Id = e.ECategoryId
	WHERE e.UpdatedBy = userId
		AND e.IsActive = TRUE
		AND e.IsArchived = FALSE
		AND (date(e.EDate) BETWEEN startDate AND endDate)
	GROUP BY month(e.EDate), e.ECategoryId
    ORDER BY e.ECategoryId, MonthNumber ASC;
END$$
DELIMITER ;

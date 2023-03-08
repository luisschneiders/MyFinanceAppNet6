DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spExpense_GetRecordsSumLast3Months`(
	IN userId varchar(28)
)
BEGIN
	SELECT
		SUM(Amount) as TotalAmount,
        month(EDate) as MonthNumber
	FROM Expense
	WHERE (EDate >= DATE_SUB(subdate(curdate(), (day(curdate())-1)), INTERVAL 3 MONTH))
		AND UpdatedBy = userId
		AND IsActive = TRUE
		AND IsArchived = FALSE
	GROUP BY month(EDate)
    ORDER BY month(EDate) DESC;
END$$
DELIMITER ;

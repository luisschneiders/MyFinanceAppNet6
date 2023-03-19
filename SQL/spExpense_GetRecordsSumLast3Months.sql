DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spExpense_GetRecordsSumLast3Months`(
	IN userId varchar(28)
)
BEGIN
	SELECT
		SUM(Amount) as TotalAmount,
		month(EDate) as MonthNumber,
		year(EDate) as YearNumber
	FROM Expense
	WHERE 
		EDate BETWEEN DATE_FORMAT(DATE_SUB(CURDATE(), INTERVAL 3 MONTH), '%Y-%m-01') 
		AND LAST_DAY(DATE_SUB(CURDATE(), INTERVAL 1 MONTH))
		AND UpdatedBy = userId
		AND IsActive = TRUE
		AND IsArchived = FALSE
	GROUP BY month(EDate), year(EDate);
END$$
DELIMITER ;

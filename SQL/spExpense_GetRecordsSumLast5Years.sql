DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spExpense_GetRecordsSumLast5Years`(
	IN userId varchar(28)
)
BEGIN
	SELECT
		SUM(Amount) as TotalAmount,
        year(EDate) as YearNumber
	FROM Expense
    WHERE
		EDate BETWEEN DATE_FORMAT(DATE_SUB(CURDATE(), INTERVAL 5 YEAR), '%Y-%m-01')
        AND LAST_DAY(DATE_FORMAT(DATE_SUB(CURDATE(), INTERVAL 1 YEAR), '%Y-12-31'))
		AND UpdatedBy = userId
		AND IsActive = TRUE
		AND IsArchived = FALSE
	GROUP BY year(EDate);
END$$
DELIMITER ;

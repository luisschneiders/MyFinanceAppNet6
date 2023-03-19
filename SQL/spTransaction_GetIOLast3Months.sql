DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spTransaction_GetIOLast3Months`(
	IN userId varchar(28)
)
BEGIN
	SELECT
		t.Label,
		IFNULL(SUM(t.Amount), 0) as TotalAmount,
		month(t.TDate) AS MonthNumber,
		year(t.TDate) as YearNumber
	FROM Transaction t
	WHERE 
		TDate BETWEEN DATE_FORMAT(DATE_SUB(CURDATE(), INTERVAL 3 MONTH), '%Y-%m-01') 
		AND LAST_DAY(DATE_SUB(CURDATE(), INTERVAL 1 MONTH))
		AND UpdatedBy = userId
		AND IsActive = TRUE
		AND t.IsArchived = FALSE
		AND t.Label <> "T"
	GROUP BY month(t.TDate), year(t.TDate), t.Label;
END$$
DELIMITER ;

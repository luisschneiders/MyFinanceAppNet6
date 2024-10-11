DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spExpense_GetRecordsByDateRange`(
	IN userId varchar(28),
    IN startDate datetime,
    IN endDate datetime
)
BEGIN
	SELECT
		e.Id,
		e.EDate,
        e.BankId,
        e.ECategoryId,
		b.Description AS BankDescription,
		ec.Description AS ExpenseCategoryDescription,
        ec.Color AS ExpenseCategoryColor,
		e.Comments,
		CASE
			WHEN e.TaxCategoryId <> 0 THEN (e.Amount * taxc.Rate / (100 + taxc.Rate))
			ELSE 0
		END AS TaxAmount,
		e.Amount,
		e.IsActive
	FROM Expense e
	LEFT JOIN Bank b ON b.Id = e.BankId
    LEFT JOIN ExpenseCategory ec ON ec.Id = e.ECategoryId
	LEFT JOIN TaxCategory taxc ON taxc.Id = e.TaxCategoryId
	WHERE e.UpdatedBy = userId
		AND (date(e.EDate) >= date(startDate) AND date(e.EDate) <= date(endDate))
        AND e.IsActive = TRUE
		AND e.IsArchived = FALSE
	ORDER BY e.EDate DESC;
END$$
DELIMITER ;

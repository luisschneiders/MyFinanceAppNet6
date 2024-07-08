DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spExpense_GetAmountHistory`(
	IN userId varchar(28)
)
BEGIN
	SELECT 
	  e.ECategoryId,
      ec.Description,
	  SUM(e.Amount) AS Total,
	  MAX(e.Amount) AS Highest,
	  MIN(e.Amount) AS Lowest
	FROM Expense e
    JOIN ExpenseCategory ec ON ec.Id = e.ECategoryId
	WHERE e.UpdatedBy = userId
		AND ec.IsArchived = FALSE
        AND e.IsArchived = FALSE
	GROUP BY ECategoryId;
END$$
DELIMITER ;

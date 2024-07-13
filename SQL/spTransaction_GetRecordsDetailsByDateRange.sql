DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spTransaction_GetRecordsDetailsByDateRange`(
	IN userId varchar(28),
    IN startDate datetime,
    IN endDate datetime
)
BEGIN
	SELECT
		t.Id,
		t.TDate,
		b.Description AS BankDescription,
		CASE 
			WHEN t.TCategoryId = 0 THEN 'Expenses'
			ELSE tc.Description
		END AS TCategoryDescription,
        t.Label,
        CASE 
            WHEN t.Label = 'D' THEN '220, 53, 69'
            WHEN t.Label = 'T' THEN '13, 202, 240'
            WHEN t.Label = 'C' THEN '25, 135, 84'
            ELSE 'undefined'
        END AS TCategoryColor,
		t.Comments,
		t.Amount
	FROM Transaction t
	JOIN Bank b ON b.Id = t.FromBank
    LEFT JOIN TransactionCategory tc ON tc.Id = t.TCategoryId
	WHERE t.UpdatedBy = userId
		AND (date(t.TDate) >= date(startDate) AND date(t.TDate) <= date(endDate))
        AND (t.Action <> "D" OR t.Label <> "T")
        AND t.IsActive = TRUE
		AND t.IsArchived = FALSE
	ORDER BY t.TDate DESC;
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spTransaction_GetRecordsByDateRange`(
	IN userId varchar(28),
    IN startDate datetime,
    IN endDate datetime
)
BEGIN
	SELECT
		t.Id,
        t.Link,
		t.TDate,
		b.Description AS BankDescription,
		tc.Description AS TCategoryTypeDescription,
		t.Action,
        t.Label,
		t.Comments,
		t.Amount,
		t.IsActive
	FROM Transaction t
	JOIN Bank b ON b.Id = t.FromBank
    JOIN TransactionCategory tc ON tc.Id = t.TCategoryType
	WHERE t.UpdatedBy = userId
		AND (date(t.TDate) >= date(startDate) AND date(t.TDate) <= date(endDate))
        AND (t.Action <> "D" OR t.Label <> "T")
        AND t.IsActive = TRUE
		AND t.IsArchived = FALSE
	ORDER BY t.TDate DESC;
END$$
DELIMITER ;

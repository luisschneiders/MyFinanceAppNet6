DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spBankTransactionHistory_GetRecordsByDateRange`(
	IN userId varchar(28),
    IN bankId int,
	IN startDate datetime,
    IN endDate datetime
)
BEGIN
	SELECT
		bt.BDate,
		b.Description AS BankDescription,
        CASE
            WHEN bt.Action = 'D' THEN 'Debit'
            WHEN bt.Action = 'C' THEN 'Credit'
            ELSE 'undefined'
        END AS ActionDescription,
		bt.PreviousBalance,
		bt.Amount,
		bt.CurrentBalance,
		bt.CreatedAt,
		bt.UpdatedAt
	FROM BankTransactionHistory bt
    LEFT JOIN Bank b ON b.Id = bt.BankId
	WHERE bt.BankId = bankId
		AND bt.UpdatedBy = userId
		AND (date(bt.BDate) >= date(startDate) AND date(bt.BDate) <= date(endDate))
	ORDER BY bt.BDate DESC, bt.CreatedAt DESC;
END$$
DELIMITER ;

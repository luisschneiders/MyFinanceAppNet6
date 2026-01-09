DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spBankTransactionHistory_GetRecordsByBank_LoadMore`(
	IN userId VARCHAR(28),
    IN bankId INT,
    IN lastId INT,			-- NULL for first load
    IN loadMore INT
)
BEGIN
	SELECT
		bt.Id,
		bt.BDate,
		b.Description AS BankDescription,
        ec.Description AS ExpenseDescription,
        bt.Action,
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
    LEFT JOIN Expense e ON e.TransactionId = bt.TransactionId
	LEFT JOIN ExpenseCategory ec ON ec.Id = e.ECategoryId
	WHERE bt.BankId = bankId
      AND bt.UpdatedBy = userId
      AND (
            lastId IS NULL
            OR bt.Id < lastId
          )
    ORDER BY bt.Id DESC, bt.BDate DESC, bt.CreatedAt DESC
    LIMIT loadMore;    
END$$
DELIMITER ;

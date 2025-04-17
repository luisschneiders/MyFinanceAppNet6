DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spExpense_Archive`(
	IN expenseId int,
    IN expenseBankId int,
    IN expenseTransactionId int,
    IN expenseAmount decimal(10,2),
	IN expenseIsActive bool,
    IN expenseIsArchived bool,
    IN expenseUpdatedBy varchar(28),
    IN expenseUpdatedAt datetime
)
BEGIN
	DECLARE rowCountTransaction int default 0;
	DECLARE rowCountBank int default 0;
    DECLARE rowCountExpense int default 0;
    DECLARE varCurrentBalance decimal(10,2) default 0;
    DECLARE locationExpenseExists int;
    DECLARE expenseLocationId int default 0;
    
    START TRANSACTION;
    
	SELECT
		CurrentBalance
	INTO varCurrentBalance
	FROM Bank
	WHERE Id = expenseBankId;

	UPDATE `myfinancedb`.`Bank`
	SET
		`CurrentBalance` = (varCurrentBalance + expenseAmount),
		`UpdatedBy` = expenseUpdatedBy,
		`UpdatedAt` = expenseUpdatedAt
	WHERE (`Id` = expenseBankId);
	SET rowCountBank = ROW_COUNT();
    
	UPDATE `myfinancedb`.`Transaction`
	SET
		`IsActive` = expenseIsActive,
		`IsArchived` = expenseIsArchived,
		`UpdatedBy` = expenseUpdatedBy,
		`UpdatedAt` = expenseUpdatedAt
	WHERE `Id` = expenseTransactionId;
	SET rowCountTransaction = ROW_COUNT();
        
	UPDATE `myfinancedb`.`Expense`
	SET
		`IsActive` = expenseIsActive,
		`IsArchived` = expenseIsArchived,
		`UpdatedBy` = expenseUpdatedBy,
		`UpdatedAt` = expenseUpdatedAt
	WHERE `Id` = expenseId;
	SET rowCountExpense = ROW_COUNT(); 
    
    SELECT COUNT(ExpenseId) FROM LocationExpense
    WHERE ExpenseId = expenseId INTO locationExpenseExists;
    
    IF (locationExpenseExists > 0) THEN
		SET expenseLocationId = expenseId;
		CALL spLocationExpense_Archive(
			expenseLocationId,
            expenseIsActive,
            expenseIsArchived,
            expenseUpdatedBy,
            expenseUpdatedAt);
    END IF;

	IF (rowCountBank > 0 AND
		rowCountTransaction > 0 AND
        rowCountExpense > 0) THEN
        CALL spBankTransactionHistory_GenerateById(expenseUpdatedBy, expenseBankId);
		COMMIT;
	ELSE
		ROLLBACK;
	END IF;
END$$
DELIMITER ;

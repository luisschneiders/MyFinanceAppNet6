DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spExpense_Create`(
	IN expenseEDate datetime,
	IN expenseBankId int,
	IN expenseECategoryId int,
    IN expenseComments varchar(200),
	IN expenseAmount decimal(10,2),
    IN expenseLocationId varchar(300),
    IN expenseLocationAddress varchar(300),
    IN expenseLocationLatitude decimal(8,6),
    IN expenseLocationLongitude decimal (9,6),
	IN expenseUpdatedBy varchar(28),
	IN expenseCreatedAt datetime,
	IN expenseUpdatedAt datetime
)
BEGIN
	DECLARE rowCountBankDebit int default 0;
    DECLARE rowCountTransactionDebit int default 0;
	DECLARE rowCountExpense int default 0;
	DECLARE bankCurrentBalance decimal(10,2) default 0;
    DECLARE transactionTCategoryId int default 0;
    DECLARE transactionAction char(1);
    DECLARE transactionLabel char(1);
    DECLARE lastInsertedTransactionId int default 0;
    DECLARE lastInsertedExpenseId int default 0;

    START TRANSACTION;
    
		SELECT
			CurrentBalance
		INTO bankCurrentBalance
		FROM Bank
		WHERE Id = expenseBankId
        AND IsActive = TRUE
		AND IsArchived = FALSE;
        
		UPDATE `myfinancedb`.`Bank`
		SET
			`CurrentBalance` = (bankCurrentBalance - expenseAmount),
			`UpdatedBy` = expenseUpdatedBy,
			`UpdatedAt` = expenseUpdatedAt
		WHERE (`Id` = expenseBankId);
        SET rowCountBankDebit = ROW_COUNT();

        SET transactionTCategoryId = 0;
        SET transactionAction = "D";
        SET transactionLabel = "D";

		INSERT INTO `myfinancedb`.`Transaction` (
			`TDate`,
			`FromBank`,
			`TCategoryId`,
			`Action`,
			`Label`,
			`Amount`,
			`Comments`,
			`UpdatedBy`,
			`CreatedAt`,
			`UpdatedAt`
		)
		VALUES (
            DATE(expenseEDate),
			expenseBankId,
			transactionTCategoryId,
			transactionAction,
			transactionLabel,
			expenseAmount,
			expenseComments,
			expenseUpdatedBy,
			expenseCreatedAt,
			expenseUpdatedAt
		);
        SET rowCountTransactionDebit = ROW_COUNT();
		SET lastInsertedTransactionId = LAST_INSERT_ID();
		
        INSERT INTO `myfinancedb`.`Expense` (
			`EDate`,
			`BankId`,
			`ECategoryId`,
			`Comments`,
			`Amount`,
            `TransactionId`,
			`UpdatedBy`,
			`CreatedAt`,
			`UpdatedAt`
		)
		VALUES (
			DATE(expenseEDate),
			expenseBankId,
			expenseECategoryId,
			expenseComments,
			expenseAmount,
            lastInsertedTransactionId,
			expenseUpdatedBy,
			expenseCreatedAt,
			expenseUpdatedAt
		);
        SET rowCountExpense = ROW_COUNT();
        SET lastInsertedExpenseId = LAST_INSERT_ID();
        
        IF (expenseLocationId is not null OR expenseLocationId != '') THEN
			CALL spLocationExpense_Create(
                DATE(expenseEDate),
				lastInsertedExpenseId,
				expenseLocationId,
				expenseLocationAddress,
				expenseLocationLatitude,
				expenseLocationLongitude,
				expenseUpdatedBy,
				expenseCreatedAt,
				expenseUpdatedAt
            );
        END IF;

		IF (rowCountBankDebit > 0 AND
			rowCountTransactionDebit > 0 AND 
			rowCountExpense > 0) THEN
			COMMIT;
		ELSE
			ROLLBACK;
		END IF;
END$$
DELIMITER ;

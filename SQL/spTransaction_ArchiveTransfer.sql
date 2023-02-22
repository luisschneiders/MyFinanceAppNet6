DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spTransaction_ArchiveTransfer`(
    IN transactionId int,
    IN transactionIsActive bool,
    IN transactionIsArchived bool,
    IN transactionUpdatedBy varchar(28),
    IN transactionUpdatedAt datetime
)
BEGIN
	DECLARE rowCountTransactionCredit int default 0;
    DECLARE rowCountTransactionDebit int default 0;
	DECLARE rowCountBankCredit int default 0;
    DECLARE rowCountBankDebit int default 0;
    DECLARE varId int default 0;
    DECLARE varLink int default 0;
	DECLARE varFromBankCredit int default 0;
    DECLARE varFromBankDebit int default 0;
    DECLARE varAmount decimal(10,2) default 0;
	DECLARE varUpdatedBy varchar(28);
    DECLARE varCurrentBalance decimal(10,2) default 0;

    START TRANSACTION;
		SELECT 
			Id,
            Link,
            FromBank,
			Amount,
            UpdatedBy
		INTO varId, varLink, varFromBankCredit, varAmount, varUpdatedBy
        FROM Transaction
        WHERE Id = transactionId
        AND UpdatedBy = transactionUpdatedBy;
        
		/* Transaction Credit*/
    	UPDATE `myfinancedb`.`Transaction`
		SET
			`IsActive` = transactionIsActive,
			`IsArchived` = transactionIsArchived,
			`UpdatedBy` = transactionUpdatedBy,
			`UpdatedAt` = transactionUpdatedAt
		WHERE `Id` = transactionId;
        SET rowCountTransactionCredit = ROW_COUNT();

        /* Transaction Debit*/
		UPDATE `myfinancedb`.`Transaction`
		SET
			`IsActive` = transactionIsActive,
			`IsArchived` = transactionIsArchived,
			`UpdatedBy` = transactionUpdatedBy,
			`UpdatedAt` = transactionUpdatedAt
		WHERE `Id` = varLink;
		SET rowCountTransactionDebit = ROW_COUNT();

        /* Bank Credit*/
        IF rowCountTransactionCredit > 0 THEN
			SELECT
				CurrentBalance
			INTO varCurrentBalance
			FROM Bank
			WHERE Id = varFromBankCredit;
        
			UPDATE `myfinancedb`.`Bank`
			SET
				`CurrentBalance` = (varCurrentBalance - varAmount),
				`UpdatedBy` = transactionUpdatedBy,
				`UpdatedAt` = transactionUpdatedAt
			WHERE (`Id` = varFromBankCredit);
            SET rowCountBankCredit = ROW_COUNT();
		END IF;

		/* Bank Debit*/
		IF rowCountTransactionDebit > 0 THEN
			SELECT 
				Id,
				FromBank,
				Amount,
				UpdatedBy
			INTO varId, varFromBankDebit, varAmount, varUpdatedBy
			FROM Transaction
			WHERE Id = varLink
			AND UpdatedBy = transactionUpdatedBy;

			IF rowCountBankCredit > 0 THEN        
				SELECT
					Id,
                    CurrentBalance
				INTO varFromBankCredit, varCurrentBalance
				FROM Bank
                WHERE Id = varFromBankDebit
				AND UpdatedBy = transactionUpdatedBy;
                
				UPDATE `myfinancedb`.`Bank`
				SET
					`CurrentBalance` = (varCurrentBalance + varAmount),
					`UpdatedBy` = transactionUpdatedBy,
					`UpdatedAt` = transactionUpdatedAt
				WHERE (`Id` = varFromBankCredit);
				SET rowCountBankDebit = ROW_COUNT();
			END IF;
		END IF;
            
		IF (rowCountTransactionCredit > 0 AND rowCountTransactionDebit > 0 AND 
			rowCountBankCredit > 0 AND rowCountBankDebit > 0) THEN
			COMMIT;
		ELSE
			ROLLBACK;
		END IF;
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spTransaction_ArchiveTransfer`(
    IN transactionId int,
    IN transactionFromBank int,
    IN transactionLink int,
    IN transactionAmount decimal(10,2),
    IN transactionIsArchived bool,
    IN transactionUpdatedBy varchar(28),
    IN transactionUpdatedAt datetime
)
BEGIN
	DECLARE rowCountTransactionCredit int default 0;
    DECLARE rowCountTransactionDebit int default 0;
	DECLARE rowCountBankCredit int default 0;
    DECLARE rowCountBankDebit int default 0;
    DECLARE fromBank int default 0;

    START TRANSACTION;
		/* Revert Credit Transaction */
    	UPDATE `myfinancedb`.`Transaction`
		SET
			`IsArchived` = transactionIsArchived,
			`UpdatedBy` = transactionUpdatedBy,
			`UpdatedAt` = transactionUpdatedAt
		WHERE `Id` = transactionId;
        SET rowCountTransactionCredit = ROW_COUNT();

        IF rowCountTransactionCredit > 0 THEN
			UPDATE `myfinancedb`.`Bank`
			SET
				`CurrentBalance` = (bankCurrentBalance - transactionAmount),
				`UpdatedBy` = transactionUpdatedBy,
				`UpdatedAt` = transactionUpdatedAt
			WHERE (`Id` = transactionFromBank);
            SET rowCountBankCredit = ROW_COUNT();
		END IF;

		/* Revert Debit Transaction */
		IF rowCountBankCredit > 0 THEN
			UPDATE `myfinancedb`.`Transaction`
			SET
				`IsArchived` = transactionIsArchived,
				`UpdatedBy` = transactionUpdatedBy,
				`UpdatedAt` = transactionUpdatedAt
			WHERE `Id` = transactionLink;
			SET rowCountTransactionDebit = ROW_COUNT();
            
            IF rowCountTransactionDebit > 0 THEN
				SELECT
					FromBank
				INTO fromBank
				FROM Transaction
                WHERE Id = transactionLink;
            
				UPDATE `myfinancedb`.`Bank`
				SET
					`CurrentBalance` = (bankCurrentBalance + transactionAmount),
					`UpdatedBy` = transactionUpdatedBy,
					`UpdatedAt` = transactionUpdatedAt
				WHERE (`Id` = fromBank);
				SET rowCountBankDebit = ROW_COUNT();
			END IF;
		END IF;
            
		IF (rowCountTransactionCredit > 0 AND rowCountTransactionDEBIT > 0 AND 
			rowCountBankCredit > 0 AND rowCountBankDebit > 0) THEN
			COMMIT;
		ELSE
			ROLLBACK;
		END IF;
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spTransaction_ArchiveCredit`(
    IN transactionId int,
    IN transactionIsActive bool,
    IN transactionIsArchived bool,
    IN transactionUpdatedBy varchar(28),
    IN transactionUpdatedAt datetime
)
BEGIN
	DECLARE rowCountTransaction int default 0;
	DECLARE rowCountBank int default 0;
	DECLARE varFromBank int default 0;
    DECLARE varAmount decimal(10,2) default 0;
    DECLARE varCurrentBalance decimal(10,2) default 0;
    
	START TRANSACTION;
		SELECT
            FromBank,
			Amount
		INTO varFromBank, varAmount
        FROM Transaction
        WHERE Id = transactionId
        AND UpdatedBy = transactionUpdatedBy;

	/* Revert Credit Transaction */
		UPDATE `myfinancedb`.`Transaction`
		SET
			`IsActive` = transactionIsActive,
			`IsArchived` = transactionIsArchived,
			`UpdatedBy` = transactionUpdatedBy,
			`UpdatedAt` = transactionUpdatedAt
		WHERE `Id` = transactionId;
        SET rowCountTransaction = ROW_COUNT();
        
		IF rowCountTransaction > 0 THEN
			SELECT
				CurrentBalance
			INTO varCurrentBalance
			FROM Bank
			WHERE Id = varFromBank;

			UPDATE `myfinancedb`.`Bank`
			SET
				`CurrentBalance` = (varCurrentBalance - varAmount),
				`UpdatedBy` = transactionUpdatedBy,
				`UpdatedAt` = transactionUpdatedAt
			WHERE (`Id` = varFromBank);
            SET rowCountBank = ROW_COUNT();
		END IF;
        
        IF (rowCountTransaction > 0 AND rowCountBank > 0) THEN
			COMMIT;
		ELSE
			ROLLBACK;
		END IF;
END$$
DELIMITER ;

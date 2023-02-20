DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spTransaction_ArchiveCredit`(
    IN transactionId int,
    IN transactionFromBank int,
    IN transactionAmount decimal(10,2),
    IN transactionIsArchived bool,
    IN transactionUpdatedBy varchar(28),
    IN transactionUpdatedAt datetime
)
BEGIN
	DECLARE rowCountTransaction int default 0;
	DECLARE rowCountBank int default 0;
    
	START TRANSACTION;
	/* Revert Credit Transaction */
		UPDATE `myfinancedb`.`Transaction`
		SET
			`IsArchived` = transactionIsArchived,
			`UpdatedBy` = transactionUpdatedBy,
			`UpdatedAt` = transactionUpdatedAt
		WHERE `Id` = transactionId;
        SET rowCountTransaction = ROW_COUNT();
        
		IF rowCountTransaction > 0 THEN
			UPDATE `myfinancedb`.`Bank`
			SET
				`CurrentBalance` = (bankCurrentBalance - transactionAmount),
				`UpdatedBy` = transactionUpdatedBy,
				`UpdatedAt` = transactionUpdatedAt
			WHERE (`Id` = transactionFromBank);
            SET rowCountBank = ROW_COUNT();
		END IF;
        
        IF (rowCountTransaction > 0 AND rowCountBank > 0) THEN
			COMMIT;
		ELSE
			ROLLBACK;
		END IF;
END$$
DELIMITER ;

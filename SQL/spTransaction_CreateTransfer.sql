DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spTransaction_CreateTransfer`(
	IN transactionTDate datetime,
	IN transactionFromBank int,
    IN transactionToBank int,
	IN transactionTCategoryId int,
    IN transactionLabel char (1),
	IN transactionAmount decimal(10,2),
    IN transactionComments varchar(200),
	IN transactionUpdatedBy varchar(28),
	IN transactionCreatedAt datetime,
	IN transactionUpdatedAt datetime
)
BEGIN
	DECLARE bankCurrentBalance decimal(10,2) default 0;
    DECLARE transactionAction char(1);
    DECLARE transactionLink int;
    DECLARE lastInsertedId int default 0;
    
    START TRANSACTION;
	/* Debit transaction */
		SET transactionAction = "D";

		SELECT
			CurrentBalance
		INTO bankCurrentBalance
		FROM Bank
		WHERE Id = transactionFromBank
		AND IsActive = TRUE
		AND IsArchived = FALSE;
		
		UPDATE `myfinancedb`.`Bank`
		SET
			`CurrentBalance` = (bankCurrentBalance - transactionAmount),
			`UpdatedBy` = transactionUpdatedBy,
			`UpdatedAt` = transactionUpdatedAt
		WHERE (`Id` = transactionFromBank);
        
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
			transactionTDate,
			transactionFromBank,
			transactionTCategoryId,
			transactionAction,
			transactionLabel,
			transactionAmount,
			transactionComments,
			transactionUpdatedBy,
			transactionCreatedAt,
			transactionUpdatedAt
		);
	/* Get last inserted Id from Transaction */        
        SET lastInsertedId = LAST_INSERT_ID();
    
	/* Credit transaction */
		IF lastInsertedId > 0 THEN
			SET transactionAction = "C";
			SET transactionLink = lastInsertedId;

			SELECT
				CurrentBalance
			INTO bankCurrentBalance
			FROM Bank
			WHERE Id = transactionToBank
			AND IsActive = TRUE
			AND IsArchived = FALSE;
			
			UPDATE `myfinancedb`.`Bank`
			SET
				`CurrentBalance` = (bankCurrentBalance + transactionAmount),
				`UpdatedBy` = transactionUpdatedBy,
				`UpdatedAt` = transactionUpdatedAt
			WHERE (`Id` = transactionToBank);
			
			INSERT INTO `myfinancedb`.`Transaction` (
				`Link`,
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
				transactionLink,
				transactionTDate,
				transactionToBank,
				transactionTCategoryId,
				transactionAction,
				transactionLabel,
				transactionAmount,
				transactionComments,
				transactionUpdatedBy,
				transactionCreatedAt,
				transactionUpdatedAt
			);
		
		COMMIT;
	ELSE
		ROLLBACK;
	END IF;
END$$
DELIMITER ;

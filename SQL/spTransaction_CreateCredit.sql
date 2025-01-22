DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spTransaction_CreateCredit`(
	IN transactionTDate datetime,
	IN transactionFromBank int,
	IN transactionTCategoryId int,
    IN transactionAction char (1),
    IN transactionLabel char (1),
	IN transactionAmount decimal(10,2),
    IN transactionComments varchar(200),
	IN transactionUpdatedBy varchar(28),
	IN transactionCreatedAt datetime,
	IN transactionUpdatedAt datetime
)
BEGIN
    DECLARE bankCurrentBalance decimal(10,2) default 0;

	START TRANSACTION;

		SELECT
			CurrentBalance
		INTO bankCurrentBalance
		FROM Bank
		WHERE Id = transactionFromBank
		AND IsActive = TRUE
		AND IsArchived = FALSE;
		
		UPDATE `myfinancedb`.`Bank`
		SET
			`CurrentBalance` = (bankCurrentBalance + transactionAmount),
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
			Date(transactionTDate),
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
	
    COMMIT;
    
END$$
DELIMITER ;

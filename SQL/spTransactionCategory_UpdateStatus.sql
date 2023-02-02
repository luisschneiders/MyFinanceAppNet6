DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spTransactionCategory_UpdateStatus`(
    IN transactionCategoryId int,
    IN transactionCategoryIsActive bool,
    IN transactionCategoryUpdatedBy varchar(28),
    IN transactionCategoryUpdatedAt datetime
)
BEGIN
	UPDATE `myfinancedb`.`TransactionCategory`
	SET
		`IsActive` = transactionCategoryIsActive,
		`UpdatedBy` = transactionCategoryUpdatedBy,
		`UpdatedAt` = transactionCategoryUpdatedAt
	WHERE `Id` = transactionCategoryId;
END$$
DELIMITER ;

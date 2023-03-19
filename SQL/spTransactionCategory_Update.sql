DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spTransactionCategory_Update`(
	IN transactionCategoryId int,
	IN transactionCategoryDescription varchar(45),
	IN transactionCategoryActionType varchar(1),
	IN transactionCategoryIsActive bool,
	IN transactionCategoryUpdatedBy varchar(28),
	IN transactionCategoryUpdatedAt datetime
)
BEGIN
	UPDATE `myfinancedb`.`TransactionCategory`
	SET
		`Description` = transactionCategoryDescription,
		`ActionType` = transactionCategoryActionType,
		`IsActive` = transactionCategoryIsActive,
		`UpdatedBy` = transactionCategoryUpdatedBy,
		`UpdatedAt` = transactionCategoryUpdatedAt
	WHERE (`Id` = transactionCategoryId);
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spTransactionCategory_Create`(
	IN transactionCategoryDescription varchar(45),
	IN transactionCategoryActionType varchar(1),
	IN transactionCategoryUpdatedBy varchar(28),
	IN transactionCategoryCreatedAt datetime,
	IN transactionCategoryUpdatedAt datetime
)
BEGIN
INSERT INTO `myfinancedb`.`TransactionCategory` (
	`Description`,
	`ActionType`,
	`UpdatedBy`,
	`CreatedAt`,
	`UpdatedAt`
)
VALUES (
	transactionCategoryDescription,
	transactionCategoryActionType,
	transactionCategoryUpdatedBy,
	transactionCategoryCreatedAt,
	transactionCategoryUpdatedAt
);
END$$
DELIMITER ;

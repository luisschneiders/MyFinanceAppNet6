DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spTransactionCategory_Archive`(
    IN transactionCategoryId int,
    IN transactionCategoryIsArchived bool,
    IN transactionCategoryUpdatedBy varchar(28),
    IN transactionCategoryUpdatedAt datetime
)
BEGIN
	UPDATE `myfinancedb`.`TransactionCategory`
	SET
		`IsArchived` = transactionCategoryIsArchived,
		`UpdatedBy` = transactionCategoryUpdatedBy,
		`UpdatedAt` = transactionCategoryUpdatedAt
	WHERE `Id` = transactionCategoryId;
END$$
DELIMITER ;

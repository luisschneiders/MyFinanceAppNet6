DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spExpenseCategory_Archive`(
    IN expenseCategoryId int,
    IN expenseCategoryIsArchived bool,
    IN expenseCategoryUpdatedBy varchar(28),
    IN expenseCategoryUpdatedAt datetime
)
BEGIN
	UPDATE `myfinancedb`.`ExpenseCategory`
	SET
		`IsArchived` = expenseCategoryIsArchived,
		`UpdatedBy` = expenseCategoryUpdatedBy,
		`UpdatedAt` = expenseCategoryUpdatedAt
	WHERE `Id` = expenseCategoryId;
END$$
DELIMITER ;

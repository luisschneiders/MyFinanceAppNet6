DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spExpenseCategory_UpdateStatus`(
    IN expenseCategoryId int,
    IN expenseCategoryIsActive bool,
    IN expenseCategoryUpdatedBy varchar(28),
    IN expenseCategoryUpdatedAt datetime
)
BEGIN
	UPDATE `myfinancedb`.`ExpenseCategory`
	SET
		`IsActive` = expenseCategoryIsActive,
		`UpdatedBy` = expenseCategoryUpdatedBy,
		`UpdatedAt` = expenseCategoryUpdatedAt
	WHERE `Id` = expenseCategoryId;
END$$
DELIMITER ;

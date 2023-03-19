DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spExpenseCategory_Update`(
	IN expenseCategoryId int,
    IN expenseCategoryDescription varchar(45),
    IN expenseCategoryColor varchar(45),
    IN expenseCategoryIsActive bool,
    IN expenseCategoryUpdatedBy varchar(28),
    IN expenseCategoryUpdatedAt datetime
)
BEGIN
	UPDATE `myfinancedb`.`ExpenseCategory`
	SET
		`Description` = expenseCategoryDescription,
        `Color` = expenseCategoryColor,
		`IsActive` = expenseCategoryIsActive,
		`UpdatedBy` = expenseCategoryUpdatedBy,
		`UpdatedAt` = expenseCategoryUpdatedAt
	WHERE (`Id` = expenseCategoryId);
END$$
DELIMITER ;

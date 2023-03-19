DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spExpenseCategory_Create`(
	IN expenseCategoryDescription varchar(45),
    IN expenseCategoryColor varchar(45),
	IN expenseCategoryUpdatedBy varchar(28),
	IN expenseCategoryCreatedAt datetime,
	IN expenseCategoryUpdatedAt datetime
)
BEGIN
	INSERT INTO `myfinancedb`.`ExpenseCategory` (
		`Description`,
        `Color`,
		`UpdatedBy`,
		`CreatedAt`,
		`UpdatedAt`
	)
	VALUES (
		expenseCategoryDescription,
        expenseCategoryColor,
		expenseCategoryUpdatedBy,
		expenseCategoryCreatedAt,
		expenseCategoryUpdatedAt
	);
END$$
DELIMITER ;

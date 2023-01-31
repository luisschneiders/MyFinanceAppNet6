DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spExpense_Create`(
	IN expenseDescription varchar(45),
	IN expenseUpdatedBy varchar(28),
	IN expenseCreatedAt datetime,
	IN expenseUpdatedAt datetime
)
BEGIN
INSERT INTO `myfinancedb`.`Expense` (
	`Description`,
	`UpdatedBy`,
	`CreatedAt`,
	`UpdatedAt`
)
VALUES (
	expenseDescription,
	expenseUpdatedBy,
	expenseCreatedAt,
	expenseUpdatedAt
);
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spExpense_Update`(
	IN expenseId int,
    IN expenseDescription varchar(45),
    IN expenseIsActive bool,
    IN expenseUpdatedBy varchar(28),
    IN expenseUpdatedAt datetime
)
BEGIN
	UPDATE `myfinancedb`.`Expense`
	SET
		`Description` = expenseDescription,
		`IsActive` = expenseIsActive,
		`UpdatedBy` = expenseUpdatedBy,
		`UpdatedAt` = expenseUpdatedAt
	WHERE (`Id` = expenseId);
END$$
DELIMITER ;

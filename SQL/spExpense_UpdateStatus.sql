DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spExpense_UpdateStatus`(
    IN expenseId int,
    IN expenseIsActive bool,
    IN expenseUpdatedBy varchar(28),
    IN expenseUpdatedAt datetime
)
BEGIN
	UPDATE `myfinancedb`.`Expense`
	SET
		`IsActive` = expenseIsActive,
		`UpdatedBy` = expenseUpdatedBy,
		`UpdatedAt` = expenseUpdatedAt
	WHERE `Id` = expenseId;
END$$
DELIMITER ;

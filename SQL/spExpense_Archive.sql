DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spExpense_Archive`(
    IN expenseId int,
    IN expenseIsArchived bool,
    IN expenseUpdatedBy varchar(28),
    IN expenseUpdatedAt datetime
)
BEGIN
	UPDATE `myfinancedb`.`Expense`
	SET
		`IsArchived` = expenseIsArchived,
		`UpdatedBy` = expenseUpdatedBy,
		`UpdatedAt` = expenseUpdatedAt
	WHERE `Id` = expenseId;
END$$
DELIMITER ;

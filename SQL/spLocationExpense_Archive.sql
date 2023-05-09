DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spLocationExpense_Archive`(
	IN expenseLocationId int,
	IN expenseIsActive bool,
    IN expenseIsArchived bool,
    IN expenseUpdatedBy varchar(28),
    IN expenseUpdatedAt datetime
)
BEGIN
	UPDATE `myfinancedb`.`LocationExpense`
	SET
		`IsActive` = expenseIsActive,
		`IsArchived` = expenseIsArchived,
		`UpdatedBy` = expenseUpdatedBy,
		`UpdatedAt` = expenseUpdatedAt
	WHERE (`ExpenseId` = expenseLocationId);
END$$
DELIMITER ;

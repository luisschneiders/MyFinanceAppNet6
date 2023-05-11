DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spLocationExpense_Create`(
	IN expenseEDate datetime,
    IN lastInsertedExpenseId int,
    IN expenseLocationId varchar(300),
    IN expenseLocationAddress varchar(300),
    IN expenseLocationLatitude decimal(8,6),
    IN expenseLocationLongitude decimal (9,6),
	IN expenseUpdatedBy varchar(28),
	IN expenseCreatedAt datetime,
	IN expenseUpdatedAt datetime
)
BEGIN
	INSERT INTO `myfinancedb`.`LocationExpense` (
		`LDate`,
		`ExpenseId`,
		`LocationId`,
		`Address`,
		`Latitude`,
		`Longitude`,
		`UpdatedBy`,
		`CreatedAt`,
		`UpdatedAt`
	)
	VALUES (
		expenseEDate,
		lastInsertedExpenseId,
		expenseLocationId,
		expenseLocationAddress,
		expenseLocationLatitude,
		expenseLocationLongitude,
		expenseUpdatedBy,
		expenseCreatedAt,
		expenseUpdatedAt
	);
END$$
DELIMITER ;

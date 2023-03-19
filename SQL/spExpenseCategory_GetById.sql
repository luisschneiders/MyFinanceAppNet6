DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spExpenseCategory_GetById`(
	IN userId varchar(28),
	IN expenseCategoryId int
)
BEGIN
	SELECT
		Id,
		Description,
		Color,
		IsActive
	FROM ExpenseCategory
	WHERE UpdatedBy = userId
		AND Id = expenseCategoryId
		AND IsArchived = FALSE;
END$$
DELIMITER ;

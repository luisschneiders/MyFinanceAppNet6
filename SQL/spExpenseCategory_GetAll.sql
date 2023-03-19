DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spExpenseCategory_GetAll`(
	IN userId varchar(28)
)
BEGIN
	SELECT
		Id,
        Description,
		IsActive
	FROM ExpenseCategory
	WHERE UpdatedBy = userId
		AND IsArchived = FALSE
	ORDER BY Description ASC;
END$$
DELIMITER ;

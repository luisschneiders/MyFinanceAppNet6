DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spExpenseCategory_GetAllActive`(
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
    AND IsActive = TRUE;
END$$
DELIMITER ;

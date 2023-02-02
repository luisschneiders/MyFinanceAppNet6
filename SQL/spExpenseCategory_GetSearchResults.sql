DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spExpenseCategory_GetSearchResults`(
	IN userId varchar(28),
    IN searchExpenseCategory varchar(45)
)
BEGIN
	SELECT
		Id,
		Description,
		IsActive
	FROM ExpenseCategory
	WHERE (UpdatedBy = userId
		AND IsArchived = FALSE
		AND Description LIKE CONCAT('%', searchExpenseCategory, '%')
	);
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spExpense_GetSearchResults`(
	IN userId varchar(28),
    IN searchExpense varchar(45)
)
BEGIN
SELECT
    Id,
    Description,
    IsActive
FROM Expense
    WHERE (UpdatedBy = userId
		AND IsArchived = FALSE
		AND Description LIKE CONCAT('%', searchExpense, '%')
	);
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spExpense_GetById`(
	IN userId varchar(28),
    IN expenseId int
)
BEGIN
SELECT
	Id,
    Description,
    IsActive
FROM Expense
    WHERE UpdatedBy = userId
    AND Id = expenseId
    AND IsArchived = FALSE;
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spExpense_GetAll`(
	IN userId varchar(28)
)
BEGIN
SELECT Id, Description, IsActive
    FROM Expense
    WHERE UpdatedBy = userId
    AND IsArchived = FALSE;
END$$
DELIMITER ;

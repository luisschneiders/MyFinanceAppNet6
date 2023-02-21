DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spTransactionCategory_GetAllActive`(
	IN userId varchar(28)
)
BEGIN
	SELECT 
		Id,
        Description,
        ActionType,
        IsActive
	FROM TransactionCategory
	WHERE UpdatedBy = userId
	AND IsArchived = FALSE
    AND IsActive = TRUE;
END$$
DELIMITER ;

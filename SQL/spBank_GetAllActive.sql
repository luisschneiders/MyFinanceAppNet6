DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spBank_GetAllActive`(
	IN userId varchar(28)
)
BEGIN
	SELECT 
		Id,
        Description,
        Account,
        InitialBalance,
        CurrentBalance,
        IsActive
	FROM Bank
	WHERE UpdatedBy = userId
		AND IsArchived = FALSE
		AND IsActive = TRUE
	ORDER BY Description ASC;
END$$
DELIMITER ;

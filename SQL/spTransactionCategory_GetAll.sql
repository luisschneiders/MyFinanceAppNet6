DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spTransactionCategory_GetAll`(
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
	ORDER BY Description ASC;
END$$
DELIMITER ;

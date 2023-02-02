DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spTransactionCategory_GetById`(
	IN userId varchar(28),
    IN transactionCategoryId int
)
BEGIN
	SELECT
		Id,
		Description,
		ActionType,
		IsActive
	FROM TransactionCategory
		WHERE UpdatedBy = userId
		AND Id = transactionCategoryId
		AND IsArchived = FALSE;
END$$
DELIMITER ;

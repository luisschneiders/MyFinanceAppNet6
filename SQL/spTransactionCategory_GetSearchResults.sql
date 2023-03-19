DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spTransactionCategory_GetSearchResults`(
	IN userId varchar(28),
	IN searchTransactionCategory varchar(45)
)
BEGIN
	SELECT
		Id,
		Description,
		ActionType,
		IsActive
	FROM TransactionCategory
		WHERE (UpdatedBy = userId
		AND IsArchived = FALSE
		AND (
			Description LIKE CONCAT('%', searchTransactionCategory, '%')
		));
END$$
DELIMITER ;

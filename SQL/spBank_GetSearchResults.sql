DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spBank_GetSearchResults`(
	IN userId varchar(28),
	IN searchBank varchar(45)
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
	WHERE (UpdatedBy = userId
		AND IsArchived = FALSE
		AND (
			Account LIKE CONCAT('%', searchBank, '%')
			OR
			Description LIKE CONCAT('%', searchBank, '%')
		));
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spCompany_GetSearchResults`(
	IN userId varchar(28),
	IN searchCompany varchar(45)
)
BEGIN
	SELECT
		Id,
		Description,
		Rate,
		CType,
		IsActive
	FROM Company
	WHERE (UpdatedBy = userId
		AND IsArchived = FALSE
		AND Description LIKE CONCAT('%', searchCompany, '%'));
END$$
DELIMITER ;

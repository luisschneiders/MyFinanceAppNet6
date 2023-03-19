DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spCompany_GetById`(
	IN userId varchar(28),
	IN companyId int
)
BEGIN
	SELECT
		Id,
		Description,
		Rate,
		CType,
		IsActive
	FROM Company
	WHERE UpdatedBy = userId
		AND Id = companyId
		AND IsArchived = FALSE;
END$$
DELIMITER ;

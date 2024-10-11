DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spTaxCategory_GetAll`(
	IN userId varchar(28)
)
BEGIN
	SELECT
		Id,
		Description,
		Rate,
		IsActive
	FROM TaxCategory
	WHERE UpdatedBy = userId
		AND IsArchived = FALSE
	ORDER BY Description ASC;
END$$
DELIMITER ;

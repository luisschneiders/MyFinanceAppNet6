DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spTaxCategory_GetAllActive`(
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
		AND IsActive = TRUE
	ORDER BY Description ASC;
END$$
DELIMITER ;

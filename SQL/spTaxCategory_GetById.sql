DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spTaxCategory_GetById`(
	IN userId varchar(28),
	IN taxCategoryId int
)
BEGIN
	SELECT
		Id,
		Description,
		Rate,
		IsActive
	FROM TaxCategory
		WHERE UpdatedBy = userId
		AND Id = taxCategoryId
		AND IsArchived = FALSE;
END$$
DELIMITER ;

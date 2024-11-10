DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spTaxCategory_GetSearchResults`(
	IN userId varchar(28),
	IN searchTaxCategory varchar(90)
)
BEGIN
	SELECT
		Id,
		Description,
		Rate,
		IsActive
	FROM TaxCategory
		WHERE (UpdatedBy = userId
		AND IsArchived = FALSE
		AND (
			Description LIKE CONCAT('%', searchTaxCategory, '%')
		));
END$$
DELIMITER ;

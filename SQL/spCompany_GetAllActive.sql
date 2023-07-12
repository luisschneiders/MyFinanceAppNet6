DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spCompany_GetAllActive`(
	IN userId varchar(28)
)
BEGIN
	SELECT 
		Id,
        Description,
        Position,
        Rate,
        CType,
        IsActive
	FROM Company
	WHERE UpdatedBy = userId
		AND IsArchived = FALSE
		AND IsActive = TRUE
	ORDER BY Description ASC;
END$$
DELIMITER ;

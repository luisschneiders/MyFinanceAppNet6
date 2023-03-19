DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spCompany_GetAll`(
	IN userId varchar(28)
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
		AND IsArchived = FALSE
	ORDER BY Description ASC;
END$$
DELIMITER ;

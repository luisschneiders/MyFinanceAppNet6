DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spCompany_GetAllActive`(
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
	AND IsActive = TRUE;
END$$
DELIMITER ;

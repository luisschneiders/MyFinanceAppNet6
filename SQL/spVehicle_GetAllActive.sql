DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spVehicle_GetAllActive`(
	IN userId varchar(28)
)
BEGIN
	SELECT 
		Id,
        Description,
        Year,
        Carmaker,
        Plate,
        IsActive
	FROM Vehicle
	WHERE UpdatedBy = userId
		AND IsArchived = FALSE
		AND IsActive = TRUE
	ORDER BY Description ASC;
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spVehicle_GetAll`(
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
	ORDER BY Description ASC;
END$$
DELIMITER ;

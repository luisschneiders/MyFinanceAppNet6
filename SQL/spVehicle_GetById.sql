DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spVehicle_GetById`(
	IN userId varchar(28),
    IN vehicleId int
)
BEGIN
	SELECT
		Id,
		Description,
        Plate,
		IsActive
	FROM Vehicle
		WHERE UpdatedBy = userId
		AND Id = vehicleId
		AND IsArchived = FALSE;
END$$
DELIMITER ;

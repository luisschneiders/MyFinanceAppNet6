DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spVehicle_GetSearchResults`(
	IN userId varchar(28),
    IN searchVehicle varchar(45)
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
	WHERE (UpdatedBy = userId
		AND IsArchived = FALSE
		AND (
			Description LIKE CONCAT('%', searchVehicle, '%')
			OR
			Plate LIKE CONCAT('%', searchVehicle, '%')
		));
END$$
DELIMITER ;

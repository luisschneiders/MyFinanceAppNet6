DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spVehicle_Update`(
	IN vehicleId int,
    IN vehicleDescription varchar(45),
    IN vehicleYear int,
    IN vehicleCarmaker varchar(45),
    IN vehiclePlate varchar(45),
    IN vehicleIsActive bool,
    IN vehicleUpdatedBy varchar(28),
    IN vehicleUpdatedAt datetime
)
BEGIN
	UPDATE `myfinancedb`.`Vehicle`
	SET
		`Description` = vehicleDescription,
        `Plate` = vehiclePlate,
        `Year` = vehicleYear,
        `Carmaker` = vehicleCarmaker,
		`IsActive` = vehicleIsActive,
		`UpdatedBy` = vehicleUpdatedBy,
		`UpdatedAt` = vehicleUpdatedAt
	WHERE (`Id` = vehicleId);
END$$
DELIMITER ;

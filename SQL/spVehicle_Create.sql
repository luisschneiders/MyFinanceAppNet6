DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spVehicle_Create`(
	IN vehicleDescription varchar(45),
	IN vehicleYear int,
	IN vehicleCarmaker varchar(45),
    IN vehiclePlate varchar(45),
	IN vehicleUpdatedBy varchar(28),
	IN vehicleCreatedAt datetime,
	IN vehicleUpdatedAt datetime
)
BEGIN
	INSERT INTO `myfinancedb`.`Vehicle` (
		`Description`,
        `Year`,
        `Carmaker`,
        `Plate`,
		`UpdatedBy`,
		`CreatedAt`,
		`UpdatedAt`
	)
	VALUES (
		vehicleDescription,
        vehicleYear,
        vehicleCarmaker,
        vehiclePlate,
		vehicleUpdatedBy,
		vehicleCreatedAt,
		vehicleUpdatedAt
	);
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spVehicle_Create`(
	IN vehicleDescription varchar(45),
	IN vehiclePlate varchar(45),
	IN vehicleUpdatedBy varchar(28),
	IN vehicleCreatedAt datetime,
	IN vehicleUpdatedAt datetime
)
BEGIN
	INSERT INTO `myfinancedb`.`Vehicle` (
		`Description`,
		`Plate`,
		`UpdatedBy`,
		`CreatedAt`,
		`UpdatedAt`
	)
	VALUES (
		vehicleDescription,
		vehiclePlate,
		vehicleUpdatedBy,
		vehicleCreatedAt,
		vehicleUpdatedAt
	);
END$$
DELIMITER ;

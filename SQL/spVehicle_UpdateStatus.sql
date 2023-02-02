DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spVehicle_UpdateStatus`(
    IN vehicleId int,
    IN vehicleIsActive bool,
    IN vehicleUpdatedBy varchar(28),
    IN vehicleUpdatedAt datetime
)
BEGIN
	UPDATE `myfinancedb`.`Vehicle`
	SET
		`IsActive` = vehicleIsActive,
		`UpdatedBy` = vehicleUpdatedBy,
		`UpdatedAt` = vehicleUpdatedAt
	WHERE `Id` = vehicleId;
END$$
DELIMITER ;

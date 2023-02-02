DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spVehicle_Archive`(
    IN vehicleId int,
    IN vehicleIsArchived bool,
    IN vehicleUpdatedBy varchar(28),
    IN vehicleUpdatedAt datetime
)
BEGIN
	UPDATE `myfinancedb`.`Vehicle`
	SET
		`IsArchived` = vehicleIsArchived,
		`UpdatedBy` = vehicleUpdatedBy,
		`UpdatedAt` = vehicleUpdatedAt
	WHERE `Id` = vehicleId;
END$$
DELIMITER ;

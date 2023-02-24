DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spTrip_Create`(
	IN tripTDate datetime,
    IN tripVehicleId int,
    IN tripDistance decimal(10,2),
	IN tripUpdatedBy varchar(28),
	IN tripCreatedAt datetime,
	IN tripUpdatedAt datetime
)
BEGIN
	INSERT INTO `myfinancedb`.`Trip` (
		`TDate`,
        `VehicleId`,
        `Distance`,
		`UpdatedBy`,
		`CreatedAt`,
		`UpdatedAt`
	)
	VALUES (
		tripTDate,
        tripVehicleId,
        tripDistance,
		tripUpdatedBy,
		tripCreatedAt,
		tripUpdatedAt
	);
END$$
DELIMITER ;

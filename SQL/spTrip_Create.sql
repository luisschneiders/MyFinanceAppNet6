DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spTrip_Create`(
	IN tripTDate datetime,
    IN tripVehicleId int,
    IN tripStartOdometer decimal(10,2),
    IN tripEndOdometer decimal(10,2),
    IN tripDistance decimal(10,2),
    IN tripPayStatus int,
    IN tripTCategoryId int,
	IN tripUpdatedBy varchar(28),
	IN tripCreatedAt datetime,
	IN tripUpdatedAt datetime
)
BEGIN
	INSERT INTO `myfinancedb`.`Trip` (
		`TDate`,
        `VehicleId`,
        `StartOdometer`,
        `EndOdometer`,
        `Distance`,
        `PayStatus`,
        `TCategoryId`,
		`UpdatedBy`,
		`CreatedAt`,
		`UpdatedAt`
	)
	VALUES (
		tripTDate,
        tripVehicleId,
        tripStartOdometer,
        tripEndOdometer,
        tripDistance,
        tripPayStatus,
        tripTCategoryId,
		tripUpdatedBy,
		tripCreatedAt,
		tripUpdatedAt
	);
END$$
DELIMITER ;

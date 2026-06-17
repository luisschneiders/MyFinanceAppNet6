DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spTrip_Update`(
	IN tripId int,
	IN tripTDate datetime,
    IN tripStartOdometer decimal(10,2),
    IN tripEndOdometer decimal(10,2),
    IN tripDistance decimal(10,2),
    IN tripCPK decimal(10,4),
    IN tripComments varchar(45),
    IN tripPayStatus int,
    IN tripTCategoryId int,
    IN tripUpdatedBy varchar(28),
    IN tripUpdatedAt datetime
)
BEGIN
	UPDATE `myfinancedb`.`Trip`
	SET
		`TDate` = tripTDate,
        `StartOdometer` = tripStartOdometer,
        `EndOdometer` = tripEndOdometer,
        `Distance` = tripDistance,
        `CPK` = tripCPK,
		`Comments` = tripComments,
        `PayStatus` = tripPayStatus,
        `TCategoryId` = tripTCategoryId,
		`UpdatedBy` = tripUpdatedBy,
		`UpdatedAt` = tripUpdatedAt
	WHERE (`Id` = tripId);
END$$
DELIMITER ;

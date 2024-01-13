DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spTrip_UpdatePayStatus`(
    IN tripId int,
    IN tripPayStatus int,
    IN tripUpdatedBy varchar(28),
    IN tripUpdatedAt datetime
)
BEGIN
	UPDATE `myfinancedb`.`Trip`
	SET
		`PayStatus` = tripPayStatus,
		`UpdatedBy` = tripUpdatedBy,
		`UpdatedAt` = tripUpdatedAt
	WHERE `Id` = tripId;
END$$
DELIMITER ;

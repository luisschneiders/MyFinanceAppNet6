DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spTrip_UpdateCategory`(
    IN tripId int,
    IN tripTCategoryId int,
    IN tripUpdatedBy varchar(28),
    IN tripUpdatedAt datetime
)
BEGIN
	UPDATE `myfinancedb`.`Trip`
	SET
		`TCategoryId` = tripTCategoryId,
		`UpdatedBy` = tripUpdatedBy,
		`UpdatedAt` = tripUpdatedAt
	WHERE `Id` = tripId;
END$$
DELIMITER ;

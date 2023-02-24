DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spTrip_Archive`(
    IN tripId int,
	IN tripIsActive bool,
    IN tripIsArchived bool,
    IN tripUpdatedBy varchar(28),
    IN tripUpdatedAt datetime
)
BEGIN
	UPDATE `myfinancedb`.`Trip`
	SET
		`IsActive` = tripIsActive,
		`IsArchived` = tripIsArchived,
		`UpdatedBy` = tripUpdatedBy,
		`UpdatedAt` = tripUpdatedAt
	WHERE `Id` = tripId;
END$$
DELIMITER ;

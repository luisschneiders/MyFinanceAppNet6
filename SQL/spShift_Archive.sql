DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spShift_Archive`(
    IN shiftId int,
    IN shiftIsArchived bool,
    IN shiftUpdatedBy varchar(28),
    IN shiftUpdatedAt datetime
)
BEGIN
	UPDATE `myfinancedb`.`Shift`
	SET
		`IsArchived` = shiftIsArchived,
		`UpdatedBy` = shiftUpdatedBy,
		`UpdatedAt` = shiftUpdatedAt
	WHERE `Id` = shiftId;
END$$
DELIMITER ;

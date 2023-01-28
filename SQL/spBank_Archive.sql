DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spBank_Archive`(
    IN bankId int,
    IN bankIsArchived bool,
    IN bankUpdatedBy varchar(28),
    IN bankUpdatedAt datetime
)
BEGIN
	UPDATE `myfinancedb`.`Bank`
	SET
		`IsArchived` = bankIsArchived,
		`UpdatedBy` = bankUpdatedBy,
		`UpdatedAt` = bankUpdatedAt
	WHERE `Id` = bankId;
END$$
DELIMITER ;

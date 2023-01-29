DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spBank_UpdateStatus`(
    IN bankId int,
    IN bankIsActive bool,
    IN bankUpdatedBy varchar(28),
    IN bankUpdatedAt datetime
)
BEGIN
	UPDATE `myfinancedb`.`Bank`
	SET
		`IsActive` = bankIsActive,
		`UpdatedBy` = bankUpdatedBy,
		`UpdatedAt` = bankUpdatedAt
	WHERE `Id` = bankId;
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spCompany_UpdateStatus`(
    IN companyId int,
    IN companyIsActive bool,
    IN companyUpdatedBy varchar(28),
    IN companyUpdatedAt datetime
)
BEGIN
	UPDATE `myfinancedb`.`Company`
	SET
		`IsActive` = companyIsActive,
		`UpdatedBy` = companyUpdatedBy,
		`UpdatedAt` = companyUpdatedAt
	WHERE `Id` = companyId;
END$$
DELIMITER ;

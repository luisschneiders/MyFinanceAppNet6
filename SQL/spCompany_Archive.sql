DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spCompany_Archive`(
    IN companyId int,
    IN companyIsArchived bool,
    IN companyUpdatedBy varchar(28),
    IN companyUpdatedAt datetime
)
BEGIN
	UPDATE `myfinancedb`.`Company`
	SET
		`IsArchived` = companyIsArchived,
		`UpdatedBy` = companyUpdatedBy,
		`UpdatedAt` = companyUpdatedAt
	WHERE `Id` = companyId;
END$$
DELIMITER ;

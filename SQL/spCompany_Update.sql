DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spCompany_Update`(
	IN companyId int,
	IN companyDescription varchar(45),
	IN companyRate decimal(10,2),
	IN companyCType int,
	IN companyIsActive bool,
	IN companyUpdatedBy varchar(28),
	IN companyUpdatedAt datetime
)
BEGIN
	UPDATE `myfinancedb`.`Company`
	SET
		`Description` = companyDescription,
		`Rate` = companyRate,
		`CType` = companyCType,
		`IsActive` = companyIsActive,
		`UpdatedBy` = companyUpdatedBy,
		`UpdatedAt` = companyUpdatedAt
	WHERE (`Id` = companyId);
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spCompany_Create`(
	IN companyDescription varchar(45),
    IN companyPosition varchar(45),
    IN companyStandardHours int,
	IN companyRate decimal(10,2),
	IN companyCType int,
	IN companyUpdatedBy varchar(28),
	IN companyCreatedAt datetime,
	IN companyUpdatedAt datetime
)
BEGIN
	INSERT INTO `myfinancedb`.`Company` (
		`Description`,
        `Position`,
        `StandardHours`,
        `Rate`,
		`CType`,
		`UpdatedBy`,
		`CreatedAt`,
		`UpdatedAt`
	)
	VALUES (
		companyDescription,
        companyPosition,
        companyStandardHours,
		companyRate,
		companyCType,
		companyUpdatedBy,
		companyCreatedAt,
		companyUpdatedAt
	);
END$$
DELIMITER ;

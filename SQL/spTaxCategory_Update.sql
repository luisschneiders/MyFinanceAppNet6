DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spTaxCategory_Update`(
	IN taxCategoryId int,
	IN taxCategoryDescription varchar(90),
	IN taxCategoryRate decimal(10,2),
	IN taxCategoryIsActive bool,
	IN taxCategoryUpdatedBy varchar(28),
	IN taxCategoryUpdatedAt datetime
)
BEGIN
	UPDATE `myfinancedb`.`TaxCategory`
	SET
		`Description` = taxCategoryDescription,
		`Rate` = taxCategoryRate,
		`IsActive` = taxCategoryIsActive,
		`UpdatedBy` = taxCategoryUpdatedBy,
		`UpdatedAt` = taxCategoryUpdatedAt
	WHERE (`Id` = taxCategoryId);
END$$
DELIMITER ;

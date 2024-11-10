DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spTaxCategory_UpdateStatus`(
    IN taxCategoryId int,
    IN taxCategoryIsActive bool,
    IN taxCategoryUpdatedBy varchar(28),
    IN taxCategoryUpdatedAt datetime
)
BEGIN
	UPDATE `myfinancedb`.`TaxCategory`
	SET
		`IsActive` = taxCategoryIsActive,
		`UpdatedBy` = taxCategoryUpdatedBy,
		`UpdatedAt` = taxCategoryUpdatedAt
	WHERE `Id` = taxCategoryId;
END$$
DELIMITER ;

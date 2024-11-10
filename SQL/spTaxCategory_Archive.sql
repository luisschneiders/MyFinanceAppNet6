DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spTaxCategory_Archive`(
    IN taxCategoryId int,
    IN taxCategoryIsArchived bool,
    IN taxCategoryUpdatedBy varchar(28),
    IN taxCategoryUpdatedAt datetime
)
BEGIN
	UPDATE `myfinancedb`.`TaxCategory`
	SET
		`IsArchived` = taxCategoryIsArchived,
		`UpdatedBy` = taxCategoryUpdatedBy,
		`UpdatedAt` = taxCategoryUpdatedAt
	WHERE `Id` = taxCategoryId;
END$$
DELIMITER ;

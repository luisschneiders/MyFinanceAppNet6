DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spTaxCategory_Create`(
	IN taxCategoryDescription varchar(90),
	IN taxCategoryRate decimal(10,2),
	IN taxCategoryUpdatedBy varchar(28),
	IN taxCategoryCreatedAt datetime,
	IN taxCategoryUpdatedAt datetime
)
BEGIN
INSERT INTO `myfinancedb`.`TaxCategory` (
	`Description`,
	`Rate`,
	`UpdatedBy`,
	`CreatedAt`,
	`UpdatedAt`
)
VALUES (
	taxCategoryDescription,
	taxCategoryRate,
	taxCategoryUpdatedBy,
	taxCategoryCreatedAt,
	taxCategoryUpdatedAt
);
END$$
DELIMITER ;

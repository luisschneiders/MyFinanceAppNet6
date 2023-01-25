DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spBank_Create`(
	IN bankAccount varchar(45),
	IN bankDescription varchar(45),
	IN bankInitialBalance decimal(10,2),
	IN bankCurrentBalance decimal(10,2),
	IN bankUpdatedBy varchar(28),
	IN bankCreatedAt datetime,
	IN bankUpdatedAt datetime
)
BEGIN

INSERT INTO `myfinancedb`.`Bank` (
	`Account`,
	`Description`,
	`InitialBalance`,
	`CurrentBalance`,
	`UpdatedBy`,
	`CreatedAt`,
	`UpdatedAt`
)
VALUES (
	bankAccount,
	bankDescription,
	bankInitialBalance,
	bankCurrentBalance,
	bankUpdatedBy,
	bankCreatedAt,
	bankUpdatedAt
);

END$$
DELIMITER ;

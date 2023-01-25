DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spBank_Create`(
	IN Account varchar(45),
	IN Description varchar(45),
	IN InitialBalance decimal(10,2),
	IN CurrentBalance decimal(10,2),
	IN UpdatedBy varchar(28),
	IN CreatedAt datetime,
	IN UpdatedAt datetime
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
	Account,
	Description,
	InitialBalance,
	CurrentBalance,
	UpdatedBy,
	CreatedAt,
	UpdatedAt
);

SET @LastInsertedId = LAST_INSERT_ID();
SELECT @LastInsertedId;

END$$
DELIMITER ;

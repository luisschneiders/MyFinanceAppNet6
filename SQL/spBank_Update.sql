DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spBank_Update`(
	IN bankId int,
    IN bankAccount varchar(45),
    IN bankDescription varchar(45),
    IN bankCurrentBalance decimal(10,2),
    IN bankIsActive bool,
    IN bankUpdatedBy varchar(28),
    IN bankUpdatedAt datetime
)
BEGIN
	UPDATE `myfinancedb`.`Bank`
	SET
		`Account` = bankAccount,
		`Description` = bankDescription,
		`CurrentBalance` = bankCurrentBalance,
		`IsActive` = bankIsActive,
		`UpdatedBy` = bankUpdatedBy,
		`UpdatedAt` = bankUpdatedAt
	WHERE `Id` = bankId;
END$$
DELIMITER ;

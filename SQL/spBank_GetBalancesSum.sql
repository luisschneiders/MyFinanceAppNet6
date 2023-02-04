DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spBank_GetBalancesSum`(
	IN userId varchar(28)
)
BEGIN
	SELECT 
		SUM(InitialBalance) BankTotalInitialBalance,
		SUM(CurrentBalance) BankTotalCurrentBalance
	FROM Bank
	WHERE UpdatedBy = userId
	AND IsArchived = FALSE;
END$$
DELIMITER ;

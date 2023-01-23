DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spBank_GetAllBanksByUserId`(IN userId INT)
BEGIN
SELECT Id, Description, Account, InitialBalance, CurrentBalance, IsActive
    FROM Bank
    WHERE UpdatedBy = userId;
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spBank_GetAllBanks`(IN userId varchar(28))
BEGIN
SELECT Id, Description, Account, InitialBalance, CurrentBalance, IsActive
    FROM Bank
    WHERE UpdatedBy = userId;
END$$
DELIMITER ;

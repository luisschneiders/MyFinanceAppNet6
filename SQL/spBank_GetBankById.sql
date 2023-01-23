DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spBank_GetBankById`(IN userId varchar(28), bankId int)
BEGIN
SELECT Id, Description, Account, InitialBalance, CurrentBalance, IsActive
    FROM Bank
    WHERE UpdatedBy = userId
    AND Id = bankId
    AND IsArchived = FALSE;
END$$
DELIMITER ;

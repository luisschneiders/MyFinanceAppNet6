DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spBank_GetById`(
	IN userId varchar(28),
    IN bankId int)
BEGIN
SELECT
    Id,
    Description,
    Account,
    InitialBalance,
    CurrentBalance,
    IsActive
FROM Bank
    WHERE UpdatedBy = userId
    AND Id = bankId
    AND IsArchived = FALSE;
END$$
DELIMITER ;

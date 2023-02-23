DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spTransaction_GetById`(
	IN userId varchar(28),
    IN transactionId int
)
BEGIN
	SELECT
		Id,
		FromBank,
		Link,
		Action,
		Label,
        Amount
	FROM Transaction
	WHERE UpdatedBy = userId
	AND Id = transactionId
	AND IsArchived = FALSE;
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spLocationExpense_GetRecordsByDateRange`(
	IN userId varchar(28),
    IN startDate datetime,
    IN endDate datetime
)
BEGIN
	SELECT DISTINCT
        LocationId,
        Address,
        Latitude,
        Longitude
	FROM LocationExpense
	WHERE UpdatedBy = userId
	AND (date(LDate) >= date(startDate) AND date(LDate) <= date(endDate))
	AND IsActive = TRUE
	AND IsArchived = FALSE;
END$$
DELIMITER ;

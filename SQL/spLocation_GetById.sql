DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spLocation_GetById`(
	IN userId varchar(28)
)
BEGIN
	SELECT
		Address,
		Latitude,
		Longitude
	FROM Location
	WHERE UpdatedBy = userId
		AND IsArchived = FALSE;
END$$
DELIMITER ;

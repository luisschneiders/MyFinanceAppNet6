DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spTrip_GetById`(
	IN userId varchar(28),
    IN tripId int
)
BEGIN
	SELECT
		Id,
        TDate,
        StartOdometer,
        EndOdometer,
		Distance,
        PayStatus,
        TCategoryId,
        IsActive
	FROM Trip
	WHERE UpdatedBy = userId
	AND Id = tripId
	AND IsArchived = FALSE;
END$$
DELIMITER ;

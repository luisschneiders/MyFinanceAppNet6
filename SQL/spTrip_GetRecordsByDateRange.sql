DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spTrip_GetRecordsByDateRange`(
	IN userId varchar(28),
    IN startDate datetime,
    IN endDate datetime
)
BEGIN
	SELECT
		t.Id,
        t.TDate,
		v.Description  AS VehicleDescription,
		v.Plate AS VehiclePlate,
        t.StartOdometer,
        t.EndOdometer,
		t.Distance,
        t.PayStatus,
        t.TCategoryId,
		t.IsActive
	FROM Trip t
	JOIN Vehicle v ON v.Id = t.VehicleId
	WHERE t.UpdatedBy = userId
		AND (date(t.TDate) >= startDate AND date(t.TDate) <= endDate)
		AND t.IsArchived = FALSE
	ORDER BY t.TDate DESC, t.EndOdometer DESC;
END$$
DELIMITER ;

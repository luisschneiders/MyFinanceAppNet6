DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spShift_GetRecordsByDateRange`(
	IN userId varchar(28),
    IN startDate datetime,
    IN endDate datetime
)
BEGIN
	SELECT
		s.Id,
        s.CompanyId,
		c.Description,
		s.SDate,
		s.IsActive
	FROM Shift s
	JOIN Company c ON c.Id = s.CompanyId
	WHERE s.UpdatedBy = userId 
		AND s.IsArchived = FALSE 
        AND (date(s.SDate) BETWEEN startDate AND endDate)
	ORDER BY s.SDate ASC;
END$$
DELIMITER ;

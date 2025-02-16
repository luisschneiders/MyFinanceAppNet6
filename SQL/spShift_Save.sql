DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spShift_Save`(
	IN shiftSDate datetime,
    IN shiftCompanyId varchar(28),
    IN shiftIsAvailable bool,
	IN shiftUpdatedBy varchar(28),
	IN shiftCreatedAt datetime,
	IN shiftUpdatedAt datetime
)
BEGIN    
	DECLARE recordCount INT;
    
    -- Check if record exists
    SELECT COUNT(*) INTO recordCount
    FROM Shift 
    WHERE CompanyId = shiftCompanyId
		AND SDate = shiftSDate;
    
    IF (recordCount > 0) THEN
		-- Update existing record
        UPDATE `myfinancedb`.`Shift`
        SET
			`SDate` = DATE(shiftSDate),
			`CompanyId` = shiftCompanyId,
            `IsAvailable` = shiftIsAvailable,
            `IsArchived` = false,
			`UpdatedBy` = shiftUpdatedBy,
			`UpdatedAt` = shiftUpdatedAt
		WHERE (`CompanyId` = shiftCompanyId AND `SDate` = shiftSDate);
	ELSE
		INSERT INTO `myfinancedb`.`Shift` (
			`SDate`,
			`CompanyId`,
            `IsAvailable`,
            `IsArchived`,
			`UpdatedBy`,
			`CreatedAt`,
			`UpdatedAt`
		)
		VALUES (
			DATE(shiftSDate),
			shiftCompanyId,
            shiftIsAvailable,
            false,
			shiftUpdatedBy,
			shiftCreatedAt,
			shiftUpdatedAt
		);
	END IF;
END$$
DELIMITER ;

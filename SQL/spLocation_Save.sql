DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spLocation_Save`(
	IN locationAddress varchar(300),
    IN locationLatitude decimal(8,6),
    IN locationLongitude decimal(9,6),
	IN locationUpdatedBy varchar(28),
    IN locationCreatedAt datetime,
	IN locationUpdatedAt datetime
)
BEGIN
	DECLARE recordCount INT;
    
    -- Check if record exists
    SELECT COUNT(*) INTO recordCount FROM Location WHERE UpdatedBy = locationUpdatedBy;
    
    IF (recordCount > 0) THEN
		-- Update existing record
        UPDATE `myfinancedb`.`Location`
        SET
			`Address` = locationAddress,
			`Latitude` = locationLatitude,
			`Longitude` = locationLongitude,
			`UpdatedBy` = locationUpdatedBy,
			`UpdatedAt` = locationUpdatedAt
		WHERE (`UpdatedBy` = locationUpdatedBy);
	ELSE
		INSERT INTO `myfinancedb`.`Location` (
			`Address`,
			`Latitude`,
			`Longitude`,
			`UpdatedBy`,
			`CreatedAt`,
			`UpdatedAt`
		)
		VALUES (
			locationAddress,
			locationLatitude,
			locationLongitude,
			locationUpdatedBy,
			locationCreatedAt,
			locationUpdatedAt
		);
	END IF;
END$$
DELIMITER ;

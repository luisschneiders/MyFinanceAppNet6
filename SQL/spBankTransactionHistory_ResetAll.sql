DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `spBankTransactionHistory_ResetAll`()
BEGIN
    DECLARE done INT DEFAULT FALSE;
    DECLARE v_BankId BIGINT;
    DECLARE v_TDate DATETIME;
    DECLARE v_TransactionId BIGINT;
    DECLARE v_Amount DECIMAL(10,2);
    DECLARE v_Action VARCHAR(10);
    
	DECLARE v_UpdatedBy VARCHAR(28);
	DECLARE v_CreatedAt DATETIME;
	DECLARE v_UpdatedAt DATETIME;
    
    DECLARE v_PreviousBalance DECIMAL(10,2) DEFAULT 0.00;
    DECLARE v_CurrentBalance DECIMAL(10,2) DEFAULT 0.00;
    DECLARE v_LastBankId BIGINT DEFAULT NULL;
    
    -- Cursor to iterate over the ordered transactions
    DECLARE cur CURSOR FOR
        SELECT Id, FromBank, TDate, Amount, Action, UpdatedBy, CreatedAt, UpdatedAt
        FROM `Transaction`
        WHERE IsActive = TRUE AND IsArchived = FALSE
        ORDER BY FromBank, TDate, Id;

    DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;

    -- Delete all history (with safety)
    DELETE FROM BankTransactionHistory
    WHERE Id > 0;

    OPEN cur;

    read_loop: LOOP
        FETCH cur INTO v_TransactionId, v_BankId, v_TDate, v_Amount, v_Action, v_UpdatedBy, v_CreatedAt, v_UpdatedAt;

        IF done THEN
            LEAVE read_loop;
        END IF;

        -- Reset balance if we're switching to a new bank
        IF v_LastBankId IS NULL OR v_LastBankId != v_BankId THEN
            SET v_PreviousBalance = 0.00;
        ELSE
            SET v_PreviousBalance = v_CurrentBalance;
        END IF;

        -- Calculate current balance based on action
        IF v_Action = 'C' THEN
            SET v_CurrentBalance = v_PreviousBalance + v_Amount;
        ELSEIF v_Action = 'D' THEN
            SET v_CurrentBalance = v_PreviousBalance - v_Amount;
        END IF;

        -- Insert new record into BankTransactionHistory
        INSERT INTO BankTransactionHistory (
            BDate,
            BankId,
            TransactionId,
            Action,
            PreviousBalance,
            Amount,
            CurrentBalance,
            UpdatedBy,
            CreatedAt,
            UpdatedAt
        ) VALUES (
            v_TDate,
            v_BankId,
            v_TransactionId,
            v_Action,
            v_PreviousBalance,
            v_Amount,
            v_CurrentBalance,
            v_UpdatedBy,
            v_CreatedAt,
            v_UpdatedAt
        );

        -- Set last bank id
        SET v_LastBankId = v_BankId;

    END LOOP;

    CLOSE cur;

END$$
DELIMITER ;

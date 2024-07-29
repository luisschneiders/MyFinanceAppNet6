UPDATE myfinancedb.Timesheet t
LEFT JOIN myfinancedb.Company c ON t.CompanyId = c.Id
SET t.Overtime = SEC_TO_TIME(GREATEST(TIME_TO_SEC(t.HoursWorked) - c.StandardHours * 3600, 0));

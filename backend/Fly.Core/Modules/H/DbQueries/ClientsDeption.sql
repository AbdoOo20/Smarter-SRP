SELECT
    c.Name AS ClientName,                                           -- اسم العميل: Client name from tclient
    e.Date AS EntryDate,                                            -- تاريخ القيد: Entry date (invoice issue date)
    SUM(CASE WHEN d.Debit > 0 THEN d.Debit ELSE 0 END) AS TotalDebit,  -- إجمالي المدين: Total payments made (Debit)
    SUM(CASE WHEN d.Credit > 0 THEN d.Credit ELSE 0 END) AS TotalCredit, -- إجمالي الدائن: Total bill amount (Credit)
    (SUM(CASE WHEN d.Credit > 0 THEN d.Credit ELSE 0 END) - SUM(CASE WHEN d.Debit > 0 THEN d.Debit ELSE 0 END)) AS RemainingDebt, -- الدين المتبقي: Remaining debt (Credit - Debit)
    DATEDIFF(CURDATE(), e.Date) AS OverdueDays                      -- مدة التأخير: Days overdue
FROM
    tentry e
    JOIN tentrydetail d ON e.Id = d.EntryId                         -- Join tentry and tentrydetail to get the details of each entry
    JOIN accounts acc ON d.AccountId = acc.Id                       -- Join accounts to get the account name
    JOIN tclient c ON acc.Id = c.AccountId                          -- Join tclient to get the client associated with the account
GROUP BY
    c.Name                                                         -- Group by client name
HAVING
    SUM(d.Debit) < SUM(d.Credit)                                    -- Only show entries with outstanding debt (Debit < Credit)
ORDER BY
    OverdueDays ASC;                                                -- Order by overdue days

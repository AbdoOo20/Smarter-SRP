 SELECT
    s.Date AS InvoiceDate,                                    -- تاريخ الفاتورة: Invoice date
    s.Number AS InvoiceNumber,                                -- رقم الفاتورة: Unique invoice number
    c.Name AS ClientName,                                     -- اسم العميل: Client name
    m.Name AS MarketerName,                                     -- اسم العميل: Client name
    c.Identifier AS ClientNumber,                             -- رقم العميل: Unique client number
    i.Name AS ItemName,                                       -- اسم الصنف: Item name
    i.Barcode AS ItemNumber,                                  -- رقم الصنف: Unique item number (assuming barcode is the unique identifier)
    d.Quantity AS QuantitySold,                               -- الكمية: Quantity of the item sold
    d.Price AS UnitPrice,                                     -- سعر الوحدة: Unit price of the item
    (d.Quantity * d.Price) AS SalesValue,                     -- قيمة المبيعات: Total sales value (quantity * unit price)
    ROUND((d.Quantity * d.Price) * 100 / (SELECT SUM(d2.Quantity * d2.Price)
                                            FROM tsalesinvoicedetail d2 WHERE (NOT d2.IsDeleted)), 2) AS SalesPercentage, -- نسبة المبيعات: Sales percentage of the item
    IF(s.IsPaid = 1, s.TotalAmount, 0) AS CollectionValue,     -- قيمة التحصيل: Amount collected from the invoice
    IF(s.IsPaid = 1, ROUND((s.TotalAmount * 100) / (d.Quantity * d.Price), 2), 0) AS CollectionPercentage, -- نسبة التحصيل: Percentage of collected amount from the total sales value
    (d.Quantity * d.Price) - IF(s.IsPaid = 1, s.TotalAmount, 0) AS InvoiceBalance -- رصيد الفاتورة: Remaining balance after collection
FROM
    tsalesinvoice s
    LEFT JOIN tmarketer m ON s.RepresentativeId = m.Id                            -- Join item table
    JOIN tsalesinvoicedetail d ON s.Id = d.SalesInvoiceId       -- Join sales invoice with sales detail
    JOIN tclient c ON s.ClientId = c.Id                        -- Join client table
    JOIN titem i ON d.ItemId = i.Id                            -- Join item table
    WHERE (NOT s.IsDeleted)
ORDER BY
    s.Date ASC;    
using Fly.Framework.Server.Persistence;
using Fly.Framework.Shared.Common.Http;
using Fly.Framework.Shared.Extentions;
using Fly.Shared.Modules.Reporting.Requests;
using Fly.Shared.Modules.Reporting.Response;
using Fly.Shared.Modules.Reports.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Text;

namespace Fly.Core.Modules.H.Controller;

public partial class ReportsController
{

	private string GetEmbededFileContent(string path)
	{
		try
		{

			var filename = Path.GetFileNameWithoutExtension(path);
			using var stream = Assembly.GetExecutingAssembly()
				.GetManifestResourceStream(path);
			using var reader = new System.IO.StreamReader(stream);
			return reader.ReadToEnd();
		}
		catch (Exception ex)
		{
			// Log the error or throw a custom exception
			throw new Exception($"Error reading embedded file: {path}\n" +
			                    string.Format(",", Assembly.GetExecutingAssembly().GetManifestResourceNames()), ex);
		}
	}


	[HttpPost("SalesCollectionInsights")]
	/// <summary>
	///   تقرير حركة مبيعات وتحصيل ( الكميات + القيمة + نسبة المبيعات والتحصيل)
	/// Sales & Collection Insights : A Deep Dive into Quantities, Values, and Performance
	/// </summary>
	/// <returns></returns>
	public ValueTask<HttpResponseWrapper<DynamicApiReports>> SalesCollectionInsights([FromBody] SalesInvoiceDetailRequest request)
		=>
			TryOne<DynamicApiReports>(async opt =>
			{
				//
				//
				//  1) Prepare needed servcies
				//
				var DynamicApiRow = new DynamicApiReports();
				var generalRepositry = opt.GetService<IRepositryBase>();


				ICollection<DynamicApiRow> rows = [];
				//
				//
				//  2) Prepare columns headers
				//
				DynamicApiRow.AppendRow([
					"ت.فاتورة",
					"ر.فاتورة",
					"اسم العميل",
					"اسم المندوب",
					"اسم الصنف",
					"رقم الصنف",
					"الكمية",
					"سعر الوحدة",
					"قيمة المبيعات",
					"نسبة المبيعات",
					"قيمة التحصيل",
					//"نسبة التحصيل",
					//"رصيد"
				]);

				//
				//
				//  3) Fetch the report raw data
				//
				var sql = new StringBuilder(@"
                     SELECT
                    --    s.Id,
                        s.Date AS InvoiceDate,                                    		-- تاريخ الفاتورة: Invoice date
                        s.Number AS InvoiceNumber,                                		-- رقم الفاتورة: Unique invoice number
                        c.Name AS ClientName,                                     		-- اسم العميل: Client name
                        m.Name AS MarketerName,                                    	    -- اسم العميل: Client name
                        c.Phone AS ClientNumber,                             			-- رقم العميل: Unique client number
                        sf.Name AS SafeName,
                        i.Name AS ItemName,                                       		-- اسم الصنف: Item name
                         IFNULL(i.Barcode, 0) AS ItemNumber,                            -- رقم الصنف: Unique item number (assuming barcode is the unique identifier)
                        d.Quantity AS QuantitySold,                               		-- الكمية: Quantity of the item sold
                        d.Price AS UnitPrice,                                     		-- سعر الوحدة: Unit price of the item
                        (d.Quantity * d.Price) AS SalesValue,                     		-- قيمة المبيعات: Total sales value (quantity * unit price)
                        ROUND((d.Quantity * d.Price) * 100 / (SELECT SUM(d2.Quantity * d2.Price)
                          FROM 
			                    tsalesinvoicedetail d2 WHERE (NOT d2.IsDeleted)), 2) 
		                    AS SalesPercentage, 										-- نسبة المبيعات: Sales percentage of the item
                    --    IF(s.IsPaid = 1, s.TotalAmount, 0) AS CollectionValue,     	-- قيمة التحصيل: Amount collected from the invoice
	                     ( SELECT ed0.Debit from tentry e0  INNER JOIN tentrydetail ed0 ON ed0.EntryId = e0.Id AND  e0.RelatedId = s.Id AND ed0.AccountId = sf.AccountId WHERE (NOT e0.IsDeleted)  )
	                     AS CollectionValue
                    --	ed.Debit AS CollectionPercentage,
                    --    IF(s.IsPaid = 1, ROUND((s.TotalAmount * 100) / (d.Quantity * d.Price), 2), 0) AS CollectionPercentage, -- نسبة التحصيل: Percentage of collected amount from the total sales value
                    --    (d.Quantity * d.Price) - IF(s.IsPaid = 1, s.TotalAmount, 0) ASnvoiceBalance -- رصيد الفاتورة: Remaining balance after collection
                    --   s.TotalAmount AS  InvoiceBalance 								-- رصيد الفاتورة: Remaining balance after collection
                    FROM tsalesinvoicedetail d
                     INNER JOIN tsalesinvoice s ON s.Id = d.SalesInvoiceId      		-- Join sales invoice with sales detail
                     LEFT JOIN tmarketer m ON s.RepresentativeId = m.Id         		-- Join item table
                     LEFT JOIN tclient c ON s.ClientId = c.Id                        	-- Join client table
                     LEFT JOIN titem i ON d.ItemId = i.Id                            	-- Join item table
                     LEFT JOIN tentry e ON e.RelatedId = s.Id AND e.RelatedIndex = 20
                     LEFT JOIN tentrydetail ed ON ed.EntryId = e.Id
                     LEFT JOIN tsafe sf ON sf.Id = s.SafeId
                     WHERE 
                            (NOT s.IsDeleted)
                ");
				sql.Append(" AND s.Date  >=  " + (request.StartDate is not null ? "'" + request.StartDate.ToISO8601() + "'" : " CURDATE() "));
				sql.Append(" AND s.Date  <=  " + (request.EndDate is not null ? "'" + request.EndDate.ToISO8601() + "'" : " ADDDATE(CURDATE(),1) "));
				sql.Append(@"
                    ORDER BY
                        s.Date ASC   
                    ");
				var q = sql.ToString();
				var result = await generalRepositry.QueryList<SalesInvoiceDetailResponse>(q);
				foreach (var res in result)
				{
					DynamicApiRow.AppendRow([
						res.InvoiceDate.ToDottedDate(),
						res.InvoiceNumber.ToString(),
						res.ClientName,
						res.MarketerName,
						res.ItemName,
						res.ItemNumber?.ToString(),
						res.QuantitySold.ToString(),
						res.UnitPrice.ToDottedCommaedNumber(),
						res.SalesValue.ToDottedCommaedNumber(),
						res.SalesPercentage.ToDottedCommaedNumber(),
						res.CollectionValue.ToDottedCommaedNumber(),
						//res.CollectionPercentage.ToDottedCommaedNumber(),
						//res.InvoiceBalance.ToDottedCommaedNumber(),
					]);

				}
				//foreach (var item in all)
				//{


				//}
				//
				//
				//  4) Prepare rows data
				//
				//
				//DynamicApiRow.AppendRows(rows );
				//
				//
				//  5) return response
				//
				return opt.Success(DynamicApiRow);
			});




	[HttpPost("ClientsOverdueDebtsx")]
	/// <summary>
	///   تقرير حركة مبيعات وتحصيل ( الكميات + القيمة + نسبة المبيعات والتحصيل)
	/// Sales & Collection Insights : A Deep Dive into Quantities, Values, and Performance
	/// </summary>
	/// <returns></returns>
	public ValueTask<HttpResponseWrapper<DynamicApiReports>> ClientsOverdueDebtsx([FromBody] ClientsOverdueDebtsRequest request)
		=>
			TryOne<DynamicApiReports>(async opt =>
			{
				//
				//
				//  1) Prepare needed servcies
				//
				var parameters = new List<object>();
				var DynamicApiRow = new DynamicApiReports();
				var generalRepositry = opt.GetService<IRepositryBase>();
				ICollection<DynamicApiRow> rows = [];
				//
				//
				//  2) Prepare columns headers
				//
				DynamicApiRow.AppendRow([
					"اسم العميل",
					"إجمالي مدين",
					"إجمالي دائن",
					"الرصيد",
					"الفترة"
				]);

				//
				//
				//  3) Fetch the report raw data
				//
				var daysFilter = request.DynamicFields.FirstOrDefault(W => W.QueryName == "days");
				if (daysFilter != null)
				{
					if (daysFilter.Value is not null && int.TryParse(daysFilter.Value.ToString(), out var d))
					{
						request.Days = d;
					}
				}

				var sql = new StringBuilder(@"
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
                   
                ");
				bool useWhere = true;
				if (request.Days is not null)
				{
					parameters.Add(request.Days);
					if (useWhere)
					{
						sql.Append($" WHERE ");
						useWhere = false;
					}
					sql.Append($" DATEDIFF(CURDATE(), e.Date) >= {request.Days}");
				}
				if (request.AccountId is not null)
				{
					//parameters.Add(request.Days);
					if (useWhere)
					{
						sql.Append($" WHERE ");
						useWhere = false;
					}
					sql.Append($" c.AccountId = '{request.AccountId}'");
				}
				sql.Append($@"
                    GROUP BY
                        c.Name                                                         -- Group by client name
                    HAVING
                        SUM(d.Debit) < SUM(d.Credit)                                    -- Only show entries with outstanding debt (Debit < Credit)
                    ORDER BY
                        OverdueDays ASC;                                                -- Order by overdue days
                ");
				var result = await generalRepositry.QueryList<ClientsOverdueDebtsResponse>(sql.ToString()/*, parameters.ToArray()*/);
				foreach (var res in result)
				{
					DynamicApiRow.AppendRow([

						res.ClientName,
						res.TotalDebit.ToDottedCommaedNumber(),
						res.TotalCredit.ToDottedCommaedNumber(),
						res.RemainingDebt.ToDottedCommaedNumber(),
						res.OverdueDays.ToString()
					]);
				}
				//
				//  4) Prepare rows data
				//
				//  5) return response
				//
				return opt.Success(DynamicApiRow);
			});

	[HttpPost("RepresentativeAmounts")]
	/// <summary>
	///   تقرير حركة مبيعات وتحصيل ( الكميات + القيمة + نسبة المبيعات والتحصيل)
	/// Sales & Collection Insights : A Deep Dive into Quantities, Values, and Performance
	/// </summary>
	/// <returns></returns>
	public ValueTask<HttpResponseWrapper<DynamicApiReports>> PostRepresentativeAmounts([FromBody] RepresentativeAmountsRequest request)
		=>
			TryOne<DynamicApiReports>(async opt =>
			{
				//
				//
				//  1) Prepare needed servcies
				//
				var parameters = new List<object>();
				var DynamicApiRow = new DynamicApiReports();
				var generalRepositry = opt.GetService<IRepositryBase>();
				ICollection<DynamicApiRow> rows = [];
				//
				//
				//  2) Prepare columns headers
				//
				DynamicApiRow.AppendRow([
					"المندوب",
					"هاتف المندوب",
					"حصة المندوب",
					"قيمة الفاتورة",
					"رقم الفاتورة",
					"تاريخ الفاتورة",
				]);

				//
				//
				//  3) Fetch the report raw data
				//
				//SELECT
				//                  mrk.Name,
				//                 entdet.Debit,
				//                 sls.TotalAmount,
				//                 sls.Number,
				//                 mrk.Phone,
				//                 sls.Date
				//            FROM
				//             `tentrydetail` entdet
				//            INNER JOIN
				//             `tentry` ent ON ent.Id = entdet.EntryId
				//            INNER JOIN	
				//             `tmarketer` mrk ON entdet.AccountId = mrk.AccountId
				//            LEFT JOIN 
				//             `tsalesinvoice` sls ON sls.Id = ent.RelatedId
				//                WHERE NOT entdet.IsDeleted AND sls.TotalAmount IS  NOT NULL
				var sql = new StringBuilder(@"
                        SELECT 
	                        `NAME`,
	                        `Phone`,
	                        `Number`,
	                        `Date`,
	                        `RepresentativePercentage`,
	                        SUM(`TotalAmount`) AS TotalAmount,
	                        SUM(`Amount`) AS Amount
	                           FROM
		                        (
			                        SELECT
				                        mrk.Name,
				                        mrk.Phone,
			                           sls.TotalAmount,
			                           sls.Number,
			                           sls.Date,
			                           sls.RepresentativePercentage,
			                           (sls.RepresentativePercentage * (sls.TotalAmount/ 100))  AS Amount
			                         FROM
			                             `tsalesinvoice` sls
			                         INNER JOIN	
			                             `tmarketer` mrk ON sls.RepresentativeId=mrk.Id
		                            WHERE 
                                        NOT sls.IsDeleted -- AND sls.TotalAmount IS  NOT NULL
                ");
				if (request.AccountId is not null)
				{
					sql.Append($" AND mrk.Id = '{request.AccountId}'");
				}
				sql.Append(@") AS C");
				var result = await generalRepositry.QueryList<RepresentativeAmountsResponseV2>(sql.ToString()/*, parameters.ToArray()*/);
				foreach (var res in result)
				{
					DynamicApiRow.AppendRow([

						res.Name,
						res.Phone.ToString(),
						res.Amount.ToDottedCommaedNumber(),
						res.TotalAmount.ToDottedCommaedNumber(),
						res.Number.ToString(),
						res.Date.ToDottedDate(),
					]);
				}
				//
				//  4) Prepare rows data
				//
				//  5) return response
				//
				return opt.Success(DynamicApiRow);
			});

        


	[HttpPost("ClientsOverdueDebts")]
	/// <summary>
	///   تقرير حركة مبيعات وتحصيل ( الكميات + القيمة + نسبة المبيعات والتحصيل)
	/// Sales & Collection Insights : A Deep Dive into Quantities, Values, and Performance
	/// </summary>
	/// <returns></returns>
	public ValueTask<HttpResponseWrapper<DynamicApiReports>> ClientsOverdueDebts([FromBody] ClientsOverdueDebtsRequest request)
		=>
			TryOne<DynamicApiReports>(async opt =>
			{
				//
				//
				//  1) Prepare needed servcies
				//
				var parameters = new List<object>();
				var DynamicApiRow = new DynamicApiReports();
				var generalRepositry = opt.GetService<IRepositryBase>();
				ICollection<DynamicApiRow> rows = [];
				//
				//
				//  2) Prepare columns headers
				//
				DynamicApiRow.AppendRow([
					"اسم العميل",
					"إجمالي مدين",
					"إجمالي دائن",
					"الرصيد",
					"الفترة",
					"تاريخ آخر دفعة",
				]);

				//
				//
				//  3) Fetch the report raw data
				//
				var days = 0;
				var daysFilter = request.DynamicFields.FirstOrDefault(W => W.QueryName == "days");
				if (daysFilter != null)
				{
					if (daysFilter.Value is not null && int.TryParse(daysFilter.Value.ToString(), out var d))
					{
						days = d;
					}
				}

				var sql = new StringBuilder(@"
                    SELECT 
	                    *,
                        (REPORT0.TotalDebit - REPORT0.TotalCredit) AS RemainingDebt
                    FROM 
	                    (
		                    SELECT
			                    cl.Name AS ClientName
			                    ,(SELECT SUM(endt0.Debit) FROM `tentrydetail` AS endt0 WHERE cl.AccountId = endt0.AccountId AND NOT endt0.IsDeleted) AS TotalDebit
			                    ,(SELECT SUM(endt0.Credit) FROM `tentrydetail` AS endt0 WHERE cl.AccountId = endt0.AccountId AND NOT endt0.IsDeleted) AS TotalCredit
			                    ,(
				                    SELECT 
					                    -- endt0.AccountId,
					                    -- en0.Date, 
					                    -- IF(DATEDIFF(CURDATE(), en0.Date) = 0, 99, DATEDIFF(CURDATE(), en0.Date)) AS OverdueDays
					                    DATEDIFF(CURDATE(), en0.Date) AS OverdueDays 
				                    FROM `tentry` AS  en0 
				                    INNER JOIN `tentrydetail` AS endt0 
					                    ON en0.Id=endt0.EntryId  
				                    WHERE 
					                    cl.AccountId = endt0.AccountId 
					                    AND NOT endt0.IsDeleted
				                    ORDER BY en0.Date
				                    LIMIT 1
			                    )  AS OverdueDays
			                    ,(
				                    SELECT 
					                    -- endt0.AccountId,
					                    en0.Date
					                    -- DATEDIFF(CURDATE(), en0.Date) AS OverdueDays 
				                    FROM `tentry` AS  en0 
				                    INNER JOIN `tentrydetail` AS endt0 
					                    ON en0.Id=endt0.EntryId  
				                    WHERE 
					                    cl.AccountId = endt0.AccountId AND endt0.Credit > 0
					                    AND NOT endt0.IsDeleted
				                    ORDER BY en0.Date
				                    LIMIT 1
			                    )  AS Date
		                    FROM
			                    `tclient` AS cl

	                    ) AS REPORT0
	                    WHERE 
		                    (
			                    ( 	REPORT0.OverdueDays > @DAYS@
				                    OR 
				                    ISNULL(REPORT0.DATE)
			                    )
			                    AND 
			                    REPORT0.TotalDebit > REPORT0.TotalCredit
		                    );");
				var q = sql.ToString().Replace("@DAYS@", days.ToString());
				var result = await generalRepositry.QueryList<ClientsOverdueDebtsResponse>(q);
				foreach (var res in result)
				{
					DynamicApiRow.AppendRow([

						res.ClientName
						,res.TotalDebit.ToDottedCommaedNumber()
						,res.TotalCredit.ToDottedCommaedNumber()
						,res.RemainingDebt.ToDottedCommaedNumber()
						,res.OverdueDays.ToString()
						,res.Date.ToDottedDate()
					]);
				}
				//
				//  4) Prepare rows data
				//
				//  5) return response
				//
				return opt.Success(DynamicApiRow);
			});





}


/*



SELECT
	`TYPE`,
	Number,
	Date,
	itemName,
	unitName,
	RelatedIndex,
	RelatedId,
	Quantity,
	TotalQuantity,
	ExpireDate,
	note
FROM

(

	SELECT
		'مبيعات' AS `TYPE`,
		sinv.Number,
		sinv.Date,
		tm.name AS itemName,
		unt.Name AS unitName,
		inv.RelatedIndex,
		inv.RelatedId,
		inv.Quantity,
		inv.TotalQuantity,
		inv.ExpireDate,
		'' AS note
	FROM
		`tinventory` AS inv
	INNER JOIN
		`TSalesInvoice` AS sinv ON sinv.Id = inv.RelatedId

	INNER JOIN
		`titem` AS tm ON tm.Id = inv.ItemId
	INNER JOIN
		`tunit` AS unt ON unt.Id = inv.UnitId

	UNION

	SELECT
		'مشتريات' AS `TYPE`,
		sinv.Number,
		sinv.Date,
		tm.name AS itemName,
		unt.Name AS unitName,
		inv.RelatedIndex,
		inv.RelatedId,
		inv.Quantity,
		inv.TotalQuantity,
		inv.ExpireDate,
		'' AS note
	FROM
		`tinventory` AS inv
	INNER JOIN
		`TPurchaseInvoice` AS sinv ON sinv.Id = inv.RelatedId AND NOT inv.IsDeleted

	INNER JOIN
		`titem` AS tm ON tm.Id = inv.ItemId
	INNER JOIN
		`tunit` AS unt ON unt.Id = inv.UnitId

	UNION

	SELECT
		'ارجاع مبيعات' AS `TYPE`,
		sinv.Number,
		sinv.Date,
		tm.name AS itemName,
		unt.Name AS unitName,
		inv.RelatedIndex,
		inv.RelatedId,
		inv.Quantity,
		inv.TotalQuantity,
		inv.ExpireDate,
		''+seinv.Number
	FROM
		`tinventory` AS inv
	INNER JOIN
		`TSellReturnInvoice` AS sinv ON sinv.Id = inv.RelatedId AND NOT inv.IsDeleted
	INNER JOIN
		`TSalesInvoice` AS seinv ON seinv.Id = sinv.InvoiceId
	INNER JOIN
		`titem` AS tm ON tm.Id = inv.ItemId
	INNER JOIN
		`tunit` AS unt ON unt.Id = inv.UnitId

	UNION

	SELECT
		'صرف لمسوق' AS `TYPE`,
		dinv.Number,
		dinv.Date,
		tm.name AS itemName,
		unt.Name AS unitName,
		inv.RelatedIndex,
		inv.RelatedId,
		inv.Quantity,
		inv.TotalQuantity,
		inv.ExpireDate,
		'' AS note
	FROM
		`tinventory` AS inv
	INNER JOIN
		`tdisbursementpermit` AS dinv ON dinv.Id = inv.RelatedId AND NOT inv.IsDeleted
	INNER JOIN
		`titem` AS tm ON tm.Id = inv.ItemId
	INNER JOIN
		`tunit` AS unt ON unt.Id = inv.UnitId
) AS d
ORDER BY d.date


*/
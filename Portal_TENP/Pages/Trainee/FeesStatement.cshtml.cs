using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace Portal_TENP.Pages.Trainee
{
    public class FeesStatementModel : PageModel
    {
        private readonly IConfiguration _config;

        public FeesStatementModel(IConfiguration config)
        {
            _config = config;
        }

        public List<StatementItem> Statement = new List<StatementItem>();

        public void OnGet()
        {
            string admissionNo = HttpContext.Session.GetString("AdmissionNo");

            string connString = _config.GetConnectionString("SIMSDB");

            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();

                string query = @"SELECT *,
       SUM(ISNULL(CAST(Debit AS DECIMAL(18,2)),0) 
           - ISNULL(CAST(Credit AS DECIMAL(18,2)),0))
       OVER (ORDER BY MyDate, ReferenceNo) AS RunningBalance
FROM
(
    SELECT DISTINCT dbo.Courses.ID,t.RegNo,t.Amount,t.Debit,t.Credit,CONVERT(VARCHAR,t.dated,103) AS Dated,t.dated as MyDate,t.Fees_Type,t.MyMode,t.ReferenceNo,t.SemesterCode,t.TDescription,CAST(t.TransactionCode AS varchar) AS TransactionCode,t.auditId,CONVERT(VARCHAR,t.audittime,100) AS audittime,t.ReferenceNo as Trans_code,t.TDescription AS Description,t.dated AS date_trans, [Students].[Status_Type] AS Status_ID, dbo.Students.[Name],CASE WHEN Students.Status_Type=0 THEN 'INACTIVE' ELSE 'ACTIVE' END AS stud_status,[dbo].[Students].Stud_Category,dbo.Billing_Codes.Name AS Course, dbo.faculty.faculty_name as faculty, dbo.faculty.faculty_id, dbo.Students_Master.Course_Group,dbo.Students.Gender, dbo.Students_Master.Student_Type_ID, dbo.Student_Category_Types.Name AS CategoryName, Billing_Codes.BeginDate,Billing_Codes.Code, Billing_Codes.EndDate,[Fees_Balance_Detail].[Bal] AS StudentFeesBalance, 1 AS Year_Paid FROM (SELECT DISTINCT CAST(rcptno AS varchar) AS ReferenceNo,abs(sum1) as Amount,'' AS Debit,abs(sum1) as Credit,dte as dated,Descr as TDescription,SemesterID as SemesterCode,admno as RegNo,'PAYMENT' as Fees_Type,Payment_by AS MyMode,Chno AS TransactionCode,auditId,audittime FROM feespay 
UNION SELECT DISTINCT cast(InvoiceID AS varchar) AS ReferenceNo,abs(Amounts) as Amount,abs(Amounts) as Debit, '' AS Credit,Dated as dated,InvoiceDescr as TDescription,SemesterID as SemesterCode,Regno as RegNo,'INVOICE' as Fees_Type,'Invoice' AS MyMode,CAST(invoiceid AS varchar) AS TransactionCode,auditId,audittime FROM Student_invoices WHERE Amounts>=0 
UNION SELECT DISTINCT cast(InvoiceID AS varchar) AS ReferenceNo,abs(Amounts) as Amount,'' as Debit, abs(Amounts) AS Credit,Dated as dated,InvoiceDescr as TDescription,SemesterID as SemesterCode,Regno as RegNo,'INVOICE' as Fees_Type,'CREDIT NOTE' AS MyMode,CAST(invoiceid AS varchar) AS TransactionCode,auditId,audittime FROM Student_invoices WHERE Amounts<0 
UNION SELECT DISTINCT 'OP_BAL' AS ReferenceNo,abs(Balance) as Amount,abs(Balance) as Debit, '' AS Credit,Dated as dated,'Opening Balance' as TDescription,NULL as SemesterCode,Regno as RegNo,'OpeningBalance' as Fees_Type,'Invoice' AS MyMode,'OP_BAL' AS TransactionCode,NULL AS auditId,NULL AS audittime FROM [Student_Opening_Balances] WHERE Balance>0 
UNION SELECT DISTINCT 'OP_BAL' AS ReferenceNo,abs(Balance) as Amount,'' as Debit, abs(Balance) AS Credit,Dated as dated,'Opening Balance' as TDescription,NULL as SemesterCode,Regno as RegNo,'OpeningBalance' as Fees_Type,'Invoice' AS MyMode,'OP_BAL' AS TransactionCode,NULL AS auditId,NULL AS audittime FROM [Student_Opening_Balances] WHERE Balance<0 
UNION SELECT DISTINCT cast(InvoiceID AS varchar) AS ReferenceNo,abs(Amounts) as Amount,abs(Amounts) as Debit, '' AS Credit,Dated as dated,InvoiceDescr as TDescription,SemesterID as SemesterCode,Regno as RegNo,'INVOICE' as Fees_Type,'Invoice' AS MyMode,CAST(invoiceid AS varchar) AS TransactionCode,auditId,audittime FROM [Student_Refunds] WHERE Amounts>=0 
UNION SELECT DISTINCT unit_code AS ReferenceNo,abs(amount) as Amount,'' as Debit, abs(amount) AS Credit,Dated as dated,unit_description as TDescription,SemesterID as SemesterCode,Regno as RegNo,'PAYMENT' as Fees_Type,'Deposit' AS MyMode,CAST(No AS varchar) AS TransactionCode,AuditID AS auditId,Audittime AS audittime FROM [LOANS] ) as t 
INNER JOIN dbo.Student_Sessions ON (dbo.Student_Sessions.Regno = t.RegNo) 
INNER JOIN dbo.Students ON (dbo.Student_Sessions.Regno = dbo.Students.regno) 
INNER JOIN dbo.Students_Master ON (dbo.Students_Master.Regno = dbo.Students.regno) 
INNER JOIN dbo.Courses ON (dbo.Students_Master.Degree_ID = dbo.Courses.ID) 
INNER JOIN dbo.faculty ON (dbo.Students.Faculty_ID = dbo.faculty.ID) 
INNER JOIN dbo.Student_Category_Types ON (Student_Category_Types.ID = dbo.Students_Master.Student_Type_ID) 
INNER JOIN Billing_Codes ON Students.Billing_ID = Billing_Codes.ID  
INNER JOIN [Fees_Balance_Detail] ON ([Fees_Balance_Detail].[Regno]=t.RegNo) 
WHERE  dbo.Student_Sessions.Regno=@AdmissionNo                                       
) AS StatementData
ORDER BY MyDate ASC, ReferenceNo ASC";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandTimeout = 360; // seconds
                cmd.Parameters.AddWithValue("@AdmissionNo", admissionNo);

                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Statement.Add(new StatementItem
                    {
                        Date = dr["MyDate"].ToString(),
                        Description = dr["TDescription"].ToString(),
                        Debit = dr["Debit"].ToString(),
                        Credit = dr["Credit"].ToString(),
                        Balance = dr["RunningBalance"].ToString()
                    });
                }
            }
        }

        public class StatementItem
        {
            public string Date { get; set; }
            public string Description { get; set; }
            public string Debit { get; set; }
            public string Credit { get; set; }
            public string Balance { get; set; }
        }
    }
}

using College_Portal.Data;
using College_Portal.Models;
using College_Portal.Documents;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using QuestPDF.Fluent;
using Microsoft.EntityFrameworkCore;


namespace College_Portal.Reports
{
    public class LedgerModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public LedgerModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            var admissionNo = User.Identity.Name; // or get from database

            if (string.IsNullOrEmpty(admissionNo))
                return RedirectToPage("/Account/Login");

            var data = _context.StudentLedgers
                .FromSqlRaw("EXEC sp_GetStudentLedger @RegNo",
                    new SqlParameter("@RegNo", admissionNo))
                .ToList();

            if (!data.Any())
                return NotFound();

            var document = new LedgerDocument(data);

            var stream = new MemoryStream();
            document.GeneratePdf(stream);
            stream.Position = 0;

            return File(stream, "application/pdf", "LedgerStatement.pdf");
        }
    }
}

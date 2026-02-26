using College_Portal.Models;
using QuestPDF.Infrastructure;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace College_Portal.Documents
{
    public class LedgerDocument : IDocument
    {
        private readonly List<StudentLedger> _data;

        public LedgerDocument(List<StudentLedger> data)
        {
            _data = data.OrderBy(x => x.MyDate).ToList();
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(30);

                page.Header().Column(col =>
                {
                    col.Item().Text("YOUR COLLEGE NAME")
                        .Bold().FontSize(18).AlignCenter();

                    col.Item().Text("FEES LEDGER STATEMENT")
                        .Bold().AlignCenter();
                });

                page.Content().Column(column =>
                {
                    column.Spacing(5);

                    var student = _data.First();

                    column.Item().Text($"Name: {student.Name}");
                    column.Item().Text($"Admission No: {student.RegNo}");

                    column.Item().LineHorizontal(1);

                    decimal runningBalance = 0;
                    decimal totalDebit = 0;
                    decimal totalCredit = 0;

                    column.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(80);  // Date
                            columns.ConstantColumn(80);  // Ref
                            columns.RelativeColumn();    // Description
                            columns.ConstantColumn(80);  // Debit
                            columns.ConstantColumn(80);  // Credit
                            columns.ConstantColumn(90);  // Balance
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("Date").Bold();
                            header.Cell().Text("Ref No").Bold();
                            header.Cell().Text("Description").Bold();
                            header.Cell().AlignRight().Text("Debit").Bold();
                            header.Cell().AlignRight().Text("Credit").Bold();
                            header.Cell().AlignRight().Text("Balance").Bold();
                        });

                        foreach (var item in _data)
                        {
                            decimal debit = item.Debit ?? 0;
                            decimal credit = item.Credit ?? 0;

                            runningBalance += debit - credit;
                            totalDebit += debit;
                            totalCredit += credit;

                            table.Cell().Text(item.MyDate.ToString("dd/MM/yyyy"));
                            table.Cell().Text(item.ReferenceNo);
                            table.Cell().Text(item.TDescription);

                            table.Cell().AlignRight().Text(
                                debit > 0 ? debit.ToString("N2") : "");

                            table.Cell().AlignRight().Text(
                                credit > 0 ? credit.ToString("N2") : "");

                            table.Cell().AlignRight().Text(
                                runningBalance.ToString("N2"));
                        }
                    });

                    column.Item().LineHorizontal(1);

                    column.Item().AlignRight()
                        .Text($"Total Debit: {totalDebit:N2}").Bold();

                    column.Item().AlignRight()
                        .Text($"Total Credit: {totalCredit:N2}").Bold();

                    column.Item().AlignRight()
                        .Text($"Closing Balance: {runningBalance:N2}").Bold();
                });

                page.Footer().AlignCenter().Text(x =>
                {
                    x.Span("Generated on ");
                    x.Span(DateTime.Now.ToString("dd MMM yyyy"));
                });
            });
        }
    }
}

using System;
namespace College_Portal.Models
{
    public class StudentLedger
    {
        public string RegNo { get; set; }
        public string Name { get; set; }

        public string ReferenceNo { get; set; }
        public string TDescription { get; set; }

        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }

        public DateTime MyDate { get; set; }
    }
}

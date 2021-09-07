using System;

namespace SerilogDemo.Api2
{
    public class TraficReport
    {
        public DateTime Date { get; set; }

        public string Status { get; set; }

        public bool PoliceRequired { get; set; }
    }
}

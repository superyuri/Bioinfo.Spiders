using System;
using System.Collections.Generic;
using System.Text;

namespace BMRBSpider.Consoles
{
    class BMRBID
    {
        [CsvHelper.Configuration.Attributes.Name("BMRB ID")]
        public string BMRBId { get; set; }
    }
}

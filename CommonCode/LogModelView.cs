using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonCode
{
    public class LogModelView
    {
        public int ApplicationRefId { get; set; }

        public DateTime? errorTime { get; set; }
        
        public String exception { get; set; }

        public String errorDescription { get; set; }

        public int logLevel { get; set; }

    }
}

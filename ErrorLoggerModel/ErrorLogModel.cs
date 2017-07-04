using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ErrorLoggerModel
{
    public class ErrorLogModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int errorId { get; set; }

        public DateTime? errorTime { get; set; }

        [Required]
        [StringLength(1000)]
        public String errorDescription { get; set; }

        public int logLevel { get; set; }

        public String exception { get; set; }

        public int ApplicationRefId { get; set; }

        [ForeignKey("ApplicationRefId")]
        public virtual ApplicationModel Application { get; set; }
    }
}

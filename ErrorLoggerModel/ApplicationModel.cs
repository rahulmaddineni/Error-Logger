using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ErrorLoggerModel
{
    public class ApplicationModel
    {
        public ApplicationModel()
        {
            Users = new HashSet<UserModel>();
        }

        [Key]
        public int appId { get; set; }

        [Required]
        [StringLength(50)]
        public String appName { get; set; }

        [Required]
        public DateTime? createdDate { get; set; }

        [Required]
        public bool status { get; set; }

        public virtual ICollection<UserModel> Users { get; set; }

        public virtual ICollection<ErrorLogModel> Errors { get; set; }

    }
}

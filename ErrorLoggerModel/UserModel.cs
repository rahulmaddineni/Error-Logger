using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ErrorLoggerModel
{
    public class UserModel
    {

        public UserModel()
        {
            Applications = new HashSet<ApplicationModel>();
        }

        [Key]
        public int userId { get; set; }

        [Required]
        [StringLength(50)]
        public String firstName { get; set; }

        [Required]
        [StringLength(50)]
        public String lastName { get; set; }

        [Required]
        public String email { get; set; }

        public String phone { get; set; }

        [Required]
        public DateTime? lastLoginDate { get; set; }

        [Required]
        public bool status { get; set; }

        [Required]
        public bool role { get; set; }

        public virtual ICollection<ApplicationModel> Applications { get; set; }
    }
}
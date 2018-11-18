using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCoreIdentity.Models.ViewModel
{
    public class RoleViewModels
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Role Name")]
        public string Name { get; set; }
    }
}

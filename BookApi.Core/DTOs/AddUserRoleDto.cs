using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookApi.Core.DTOs
{
    public class AddUserRoleDto
    {
        [Required]
        public string UserId { get; set; } = string.Empty;
        [Required]
        public string Role { get; set; } = string.Empty;
    }
}

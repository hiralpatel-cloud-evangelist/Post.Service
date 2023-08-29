using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Service.DTO.Commands
{
    public class EditPostCommand
    {
        //[Required]
        public string? Sid { get; set; }

        [Required]
        public string? PostName { get; set; }

        [Required]
        public string? PostDescription { get; set; }
    }
}

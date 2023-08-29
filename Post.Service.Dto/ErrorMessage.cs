using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Service.DTO
{
    public class ErrorMessage
    {
        public int Code { get; set; }
        public string? Message { get; set; }
        public int Status { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Service.DTO.Constants
{
   
  
    public enum Status
    {
        Active = 1,
        Inactive = 2,
        Delete = 3
    }

    public class CommonConstants
    {
        //public const string MakeId = "make_id";

        public const string PostName = "PostName";
        public const string PostDescription = "PostDescription";
        public const string LastModifiedDatetime = "LastModifiedDatetime";
        public const string DocumentsFileTypesRegex = (@"(.*?)\.(doc|DOC|docx|DOCX|xls|XLS|xlsx|XLSX|pptx|PPTX|txt|TXT|pdf|PDF|png|PNG|jpg|JPG|jpeg|JPEG|csv|CSV)$");
        public const string ImageFileRegex = (@"(.*?)\.(jpg|JPG|jpeg|JPEG|png|PNG|Jfif)$");
        public const Int64 FileSize = 5000;

        public const string Asc = "asc";
        public const string Desc = "desc";

  
        public static string PostNotFoundMessage = "The requested post is not present";
 

        public static string FileNotValidErrorMessage = "Only Images are allowed";
    }
}

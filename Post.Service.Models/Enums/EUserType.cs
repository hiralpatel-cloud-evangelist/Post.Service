using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Service.Models.Enums
{
    public enum EUserType
    {
        Unidentified = 0,
        Superadmin = 1,
        Administrator = 2,
        Client = 3
    }

    public enum EImageType
    {
        UserImages = 1,
        UserDocument,
        ProductImages,
        PatternImages,
        BlogImages,
        CsvFiles
    }
}

using Post.Service.DTO.Constants;
using Post.Service.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Post.Service.Services.Extensions
{
    public class FilterExtensions
    {
        public static Expression<Func<BlogPost, dynamic>> GetDynamicQueryForBlogPosts(string propName)
        {
            Expression<Func<BlogPost, dynamic>> exp = (t) => true;
            switch (propName)
            {
                case CommonConstants.PostName:
                    exp = d => d.PostName;
                    break;
                case CommonConstants.PostDescription:
                    exp = f => f.PostDescription;
                    break;
                case CommonConstants.LastModifiedDatetime:
                    exp = f => f.LastModifiedDatetime;
                    break;
            }
            return exp;
        }

        public static Expression<Func<BlogPost, bool>> GetDynamicQueryForBlogPosts(string propName, object val)
        {
            Expression<Func<BlogPost, bool>> exp = (t) => true;
            switch (propName)
            {

                case CommonConstants.PostName:
                    exp = f => f.PostName.Contains(Convert.ToString(val));
                    break;
                case CommonConstants.PostDescription:
                    exp = l => l.PostDescription.Contains(Convert.ToString(val));
                    break;
                default:
                    break;
            }
            return exp;
        }
    }
}

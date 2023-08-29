using Post.Service.Base.HelperClasses.IHelperClasses;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Post.Service.DTO.FilterDto;
using Post.Service.DTO.Constants;
using Post.Service.Models.Enums;
using Post.Service.DTO;


namespace Post.Service.Base.BaseClasses
{
    public abstract class ApiControllerBase : ControllerBase
    {
        

        [NonAction]
        public ActionResult CommonCreatedResult<T>(T result)
        {

            return StatusCode(StatusCodes.Status201Created, result);
        }


        [NonAction]
        public virtual ActionResult CommonDeletedResult()
        {

            return StatusCode(StatusCodes.Status204NoContent);
        }
        [NonAction]
        public ActionResult CommonUpdateResult<T>(T result)
        {

            return Ok(result);


        }
       

      


    }
}

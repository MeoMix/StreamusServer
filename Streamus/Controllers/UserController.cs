using log4net;
using Streamus.Domain;
using Streamus.Domain.Interfaces;
using Streamus.Dto;
using System;
using System.Web.Mvc;

namespace Streamus.Controllers
{
    [SessionManagement]
    public class UserController : AbstractController
    {
        private readonly IUserManager UserManager;
        public UserController(ILog logger, IManagerFactory managerFactory)
            : base(logger)
        {
            try
            {
                UserManager = managerFactory.GetUserManager();
            }
            catch (TypeInitializationException exception)
            {
                Logger.Error(exception.InnerException);
                throw exception.InnerException;
            }
        }

        /// <summary>
        ///     Creates a new User object and writes it to the database.
        /// </summary>
        /// <returns>The newly created User</returns>
        [HttpPost]
        public ActionResult Create()
        {
            User user = UserManager.CreateUser();
            UserDto userDto = UserDto.Create(user);

            return new JsonServiceStackResult(userDto);
        }

        [HttpGet]
        public ActionResult Get(Guid id)
        {
            User user = UserManager.Get(id);
            UserDto userDto = UserDto.Create(user);

            return new JsonServiceStackResult(userDto);
        }

        [HttpGet]
        public ActionResult GetByGooglePlusId(string googlePlusId)
        {
            User user = UserManager.GetByGooglePlusId(googlePlusId);

            UserDto userDto = UserDto.Create(user);

            return new JsonServiceStackResult(userDto);
        }

        [HttpPost]
        public JsonResult UpdateGooglePlusId(Guid userId, string googlePlusId)
        {
            UserManager.UpdateGooglePlusId(userId, googlePlusId);

            return Json(new
            {
                success = true
            });
        }

    }
}

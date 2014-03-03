using log4net;
using NHibernate;
using Streamus.Domain;
using Streamus.Domain.Interfaces;
using Streamus.Dto;
using System;
using System.Web.Mvc;

namespace Streamus.Controllers
{
    public class UserController : StreamusController
    {
        private readonly IUserManager UserManager;

        public UserController(ILog logger, IManagerFactory managerFactory)
            : base(logger)
        {
            UserManager = managerFactory.GetUserManager();
        }

        /// <summary>
        ///     Creates a new User object and writes it to the database.
        /// </summary>
        /// <returns>The newly created User</returns>
        [HttpPost]
        public JsonResult Create()
        {
            UserDto userDto;

            using (ITransaction transaction = Session.BeginTransaction())
            {
                User user = UserManager.CreateUser();
                userDto = UserDto.Create(user);

                transaction.Commit();
            }

            return Json(userDto);
        }

        [HttpGet]
        public JsonResult Get(Guid id)
        {
            UserDto userDto;
     
            using (ITransaction transaction = Session.BeginTransaction())
            {
                User user = UserManager.Get(id);
                userDto = UserDto.Create(user);

                transaction.Commit();
            }
            return Json(userDto);
        }

        [HttpGet]
        public JsonResult GetByGooglePlusId(string googlePlusId)
        {
            UserDto userDto;
  
            using (ITransaction transaction = Session.BeginTransaction())
            {
                User user = UserManager.GetByGooglePlusId(googlePlusId);
                userDto = UserDto.Create(user);

                transaction.Commit();
            }

            return Json(userDto);
        }

        [HttpPost]
        public JsonResult UpdateGooglePlusId(Guid userId, string googlePlusId)
        {            
            using (ITransaction transaction = Session.BeginTransaction())
            {
                UserManager.UpdateGooglePlusId(userId, googlePlusId);

                transaction.Commit();
            }
            return Json(new
            {
                success = true
            });
        }

    }
}

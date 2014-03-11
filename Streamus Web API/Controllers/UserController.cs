using log4net;
using NHibernate;
using Streamus_Web_API.Domain;
using Streamus_Web_API.Domain.Interfaces;
using Streamus_Web_API.Dto;
using System;
using System.Web.Http;

namespace Streamus_Web_API.Controllers
{
    public class UserController : StreamusController
    {
        private readonly IUserManager UserManager;

        public UserController(ILog logger, ISession session, IManagerFactory managerFactory)
            : base(logger, session)
        {
            UserManager = managerFactory.GetUserManager();
        }

        /// <summary>
        ///     Creates a new User object and writes it to the database.
        /// </summary>
        /// <returns>The newly created User</returns>
        [HttpPost]
        public UserDto Create()
        {
            UserDto userDto;

            using (ITransaction transaction = Session.BeginTransaction())
            {
                User user = UserManager.CreateUser();
                userDto = UserDto.Create(user);

                transaction.Commit();
            }

            return userDto;
        }

        [HttpGet]
        public UserDto Get(Guid id)
        {
            UserDto userDto;
     
            using (ITransaction transaction = Session.BeginTransaction())
            {
                User user = UserManager.Get(id);
                userDto = UserDto.Create(user);

                transaction.Commit();
            }

            return userDto;
        }

        [HttpGet]
        public UserDto GetByGooglePlusId(string googlePlusId)
        {
            UserDto userDto;
  
            using (ITransaction transaction = Session.BeginTransaction())
            {
                User user = UserManager.GetByGooglePlusId(googlePlusId);
                userDto = UserDto.Create(user);

                transaction.Commit();
            }

            return userDto;
        }

        [HttpPost]
        public IHttpActionResult UpdateGooglePlusId(Guid userId, string googlePlusId)
        {            
            using (ITransaction transaction = Session.BeginTransaction())
            {
                UserManager.UpdateGooglePlusId(userId, googlePlusId);

                transaction.Commit();
            }

            return Ok();
        }

    }
}

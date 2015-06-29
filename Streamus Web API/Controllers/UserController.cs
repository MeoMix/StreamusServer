﻿using log4net;
using NHibernate;
using Streamus_Web_API.Domain;
using Streamus_Web_API.Domain.Interfaces;
using Streamus_Web_API.Dto;
using System;
using System.Web.Http;

namespace Streamus_Web_API.Controllers
{
    [RoutePrefix("User")]
    public class UserController : StreamusController
    {
        private readonly IUserManager UserManager;

        public UserController(ILog logger, ISession session, IManagerFactory managerFactory)
            : base(logger, session)
        {
            UserManager = managerFactory.GetUserManager(session);
        }

        /// <summary>
        ///     Creates a new User object and writes it to the database.
        /// </summary>
        /// <returns>The newly created User</returns>
        [Route("")]
        [HttpPost]
        public UserDto Create(UserDto userDto)
        {
            UserDto createdUserDto;

            using (ITransaction transaction = Session.BeginTransaction())
            {
                User user = UserManager.CreateUser(userDto.GooglePlusId ?? string.Empty);
                createdUserDto = UserDto.Create(user);

                transaction.Commit();
            }

            return createdUserDto;
        }
        
        [Route("{id:guid}")]
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

        [Route("GetByGooglePlusId")]
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

        [Route("MergeByGooglePlusId")]
        [HttpPost]
        public UserDto MergeByGooglePlusId(Guid id, string googlePlusId)
        {
            UserDto userDto;

            using (ITransaction transaction = Session.BeginTransaction())
            {
                User googlePlusUser = UserManager.MergeByGooglePlusId(id, googlePlusId);
                userDto = UserDto.Create(googlePlusUser);

                transaction.Commit();
            }

            return userDto;
        }

        [Route("{id:guid}")]
        [HttpPatch]
        public void Patch(Guid id, UserDto userDto)
        {            
            using (ITransaction transaction = Session.BeginTransaction())
            {
                User user = UserManager.Get(id);

                userDto.SetPatchableProperties(user);
                UserManager.Update(user);

                transaction.Commit();
            }
        }

        [Route("HasLinkedGoogleAccount")]
        [HttpGet]
        public bool HasLinkedGoogleAccount(string googlePlusId)
        {
            bool hasLinkedGoogleAccount;

            using (ITransaction transaction = Session.BeginTransaction())
            {
                User user = UserManager.GetByGooglePlusId(googlePlusId);
                hasLinkedGoogleAccount = user != null;

                transaction.Commit();
            }

            return hasLinkedGoogleAccount;
        }
    }
}

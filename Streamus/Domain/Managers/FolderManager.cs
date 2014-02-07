using Streamus.Domain.Interfaces;
using System;

namespace Streamus.Domain.Managers
{
    public class FolderManager : AbstractManager
    {
        private IFolderDao FolderDao { get; set; }

        public FolderManager()
        {
            FolderDao = DaoFactory.GetFolderDao();
        }

        public void Save(Folder folder)
        {
            try
            {
                folder.ValidateAndThrow();
                FolderDao.Save(folder);
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                throw;
            }
        }

        public void Delete(Guid id)
        {
            try
            {
                Folder folder = FolderDao.Get(id);
                folder.User.RemoveFolder(folder);
                FolderDao.Delete(folder);
            }
            catch (Exception exception)
            {
                Logger.Error(exception);
                throw;
            }
        }

    }
}
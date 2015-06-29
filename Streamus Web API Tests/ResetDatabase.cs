using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using Streamus_Web_API.Dao;

namespace Streamus_Web_API_Tests
{
    /// <summary>
    ///     This isn't a test, but is grouped with testing because of how useful it is.
    ///     Running this 'test' will analyze the hbm.xml NHibernate files, determine a database schema
    ///     derived from those files and reset the test database such that it reflects the current NHibernate schema.
    ///     You can then use a DbDiff tool to propagate the changes to a production database.
    ///     Inconclusive means it was ran successfully. If it errors out, try running again.
    /// </summary>
    [TestFixture]
    public class ResetDatabase
    {
        /// <summary>
        ///     This code is only ran once for the given TestFixture.
        /// </summary>
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            //new SchemaExport((new NHibernateConfiguration()).Configure().BuildConfiguration()).Execute(false, true, false);
        }
    }
}

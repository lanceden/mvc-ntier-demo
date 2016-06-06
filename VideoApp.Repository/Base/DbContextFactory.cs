using System.Runtime.Remoting.Messaging;

namespace VideoApp.Repository
{
    using System.Data.Entity;
    public static class DbContextFactory
    {
        public static DbContext GetCurrentDbContext()
        {
            var dbContext = CallContext.GetData("dbContext") as DbContext;
            if (dbContext != null) return dbContext;
            dbContext = new BaseDbContext();
            CallContext.SetData("dbContext", dbContext);
            return dbContext;
        }
    }
}

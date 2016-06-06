namespace VideoApp.Repository
{
    using System.Data.Entity;
    public class BaseDbContext:DbContext
    {
        public BaseDbContext() :
            base("name=PliEntities")
        {
            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }
    }
}

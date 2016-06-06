namespace VideoApp.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Linq.Expressions;
    public class BaseRepository<T> where T:class 
    {
        private readonly DbSet<T> _dbSet;
        private readonly DbContext _db;
        #region 1.0 構造函數初始化
        /// <summary>
        /// 1.0 構造函數初始化
        /// </summary>
        public BaseRepository()
        {
            _db = DbContextFactory.GetCurrentDbContext();
            _dbSet = _db.Set<T>();
        } 
        #endregion
        #region 2.0 獲取資料集
        /// <summary>
        /// 2.0 獲取資料集
        /// </summary>
        /// <param name="whereExpression">查詢條件</param>
        /// <returns>List集合</returns>
        public List<T> GetList(Expression<Func<T, bool>> whereExpression)
        {
            return _dbSet.AsNoTracking().Where(whereExpression).ToList();
        }
        #endregion

        public List<T> GetPageList(Expression<Func<T, bool>> whereExpression, int pageIndex, int pageSize,
            out int totalCount)
        {
            totalCount = _dbSet.Count();
            return null;
        }

        #region 3.0 新增實體
        /// <summary>
        /// 3.0 新增實體
        /// </summary>
        /// <param name="model">要新增的實體對象</param>
        public void Add(T model)
        {
            _dbSet.Add(model);
        }
        #endregion
        #region 4.0 刪除實體
        /// <summary>
        /// 4.0 刪除實體
        /// </summary>
        /// <param name="model">要刪除的實體</param>
        /// <param name="isAddedContext">是否已附加到上下文中</param>
        public void Del(T model, bool isAddedContext)
        {
            if (!isAddedContext) _dbSet.Attach(model);
            _dbSet.Remove(model);
        } 
        #endregion
        #region 4.1 帶條件刪除 + void DelWhere(Expression<Func<T, bool>> where)
        /// <summary>
        /// 4.1 刪除實體 By 條件
        /// </summary>
        /// <param name="where">要刪除的條件</param>
        public void DelWhere(Expression<Func<T, bool>> where)
        {
            _dbSet.RemoveRange(_dbSet.Where(where));
        }
        #endregion
        #region 5.0 更新實體
        /// <summary>
        /// 5.0 更新實體
        /// </summary>
        /// <param name="model">要更新的實體</param>
        /// <param name="proNames">要更新的實體參數名稱</param>
        public void Edit(T model, params string[] proNames)
        {
            var entry = _db.Entry(model);
            entry.State = EntityState.Unchanged;
            foreach (var proName in proNames)
            {
                entry.Property(proName).IsModified = true;
            }
            _db.Configuration.ValidateOnSaveEnabled = false;
        }
        #endregion
        #region 6.0 執行SQL語法
        /// <summary>
        /// 6.0 執行SQL語法
        /// </summary>
        /// <typeparam name="TResult">執行的返回值類型</typeparam>
        /// <param name="sqlStr">執行的SQL語句</param>
        /// <param name="parameters">執行的SQL變量</param>
        /// <returns>List集合</returns>
        public List<TResult> RunSql<TResult>(string sqlStr, params object[] parameters)
        {
            return _db.Database.SqlQuery<TResult>(sqlStr, parameters).ToList();
        }
        #endregion
        #region 7.0 保存 + int SaveChanges()
        /// <summary>
        /// 7.0 保存
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, object> SaveChanges()
        {
            try
            {
                var dd = _db.SaveChanges();
                return new Dictionary<int, object>()
                {
                    {1,"ok" }
                };
            }
            catch (DbEntityValidationException ex)
            {
                var entityError = ex.EntityValidationErrors.SelectMany(x => x.ValidationErrors).Select(x => x.ErrorMessage);
                var getFullMessage = string.Join("; ", entityError);
                var exceptionMessage = string.Concat(ex.Message, "errors are: ", getFullMessage);
                //NLog
                //LogException(new Exception(string.Format("File : {0} {1}.", logFile.FullName, exceptionMessage), ex));
                return new Dictionary<int, object>()
                {
                    {-1,exceptionMessage }
                };
            }
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace VideoApp.Services
{
    using VideoApp.IServices;
    using IRepository;
    public class BaseServices<T>:IBaseServices<T> where T : class
    {
        protected IBaseRepository<T> BaseRepository = null;

        #region 2.0 獲取資料集
        /// <summary>
        /// 2.0 獲取資料集
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        public List<T> GetList(Expression<Func<T, bool>> whereExpression)
        {
            return BaseRepository.GetList(whereExpression);
        }
        #endregion
        #region 3.0 新增實體
        /// <summary>
        /// 3.0 新增實體
        /// </summary>
        /// <param name="model"></param>
        public void Add(T model)
        {
            BaseRepository.Add(model);
        }
        #endregion
        #region 4.0 刪除實體 By Id
        /// <summary>
        /// 4.0 刪除實體 By Id
        /// </summary>
        /// <param name="model"></param>
        /// <param name="isAddedContext"></param>
        public void Del(T model, bool isAddedContext)
        {
            BaseRepository.Del(model, isAddedContext);
        }
        #endregion
        #region 4.1 帶條件刪除 + void DelWhere(Expression<Func<T, bool>> where)
        /// <summary>
        /// 4.1 刪除實體 By 條件
        /// </summary>
        /// <param name="where">要刪除的條件</param>
        public void DelWhere(Expression<Func<T, bool>> where)
        {
            BaseRepository.DelWhere(where);
        }
        #endregion
        #region 5.0 更新實體
        /// <summary>
        /// 5.0 更新實體
        /// </summary>
        /// <param name="model">要更新的實體</param>
        /// <param name="proNames">對應實體的名稱</param>
        public void Edit(T model, params string[] proNames)
        {
            BaseRepository.Edit(model, proNames);
        }

        #endregion
        #region 6.0 執行SQL語法 + List<TResult> RunSql<TResult>(string sql, params object[] parameters)
        /// <summary>
        /// 6.0 執行SQL語法
        /// </summary>
        /// <typeparam name="TResult">返回的</typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<TResult> RunSql<TResult>(string sql, params object[] parameters)
        {
            return BaseRepository.RunSql<TResult>(sql, parameters);
        }
        #endregion
        #region 7.0 保存 + int SaveChanges()
        /// <summary>
        /// 7.0 保存
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, object> SaveChanges()
        {
            return BaseRepository.SaveChanges();
        }
        #endregion
    }
}

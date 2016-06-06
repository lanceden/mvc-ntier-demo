using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoApp.IServices
{
    public interface IBaseServices<T> where T : class
    {
        #region 2.0 獲取資料集
        /// <summary>
        /// 2.0 獲取資料集
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        List<T> GetList(System.Linq.Expressions.Expression<Func<T, bool>> whereExpression);
        #endregion
        #region 3.0 新增實體
        /// <summary>
        /// 3.0 新增實體
        /// </summary>
        /// <param name="model"></param>
        void Add(T model);
        #endregion
        #region 4.0 刪除實體 By Id
        /// <summary>
        /// 4.0 刪除實體 By Id
        /// </summary>
        /// <param name="model"></param>
        /// <param name="isAddedContext"></param>
        void Del(T model, bool isAddedContext);
        #endregion
        #region 4.1 帶條件刪除 + void DelWhere(Expression<Func<T, bool>> where)
        /// <summary>
        /// 4.1 刪除實體 By 條件
        /// </summary>
        /// <param name="where">要刪除的條件</param>
        void DelWhere(System.Linq.Expressions.Expression<Func<T, bool>> where);
        #endregion
        #region 5.0 更新實體
        /// <summary>
        /// 5.0 更新實體
        /// </summary>
        /// <param name="model">要更新的實體</param>
        /// <param name="proNames">對應實體的名稱</param>
        void Edit(T model, params string[] proNames);
        #endregion
        #region 6.0 執行SQL語法 + List<TResult> RunSql<TResult>(string sql, params object[] parameters)
        /// <summary>
        /// 6.0 執行SQL語法
        /// </summary>
        /// <typeparam name="TResult">返回的</typeparam>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        List<TResult> RunSql<TResult>(string sql, params object[] parameters);
        #endregion
        #region 7.0 保存 + int SaveChanges()
        /// <summary>
        /// 7.0 保存
        /// </summary>
        /// <returns></returns>
        Dictionary<int, object> SaveChanges();
        #endregion
    }
}

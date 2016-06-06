using System;
using System.Collections.Generic;

namespace VideoApp.IRepository
{
    public interface IBaseRepository<T> where T:class 
    {
        #region 2.0 獲取資料集
        /// <summary>
        /// 2.0 獲取資料集
        /// </summary>
        /// <param name="whereExpression">查詢條件</param>
        /// <returns>List集合</returns>
        List<T> GetList(System.Linq.Expressions.Expression<Func<T, bool>> whereExpression);
        #endregion
        #region 3.0 新增實體
        /// <summary>
        /// 3.0 新增實體
        /// </summary>
        /// <param name="model">要新增的實體對象</param>
        void Add(T model);
        #endregion
        #region 4.0 刪除實體
        /// <summary>
        /// 4.0 刪除實體
        /// </summary>
        /// <param name="model">要刪除的實體</param>
        /// <param name="isAddedContext">是否已附加到上下文中</param>
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
        /// <param name="proNames">要更新的實體參數名稱</param>
        void Edit(T model, params string[] proNames);
        #endregion
        #region 6.0 執行SQL語法

        /// <summary>
        /// 6.0 執行SQL語法
        /// </summary>
        /// <typeparam name="TResult">執行的返回值類型</typeparam>
        /// <param name="sqlStr">執行的SQL語句</param>
        /// <param name="parameters">執行的SQL變量</param>
        /// <returns>List集合</returns>
        List<TResult> RunSql<TResult>(string sqlStr, params object[] parameters);
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

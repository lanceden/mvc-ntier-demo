namespace Commons
{
    using System;
    using System.IO;
    using System.Reflection;

    //Web 工具幫助類
    public static class Helper
    {
        #region 1.0 獲取自定義Controller程序集(DLL檔案)
        /// <summary>
        /// 1.0 獲取自定義Controller程序集(DLL檔案)
        /// </summary>
        /// <returns></returns>
        public static FileInfo[] GetControlleraAssemblies()
        {
            //1.0獲取當前運行網站的根目錄
            var phyPath = AppDomain.CurrentDomain.BaseDirectory;
            //獲取當前網站下插件Plugin的物理路徑
            var pluginPhyPath = phyPath + "lib";
            var assArr = new System.Collections.Generic.List<Assembly>();
            //獲取插件文件夾中所有帶有.dll的程序集
            var pluginFolder = new System.IO.DirectoryInfo(pluginPhyPath);
            var pluginAssemblies = pluginFolder.GetFiles("*.dll", System.IO.SearchOption.AllDirectories);
            return pluginAssemblies;
        }
        #endregion
        #region 2.0 驗證是否可將字符串轉為整數 返回 + bool IsNumber(this string strNumber)
        /// <summary>
        /// 2.0 驗證是否可將字符串轉為整數 返回 True or False
        /// </summary>
        /// <param name="strNumber">要驗證的字符串</param>
        /// <returns></returns>
        public static int CheckIdIsInt(this string str)
        {
            if (string.IsNullOrEmpty(str)) return 0;
            int rid;
            if (string.IsNullOrEmpty(str) || !int.TryParse(str, out rid)) return -1;
            rid = int.Parse(str);
            return rid;
        }
        #endregion
    }
}

namespace Blazor_OpenBMCLAPI.BackEnd.SQL
{
    public interface ISQLManager
    {
        /// <summary>
        /// 创建数据表
        /// </summary>
        /// <param name="tableName">表名</param>
        public Task CreateTable(string tableName);
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public Task UpdateValue(string tableName,string key,string value);
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="tableName">表明</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public Task InsertValue(string tableName,string key,string value);
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="key">键</param>
        /// <returns></returns>
        public Task<string> QueryValue(string tableName,string key);
        /// <summary>
        /// 判断键是否存在
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="key">键</param>
        /// <returns></returns>
        public Task<bool> Exists(string tableName,string key);
    }
}

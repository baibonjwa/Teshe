using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Teshe.Common
{
    public class DatabaseMaintenance
    {
        /// <summary>  
        /// 备份数据库  
        /// </summary>  
        /// <param name="fileName">备份文件的路径</param>  
        public static void Backup(string fileName)
        {
            //TODO SQL Server only now  
            string sqlConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            using (SqlConnection conn = new SqlConnection(sqlConnectionString))
            {
                string dbName = new SqlConnectionStringBuilder(sqlConnectionString).InitialCatalog;
                ;
                string commandText = string.Format(
                    "BACKUP DATABASE [{0}] TO DISK = '{1}' WITH FORMAT",
                    dbName,
                    fileName);

                DbCommand dbCommand = new SqlCommand(commandText, conn);
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                dbCommand.ExecuteNonQuery();
            }
        }

        /// <summary>  
        /// 还原数据库 database  
        /// </summary>  
        /// <param name="fileName">要还原的数据库文件路径</param>  
        public static void RestoreBackup(string fileName)
        {
            string sqlConnectionString = ConfigurationManager.AppSettings["HelpStoreContext"];
            using (SqlConnection conn = new SqlConnection(sqlConnectionString))
            {
                string dbName = new SqlConnectionStringBuilder(sqlConnectionString).InitialCatalog;
                string commandText = string.Format(
                    "DECLARE @ErrorMessage NVARCHAR(4000)\n" +
                    "ALTER DATABASE [{0}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE\n" +
                    "BEGIN TRY\n" +
                        "RESTORE DATABASE [{0}] FROM DISK = '{1}' WITH REPLACE\n" +
                    "END TRY\n" +
                    "BEGIN CATCH\n" +
                        "SET @ErrorMessage = ERROR_MESSAGE()\n" +
                    "END CATCH\n" +
                    "ALTER DATABASE [{0}] SET MULTI_USER WITH ROLLBACK IMMEDIATE\n" +
                    "IF (@ErrorMessage is not NULL)\n" +
                    "BEGIN\n" +
                        "RAISERROR (@ErrorMessage, 16, 1)\n" +
                    "END",
                    dbName,
                    fileName);

                DbCommand dbCommand = new SqlCommand(commandText, conn);
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                dbCommand.ExecuteNonQuery();
            }

            //clear all pools  
            SqlConnection.ClearAllPools();
        }
    }
}
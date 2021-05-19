using System;
using System.Collections.Generic;
using System.Text;

namespace Database.DatabaseContext
{
    class DatabaseSettings
    {
        private static string connectionString;
        public static string ConnectionString => connectionString ??= $"workstation id=TokenCheckerDB.mssql.somee.com;packet size=4096;user id=Kirillkik862_SQLLogin_1;pwd=fa283bb2fo;data source=TokenCheckerDB.mssql.somee.com;persist security info=False;initial catalog=TokenCheckerDB";

        public string Server { get; private set; }
        public string Port { get; private set; }
        public string Database { get; private set; }
        public string User { get; private set; }
        public string Password { get; private set; }

        private static DatabaseSettings GetDefaultSettings()
        {
            return new DatabaseSettings()
            {
                Server = "TokenCheckerDB.mssql.somee.com",
                Port = "3306",
                Database = "paritettest",
                User = "Kirillkik862_SQLLogin_1",
                Password = "fa283bb2fo"
            };
        }

        public static DatabaseSettings Settings = GetDefaultSettings();
    }
}

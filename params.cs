using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using System.IO;
namespace GeneralLayer
{
    public class paramFile
    {
        string filePath = "";
        string dbProvider = "";
        public paramFile(string path = "")
        {
            if (path == "")
            {
                path = @"D:\Project\WebAPIDemo\GeneralLayer\Files\params.json";
            }
            filePath = path;
            setDbProvider();
        }
        private void setDbProvider()
        {
            try
            {
                JObject jsonObject = JObject.Parse(File.ReadAllText(filePath));
                JToken dbSource = jsonObject.SelectToken("DBProvider");
                dbProvider = dbSource.ToString().ToUpper();


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public string getDatabaseConnectionString(string dbType)
        {
            string dbString = "";
            try
            {
                JObject jsonObject = JObject.Parse(File.ReadAllText(filePath));
                foreach (JToken dbSource in jsonObject.SelectToken("DataBase"))
                {
                    string type = (string)dbSource["type"];
                    if (type == dbType)
                    {
                        if (dbProvider == "" || dbProvider == "MYSQL")
                        {
                            dbString = "Persist Security Info=False;database=" + (string)dbSource["name"] + "; server=" + (string)dbSource["server"] + "; Connect Timeout=30;user id=" + (string)dbSource["id"] + "; pwd=" + (string)dbSource["pwd"] + "|" + dbProvider;
                        }
                        else if (dbProvider == "SQL")
                        {
                            dbString = "Data Source=" + (string)dbSource["server"] + "; Initial Catalog=" + (string)dbSource["name"] + ";Integrated Security=true; user id=" + (string)dbSource["id"] + "; pwd=" + (string)dbSource["pwd"] + "|" + dbProvider;
                        }
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dbString;
        }
        public string getKey(string keyType)
        {
            string key = "";
            try
            {
                JObject jsonObject = JObject.Parse(File.ReadAllText(filePath));
                foreach (JToken dbSource in jsonObject.SelectToken("key"))
                {
                    key = (string)dbSource[keyType];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return key;
        }
    }
}

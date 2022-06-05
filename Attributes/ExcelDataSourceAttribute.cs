using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;
using System.Data;
using System.Data.OleDb;

namespace SendX.Test.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple=false, Inherited=false)]
    internal class ExcelDataSourceAttribute : DataAttribute
    {
        private readonly string _query;
        private readonly string _connectionString =
            @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES';";

        public ExcelDataSourceAttribute(string filePath, string query)
        {
            _query = query;
            var fullFileName = GetFileFullPath(filePath);
            _connectionString = string.Format(_connectionString, fullFileName);
        }

        public override IEnumerable<object[]> GetData(MethodInfo method)
        {
            if(method == null)
                throw new ArgumentNullException("Method not found");

            var paramTypes = method.GetParameters().Select(x => x.ParameterType).ToArray();
            return GetDataFromExcel(paramTypes);
        }

        private IEnumerable<object[]> GetDataFromExcel(Type[] paramTypes)
        {
            var adapter = new OleDbDataAdapter(_query, _connectionString);
            var dataset = new DataSet();

            try
            {
                adapter.Fill(dataset);

                if(dataset != null && dataset.Tables.Count > 0)
                {
                    foreach(DataRow row in dataset.Tables[0].Rows)
                    {
                        yield return GetCellValue(row.ItemArray, paramTypes);
                    }
                }
            }
            finally
            {
                if(adapter != null)
                {
                    adapter.Dispose();
                }
            }
        }

        private string GetFileFullPath(string filePath)
        {
            var executableDir = Directory.GetParent(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
            return Path.GetFullPath(Path.Combine(executableDir.FullName, filePath));
        }

        private object[] GetCellValue(object[] values, Type[] objectTypes)
        {
            var result = new object[values.Length];

            int idx = 0;
            foreach(var item in values)
            {
                result[idx++] = item;
            }

            return result;
        }
    }
}

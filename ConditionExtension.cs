using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Common
{
    public static class ConditionExtension
    {
        public static string toSqlString(this List<Conditions> condition)
        {
            string where = " 1=1 ";
            if (condition.Count > 0)
            {
                foreach (var item in condition)
                {
                    where += item.toSqlString();
                }
            }
            return where;
        }

        public static string toSqlTempletString(this List<Conditions> condition)
        {
            string where = " 1=1 ";
            if (condition.Count > 0)
            {
                where += " and ";
                foreach (var item in condition)
                {
                    where += item.toSqlTempletString();
                }
            }
            return where;
        }

        public static List<SqlParameter> getSqlParameters(this List<Conditions> condition)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>();
            if (condition.Count > 0)
            {
                foreach (var item in condition)
                {
                    sqlParameters.AddRange(item.getSqlParameters());
                }
            }
            return sqlParameters;
        }
    }
}

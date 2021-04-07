using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Collections;
namespace HY.TMS.Common
{
    public class Condition
    {
        public static string[] logicOpers = new string[] { "and", "or" };
        public static string[] compareOpers = new string[] { ">", "<", "<=", ">=", "=", "<>", "like", "not like", "in", "is not null", "is null" };

        public string compareOper = null;
        public string name = null;
        public string templateName = null;
        public string valType = null;
        public string logicOper = null;
        public object val = null;

        public Condition() { }

        public Condition(CompareOper co, string valType, string name, string logicOper, object val)
        {
            this.compareOper = compareOpers[(int)co];
            this.name = name;
            templateName = name;
            this.logicOper = logicOper;
            this.valType = valType;
            this.val = val;
        }
        public Condition(CompareOper co, string valType, string name, string logicOper, object val, string templateName)
        {
            this.compareOper = compareOpers[(int)co];
            this.name = name;
            this.templateName = templateName;
            this.valType = valType;
            this.logicOper = logicOper;
            this.val = val;
        }

        public string toSqlString()
        {
            string[] arr1 = (string[])operaters.ToArray("".GetType());
            Condition[] arr2 = (Condition[])conditions.ToArray((new Condition()).GetType());

            StringBuilder outStr = new StringBuilder();

            int count = 0;
            if (!string.IsNullOrWhiteSpace(name) && val != null && !string.IsNullOrWhiteSpace(val.ToString()))
            {

                if (Convert.ToInt32(logicOper) < 9)
                {
                    outStr.Append(name);
                    outStr.Append(" ");
                    outStr.Append(compareOpers[Convert.ToInt32(logicOper)]);
                    outStr.Append(" ");
                }
                else if (Convert.ToInt32(logicOper) == 9)
                {
                    outStr.Append("(");
                    outStr.Append(name);
                    outStr.Append(" ");
                    outStr.Append(compareOpers[Convert.ToInt32(logicOper)]);
                    outStr.Append(string.Format(" and {0} <>'' ", name));
                    outStr.Append(")");
                    return outStr.ToString();
                }
                else  if(Convert.ToInt32(logicOper) == 10)
                {
                    outStr.Append("(");
                    outStr.Append(name);
                    outStr.Append(" ");
                    outStr.Append(compareOpers[Convert.ToInt32(logicOper)]);
                    outStr.Append(string.Format(" or {0} ='' ", name));
                    outStr.Append(")");
                    return outStr.ToString();
                }
                if (valType.ToLower() == "int"
                    || valType.ToLower() == "float"
                    || valType.ToLower() == "double"
                    || valType.ToLower() == "bool"
                    || valType.ToLower() == "number"

     )
                {
                    outStr.Append(val);
                }
                else if (valType.ToLower() == "string")
                {
                    string tmp = (string)val;
                    outStr.Append("'" + tmp.Replace("'", "''") + "'");
                }
                else if (valType.ToLower() == "date")
                {
                    DateTime dt = (DateTime)val;
                    outStr.Append("'" + dt.ToString("yyyy-MM-dd") + "'");
                }
                else if (valType.ToLower() == "datetime")
                {
                    DateTime dt = (DateTime)val;
                    outStr.Append("'" + dt.ToString("yyyy-MM-dd hh:mm:ss.fff") + "'");
                }
                else
                {
                    string tmp = val.ToString();
                    outStr.Append("'" + tmp.Replace("'", "''") + "'");
                }
                count++;
            }
            if (arr1.Length > 1)
            {

                for (int i = 0; i < arr1.Length; i++)
                {
                    if (arr2[i].toSqlTempletString() == "")
                        continue;
                    count++;
                    if ((name != null && val != null) || count > 0)
                    {
                        if (count > 1)
                        {
                            outStr.Append(" ");
                            outStr.Append(arr1[i]);
                            outStr.Append(" ");
                        }

                        //if (arr1[i] == "or")
                        //{
                        //    outStr.Append(" (");
                        //}
                        outStr.Append(arr2[i].toSqlString());
                        //if (arr1[i] == "or")
                        //{
                        //    outStr.Append(" )");
                        //}
                    }

                }
            }
            //if (count > 1)
            //{
            //    outStr.Insert(0, '(');
            //    outStr.Append(')');
            //}
            return outStr.ToString();
        }

        public string toSqlTempletString()
        {
            string[] arr1 = (string[])operaters.ToArray("".GetType());
            Condition[] arr2 = (Condition[])conditions.ToArray((new Condition()).GetType());

            StringBuilder outStr = new StringBuilder();

            int count = 0;
            if (name != null && val != null)
            {

                if (Convert.ToInt32(logicOper) != 9)
                {
                    outStr.Append(name);
                    outStr.Append(" ");
                    outStr.Append(compareOpers[Convert.ToInt32(logicOper)]);
                    outStr.Append(" @");
                    outStr.Append(templateName);
                }
                else
                {
                    outStr.Append("(");
                    outStr.Append(name);
                    outStr.Append("");
                    outStr.Append(compareOpers[Convert.ToInt32(logicOper)]);
                    outStr.Append("and");
                    outStr.Append(name);
                    outStr.Append(" <> ''");
                    outStr.Append(")");
                }
                count++;
            }

            if (arr1.Length > 0)
            {
                for (int i = 0; i < arr1.Length; i++)

                {
                    if (arr2[i].toSqlTempletString() == "")
                        continue;
                    count++;
                    if ((name != null && val != null) || count > 1)
                    {
                        outStr.Append(" ");
                        outStr.Append(arr1[i]);
                        outStr.Append(" ");
                    }
                    outStr.Append(arr2[i].toSqlTempletString());
                }
            }
            //if (count > 1)
            //{
            //    outStr.Insert(0, '(');
            //    outStr.Append(')');
            //}
            return outStr.ToString();
        }

        public SqlParameter[] getSqlParameters()
        {
            ArrayList tmp = new ArrayList();
            if (name != null && val != null)
            {
                tmp.Add(new SqlParameter("@" + templateName, val));
            }
            Condition[] arr = (Condition[])conditions.ToArray((new Condition()).GetType());

            for (int i = 0; i < arr.Length; i++)
            {
                SqlParameter[] sps = arr[i].getSqlParameters();
                for (int j = 0; j < sps.Length; j++)
                {
                    tmp.Add(sps[j]);
                }
            }
            return (SqlParameter[])tmp.ToArray(new SqlParameter("", "").GetType());
        }

        ArrayList operaters = new ArrayList();
        ArrayList conditions = new ArrayList();

        public void addCondition(LogicOper lo, Condition c)
        {
            operaters.Add(logicOpers[(int)lo]);
            conditions.Add(c);
        }
        public enum LogicOper : int
        {
            and = 0, or = 1
        }
        public enum CompareOper : int
        {
            moreThan = 0, lessThan = 1, notMoreThan = 2, notLessThan = 3, equal = 4, notEqual = 5, like = 6, notLike = 7, IN = 8, IsNotNull = 9, IsNull = 10
        }
    }
}

using DFYH.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static HY.TMS.Common.Condition;

namespace HY.TMS.Common
{
    public class Conditions
    {
        /// <summary>
        /// 操作符
        /// </summary>
        public int logicOper { get; set; }
        /// <summary>
        /// 条件集合
        /// </summary>
        public List<Condition> Condition { get; set; }

        public string toSqlString()
        {
            StringBuilder outStr = new StringBuilder();
            if (Condition.Count > 1)
            {
                string arr1 = logicOpers[logicOper];
                Condition conditionAdd = new Condition();
                outStr.Append(" " + arr1 + " ");
                outStr.Append('(');
                foreach (Condition condition in Condition)
                {
                    if (condition.val == null) { continue; }
                    conditionAdd.addCondition((LogicOper)Convert.ToInt32(condition.compareOper), condition);
                }
                outStr.Append(conditionAdd.toSqlString());
                outStr.Append(')');
            }
            else 
            {
                if (Condition.FirstOrDefault().val == null || Condition.FirstOrDefault().val.ToString()=="") { return string.Empty; }
                string arr1 = logicOpers[logicOper];
                outStr.Append(" " + arr1 + " ");
                outStr.Append(Condition[0].toSqlString());
            }
            return outStr.ToString();
        }

        public string toSqlTempletString()
        {
            StringBuilder outStr = new StringBuilder();
            if (Condition.Count > 1)
            {
                string arr1 = logicOpers[logicOper];
                Condition conditionAdd = new Condition();
                outStr.Append(" " + arr1 + " ");
                outStr.Append('(');
                foreach (Condition condition in Condition)
                {
                    conditionAdd.addCondition((LogicOper)Convert.ToInt32(condition.compareOper), condition);
                }
                outStr.Append(conditionAdd.toSqlTempletString());
                outStr.Append(')');
            }
            else
            {
                outStr.Append(Condition[0].toSqlTempletString());
            }
            return outStr.ToString();

        }

        public SqlParameter[] getSqlParameters()
        {
            Condition conditionAdd = new Condition();
            foreach (Condition condition in Condition)
            {
                conditionAdd.addCondition((LogicOper)Convert.ToInt32(condition.compareOper), condition);
            }
            return conditionAdd.getSqlParameters();
        }
    }
}

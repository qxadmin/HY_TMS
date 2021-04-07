using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    public class qx_FeeSH
    {
        public int GetType() 
        {
            string sql = "select type from qx_FeeSH";
            return DB<int>.QueryFirstOrDefault(sql,new { });
        }
    }
}

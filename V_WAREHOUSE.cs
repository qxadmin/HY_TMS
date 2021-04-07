using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    /// <summary>
    /// 仓库信息
    /// </summary>
    public partial class V_WAREHOUSE
    {
        /// <summary>
        /// 根据工厂编号获取仓库信息
        /// </summary>
        /// <returns></returns>
        public List<DictionaryModel> GetByFactoryNo() 
        {
            string sql = "select Code warehouseNo,warehouseName value from V_WAREHOUSE where factoryNo=@factoryNo";
            return DB<DictionaryModel>.Query(sql,new { factoryNo=this.factoryNo });
        } 
    }
}

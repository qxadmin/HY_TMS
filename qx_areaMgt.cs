using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    /// <summary>
    /// 省份，一级市，二级市
    /// </summary>
    public partial class qx_areaMgt
    {

        /// <summary>
        /// 获取省
        /// </summary>
        public List<string> GetProvince()
        {
            string sql = "select distinct PROVINCE  from qx_areaMgt ";
            return DB<string>.Query(sql, new { });
        }

        /// <summary>
        /// 根据省获取省
        /// </summary>
        public List<string> GetProvinceBySelf()
        {
            string sql = "select distinct PROVINCE  from qx_areaMgt where PROVINCE=@PROVINCE";
            return DB<string>.Query(sql, new { });
        }

        /// <summary>
        /// 根据省获取一级市
        /// </summary>
        public List<string> GetCity1ByProvince()
        {
            string sql = "select distinct City1  from qx_areaMgt where PROVINCE=@PROVINCE";
            return DB<string>.Query(sql, new { PROVINCE = this.PROVINCE });
        }

        /// <summary>
        /// 根据一级市获取二级市 
        /// </summary>
        public List<string> GetCity2ByCity1()
        {
            string sql = "select distinct City2  from qx_areaMgt where CITY1=@CITY1";
            return DB<string>.Query(sql, new { City1 = this.CITY1 });
        }

        /// <summary>
        /// 根据一级市获取一级市 
        /// </summary>
        public List<string> GetCity1BySelf()
        {
            string sql = "select distinct City2  from qx_areaMgt where CITY1=@CITY1";
            return DB<string>.Query(sql, new { City1 = this.CITY1 });
        }

        /// <summary>
        /// 根据二级市获取二级市 
        /// </summary>
        public List<string> GetCity2BySelf()
        {
            string sql = "select distinct City2  from qx_areaMgt where CITY2=@CITY2";
            return DB<string>.Query(sql, new { City2 = this.CITY2 });
        }

        /// <summary>
        /// 根据二级市获取对应的一级市和省份
        /// </summary>
        /// <returns></returns>
        public qx_areaMgt getInfoByCity2()
        {
            string sql = "select PROVINCE,CITY1,CITY2  from qx_areaMgt where CITY2=@CITY2";
            return DB<qx_areaMgt>.QueryFirstOrDefault(sql, new { CITY2 = this.CITY2 });
        }
    }
}

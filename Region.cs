using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    public partial class Region
    {
        /// <summary>
        /// 根据国家得到省份或直辖市 
        /// </summary>
        public List<DictionaryModel> GetAccountProvince()
        {
            string sql = "select regionname Code, regionid value from region where Depth=1 and active=1 and ParentId=@ParentId order by OrderNum ";
            return DB<DictionaryModel>.Query(sql, new { ParentId = this.ParentId });
        }

        /// <summary>
        /// 根据省份或直辖市得到市或区 
        /// </summary>
        public List<DictionaryModel> GetAccountCity()
        {
            string sql = "select regionname Code, regionid value from region where Depth=2 and active=1 and ParentId=@ParentId order by OrderNum ";
            return DB<DictionaryModel>.Query(sql, new { ParentId = this.ParentId });
        }

        /// <summary>
        /// 根据市得到县 
        /// </summary>
        public List<DictionaryModel> GetAccountDistrict()
        {
            string sql = "select regionname Code, regionid value  from region where Depth=3 and active=1 and ParentId=@ParentId order by OrderNum ";
            return DB<DictionaryModel>.Query(sql, new { ParentId = this.ParentId });
        }
    }
}

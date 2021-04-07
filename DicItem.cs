using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    public partial class DicItem
    {
        /// <summary>
        /// 获取字典数据根据字典类型
        /// </summary>
        /// <returns></returns>
        public List<DictionaryModel> GetByCodeType()
        {
            DicItem dictionary = new DicItem();
            string sql = @"SELECT 
                           [Code]
                          ,[Value]
                          FROM [TMS_DB].[dbo].[DicItem]
                          where CodeType=@CodeType
                          and IsEnable=1";
            return DB<DictionaryModel>.Query(sql, new { CodeType = this.CodeType });
          

        }
    }
}

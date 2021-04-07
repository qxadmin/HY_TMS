using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    public partial class qx_KeyValue
    {
        /// <summary>
        /// 根据编码类型获取绑定数据
        /// </summary>
        public List<DictionaryModel> GetDictionaryItemByTypeNo() 
        {
            string sql= "select SKEY Code,SVALUE value from qx_keyvalue where TYPENO=@TYPENO and STATUS=1";
            return DB<DictionaryModel>.Query(sql,new { TYPENO= this.TYPENO });
        }
    }
}

using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    public partial class qx_Forwarder
    {
        /// <summary>
        /// 订单物流商是否是共生平台或虹运物流
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public bool LogisticsIsGsOrHYPlatFrom(string INORDERNO)
        {
            string sql = @"SELECT *
                          FROM [dbo].[qx_InAndOutBoundOrder] o witH(nolock)
                          left join [TMS_DB].[dbo].[qx_Forwarder] f with(nolock) on f.FORWARDERNO=o.LOGISTICS
                          where o.INORDERNO=@INORDERNO
                          and (FORWARDERNO='YS12000170' OR FORWARDERNO='YS12000163')";
            return DB<int>.QueryFirst(sql, new { INORDERNO = INORDERNO }) > 0;
        }

        /// <summary>
        /// 获取承运商
        /// </summary>
        /// <param name="FACTORYNO">工厂编号</param>
        /// <param name="FORWARDERNO">物流商编号</param>
        /// <returns></returns>
        public List<DictionaryModel> GetForwarder(string factoryNo, string forwarderNo) 
        {
            string sql = @"select distinct FORWARDERNO Code,FORWARDERNAME value from (select * from HY_Forwarder f left  join (select FACTORYNO,LOGISTICSNO from qx_LogistRelation
                           group by FACTORYNO,LOGISTICSNO) l on f.FORWARDERNO=l.LOGISTICSNO)a  where FORWORDSTATUS=1";
            if (!string.IsNullOrWhiteSpace(factoryNo) && factoryNo!="-1") 
            {
                sql += " and (FACTORYNO=@FACTORYNO or FORWARDERNO='YS12000070' or FORWARDERNO='YS12000066'";
                if (factoryNo == "YH10")
                {
                    sql += " or FORWARDERNO='YS12000083' or FORWARDERNO='YS12000081'";
                }
                sql += ")";
            }
            if (forwarderNo != "-1")
            {
                sql += " and FORWARDERNO=@FORWARDERNO";
            }
            return DB<DictionaryModel>.Query(sql, new { FACTORYNO = factoryNo, FORWARDERNO = forwarderNo });
        }
    }
}

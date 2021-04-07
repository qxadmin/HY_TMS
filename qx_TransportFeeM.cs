using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    /// <summary>
    /// 运费基表信息
    /// </summary>
    public partial class qx_TransportFeeM
    {
        /// <summary>
        /// 获取价格
        /// </summary>
        /// <param name="fnoList"></param>
        /// <returns></returns>
        public qx_TransportFeeM GetPrice(List<string> fnoList)
        {
            string sql = @"select PRICE,FACTORYNO from qx_TransportFeeM 
                      where  FACTORYNO in @FACTORYNOLst and PROVINCE = @PROVINCE and TRANSPORTID = '公路' and WEIGHTTYPE = @WEIGHTTYPE and  CITY = @CITY  order by PRICE asc";
            //PROVINCE = @PROVINCE and CITY = @CITY and WEIGHTTYPE = @WEIGHTTYPE
            //          and TRANSPORTID = '公路' and
            return DB<qx_TransportFeeM>.QueryFirstOrDefault(sql, new { FACTORYNOLst = fnoList, PROVINCE = this.PROVINCE,  WEIGHTTYPE = this.WEIGHTTYPE,CITY = this.CITY});
        }


        /// <summary>
        /// 获取运费结算信息
        /// </summary>
        /// <param name="weightType">重量标识</param>
        /// <returns></returns>
        public qx_TransportFeeM GetTransportFeeMInfo(int weightType)
        {
            string sql = @"select PRICE,TIMECOST,RETURNCOST,LOWPRICERANGE from qx_TransportFeeM 
                            where FACTORYNO = @FACTORYNO and PERIODFROM <= @PERIODFROM and PERIODTO>=@PERIODTO
                            and PROVINCE = @PROVINCE and TRANSPORTID = @TRANSPORTID and WEIGHTTYPE = @WEIGHTTYPE";
            //and CITY = @CITY
            return DB<qx_TransportFeeM>.QueryFirstOrDefault(sql, new
            {
                FACTORYNO = this.FACTORYNO,
                PERIODFROM = this.PERIODFROM,
                PERIODTO = this.PERIODTO,
                PROVINCE = this.PROVINCE,
                CITY = this.CITY,
                TRANSPORTID = this.TRANSPORTID,
                WEIGHTTYPE = weightType
            });
        }


        /// <summary>
        /// 在途时间计算+1
        /// </summary>
        /// <param name="tog">工厂编号</param>
        /// <param name="tos">到货省份</param>
        /// <param name="toc">二级市</param>
        /// <param name="toz">总重量（T）</param>
        /// <param name="toz1">一级市</param>
        /// <returns></returns>
        public static int getatime(string tog, string tos, string toc, string toz, string toc1)
        {
            try
            {
                if (!string.IsNullOrEmpty(toz))
                {
                    float num = float.Parse(toz);
                    if (num > 0 && num < 5)
                    {
                        toz = "1";
                    }
                    else if (num >= 5 && num < 10)
                    {
                        toz = "2";
                    }
                    else if (num >= 10 && num < 20)
                    {
                        toz = "3";
                    }
                    else if (num >= 20 && num < 25)
                    {
                        toz = "4";
                    }
                    else if (num >= 25)
                    {
                        toz = "5";
                    }
                    else
                    {
                        return 0;
                    }
                    string sql = @" select top 1 TIMECOST from dbo.qx_TransportFeeM where FACTORYNO=@FACTORYNO and PROVINCE=@PROVINCE and CITY = @CITY and WEIGHTTYPE=@WEIGHTTYPE ";
                    var timeCost = DB<qx_TransportFeeM>.QueryFirst(sql, new { FACTORYNO = tog, PROVINCE = tos, CITY = toc, WEIGHTTYPE = toz });
                    if (timeCost != null)
                    {
                        return Convert.ToInt32(timeCost.TIMECOST.Value) + 1;
                    }
                    else
                    {
                        sql = @" select top 1 TIMECOST from dbo.qx_TransportFeeM where FACTORYNO=@FACTORYNO and PROVINCE=@PROVINCE and CITY = @CITY and WEIGHTTYPE=@WEIGHTTYPE ";
                        timeCost = DB<qx_TransportFeeM>.QueryFirst(sql, new { FACTORYNO = tog, PROVINCE = tos, CITY = toc, WEIGHTTYPE = toz });
                        if (timeCost!=null)
                        {
                            return Convert.ToInt32(timeCost.TIMECOST.Value) + 1;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}

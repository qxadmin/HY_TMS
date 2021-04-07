using HY.TMS.Common;
using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    /// <summary>
    /// 运费底表信息
    /// </summary>
    public partial class HY_TansportPriceTable
    {




        /// <summary>
        /// 获取运价底表分页数据
        /// </summary>
        /// <param name="where">筛选条件</param>
        /// <param name="orderby">排序字段</param>
        /// <param name="pageIndex">页索引</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="total">总记录数</param>
        public List<HY_TansportPriceTable> GetTansportPriceTableList(string where, string orderby, int pageIndex, int pageSize, out int total)
        {
            string files = @"a.Id, a.TransportBusinessNo, a.CustomerShippingPointNo, a.FactoryNo, a.ShippingPointPlace, a.ContractStartDate,a.ContractEndDate, a.ArriveProvince, a.ArriveCity, a.TransportType, a.WeightRangeSign, a.Price, a.TravelDay, a.ReturnOrderDay, a.Range, a.WeightRangeValue, a.CreateUser, a.CreateDate, a.Modify, a.ModifyDate,FORWARDERNAME TransportBusinessName";
            string tableName = @"[TMS_DB].[dbo].[HY_TansportPriceTable] a
                                 Left join[dbo].[HY_Forwarder] on FORWARDERNO = TransportBusinessNo";
            List<Conditions> conditions = where.GetModel<List<Conditions>>();
            where = conditions.toSqlString();
            return DB<HY_TansportPriceTable>.GetPageList(files, tableName, where, orderby, pageIndex, pageSize, out total);
        }

        /// <summary>
        /// 获取价格
        /// </summary>
        /// <param name="fnoList"></param>
        /// <returns></returns>
        public HY_TansportPriceTable GetPrice(List<string> fnoList)
        {
            string sql = @"  select Price,FACTORYNO from HY_TansportPriceTable 
                      where  FactoryNo in @FACTORYNOLst and ArriveProvince = @ArriveProvince and TransportType = '公路' and WeightRangeSign = @WeightRangeSign and  ArriveCity = @ArriveCity  order by PRICE asc";
            return DB<HY_TansportPriceTable>.QueryFirstOrDefault(sql, new { FACTORYNOLst = fnoList, ArriveProvince = this.ArriveProvince, WeightRangeSign = this.WeightRangeSign, ArriveCity = this.ArriveCity });
        }


        /// <summary>
        /// 获取运费结算信息
        /// </summary>
        /// <param name="weightType">重量标识</param>
        /// <returns></returns>
        public HY_TansportPriceTable GetTransportFeeMInfo(int weightType)
        {
            string sql = @"SELECT 
                           id,
                           PRICE,
                           TravelDay,
                           ReturnOrderDay
                           FROM   HY_TansportPriceTable
                           WHERE  FACTORYNO = @FACTORYNO
                           AND ArriveProvince = @ArriveProvince
                           AND ArriveCity = @ArriveCity
                           AND TransportType = @TransportType
                           AND WeightRangeSign = @WeightRangeSign ";
            //and CITY = @CITY
            return DB<HY_TansportPriceTable>.QueryFirstOrDefault(sql, new
            {
                FACTORYNO = this.FactoryNo,
                ArriveProvince = this.ArriveProvince,
                ArriveCity = this.ArriveCity,
                TransportType = this.TransportType,
                WeightRangeSign = weightType
            });
        }

        /// <summary>
        /// 获取运费结算信息
        /// </summary>
        /// <param name="weightType">重量标识</param>
        /// <returns></returns>
        public List<HY_TansportPriceTable> GetTransportFeeMInfoLst(int weightType)
        {
            string sql = @"SELECT PRICE,
                           TravelDay,
                           ReturnOrderDay
                           FROM   HY_TansportPriceTable
                           WHERE  FACTORYNO = @FACTORYNO
                           AND ArriveProvince = @ArriveProvince
                           AND ArriveCity = @ArriveCity
                           AND TransportType = @TransportType
                           AND WeightRangeSign = @WeightRangeSign
                           AND TransportBusinessNo = @TransportBusinessNo ";
            //and CITY = @CITY
            return DB<HY_TansportPriceTable>.Query(sql, new
            {
                FACTORYNO = this.FactoryNo,
                ArriveProvince = this.ArriveProvince,
                ArriveCity = this.ArriveCity,
                TransportType = this.TransportType,
                WeightRangeSign = weightType,
                TransportBusinessNo = this.TransportBusinessNo
            });
        }


        /// <summary>
        /// 获取重量标识
        /// </summary>
        /// <param name="weightType">重量标识</param>
        /// <returns></returns>
        public List<HY_TansportPriceTable> GetWeightRangeSign(float weightType)
        {
            string sql = @"SELECT WeightRangeSign,
                           ReturnOrderDay
                           FROM   HY_TansportPriceTable
                           WHERE  FACTORYNO = @FACTORYNO
                           AND ArriveProvince = @ArriveProvince
                           AND ArriveCity = @ArriveCity
                           AND TransportType=@TransportType
                           AND  TransportBusinessNo = @TransportBusinessNo 
                           order by WeightRangeSign ASC";
            //and CITY = @CITY
            return DB<HY_TansportPriceTable>.Query(sql, new
            {
                FACTORYNO = this.FactoryNo,
                ArriveProvince = this.ArriveProvince,
                ArriveCity = this.ArriveCity,
                TransportType = this.TransportType,
                TransportBusinessNo = this.TransportBusinessNo
            });
        }




        public decimal GetRangePrice()
        {
            string sql = "  select Price * Range from HY_TansportPriceTable WIHT(NOLOCK) where ArriveProvince=@ArriveProvince and WeightRangeSign=@WeightRangeSign and FactoryNo=@FactoryNo";
            return DB<decimal>.QueryFirstOrDefault(sql, new { ArriveProvince = this.ArriveProvince, WeightRangeSign = this.WeightRangeSign, FactoryNo = this.FactoryNo });
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
                    string sql = @" select *  from dbo.HY_TansportPriceTable where FACTORYNO=@FACTORYNO and ArriveProvince=@ArriveProvince and ArriveCity = @ArriveCity and WeightRangeSign=@WeightRangeSign ";
                    var timeCost = DB<HY_TansportPriceTable>.QueryFirst(sql, new { FACTORYNO = tog, ArriveProvince = tos, ArriveCity = toc, WeightRangeSign = toz });
                    if (timeCost != null)
                    {
                        return Convert.ToInt32(timeCost.TravelDay.Value) + 1;
                    }
                    else
                    {
                        return 0;
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


        public HY_TansportPriceTable GetByTansportPriceTableIdById()
        {
            string sql = @"select * from [dbo].[HY_TansportPriceTable] where Id=@Id";
            return DB<HY_TansportPriceTable>.QuerySingleOrDefault(sql, new { Id = this.Id });
        }

        public bool Insert()
        {
            string sql = @"insert into [dbo].[HY_TansportPriceTable]
                            (
                              TransportBusinessNo,
                              TransportBusinessName,
                              CustomerShippingPointNo,
                              FactoryNo, 
                              ShippingPointPlace,
                              ContractStartDate,
                              ContractEndDate,
                              ArriveProvince,
                              ArriveCity,
                              TransportType, 
                              WeightRangeSign,
                              Price,
                              TravelDay, 
                              ReturnOrderDay, 
                              Range, 
                              WeightRangeValue,
                              CreateDate, 
                              Modify,
                              ModifyDate
                            )
                            values
                            (
                              @TransportBusinessNo,
                              @TransportBusinessName,
                              @CustomerShippingPointNo,
                              @FactoryNo, 
                              @ShippingPointPlace,   
                              @ContractStartDate,
                              @ContractEndDate,
                              @ArriveProvince,
                              @ArriveCity,
                              @TransportType, 
                              @WeightRangeSign,
                              @Price,
                              @TravelDay, 
                              @ReturnOrderDay, 
                              @Range, 
                              @WeightRangeValue,
                              GETDATE(), 
                              @Modify,
                              GETDATE()
                            )";
            return DB<int>.Execute(sql, this) > 0;
        }

        public bool Update()
        {
            string sql = @"update HY_TansportPriceTable set  
                           TransportBusinessNo=@TransportBusinessNo,
                           TransportBusinessName=@TransportBusinessName,
                           CustomerShippingPointNo=@CustomerShippingPointNo,
                           FactoryNo=@FactoryNo, 
                           ShippingPointPlace=@ShippingPointPlace,
                           ContractStartDate=@ContractStartDate,
                           ContractEndDate=@ContractEndDate,
                           ArriveProvince=@ArriveProvince,
                           ArriveCity=@ArriveCity,
                           TransportType=@TransportType, 
                           WeightRangeSign=@WeightRangeSign,
                           Price=@Price,
                           TravelDay=@TravelDay, 
                           ReturnOrderDay=@ReturnOrderDay, 
                           Range=@Range, 
                           WeightRangeValue=@WeightRangeValue,
                           Modify=@Modify,
                           ModifyDate=GETDATE()
                           where Id=@Id";
            return DB<int>.Execute(sql, this) > 0;
        }

        public bool Delete()
        {
            string sql = "DELETE HY_TansportPriceTable where Id=@Id ";
            return DB<int>.Execute(sql, this) > 0;
        }
    }
}

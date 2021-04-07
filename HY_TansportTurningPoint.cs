using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    public partial class HY_TansportTurningPoint
    {

        /// <summary>
        /// 结费单号
        /// </summary>
        public string SettlementNo { get; set; }

        /// <summary>
        /// 据根据订单编号获取转点数据
        /// </summary>
        /// <returns></returns>
        public List<HY_TansportTurningPoint> GetByOrderNO()
        {
            string sql = @"SELECT a.*,b.[SettlementNo] FROM [HY_TansportTurningPoint] a WITH(NOLOCK)
                           left join (select TansportTurningPointId,SettlementNo from [dbo].[HY_FreightCalculationTable]
                           group by ArriveProvince,ArriveOneCity,ArriveSecondCity,SettlementNo,TansportTurningPointId) b 
                           on b.[TansportTurningPointId]=a.[ID]
                           WHERE a.OrderNo = @OrderNo";
            return DB<HY_TansportTurningPoint>.Query(sql, new { OrderNo = this.OrderNo });
        }

        /// <summary>
        /// 新增转点信息
        /// </summary>
        /// <returns></returns>
        public int InsertTansportTurningPoint(DbConnection connection, DbTransaction dbTransaction)
        {
            string sql = @"INSERT INTO [dbo].[HY_TansportTurningPoint]  
                           ([OrderNo]
                           ,[TransportBusinessNo]
                           ,[TransportBusinessName]
                           ,[CarNumber]
                           ,[DriverName]
                           ,[DriverIDNumber]
                           ,[DriverMobile]
                           ,[CarTypeCode]
                           ,[CarTypeName]
                           ,[TranspointType]
                           ,[ArriveProvince]
                           ,[ArriveOneCity]
                           ,[ArriveSecondCity]
                           ,[TransportType]
                           ,[TransportTypeName]
                           ,[Modify],
                            [ShipmentPlace] )
                     VALUES
                           (
		                    @OrderNo
                           ,@TransportBusinessNo
                           ,@TransportBusinessName
                           ,@CarNumber
                           ,@DriverName
                           ,@DriverIDNumber
                           ,@DriverMobile
                           ,@CarTypeCode
                           ,@CarTypeName
                           ,@TranspointType 
                           ,@ArriveProvince
                           ,@ArriveOneCity
                           ,@ArriveSecondCity
                           ,@TransportType
                           ,@TransportTypeName
                           ,@Modify,
                           @ShipmentPlace);
                           Select @@Identity";

            return Convert.ToInt32(DB<object>.ExecuteScalar(sql, this, connection, dbTransaction));
            //Dictionary<string, object> dic = new Dictionary<string, object>();
            //dic.Add(sql, this);
            //return dic;
        }

        /// <summary>
        /// 修改转点信息
        /// </summary>
        /// <returns></returns>
        public bool Update(DbConnection connection, DbTransaction dbTransaction)
        {
            string sql = @"UPDATE HY_TansportTurningPoint 
                            SET  ArriveProvince=@ArriveProvince
                           ,ArriveOneCity=@ArriveOneCity
                           ,ArriveSecondCity=@ArriveSecondCity
                           ,Modify=@Modify
                           ,ModifyDate=@ModifyDate
                            WHERE ID=@ID";

            return DB<int>.Execute(sql, this, connection, dbTransaction) > 0;
        }
        /// <summary>
        /// 是否第一次转点
        /// </summary>
        /// <returns></returns>

        public bool IsFirstTurningPoint()
        {
            string sql = "select 1 from [dbo].[HY_TansportTurningPoint] WITH(NOLOCK)  where OrderNo=@OrderNo";
            return DB<int>.QueryFirstOrDefault(sql, new { OrderNo = this.OrderNo }) <= 0;
        }


        /// <summary>
        /// 获取此运单最后一个转点信息
        /// </summary>
        /// <returns></returns>
        public HY_TansportTurningPoint GetLastTurningPoint()
        {
            string sql = "select  TOP 1 * from HY_TansportTurningPoint  WITH(NOLOCK) where OrderNo=@OrderNo order by ID desc";
            return DB<HY_TansportTurningPoint>.QueryFirstOrDefault(sql, new { OrderNo = this.OrderNo });
        }

        /// <summary>
        /// 删除此运单最后一个转点信息
        /// </summary>
        /// <returns></returns>
        public bool DeleteLastTurningPoint(DbConnection connection, DbTransaction dbTransaction)
        {
            string sql = " delete from HY_TansportTurningPoint  where id in  (select top 1 Id from HY_TansportTurningPoint  WITH(NOLOCK) where OrderNo=@OrderNo order by CreateDate desc)";
            return DB<int>.Execute(sql, this, connection, dbTransaction) > 0;
            //Dictionary<string, object> dic = new Dictionary<string, object>();
            //dic.Add(sql, this);
            //return dic;
        }

        /// <summary>
        /// 查询最后一条转点的ID
        /// </summary>
        /// <returns></returns>
        public string GetLastTransportTurnPointID()
        {
            string sql = "select max(ID) from HY_TansportTurningPoint  WITH(NOLOCK) where OrderNo=@OrderNo";
            return DB<string>.QuerySingleOrDefault(sql, new { OrderNo = this.OrderNo });
        }

        /// <summary>
        /// 根据最后转点ID，查询当前订单最后一条转点的上一条转点信息
        /// </summary>
        /// <returns></returns>
        public HY_TansportTurningPoint GetLastTransportTurnPointInfoByLastPointID()
        {
            string sql = "select top 1 * from HY_TansportTurningPoint  WITH(NOLOCK) where OrderNo=@OrderNo and ID <> @LastPointID order by ID DESC";
            return DB<HY_TansportTurningPoint>.QuerySingleOrDefault(sql, new { OrderNo = this.OrderNo, LastPointID = this.ID });
        }

        /// <summary>
        /// 取消转点
        /// </summary>
        /// <returns></returns>
        public bool CancelTurningPoint()
        {
            string sql = " delete from HY_TansportTurningPoint  where id = @id";
            return DB<int>.Execute(sql, new { id = this.ID }) > 0;
        }

        /// <summary>
        /// 取消单据转点
        /// </summary>
        /// <returns></returns>
        public bool DeletelTurningPointByOrderNo()
        {
            string sql = " delete from HY_TansportTurningPoint  where OrderNo = @OrderNo";
            return DB<int>.Execute(sql, new { OrderNo = this.OrderNo }) > 0;
        }

        /// <summary>
        /// 根据转点ID获取转点信息
        /// </summary>
        /// <returns></returns>
        public HY_TansportTurningPoint GetTurningPointByID()
        {
            string sql = " Select * from HY_TansportTurningPoint  where ID = @ID";
            return DB<HY_TansportTurningPoint>.QuerySingleOrDefault(sql, new { ID = this.ID });
        }
        /// <summary>
        /// 根据订单编号获取订单最后一个转点ID
        /// </summary>
        /// <returns></returns>
        public int GetTurningPointMaxIDByOrderNo()
        {
            string sql = " select max(ID) from HY_TansportTurningPoint  WITH(NOLOCK) where OrderNo=@OrderNo";
            return DB<int>.QuerySingleOrDefault(sql, new { OrderNo = this.OrderNo });
        }

        /// <summary>
        /// 根据订单编号获取订单是否只有一条转点信息
        /// </summary>
        /// <returns></returns>
        public int GetTurningPointIsOneByOrderNo()
        {
            string sql = " select count(1) from HY_TansportTurningPoint  WITH(NOLOCK) where OrderNo=@OrderNo";
            return DB<int>.QuerySingleOrDefault(sql, new { OrderNo = this.OrderNo });
        }

        /// <summary>
        /// 根据订单编号和最大ID获取订单上一个转点信息
        /// </summary>
        /// <returns></returns>
        public HY_TansportTurningPoint GetLastTurningPointByOrderNoAndMaxID()
        {
            string sql = " select top 1 * from HY_TansportTurningPoint  WITH(NOLOCK) where OrderNo=@OrderNo and ID <> @ID order by ID DESC";
            return DB<HY_TansportTurningPoint>.QuerySingleOrDefault(sql, new { OrderNo = this.OrderNo, ID = this.ID });
        }
    }
}

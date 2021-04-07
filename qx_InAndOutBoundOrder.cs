using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    public partial class qx_InAndOutBoundOrder
    {

        /// <summary>
        /// SAP行项目
        /// </summary>
        public string SAPORDERITEM { get; set; }

        /// <summary>
        /// 司机身份证号码
        /// </summary>
        public string IdCarNumber { get; set; }
        /// <summary>
        /// 司机身份证号码
        /// </summary>
        public string DriverMoblie { get; set; }

        /// <summary>
        /// 司机姓名
        /// </summary>
        public string DrivcerName { get; set; }
        /// <summary>
        /// 获取单条订单数据
        /// </summary>
        /// <returns></returns>
        public qx_InAndOutBoundOrder getModelByInOrderNo()
        {
            string sql = "select INORDERNO,FROMPLANT,SAPORDERNO,salesaccount,saleofficedesc,logistics,endtime,receiveconpanydesc,tocontact,toaddress,gsno,STATUS from [dbo].[qx_InAndOutBoundOrder] where inorderno =@inorderno";
            return DB<qx_InAndOutBoundOrder>.QuerySingleOrDefault(sql, new { inorderno = this.INORDERNO });
        }

        /// <summary>
        /// 获取订单数据
        /// </summary>
        /// <returns></returns>
        public List<qx_InAndOutBoundOrder> getModelBySapOrderNo()
        {
            string sql = @"select INORDERNO,TWOLOGISTICS,TWOLOGISTICSNAME,FROMPLANT,SAPORDERNO,salesaccount,saleofficedesc,logistics,endtime
                            , receiveconpanydesc, tocontact, toaddress, gsno,CARNUMBER,DRIVER,STATUS
                            from[dbo].[qx_InAndOutBoundOrder] where SAPORDERNO =@SAPORDERNO and STATUS='00'";
            return DB<qx_InAndOutBoundOrder>.Query(sql, new { SAPORDERNO = this.SAPORDERNO });
        }

        /// <summary>
        /// 获取订单总重量
        /// </summary>
        /// <returns></returns>
        public decimal SumWeight()
        {
            string sql = @"select isnull(sum(b.WEIGHT*b.REQ_QTY),0)/1000 from qx_InAndOutBoundOrder as a left join qx_InAndOutOrderProductDetail as b on a.inorderno = b.orderno
            where a.INORDERNO = @INORDERNO group by a.INORDERNO";
            return DB<decimal>.QuerySingle(sql, new { INORDERNO = this.INORDERNO });
        }

        /// <summary>
        /// 查询共生所需订单数据
        /// </summary>
        /// <param name="inorderno">订单号</param>
        /// <returns></returns>
        public DataSet GetGsDataByINORDERNO()
        {
            string sql = @"--order
                            select id,gsno no,PLANLOADDATE deliTime,FROMPLANT,PLANLOADDATE+ZTDay arriTime,inorderno shipperNo,saporderno remark ,CARNUMBER plate from [dbo].[qx_InAndOutBoundOrder]
                            where INORDERNO=@INORDERNO
                            --address
                            select '1' type,b.address address,null province,b.city city,b.factoryName linkman,FROMPLANT name from [qx_InAndOutBoundOrder] as a left join V_FACTORY as b on a.[FROMPLANT] = b.factoryno
                            where INORDERNO=@INORDERNO
                            select '0' type,TOADDRESS address,TOPROVINCES province,TOCITY1 city,TOCONTACT linkman from [dbo].[qx_InAndOutBoundOrder] as a left join V_FACTORY as b on a.[FROMPLANT] = b.factoryno
                            where INORDERNO=@INORDERNO
                            --goods
                            select PRODUCTNAME name,unit_desc pack,REQ_QTY amount,weight weight,PRODUCTNO code  from [qx_InAndOutBoundOrder] as a left join qx_InAndOutOrderProductDetail as b on a.inorderno = b.orderno
                            where INORDERNO=@INORDERNO";
            return DB<DataSet>.QueryFirst(sql, new { INORDERNO = this.INORDERNO });
        }

        /// <summary>
        /// 修改是否入厂
        /// </summary>
        /// <param name="tmsNo">tms订单编号</param>
        /// <param name="isAllowInFactory">是否允许入厂</param>
        /// <returns></returns>
        public bool UpdateOrderInFactory(string tmsNo, bool isAllowInFactory)
        {
            string sql = "update qx_InAndOutBoundOrder set ISAllowInFactory=@ISAllowInFactory,CONFIRMDATE=getdate() where INORDERNO=@INORDERNO";
            return DB<int>.Execute(sql, new { ISAllowInFactory = isAllowInFactory, INORDERNO = tmsNo }) > 0;
        }

        /// <summary>
        /// 删除订单
        /// </summary>
        /// <param name="orderNo">订单编号</param>
        /// <returns></returns>
        public bool DelOrderByOrderNo(DbConnection connection, DbTransaction transaction)
        {
            string sql = "update qx_InAndOutBoundOrder set IsDel='1' where INORDERNO=@INORDERNO and STATUS='00'";      //只能删除状态00的订单  00  未处理状态  ISDEL 0 正常 1 删除
            return DB<int>.Execute(sql, new { INORDERNO = this.INORDERNO},connection,transaction) > 0;
        }
        /// <summary>
        /// 撤销订单
        /// </summary>
        /// <param name="orderNo">订单编号</param>
        /// <returns></returns>
        public bool CANCELOrderByOrderNo(DbConnection connection, DbTransaction transaction)
        {
            string sql = "update qx_InAndOutBoundOrder set Status='99',ISCANCEL='1' where INORDERNO=@INORDERNO";//ISCANCEL 1 取消发运 0正常  取消发运状态99   
            return DB<int>.Execute(sql, new { INORDERNO = this.INORDERNO },connection, transaction) > 0;
        }
        /// <summary>
        /// 修改在途天数
        /// </summary>
        /// <returns></returns>
        public bool UpadateZTDay()
        {
            string sql = "update qx_InAndOutBoundOrder set Status =@Status, ISINDICAT =@ISINDICAT, PREPRICE =@PREPRICE, ZTDay =@ZTDay where INORDERNO =@INORDERNO";
            return DB<int>.Execute(sql, new { Status = this.STATUS, ISINDICAT = this.ISINDICAT, PREPRICE = this.PREPRICE, ZTDay = this.ZTDay, INORDERNO = this.INORDERNO }) > 0;
        }
        /// <summary>
        /// 修改订单状态为00未派车
        /// </summary>
        /// <returns></returns>
        public bool UpadateOrderStatusNotSendCar()
        {
            string sql = "update qx_InAndOutBoundOrder set Status='00',ISINDICAT='0' where INORDERNO = @INORDERNO";
            return DB<int>.Execute(sql, new { INORDERNO = this.INORDERNO }) > 0;
        }

        /// <summary>
        /// 修改物流信息
        /// </summary>
        /// <returns></returns>
        public bool UpdateInAndOutBoundOrder()
        {
            string sql = @"UPDATE qx_InAndOutBoundOrder SET TOPROVINCES=@TOPROVINCES,TOCITY1=@TOCITY1,TOCITY2=@TOCITY2,PLANLOADDATE=@PLANLOADDATE,LOGISTICS=@LOGISTICS,CARNUMBER=@CARNUMBER,
                           PLANARRIVEPLANTDATE=@PLANARRIVEPLANTDATE,DIRVERPLANARRIVEDATE=@DIRVERPLANARRIVEDATE,CONFIRMDATE=@CONFIRMDATE,ActualModels=@ActualModels,ForecastModels=@ForecastModels,
                           SendSMS=@SendSMS,CharteredName=@CharteredName,CharteredPrice=@CharteredPrice,TRANSPORTMODE=@TRANSPORTMODE,ForwardingType=@ForwardingType,ActualModel_num=@ActualModel_num,
                           PREPRICE=@PREPRICE,ZTDay=@ZTDay,ISINDICAT=@ISINDICAT,STATUS=@STATUS,REMARKS=@REMARKS,TRANSPORT=@TRANSPORT,ISCANCEL=@ISCANCEL,ISDEL=@ISDEL,DRIVER=@DRIVER,TWOLOGISTICS=@TWOLOGISTICS,TWOLOGISTICSNAME=@TWOLOGISTICSNAME WHERE INORDERNO=@INORDERNO";
            return DB<int>.Execute(sql, this) > 0;
        }


        /// <summary>
        /// 取消分配
        /// </summary>
        /// <returns></returns>
        public bool CancelDistribution()
        {
            string sql = "UPDATE qx_InAndOutBoundOrder SET STATUS=@STATUS,ISINDICAT=@ISINDICAT WHERE INORDERNO=@INORDERNO";
            return DB<int>.Execute(sql, new { INORDERNO = this.INORDERNO, STATUS = this.STATUS, ISINDICAT = this.ISINDICAT }) > 0;
        }
        /// <summary>
        /// 录入共生单号
        /// </summary>
        /// <returns></returns>
        public bool UpdategsNo()
        {
            string sql = @"UPDATE qx_InAndOutBoundOrder set gsNo=@gsNo where INORDERNO=@INORDERNO";
            return DB<int>.Execute(sql, new { gsNo = this.gsNo, INORDERNO = this.INORDERNO }) > 0;
        }

        /// <summary>
        /// 根据订单号模糊匹配该订单数
        /// </summary>
        /// <returns></returns>
        public int GetMaxCountByOrderNoLike()
        {
            string sql = @"select Count(1) FROM qx_InAndOutBoundOrder where INORDERNO like @INORDERNO";
            return DB<int>.QuerySingle(sql, new { INORDERNO = this.INORDERNO + "%" });
        }


        /// <summary>
        /// 保存拆分订单
        /// </summary>
        /// <returns></returns>
        public bool CreateOrder(string newOrderNo, DbConnection connection, DbTransaction dbTransaction)
        {
            string sql = @"insert into qx_InAndOutBoundOrder(
                           [INORDERNO]
                          ,[SAPORDERNO]
                          ,[ZORDERTYPE]
                          ,[DELIVERTIME]
                          ,[FROMPLANT]
                          ,[FROMPLANTDESC]
                          ,[FROMWAREHOUSE]
                          ,[FROMWAREHOUSEDESC]
                          ,[STROKORG]
                          ,[STROKGROUP]
                          ,[BUYTIME]
                          ,[SALEGROUP]
                          ,[SALEGROUPDESC]
                          ,[SALEOFFICE]
                          ,[SALEOFFICEDESC]
                          ,[CUSTOMER]
                          ,[CUSTOMERDESC]
                          ,[TOPLANT]
                          ,[TOPLANTDESC]
                          ,[TOWAREHOUSE]
                          ,[TOWAREHOUSEDESC]
                          ,[SALESACCOUNT]
                          ,[SALESPHONE]
                          ,[RECEIVECONPANY]
                          ,[RECEIVECONPANYDESC]
                          ,[TOADDRESS]
                          ,[TOCONTACT]
                          ,[TOPROVINCES]
                          ,[TOCITY1]
                          ,[TOCITY2]
                          ,[ORDERTYPE]
                          ,[ORDERTYPEDESC]
                          ,[STATUS]
                          ,[ISINDICAT]
                          ,[REMARKS]
                          ,[ENDTIME]
                          ,[RKCERTIFICATE]
                          ,[WBSNO]
                          ,[WBSNAME]
                          ,[TRANSPORT]
                          ,[CERTIF]
                          ,[SAPREMARKS]
                          ,[TEXTDESC]
                          ,[RETURNFLG]
                          ,[CREATEUSER]
                          ,[CREATETIME]
                          ,[SAPCREATEUSER]
                          ,[SAPCREATETIME]
                          ,[DELIVERY]
                          ,[OVERTIME]
                          ,[PLANLOADDATE]
                          ,[LOGISTICS]
                          ,[CARNUMBER]
                          ,[ISAUDIT]
                          ,[AUDITREMARKS]
                          ,[SREMARK]
                          ,[PREPRICE]
                          ,[REALPRICE]
                          ,[UPLOADFILE]
                          ,[RECFILE]
                          ,[CONTRACTNO]
                          ,[DRIVER]
                          ,[ISDEL]
                          ,[ISCANCEL]
                          ,[ISSPEC]
                          ,[TRANSTYPE]
                          ,[TRANSPORTMODE]
                          ,[ISSIGN]
                          ,[SIGHDATE]
                          ,[ISBACKCONFIRM]
                          ,[ISRECCONFIRM]
                          ,[MFORDER]
                          ,[EBELN]
                          ,TWOLOGISTICS,
                            TWOLOGISTICSNAME)               
                          select 
                           @NewOrderNo
                          ,[SAPORDERNO]
                          ,[ZORDERTYPE]
                          ,[DELIVERTIME]
                          ,[FROMPLANT]
                          ,[FROMPLANTDESC]
                          ,[FROMWAREHOUSE]
                          ,[FROMWAREHOUSEDESC]
                          ,[STROKORG]
                          ,[STROKGROUP]
                          ,[BUYTIME]
                          ,[SALEGROUP]
                          ,[SALEGROUPDESC]
                          ,[SALEOFFICE]
                          ,[SALEOFFICEDESC]
                          ,[CUSTOMER]
                          ,[CUSTOMERDESC]
                          ,[TOPLANT]
                          ,[TOPLANTDESC]
                          ,[TOWAREHOUSE]
                          ,[TOWAREHOUSEDESC]
                          ,[SALESACCOUNT]
                          ,[SALESPHONE]
                          ,[RECEIVECONPANY]
                          ,[RECEIVECONPANYDESC]
                          ,[TOADDRESS]
                          ,[TOCONTACT]
                          ,[TOPROVINCES]
                          ,[TOCITY1]
                          ,[TOCITY2]
                          ,[ORDERTYPE]
                          ,[ORDERTYPEDESC]
                          ,'00' [STATUS]
                          ,[ISINDICAT]
                          ,[REMARKS]
                          ,[ENDTIME]
                          ,[RKCERTIFICATE]
                          ,[WBSNO]
                          ,[WBSNAME]
                          ,[TRANSPORT]
                          ,[CERTIF]
                          ,[SAPREMARKS]
                          ,[TEXTDESC]
                          ,[RETURNFLG]
                          ,@CREATEUSER
                          ,getdate() [CREATETIME]
                          ,[SAPCREATEUSER]
                          ,[SAPCREATETIME]
                          ,[DELIVERY]
                          ,[OVERTIME]
                          ,[PLANLOADDATE]
                          ,[LOGISTICS]
                          ,[CARNUMBER]
                          ,[ISAUDIT]
                          ,[AUDITREMARKS]
                          ,[SREMARK]
                          ,[PREPRICE]
                          ,[REALPRICE]
                          ,[UPLOADFILE]
                          ,[RECFILE]
                          ,[CONTRACTNO]
                          ,[DRIVER]
                          ,[ISDEL]
                          ,[ISCANCEL]
                          ,[ISSPEC]
                          ,[TRANSTYPE]
                          ,[TRANSPORTMODE]
                          ,[ISSIGN]
                          ,[SIGHDATE]
                          ,[ISBACKCONFIRM]
                          ,[ISRECCONFIRM]
                          ,[MFORDER]
                          ,[EBELN],TWOLOGISTICS,TWOLOGISTICSNAME from dbo.qx_InAndOutBoundOrder where INORDERNO=@INORDERNO";
            return DB<int>.Execute(sql, new { NewOrderNo = newOrderNo, INORDERNO = this.INORDERNO, CREATEUSER = this.CREATEUSER }, connection, dbTransaction) > 0;
        }


        /// <summary>
        /// 获取订单明细物料号
        /// </summary>
        /// <returns></returns>
        public List<string> GetOrderDetailProcutNo()
        {
            string sql = @"select distinct b.PRODUCTNO from qx_InAndOutBoundOrder as a left join qx_InAndOutOrderProductDetail as b 
                           on a.inorderno = b.orderno where inorderno = @inorderno";
            return DB<string>.Query(sql, new { inorderno = this.INORDERNO });
        }


        /// <summary>
        /// 修改订单状态
        /// </summary>
        /// <returns></returns>
        public bool UpadateOrderStatus()
        {
            string sql = "update qx_InAndOutBoundOrder set Status=@Status,ISINDICAT='0' where INORDERNO = @INORDERNO";
            return DB<int>.Execute(sql, new { INORDERNO = this.INORDERNO, Status = this.STATUS }) > 0;
        }

        /// <summary>
        /// 修改订单状态
        /// </summary>
        /// <returns></returns>
        public bool UpadateOrderStatus(DbConnection connection, DbTransaction dbTransaction)
        {
            string sql = "update qx_InAndOutBoundOrder set Status=@Status,ISINDICAT='0' where INORDERNO = @INORDERNO";
            return DB<int>.Execute(sql, new { INORDERNO = this.INORDERNO, Status = this.STATUS }, connection, dbTransaction) > 0;
        }

        /// <summary>
        /// 修改订单为已签收
        /// </summary>
        /// <returns></returns>
        public bool UpadateOrderISSIGN()
        {
            string sql = "update qx_InAndOutBoundOrder set ISSIGN='1',SIGHDATE=getdate() where INORDERNO=@INORDERNO";
            return DB<int>.Execute(sql, new { INORDERNO = this.INORDERNO }) > 0;
        }
        /// <summary>
        /// 修改订单为已签收
        /// </summary>
        /// <returns></returns>
        public bool UpadateOrderISSIGN(DbConnection connection, DbTransaction dbTransaction)
        {
            string sql = "update qx_InAndOutBoundOrder set ISSIGN='1',SIGHDATE=getdate() where INORDERNO=@INORDERNO";
            return DB<int>.Execute(sql, new { INORDERNO = this.INORDERNO }, connection, dbTransaction) > 0;
        }

        /// <summary>
        /// 根据TMS单号获取SAP单号
        /// </summary>
        /// <returns></returns>
        public string GetSapOrderNoByTmsOrderNo()
        {
            string sql = "select SapOrderNo from qx_InAndOutBoundOrder WITH(NOLOCK)   where INORDERNO = @INORDERNO";
            return DB<string>.QuerySingleOrDefault(sql, new { INORDERNO = this.INORDERNO });
        }

        /// <summary>
        /// 获取签收数据
        /// </summary>
        /// <returns></returns>
        public List<qx_InAndOutBoundOrder> GetSSIGNData()
        {
            string sql = @"SELECT 
                           a.SAPORDERNO,
                           b.SAPORDERITEM,
                           CASE
                             WHEN Isnull(c.ISSIGN, 0) = 1 THEN '已签收'
                             WHEN Isnull(c.ISSIGN, 0) = 0 THEN '未签收'
                           END ISSIGN,
                           CASE
                             WHEN a.FRKCERTIFICATE IS NULL THEN a.RKCERTIFICATE
                             ELSE a.FRKCERTIFICATE
                           END FRKCERTIFICATE
                    FROM   qx_InAndOutBoundOrder AS a  with(nolock)
                           LEFT JOIN qx_InAndOutOrderProductDetail AS b with(nolock)
                                  ON a.INORDERNO = b.ORDERNO
                           LEFT JOIN (SELECT TMSORDERNO,
                                             ISSIGN
                                      FROM   dbo.qx_OrderTracing with(nolock)
                                      GROUP  BY TMSORDERNO,
                                                ISSIGN
                                      HAVING ISSIGN = 1) AS c
                                  ON a.INORDERNO = c.TMSORDERNO
                    WHERE  a.INORDERNO = @INORDERNO";
            return DB<qx_InAndOutBoundOrder>.Query(sql, new { INORDERNO = this.INORDERNO });
        }

        /// <summary>
        /// 获取订单类型
        /// </summary>
        /// <returns></returns>
        public List<qx_InAndOutBoundOrder> getZORDERTYPE()
        {
            string sql = @"select ZORDERTYPE,INORDERNO,SAPORDERNO,ISNULL(ISSIGN,0) ISSIGN
                            from qx_InAndOutBoundOrder WITH (NOLOCK)  where SAPORDERNO=@saporderno and ISCANCEL=0 and ISDEL=0 ";
            return DB<qx_InAndOutBoundOrder>.Query(sql, new { saporderno = this.SAPORDERNO });
        }

        //获取是否逾期信息
        public string GetYuqiInfo()
        {
            string sql = @"select top 1 case when CONVERT(varchar(100), endtime+ztday, 23) >= CONVERT(varchar(100), getdate(), 23) then 1 else 0 end notzt 
                           from qx_InAndOutBoundOrder where inorderno =@inorderno";

            return DB<string>.QuerySingleOrDefault(sql, new { inorderno = this.INORDERNO });
        }

        /// <summary>
        /// 修改订单二级物流商根据sap单号
        /// </summary>
        /// <returns></returns>
        public bool UpdateOrderTwoLogisticsBySapOrderNo(string logisticsNo)
        {
            string sql = @"update qx_InAndOutBoundOrder set TWOLOGISTICS=b.FORWARDERNO,TWOLOGISTICSNAME=b.FORWARDERNAME
                           from HY_Forwarder b where FORWARDERNAME =@TWOLOGISTICSNAME
                           AND SAPORDERNO = @SAPORDERNO and LOGISTICS = @LOGISTICS";
            return DB<int>.Execute(sql, new { SAPORDERNO = this.SAPORDERNO, TWOLOGISTICSNAME=this.TWOLOGISTICSNAME, LOGISTICS= logisticsNo }) > 0;
        }


        /// <summary>
        /// 修改订单信息根据订单编号
        /// </summary>
        /// <returns></returns>
        public bool UpdateOrderInfoByOrderNo()
        {
            string sql = @"update qx_InAndOutBoundOrder set DIRVERPLANARRIVEDATE=@DIRVERPLANARRIVEDATE,CARNUMBER=@CARNUMBER,DRIVER=@DRIVER,ActualModels=@ActualModels,
                           ActualModel_num=@ActualModel_num
                           WHERE INORDERNO = @INORDERNO";
            return DB<int>.Execute(sql, this) > 0;
        }

        /// <summary>
        ///批量修改订单信息根据订单编号
        /// </summary>
        /// <returns></returns>
        public bool UpdateOrderInfoByOrderNo(List<string> orderNoLst)
        {
            string sql = @"update qx_InAndOutBoundOrder set DIRVERPLANARRIVEDATE=@DIRVERPLANARRIVEDATE,CARNUMBER=@CARNUMBER,DRIVER=@DRIVER,ActualModels=@ActualModels,
                           ActualModel_num=@ActualModel_num
                           WHERE INORDERNO in @orderNoLst";
            return DB<int>.Execute(sql, new { DIRVERPLANARRIVEDATE=this.DIRVERPLANARRIVEDATE, CARNUMBER =this.CARNUMBER, DRIVER =this.DRIVER, ActualModels =this.ActualModels ,
                                              ActualModel_num=this.ActualModel_num,orderNoLst=orderNoLst}) > 0;
        }
    }
}

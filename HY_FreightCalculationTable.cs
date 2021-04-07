using HY.TMS.Common;
using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    /// <summary>
    /// 虹运TMS运费计算表
    /// </summary>
    public partial class HY_FreightCalculationTable
    {

        /// <summary>
        /// SAP操作日期 yyyy-mm-dd HH:mm:ss
        /// </summary>
        public string SapOperationDateFormart
        {
            get
            {
                if (this.SapOperationDate.HasValue && this.SapOperationDate.Value > DateTime.MinValue)
                {
                    return this.SapOperationDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// SAP过账日期 yyyy-mm-dd HH:mm:ss
        /// </summary>
        public string SapPostingDateFormart
        {
            get
            {
                if (this.SapPostingDate.HasValue && this.SapPostingDate.Value > DateTime.MinValue)
                {
                    return this.SapPostingDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// 计算日期 yyyy-mm-dd
        /// </summary>
        public string CalculationDateFormart
        {
            get
            {
                if (this.CalculationDate.HasValue && this.CalculationDate.Value > DateTime.MinValue)
                {
                    return this.CalculationDate.Value.ToString("yyyy-MM-dd");
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// 是否卸货名称
        /// </summary>
        public string IsUnloadingGoodsName
        {
            get
            {
                if (this.IsUnloadingGoods.HasValue)
                {
                    return this.IsUnloadingGoods.Value == 1 ? "是" : "否";
                }
                return "否";
            }
        }

        /// <summary>
        /// 厂长是否审核名称
        /// </summary>
        public string IsFactoryDirectorAuditName
        {
            get
            {
                if (this.FactoryDirectorAudit.HasValue)
                {
                    return this.FactoryDirectorAudit.Value == 1 ? "是" : "否";
                }
                return "否";
            }
        }

        /// <summary>
        /// 主任是否审核名称
        /// </summary>
        public string IsDirectorAuditName
        {
            get
            {
                if (this.DirectorAudit.HasValue)
                {
                    return this.DirectorAudit.Value == 1 ? "是" : "否";
                }
                return "否";
            }
        }

        /// <summary>
        /// 物料类型编码
        /// </summary>
        public string ProductTypeCode { get; set; }


        /// <summary>
        /// 工厂确认名称
        /// </summary>
        public string FactoryConfirmationName
        {
            get
            {
                if (this.FactoryConfirmation.HasValue)
                {
                    return this.FactoryConfirmation.Value == 1 ? "是" : "否";
                }
                return "否";
            }
        }

        public string LogisticsConfirmationName
        {
            get
            {
                if (LogisticsConfirmation.HasValue)
                {
                    return LogisticsConfirmation.Value == 1 ? "是" : "否";
                }
                return "否";
            }
        }


        /// <summary>
        /// 结算价
        /// </summary>
        public decimal SettlementPrice { get; set; }
        /// <summary>
        /// 结算重量
        /// </summary>
        public decimal SettingWeight { get; set; }
        /// <summary>
        /// 净算重量
        /// </summary>
        public decimal NetWeight { get; set; }
        /// <summary>
        /// 毛算重量
        /// </summary>
        public decimal GrossWeight { get; set; }
        /// <summary>
        ///不含税金额
        /// </summary>
        public decimal NotTaxPrice { get; set; }
        /// <summary>
        /// 按吨结算金额
        /// </summary>
        public decimal SettlementByTonPrice { get; set; }
        /// <summary>
        /// 危险品单价
        /// </summary>
        public decimal DangerousPrice { get; set; }
        /// <summary>
        /// 危险品重量
        /// </summary>
        public decimal DangerousWeight { get; set; }
        /// <summary>
        /// 危险品上浮比例
        /// </summary>
        public decimal DangerousUpRate { get; set; }
        /// <summary>
        /// 危险品总价
        /// </summary>
        public decimal DangerousSumPrice { get; set; }
        /// <summary>
        ///  保温品单价
        /// </summary>
        public decimal KeepWarmPrice { get; set; }
        /// <summary>
        ///  保温品重量
        /// </summary>
        public decimal KeepWarmWeight { get; set; }
        /// <summary>
        ///  保温品上浮比例
        /// </summary>
        public decimal KeepWarmUpRate { get; set; }
        /// <summary>
        /// 保温品总价
        /// </summary>
        public decimal KeepWarmSumPrice { get; set; }
        /// <summary>
        ///  横放品单价
        /// </summary>
        public decimal HorizontalPrice { get; set; }
        /// <summary>
        ///  横放品重量
        /// </summary>
        public decimal HorizontalWeight { get; set; }
        /// <summary>
        ///  横放品上浮比例
        /// </summary>
        public decimal HorizontalUpRate { get; set; }
        /// <summary>
        /// 横放品总价
        /// </summary>
        public decimal HorizontalSumPrice { get; set; }



        /// <summary>
        /// 卸货费
        /// </summary>
        public decimal DischargeCargoPrice { get; set; }

        /// <summary>
        /// 配送费
        /// </summary>
        public decimal DistributionPrice { get; set; }
        /// <summary>
        /// 加急费
        /// </summary>
        public decimal UrgentPrice { get; set; }

        /// <summary>
        /// 转运费
        /// </summary>
        public decimal TransportPrice { get; set; }

        /// <summary>
        /// 物流考核费用
        /// </summary>
        public decimal LogisticsFee { get; set; }


        /// <summary>
        /// 车牌号码
        /// </summary>
        public string CARNUMBER { get; set; }
        /// <summary>
        /// 项目名称（成品）
        /// </summary>
        public string WBSNAME { get; set; }
        /// <summary>
        /// 业务员
        /// </summary>
        public string SALESACCOUNT { get; set; }
        /// <summary>
        /// 装车地点
        /// </summary>
        public string FROMPLANTDESC { get; set; }
        /// <summary>
        /// 客户
        /// </summary>
        public string CUSTOMERDESC { get; set; }
        /// <summary>
        /// 收货人
        /// </summary>
        public string TOCONTACT { get; set; }
        /// <summary>
        /// 送货地址
        /// </summary>
        public string TOADDRESS { get; set; }
        /// <summary>
        /// 防水领域
        /// </summary>
        public string Filed { get; set; }
        /// <summary>
        /// 销售部门
        /// </summary>
        public string SaleOfficeDesc { get; set; }

        /// <summary>
        /// 销售/采购组织
        /// </summary>
        public string SalesOrganization { get; set; }
        /// <summary>
        /// 采购组
        /// </summary>
        public string STROKGROUPDESC { get; set; }
        /// <summary>
        /// 费用承担方式
        /// </summary>
        public string TRANSPORT { get; set; }
        /// <summary>
        /// 拆单附件
        /// </summary>
        public string UploadFile { get; set; }

        /// <summary>
        /// 应到货日期
        /// </summary>
        public string TimeCost { get; set; }
        public string TimeCostFormart
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(TimeCost))
                {
                    return Convert.ToDateTime(TimeCost).ToString("yyyy.MM.dd");
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// 实际到货日期
        /// </summary>
        public string TRACETIME { get; set; }
        public string TRACETIMEFormart
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(TRACETIME))
                {
                    return Convert.ToDateTime(TRACETIME).ToString("yyyy.MM.dd");
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// 应返单日期
        /// </summary>
        public string TIMEArrival { get; set; }
        public string TIMEArrivalFormart
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(TIMEArrival))
                {
                    return Convert.ToDateTime(TIMEArrival).ToString("yyyy.MM.dd");
                }
                return string.Empty;
            }
        }
        /// <summary>
        /// 返单日期是否合格
        /// </summary>
        public string ReturnTimeResult
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(ActualReturnTime) && !string.IsNullOrWhiteSpace(TIMEArrival))
                {
                    if (Convert.ToDateTime(ActualReturnTime).Date <= Convert.ToDateTime(TIMEArrival).Date)
                    {
                        return "合格";
                    }
                    else
                    {
                        return "不合格";
                    }
                }
                return string.Empty;
            }
        }


        /// <summary>
        /// 出库日期
        /// </summary>
        public string EndTime { get; set; }
        public string EndTimeFormart
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(EndTime))
                {
                    return Convert.ToDateTime(EndTime).ToString("yyyy.MM.dd");
                }
                return string.Empty;
            }
        }
        /// <summary>
        /// 实际返单日期
        /// </summary>
        public string ActualReturnTime { get; set; }
        public string ActualReturnTimeFormart
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(ActualReturnTime))
                {
                    return Convert.ToDateTime(ActualReturnTime).ToString("yyyy.MM.dd");
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// 返单不及时是否考核物流公司
        /// </summary>
        public int ReturnOrderAssessment { get; set; }
        /// <summary>
        /// 返单不及时是否考核物流公司
        /// </summary>
        public string ReturnOrderAssessmentName { get; set; }
        /// <summary>
        /// 到货不及时是否考核物流公司
        /// </summary>
        public int LateArrivalOfGoodsAssessment { get; set; }
        /// <summary>
        /// 到货不及时是否考核物流公司
        /// </summary>
        public string LateArrivalOfGoodsAssessmentName { get; set; }
        /// <summary>
        /// 签收人不符处理措施
        /// </summary>
        public string SignedPersonNotMatchDealName { get; set; }

        /// <summary>
        /// 到货不及时处罚金额
        /// </summary>
        public decimal LateArrivalOfGoodsPrice { get; set; }
        /// <summary>
        /// 返单不及时处罚金额
        /// </summary>
        public decimal ReturnOrderPrice { get; set; }

        /// <summary>
        /// 联系人/电话
        /// </summary>
        public string ToContact { get; set; }

        /// <summary>
        /// 实际签收人
        /// </summary>
        public string ActualSigner { get; set; }
        /// <summary>
        /// 签收人是否相符
        /// </summary>
        public int ActualSignerIsMatch { get; set; }
        /// <summary>
        /// 签收人不符处理措施
        /// </summary>
        public int SignedPersonNotMatchDeal { get; set; }

        /// <summary>
        /// 签收人是否相符
        /// </summary>
        public string ActualSignerIsMatchName { get; set; }
        /// <summary>
        /// 物流责任界定
        /// </summary>
        public int LogisticsResponsibilityDefinition { get; set; }
        /// <summary>
        /// 物流责任界定
        /// </summary>
        public string LogisticsResponsibilityDefinitionName { get; set; }
        /// <summary>
        /// 收货数量不符处理措施
        /// </summary>
        public string ReceivingGoodsDealName { get; set; }

        /// <summary>
        /// 签收人不符最后确定结果
        /// </summary>
        public int ActualSignerNotMatchReult { get; set; }

        /// <summary>
        /// 签收人不符最后确定结果
        /// </summary>
        public string ActualSignerNotMatchReultName { get; set; }

        /// <summary>
        /// 回单签收人不符是否处罚物流公司
        /// </summary>
        public int ReceiptSigningNotMatch { get; set; }

        /// <summary>
        /// 回单签收人不符是否处罚物流公司
        /// </summary>
        public string ReceiptSigningNotMatchName { get; set; }

        /// <summary>
        /// 回单签收人不符处罚金额
        /// </summary>
        public decimal ReceiptSigningNotMatchPrice { get; set; }

        /// <summary>
        /// 操作日期
        /// </summary>
        public string OprateTime { get; set; }
        /// <summary>
        /// 到货日期是否合格
        /// </summary>
        /// <returns></returns>
        public string IsMatchArriveTime
        {
            get
            {
                if (string.IsNullOrWhiteSpace(TRACETIME) || string.IsNullOrWhiteSpace(TimeCost))
                {
                    return "";
                }
                if (Convert.ToDateTime(TRACETIME) <= Convert.ToDateTime(TimeCost))
                {
                    return "合格";
                }
                else
                {
                    return "不合格";
                }
            }
        }

        /// <summary>
        /// 返单是否合格
        /// </summary>
        /// <returns></returns>
        public string IsMatchReturnOrder
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ActualReturnTime) || string.IsNullOrWhiteSpace(TIMEArrival))
                {
                    return "";
                }
                if (Convert.ToDateTime(ActualReturnTime) <= Convert.ToDateTime(TIMEArrival))
                {
                    return "合格";
                }
                else
                {
                    return "不合格";
                }
            }
        }
        /// <summary>
        /// 是否数量是否相符
        /// </summary>
        /// <returns></returns>
        public string IsReceivingGoodsDeal
        {
            get
            {
                if (ActualNum <= 0)
                {
                    return "";
                }
                if (ActualNum == ReqQty)
                {
                    return "是";
                }
                else
                {
                    return "否";
                }
            }
        }


        /// <summary>
        /// 毛重
        /// </summary>
        public decimal Wieght
        {
            get
            {
                return Math.Round(Convert.ToDecimal(this.ProductWeight * this.Number / 1000), 3);
            }
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public string STATUS { get; set; }

        /// <summary>
        /// 物流信息考核ID
        /// </summary>
        public int kpiID { get; set; }

        /// <summary>
        /// 处罚总金额
        /// </summary>
        public decimal TotalDealPrice
        {
            get
            {
                return this.LateArrivalOfGoodsPrice + this.ReturnOrderPrice + this.ReceiptSigningNotMatchPrice;
            }
        }

        ///// <summary>
        ///// 电子回单确认状态
        ///// </summary>
        //public int IsBackConfirm { get; set; }

        ///// <summary>
        ///// 录音文件上传确认状态
        ///// </summary>
        //public int IsSoundRecordConfirm { get; set; }
        /// <summary>
        /// 凭证
        /// </summary>
        public string RKCERTIFICATE { get; set; }
        /// <summary>
        /// 基本单位
        /// </summary>
        public string UnitDesc { get; set; }

        /// <summary>
        /// 基本单位交货数量
        /// </summary>
        public decimal ReqQty { get; set; }
        /// <summary>
        /// 实收数量
        /// </summary>
        public decimal ActualNum { get; set; }
        /// <summary>
        /// 实收数量
        /// </summary>
        public int ReceivingGoodsDeal { get; set; }
        /// <summary>
        /// 雨虹上传附件
        /// </summary>
        public string UPLOADFILE { get; set; }

        /// <summary>
        /// 二级物流商编号
        /// </summary>
        public string TwoLogsicNo { get; set; }
        /// <summary>
        /// 二级物流商名称
        /// </summary>
        public string TwoLogsicName { get; set; }

        /// <summary>
        /// 是否已计算
        /// </summary>
        /// <returns></returns>
        public bool IsCalculation()
        {
            string sql = "select top 1 SettlementNo from HY_FreightCalculationTable WITH(NOLOCK)  where OrderNo=@OrderNo and SettlementNo is not null and SettlementNo <>''";
            return DB<HY_FreightCalculationTable>.QueryFirstOrDefault(sql, new { OrderNo = this.OrderNo }) != null;
        }

        /// <summary>
        /// 是否已计算
        /// </summary>
        /// <returns></returns>
        public bool IsCalculation(List<int> IdList)
        {
            string sql = "select top 1 SettlementNo from HY_FreightCalculationTable WITH(NOLOCK)  where ID in @IDLst and SettlementNo is not null and SettlementNo <>''";
            return DB<HY_FreightCalculationTable>.QueryFirstOrDefault(sql, new { IDLst = IdList }) != null;
        }


        /// <summary>
        /// 新增运费计算信息
        /// </summary>
        /// <returns></returns>
        public bool Insert(DbConnection connection, DbTransaction dbTransaction)
        {
            string sql = @"INSERT INTO [dbo].[HY_FreightCalculationTable]
                                ([SapOriginalOrderNo],
                                 [SapNewOrderNo],
                                 [SapOriginalDeliveryOrderNo],
                                 [SapDeliveryOrderNo],
                                 [OrderNo],
                                 [TansportTurningPointId],
                                 [SapItemNo],
                                 [CarNumber],
                                 [SapPostingDate],
                                 [SapOperationDate],
                                 [CalculationDate],
                                 [ArriveProvince],
                                 [ArriveOneCity],
                                 [ArriveSecondCity],
                                 [ArriveAddress],
                                 [IsUnloadingGoods],
                                 [SapRemark],
                                 [BearMode],
                                 [TransportType],
                                 [ProductNo],
                                 [Number],
                                 [ProductWeight],
                                 [TotalWeight],
                                 [CalculationWeight],
                                 [OutboundOrderTotalWeight],
                                 [MergeTotalWeight],
                                 [RealPrice],
                                 [OutboundOrderTotalPrice],
                                 [FactoryCommitmentPrice],
                                 [MergeTotalPrice],
                                 [DeliveryFee],
                                 [CalculationProcess],
                                 [TotalPrice],
                                 [CustomerCommitmentPrice],
                                 [CustomerCommitmentDesc],
                                 [OutsideFactoryCommitmentPrice],
                                 [OutsideFactoryCommitmentDesc],
                                 [SaleFactoryCommitmentPrice],
                                 [SaleFactoryCommitmentDesc],
                                 [TransportFee],
                                 [UrgentFee],
                                 [MergeOrderNo],
                                 [ForwarderNo],
                                 [ForwarderName],
                                 [SettlementNo],
                                 [DeliveryFactory],
                                 [ProductType],
                                 [SaleOffice],
                                 [CustomerName],
                                 [ShipmentPlace],
                                 [FactoryConfirmation],
                                 [LogisticsRemark])
                    SELECT 
                           h.ysaporderno            SapOriginalOrderNo,--SAP原订单号
                           b.SAPORDERNO             SapNewOrderNo,--Sap新订单号
                           h.yrkCertificate         SapOriginalDeliveryOrderNo,--SAP原出库单号
                           b.RKCERTIFICATE          SapDeliveryOrderNo,--SAP出库单号
                           a.ORDERNO                OrderNo,--订单号
                           @TansportTurningPointId  TansportTurningPointId,--转点ID
                           a.SAPORDERITEM           SapItemNo, --Sap行项目
                           @CarNumber               CarNumber, --车牌号码
                           b.ENDTIME                SapPostingDate,--SAP过账日期
                           b.SAPCREATETIME          SapOperationDate,--SAP操作日期
                           NULL                       CalculationDate,--计算日期
                           @ArriveProvince          ArriveProvince,--到货省份
                           @ArriveOneCity           ArriveOneCity,--一级市
                           @ArriveSecondCity        ArriveSecondCity,--二级市
                           b.TOADDRESS              ArriveAddress,--到货地址
                           0                        IsUnloadingGoods,--是否卸货
                           b.SAPREMARKS             SapRemark,--SAP备注
                           b.TRANSPORT              BearMode,--承担方式
                           @TransportType           TransportType,--运输方式
                           a.PRODUCTNO              ProductNo,--物料号
                           a.REQ_QTY                Number,--数量
                           a.WEIGHT                 ProductWeight,--物料毛重(T)
                           Isnull(a.detweightup, 0) TotalWeight,--合计毛重(T)
                           0                        CalculationWeight,--计算重量(T) 
                           0                        OutboundOrderTotalWeight,--出库单合计重量(T)
                           0                        MergeTotalWeight,--合计总重量(T)
                           0                        RealPrice,--实际费用
                           0                        OutboundOrderTotalPrice,--出库单总费用
                           0                        FactoryCommitmentPrice,--工厂承担
                           0                        MergeTotalPrice,--合并总金额
                           0                        DeliveryFee,--配送费
                           ''                       CalculationProcess,--计算过程
                           0                        TotalPrice,--总运费
                           0                        CustomerCommitmentPrice,--客户承担金额
                           ''                       CustomerCommitmentDesc,--客户承担叙述
                           0                        OutsideFactoryCommitmentPrice,--外工厂承担金额
                           ''                       OutsideFactoryCommitmentDesc,--外工厂承担叙述
                           0                        SaleFactoryCommitmentPrice,--销售承担金额
                           ''                       SaleFactoryCommitmentDesc,--销售承担叙述
                           0                        TransportFee,--转运费
                           0                        UrgentFee,--加急费
                           ''                       MergeOrderNo,--合并单号
                           @ForwarderNo             ForwarderNo,--物流运输商
                           @ForwarderName           ForwarderName,--物流运输商名称
                           ''                       SettlementNo,--结费单号
                           b.FROMPLANT              DeliveryFactory,--发货工厂
                           CASE
                             WHEN a.spemt = 'D00' THEN '危险品'
                             WHEN a.spemt = 'D01' THEN '保温品'
                             WHEN a.spemt = 'D02' THEN '横放'
                             ELSE ''
                           END                      ProductType,--物料种类
                           b.saleofficedesc         SaleOffice,--销售办公室
                           b.CUSTOMERDESC           CustomerName,--客户名称
                           @ShipmentPlace           ShipmentPlace,--起运地
                           ''                       FactoryConfirmation,--工厂确认
                           ''                       LogisticsRemark --物流备注
                          
                    FROM   qx_InAndOutOrderProductDetail a
                           LEFT JOIN qx_InAndOutBoundOrder b
                                  ON b.INORDERNO = a.ORDERNO
                           LEFT JOIN dbo.V_WMS_CERTISFACTORYMAPPING AS h
                                  ON b.RKCERTIFICATE = h.rkCertificate
                           LEFT JOIN dbo.qx_KeyValue k
                                  ON k.typeno = 1
                                     AND k.SKEY = b.TRANSPORTMODE
                    WHERE b.INORDERNO=@INORDERNO";
            return DB<int>.Execute(sql, new
            {
                INORDERNO = this.OrderNo,
                ArriveProvince = this.ArriveProvince,
                ArriveOneCity = this.ArriveOneCity,                          
                ArriveSecondCity = this.ArriveSecondCity,
                TransportType = this.TransportType,
                ForwarderNo = this.TwoLogsicNo,
                ForwarderName = this.TwoLogsicName,
                ShipmentPlace = this.ShipmentPlace,
                TansportTurningPointId = this.TansportTurningPointId,
                CarNumber=this.CARNUMBER
            }, connection, dbTransaction) > 0;
        }


        /// <summary>
        /// 获取计算数据表信息
        /// </summary>
        public List<HY_FreightCalculationTable> GetFreightCalculationTableList(string where, string orderby, int pageIndex, int pageSize, out int total)
        {
            string files = "*";
            string tableName = @"HY_FreightCalculationTable";
            List<Conditions> conditions = where.GetModel<List<Conditions>>();
            where = conditions.toSqlString();
            where += " and ISNULL(IsHide,0)=0 ";
            return DB<HY_FreightCalculationTable>.GetPageList(files, tableName, where, orderby, pageIndex, pageSize, out total);
        }

        /// <summary>
        /// 获取物流商确认运费分页信息
        /// </summary>
        public List<HY_FreightCalculationTable> GetForOrderFeeHeadPageList(string where, string orderby, int pageIndex, int pageSize, out int total)
        {
            string files = "*";
            string tableName = "HY_FreightCalculationTable WITH(NOLOCK)";
            List<Conditions> conditions = where.GetModel<List<Conditions>>();
            where = conditions.toSqlString();
            where += " and ISNULL(IsHide,0)=0 and SettlementNo is not null and SettlementNo <>''";
            if (qx_User.CurrentUser.FACTORYNO != "-1" && qx_User.CurrentUser.FACTORYNO != "")
            {
                where += " and DeliveryFactory='" + qx_User.CurrentUser.FACTORYNO + "'";
            }
            if (qx_User.CurrentUser.FORWARDERNO != "-1" && qx_User.CurrentUser.FORWARDERNO != "")
            {
                where += " and ForwarderNo='" + qx_User.CurrentUser.FORWARDERNO + "'";
            }
            return DB<HY_FreightCalculationTable>.GetPageList(files, tableName, where, orderby, pageIndex, pageSize, out total);
        }

        /// <summary>
        /// 修改是否卸货
        /// </summary>
        /// <returns></returns>
        public bool UpdateIsUnloadingGoods(List<int> idLst)
        {
            string sql = "update HY_FreightCalculationTable set IsUnloadingGoods=1 where Id in @IdLst";
            return DB<int>.Execute(sql, new { IdLst = idLst }) > 0;
        }

        /// <summary>
        /// 获取运费计算信息
        /// </summary>
        /// <param name="codeLst">订单号列表</param>
        /// <returns></returns>
        public List<HY_FreightCalculationTable> GetFreightCalculationTableList(List<int> IdLst)
        {
            string sql = @"select a.*,b.Code ProductTypeCode,f.FORWARDERNO TwoLogsicNo,f.FORWARDERNAME TwoLogsicName from HY_FreightCalculationTable a WITH(NOLOCK) 
                           left join   [dbo].[qx_InAndOutBoundOrder] c on c.[INORDERNO]=a.OrderNO
						  left join HY_Forwarder f on f.[FORWARDERNO]=c.TWOLOGISTICS
                          left join DicItem b WITH(NOLOCK) on b.Value=a.ProductType
                         where  a.ID in @IdLst";
            return DB<HY_FreightCalculationTable>.Query(sql, new { IdLst = IdLst });
        }

        /// <summary>
        /// 二级市没有维护价格，同步一级城市为二级市
        /// </summary>
        /// <param name="Codes"></param>
        /// <returns></returns>
        public bool SyncTwoCity(List<int> IDLst)
        {
            string sql = @"select [SapDeliveryOrderNo],ArriveSecondCity
                           into #temp
                            from [dbo].[HY_FreightCalculationTable] a 
                            left join [dbo].[HY_TansportPriceTable] b on a.[ArriveProvince]=b.ArriveProvince
                            where a.[ArriveSecondCity]=b.[ArriveCity] and b.[FactoryNo]=@FactoryNo and a.ID in @IdLst   and b.[Price] is null 
                            and a.[ArriveSecondCity] not like '%县' and  not exists (select 1 from [dbo].[HY_Specialcity_Maintain] where [City]=ArriveOneCity)

                            UPDATE HY_FreightCalculationTable set  ArriveSecondCity= [ArriveOneCity]
                            where exists (select 1 from #temp where #temp.SapDeliveryOrderNo=HY_FreightCalculationTable.SapDeliveryOrderNo)
                            drop table #temp";
            return DB<int>.Execute(sql, new { FactoryNo = qx_User.CurrentUser.FACTORYNO, IdLst = IDLst }) > 0;
        }

        public bool Update()
        {
            string sql = string.Format(@"update HY_FreightCalculationTable set RealPrice =@RealPrice,OutboundOrderTotalPrice =@OutboundOrderTotalPrice,CalculationProcess =@CalculationProcess,SettlementNo=@SettlementNo,OnTheWayDay=@OnTheWayDay,ReturnOrderDay=@ReturnOrderDay,MergeOrderNo=@MergeOrderNo
                                       ,CalculationDate=@CalculationDate,Modify=@Modify,ModifyDate=@ModifyDate where ID =@ID");
            return DB<int>.Execute(sql, new
            {
                RealPrice = this.RealPrice,
                OutboundOrderTotalPrice = this.OutboundOrderTotalPrice,
                CalculationProcess = this.CalculationProcess,
                SettlementNo = this.SettlementNo,
                OnTheWayDay = this.OnTheWayDay.Value,
                ReturnOrderDay = this.ReturnOrderDay.Value,
                MergeOrderNo = this.MergeOrderNo,
                CalculationDate = this.CalculationDate,
                ID = this.ID,
                Modify = this.Modify,
                ModifyDate = this.ModifyDate
            }) > 0;
        }


        public bool UpdateCalculationFailData()
        {
            string sql = string.Format(@"update HY_FreightCalculationTable set RealPrice =@RealPrice,CalculationProcess =@CalculationProcess,SettlementNo=@SettlementNo
                                         ,MergeOrderNo=@MergeOrderNo,Modify=@Modify,ModifyDate=@ModifyDate where ID =@ID");
            return DB<int>.Execute(sql, new
            {
                RealPrice = this.RealPrice,
                CalculationProcess = this.CalculationProcess,
                SettlementNo = this.SettlementNo,
                MergeOrderNo = this.MergeOrderNo,
                ID = this.ID,
                Modify = this.Modify,
                ModifyDate = this.ModifyDate
            }) > 0;
        }

        /// <summary>
        /// 调整物流商获取计算日期
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public bool AdjustmentForwardOrCalculationDate(string sql, List<int> idLst)
        {
            return DB<int>.Execute(sql, new { ForwarderNo = this.ForwarderNo, ForwarderName = this.ForwarderName, CalculationDate = this.CalculationDate, IdLst = idLst }) > 0;
        }

        /// <summary>
        /// 调整自身信息
        /// </summary>
        /// <returns></returns>
        public bool AdjustmentSelf()
        {
            string sql = @"update HY_FreightCalculationTable set CalculationWeight =@CalculationWeight,SaleOffice =@SaleOffice,ArriveProvince =@ArriveProvince,
                           ArriveOneCity=@ArriveOneCity,ArriveSecondCity=@ArriveSecondCity,ForwarderNo=@ForwarderNo,ForwarderName=@ForwarderName
                           ,CustomerCommitmentPrice=@CustomerCommitmentPrice ,CustomerCommitmentDesc=@CustomerCommitmentDesc
                           ,OutsideFactoryCommitmentPrice=@OutsideFactoryCommitmentPrice,OutsideFactoryCommitmentDesc=@OutsideFactoryCommitmentDesc
                           ,SaleFactoryCommitmentPrice=@SaleFactoryCommitmentPrice ,SaleFactoryCommitmentDesc=@SaleFactoryCommitmentDesc,TransportFee=@TransportFee
                           ,UrgentFee=@UrgentFee,AdjustmentAmount=@AdjustmentAmount,AdjustmentNotes=@AdjustmentNotes,FactoryRemark=@FactoryRemark
                           where Id =@Id";
            return DB<int>.Execute(sql, this) > 0;
        }
        /// <summary>
        /// 工厂确认
        /// </summary>
        /// <returns></returns>
        public bool FactoryConfirmationAction(List<int> idList)
        {
            string sql = @"update HY_FreightCalculationTable set  FactoryConfirmation=@FactoryConfirmation
                           where Id in @IdLst";
            return DB<int>.Execute(sql, new { FactoryConfirmation = 1, IdLst = idList }) > 0;
        }

        /// <summary>
        /// 隐藏订单
        /// </summary>
        /// <returns></returns>
        public bool HideOrder(List<int> idList)
        {
            string sql = @"update HY_FreightCalculationTable set  IsHide=@IsHide
                           where Id in @IdLst";
            return DB<int>.Execute(sql, new { IsHide = 1, IdLst = idList }) > 0;
        }


        /// <summary>
        /// 根据订单编号集合获取数据
        /// </summary>
        /// <returns></returns>
        public List<HY_FreightCalculationTable> GetModelByOrderList(List<int> idList)
        {
            string sql = @"select * from  HY_FreightCalculationTable WITH(NOLOCK) where Id in @idList";
            return DB<HY_FreightCalculationTable>.Query(sql, new { idList = idList.Distinct() });
        }

        /// <summary>
        /// 修改省市数据
        /// </summary>
        /// <returns></returns>
        public bool UpdateCityAndProvinceData(List<int> idLst)
        {
            string sql = @"UPDATE HY_FreightCalculationTable SET ArriveProvince =@ArriveProvince,
                           ArriveOneCity=@ArriveOneCity,ArriveSecondCity=@ArriveSecondCity,Modify=@Modify,ModifyDate=GETDATE()
                           where id in @IdLst";
            return DB<int>.Execute(sql, new { ArriveProvince = this.ArriveProvince, ArriveOneCity = this.ArriveOneCity, ArriveSecondCity = this.ArriveSecondCity, Modify = this.Modify, IdLst = idLst }) > 0;
        }

        /// <summary>
        /// 取消结算
        /// </summary>
        /// <param name="settmentNoLst"></param>
        /// <returns></returns>
        public bool CancelCalculationFreight(List<int> IDLst)
        {
            string sql = @"update HY_FreightCalculationTable set RealPrice =0,OutboundOrderTotalPrice =0,CalculationProcess ='',SettlementNo='',OnTheWayDay='',ReturnOrderDay='',MergeOrderNo='',CalculationDate=NULL
                           where ID in @IDLst";
            return DB<int>.Execute(sql, new { IDLst = IDLst }) > 0;
        }


        /// <summary>
        /// 修改物流是否确认
        /// </summary>
        /// <returns></returns>
        public bool UpdateLogisticsConfirmation(List<string> settmentNoLst, List<string> margeOrderNoLst)
        {
            string sql = @"update HY_FreightCalculationTable set LogisticsConfirmation=@LogisticsConfirmation
                           where SettlementNo in @settmentNoLst and MergeOrderNo in @MergeOrderNo";
            return DB<int>.Execute(sql, new { settmentNoLst = settmentNoLst, MergeOrderNo = this.MergeOrderNo, LogisticsConfirmation = this.LogisticsConfirmation }) > 0;
        }


        /// <summary>
        /// 维护物流备注
        /// </summary>
        /// <returns></returns>
        public bool UpdateLogisticsConRemark()
        {
            string sql = @"update HY_FreightCalculationTable set LogisticsRemark=@LogisticsRemark
                           where ID=@ID";
            return DB<int>.Execute(sql, new { Id = this.ID, LogisticsRemark = this.LogisticsRemark }) > 0;
        }


        /// <summary>
        /// 获取成品运费计算表信息
        /// </summary>
        public List<HY_FreightCalculationTable> GetAuditingFeeList(string where, string orderby, int pageIndex, int pageSize, out int total)
        {
            string files = @" a.[SapPostingDate],--发货单日期
	                          a.CalculationDate, --计算日期
	                          a.[SapNewOrderNo], --sap订单号
                              a.SapOriginalDeliveryOrderNo, --sap原发货单号
	                          a.[SapDeliveryOrderNo], --sap发货单号
	                          a.[DeliveryFactory], --发货工厂
	                          a.OrderNo, --订单号码
	                          a.CarNumber, --车牌号
	                          b.WBSNAME, --项目名称（成品）
                              b.SALESACCOUNT, --业务员
	                          b.FROMPLANTDESC, --装车地点
	                          b.CUSTOMERDESC, --客户
	                          b.TOCONTACT, --收货人
	                          b.TOADDRESS,--送货地址
	                          a.MergeOrderNo, --合并单号
	                          Filed, --防水领域
	                          a.[ProductWeight] ,--物料毛重 净重，毛重，结算重量=物料毛重*数量/1000 
	                          a.[Number], --数量
	                          e.SettlementPrice, --结算价 按吨结算金额=结算重量为空为0，不为空，用结算重量 * 结算单价
	                          DangerousPrice, --危险品单价
	                          DangerousWeight,--危险品重量
	                          DangerousUpRate, --危险品上浮比例
	                          DangerousSumPrice,--危险品总价
	                          KeepWarmPrice, --保温品单价
	                          KeepWarmWeight,--保温品重量
	                          KeepWarmUpRate,--保温品上浮比例
	                          KeepWarmSumPrice, --保温品总价
	                          HorizontalPrice,--横放品单价
	                          HorizontalWeight, --横放品重量
	                          HorizontalUpRate, --横放品上浮比例
	                          HorizontalSumPrice,--横放品总价
                              DischargeCargoPrice,--卸货费
	                          a.AdjustmentAmount,--其他费用
	                          UrgentPrice,--加急费用
	                          DistributionPrice,--配送费
	                          TransportPrice, --转运费用
	                          0 LogisticsFee,--物流考核费用
	                          0 FinanceFee,--财务考核费用
	                          a.[CalculationProcess], --计算过程
	                          0 SumPrice,--总运费
	                          a.FactoryCommitmentPrice, --工厂承担
	                          a.OutsideFactoryCommitmentPrice, --外工厂承担
	                          a.SaleFactoryCommitmentPrice, --销售承担
	                          a.CustomerCommitmentPrice, --客户承担
	                          0 NotTaxPrice,--不含税金额 =工厂承担金额 / 1.11 保留两位小数 四舍五入
	                          b.SaleOfficeDesc, --销售部门
	                          b.SALEORG+'/'+ b.TOPLANT SalesOrganization,--销售/采购组织
	                          b.STROKGROUPDESC, --采购组
	                          b.TRANSPORT, --费用承担方式
	                          a.AdjustmentNotes, --其他金额备注
	                          a.OutsideFactoryCommitmentDesc, --外工厂承担叙述
	                          a.DirectorAudit, --主任审核
	                          a.FactoryDirectorAudit, --厂长审核
	                          a.FactoryRemark, --工厂备注
	                          f.UploadFile, --拆单附件
                              a.ModifyDate";
            string tableName = @"[dbo].[HY_FreightCalculationTable] a
                                 left join [dbo].[HY_SettlementPrice] e on e.ItemId=a.Id
                                 left join [dbo].[qx_InAndOutBoundOrder] b on b.INORDERNO=a.[OrderNo]
                                 left join qx_chaidanReason f on f.SAPORDERNO=a.SapNewOrderNo
                                 left join (SELECT CKPZNo,
                                               Filed,
                                               Row_number()
                                                 OVER (
                                                   PARTITION BY CKPZNo
                                                   ORDER BY Filed) rk
                                        FROM   (SELECT *
                                                FROM   (SELECT CKPZNo,
                                                               Filed
                                                        FROM   qx_ProductPZ_SAP
                                                        WHERE  isnull(ProductType,'') ='FERT' or isnull(ProductType,'') ='ERSA') a
                                                GROUP  BY CKPZNo,
                                                          Filed) a) c on c.CKPZNo=b.RKCERTIFICATE";
            List<Conditions> conditions = where.GetModel<List<Conditions>>();
            where = conditions.toSqlString();
            where += " and ISNULL(a.IsHide,0)=0 and isnull(SettlementNo,0) > 0"; /*and isnull(SettlementNo,0) > 0*/
            return DB<HY_FreightCalculationTable>.GetPageList(files, tableName, where, orderby, pageIndex, pageSize, out total);
        }

        /// <summary>
        /// 主任单个审核
        /// </summary>
        /// <returns></returns>
        public bool UpdateSingleDirectorAudit()
        {
            string sql = "UPDATE HY_FreightCalculationTable SET DirectorAudit=1,Modify=@Modify,ModifyDate=@ModifyDate WHERE OrderNo=@OrderNo";
            return DB<int>.Execute(sql, this) > 0;
        }

        /// <summary>
        /// 主任一键审核
        /// </summary>
        /// <returns></returns>
        public bool UpdateAllDirectorAudit(string where)
        {
            List<Conditions> conditions = where.GetModel<List<Conditions>>();
            where = conditions.toSqlString();
            string sql = "UPDATE HY_FreightCalculationTable SET DirectorAudit=@DirectorAudit,Modify=@Modify,ModifyDate=@ModifyDate WHERE " + where;
            return DB<int>.Execute(sql, this) > 0;
        }

        /// <summary>
        /// 厂长单个审核
        /// </summary>
        /// <returns></returns>
        public bool UpdateSingleFactoryDirectorAudit()
        {
            string sql = "UPDATE HY_FreightCalculationTable SET FactoryDirectorAudit=1,Modify=@Modify,ModifyDate=@ModifyDate WHERE OrderNo=@OrderNo";
            return DB<int>.Execute(sql, this) > 0;
        }

        /// <summary>
        /// 厂长一键审核
        /// </summary>
        /// <returns></returns>
        public bool UpdateAllFactoryDirectorAudit(string where)
        {
            List<Conditions> conditions = where.GetModel<List<Conditions>>();
            where = conditions.toSqlString();
            string sql = "UPDATE HY_FreightCalculationTable SET FactoryDirectorAudit=@FactoryDirectorAudit,Modify=@Modify,ModifyDate=@ModifyDate WHERE " + where;
            return DB<int>.Execute(sql, this) > 0;
        }

        /// <summary>
        /// 生成财务编号
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public bool GenarateFinanceCode(string where)
        {
            List<Conditions> conditions = where.GetModel<List<Conditions>>();
            where = conditions.toSqlString();
            string sql = "select ID,FinanceCode from HY_FreightCalculationTable  WHERE " + where;
            var lst = DB<HY_FreightCalculationTable>.Query(sql, this);
            int index = 0;
            var reuslt = 0;
            if (lst != null && lst.Count > 0)
            {
                sql = "select MAX(isnull(FinanceCode,0)) FinanceCode from HY_FreightCalculationTable where month(FinanceTime)=@NowMonth and Year(FinanceTime)=@NowYear";
                var maxFinanceCode = DB<string>.QuerySingleOrDefault(sql, new { NowMonth = DateTime.Now.Month, NowYear = DateTime.Now.Year });
                index = Convert.ToInt32(maxFinanceCode);
                foreach (var item in lst)
                {
                    if (string.IsNullOrWhiteSpace(item.FinanceCode))
                    {
                        index++;
                        sql = "UPDATE HY_FreightCalculationTable SET FinanceCode=@FinanceCode,FinanceTime=@FinanceTime,Modify=@Modify,ModifyDate=@ModifyDate WHERE ID=@ID";
                        reuslt += DB<int>.Execute(sql, new { FinanceCode = index, FinanceTime = this.FinanceTime, Modify = this.Modify, ModifyDate = this.ModifyDate, ID = item.ID });
                    }
                    else
                    {
                        index--;
                    }
                }
            }
            return reuslt > 0;
        }

        /// <summary>
        /// 财务金额调整
        /// </summary>
        /// <returns></returns>
        public bool FinaceFeeUpdate(string where)
        {
            List<Conditions> conditions = where.GetModel<List<Conditions>>();
            where = conditions.toSqlString();
            string sql = "UPDATE HY_FreightCalculationTable SET FinanceFee=@FinanceFee,Modify=@Modify,ModifyDate=@ModifyDate WHERE " + where;
            return DB<int>.Execute(sql, this) > 0;
        }

        /// <summary>
        /// 获取物流考核数据信息
        /// </summary>
        public List<HY_FreightCalculationTable> GetLogisicKPIList(string where, string orderby, int pageIndex, int pageSize, out int total)
        {
            string files = @"  a.ID, --运费计算表ID
                               h.ID kpiID,--物流考核表ID
                               a.OrderNo,--订单编号
                               (case when c.Value is null then '已接收' else c.Value end) STATUS, --订单状态
	                           a.SapDeliveryOrderNo, --新SAP出库单号	
	                           a.DeliveryFactory,--工厂
	                           b.ENDTIME EndTime, --出库日期
	                           b.TIMECOST TimeCost, --应到货日期
	                           f.TRACETIME,--实际到货日期
	                           a.ForwarderName, --物流商名称
	                           h.LateArrivalOfGoodsAssessment,--到货不及时是否考核物流公司
							   aa.Value LateArrivalOfGoodsAssessmentName,
	                           h.LateArrivalOfGoodsPrice,--到货不及时处罚金额
	                           b.TIMEArrival, --应返单日期
	                            d.ActualReturnTime, --实际返单日期
		                        h.ReturnOrderAssessment, --返单不及时是否考核物流公司
								bb.Value ReturnOrderAssessmentName,
		                        h.ReturnOrderPrice,--返单不及时处罚金额
	                            b.ToContact, --收货人姓名/电话
		                        d.ActualSigner, --实际签收人
		                        d.ActualSignerIsMatch,--签收人是否相符
								cc.Value ActualSignerIsMatchName,
		                        dd.Value LogisticsResponsibilityDefinition, --物流责任界定
	                            gg.Value LogisticsResponsibilityDefinitionName,
		                        d.ActualSignerNotMatchReult, --签收人不符最后确定结果
								ee.Value  ActualSignerNotMatchReultName,
		                        h.ReceiptSigningNotMatch,--回单签收人不符是否处罚物流公司
								ff.Value ReceiptSigningNotMatchName,
		                        h.ReceiptSigningNotMatchPrice,--回单签收人不符处罚金额
		                        a.ProductWeight,--物料重量
		                        a.Number --数量";
            string tableName = @"[dbo].[HY_FreightCalculationTable] a
                                 left OUTER JOIN HY_Logistics_KPI h on h.FreightCalculationTableId=a.ID
                                 left join [dbo].[HY_Sign] d on d.FreightCalculationTableId=a.ID
                                 left join [dbo].[qx_InAndOutBoundOrder] b on a.OrderNo=b.[INORDERNO]
                                 left join (select TMSORDERNO,MAX(TRACETIME) TRACETIME from qx_OrderTracing group by TMSORDERNO)  f  on f.TMSORDERNO=a.OrderNo
                                 left join DicItem c on c.Code=a.OrderFeeStatus and c.CodeType='OrderStatus'
                                 left join DicItem aa on aa.Code=h.LateArrivalOfGoodsAssessment and aa.CodeType='LateArrivalOfGoodsAssessment'
                                 left join DicItem bb on bb.Code=h.ReturnOrderAssessment and bb.CodeType='ReturnOrderAssessment'
                                 left join DicItem cc on cc.Code=d.ActualSignerIsMatch and cc.CodeType='ActualSignerIsMatch'
                                 left join DicItem dd on dd.Code=d.LogisticsResponsibilityDefinition and dd.CodeType='LogisticsResponsibilityDefinition'
                                 left join DicItem ee on ee.Code=d.ActualSignerNotMatchReult and ee.CodeType='ActualSignerNotMatchReult'
                                 left join DicItem ff on ff.Code=h.ReceiptSigningNotMatch and ff.CodeType='ReceiptSigningNotMatch'
			                     left join DicItem gg on gg.Code=d.LogisticsResponsibilityDefinition and gg.CodeType='LogisticsResponsibilityDefinition'
                                 left join qx_InAndOutBoundOrder_SAP as e on a.SapNewOrderNo = e.SAPORDERNO and a.SapItemNo=e.SAPITEMNO";
            List<Conditions> conditions = where.GetModel<List<Conditions>>();
            where = conditions.toSqlString();
            where += " and ISNULL(a.IsHide,0)=0 and ISNULL(a.Status,0)>=30 ";
            if (qx_User.CurrentUser.FACTORYNO != "" && qx_User.CurrentUser.FACTORYNO != "-1")
            {
                where += " and a.DeliveryFactory='" + qx_User.CurrentUser.FACTORYNO + "'";
            }
            return DB<HY_FreightCalculationTable>.GetPageList(files, tableName, where, orderby, pageIndex, pageSize, out total);
        }
        /// <summary>
        /// 是否存在已审核
        /// </summary>
        /// <param name="idLst"></param>
        /// <returns></returns>
        public bool IsAssessmentLogisicKPI(List<int> idLst)
        {
            string sql = "select Count(1) from [dbo].[HY_FreightCalculationTable] where ID in @IdLst  and OrderFeeStatus = 60";
            return DB<int>.QuerySingle(sql, new { idLst = idLst, Modify = this.Modify, ModifyDate = this.ModifyDate }) > 0;
        }

        /// <summary>
        /// 考核物流信息
        /// </summary>
        /// <returns></returns>
        public bool AssessmentLogisicKPI(List<int> idLst)
        {
            string sql = "Update [dbo].[HY_FreightCalculationTable] set OrderFeeStatus=60,Modify=@Modify,ModifyDate=@ModifyDate where ID in @IdLst";
            return DB<int>.Execute(sql, new { idLst = idLst, Modify = this.Modify, ModifyDate = this.ModifyDate }) > 0;
        }

        /// <summary>
        /// 是否存在已接收
        /// </summary>
        /// <param name="idLst"></param>
        /// <returns></returns>
        public bool IsExistsLogisicKPI(List<int> idLst)
        {
            string sql = "select Count(1) from [dbo].[HY_FreightCalculationTable] where ID in @IdLst and OrderFeeStatus = null";
            return DB<int>.QuerySingle(sql, new { idLst = idLst, Modify = this.Modify, ModifyDate = this.ModifyDate }) > 0;
        }
        /// <summary>
        /// 是否已签收回单
        /// </summary>
        /// <param name="idLst"></param>
        /// <returns></returns>
        public bool IsExistsReceiptSign(List<int> idLst)
        {
            string sql = "select Count(1) from [dbo].[HY_FreightCalculationTable] where ID in @IdLst and OrderFeeStatus = 30";
            return DB<int>.QuerySingle(sql, new { idLst = idLst, Modify = this.Modify, ModifyDate = this.ModifyDate }) > 0;
        }

        /// <summary>
        /// 回单签收
        /// </summary>
        /// <returns></returns>
        public bool ReceiptSign(List<int> idLst)
        {
            string sql = "Update [dbo].[HY_FreightCalculationTable] set Status=30,OrderFeeStatus=30,Modify=@Modify,ModifyDate=@ModifyDate where ID in @IdLst";
            return DB<int>.Execute(sql, new { idLst = idLst, Modify = this.Modify, ModifyDate = this.ModifyDate }) > 0;
        }

        /// <summary>
        /// 取消回单状态
        /// </summary>
        /// <returns></returns>
        public bool CancelReceiptStatus(List<int> idLst)
        {
            string sql = "Update [dbo].[HY_FreightCalculationTable] set Status=20, OrderFeeStatus=NULL,Modify=@Modify,ModifyDate=@ModifyDate where ID in @IdLst";
            return DB<int>.Execute(sql, new { idLst = idLst, Modify = this.Modify, ModifyDate = this.ModifyDate }) > 0;
        }

        /// <summary>
        /// 根据订单编号获取所有计算行信息
        /// </summary>
        /// <returns></returns>
        public List<HY_FreightCalculationTable> GetByOrderNo()
        {
            string sql = "select * from  [dbo].[HY_FreightCalculationTable] where OrderNo = @OrderNo";
            return DB<HY_FreightCalculationTable>.Query(sql, new { OrderNo = this.OrderNo });
        }


        /// <summary>
        /// 获取回单签收数据信息
        /// </summary>
        public List<HY_FreightCalculationTable> GetBackSignList(string where, string orderby, int pageIndex, int pageSize, out int total)
        {
            string files = @"  a.ID,
                               (case when (a.Status='' or a.Status is null) then '未处理' else c.Value end) STATUS, --订单状态
	                           a.SapDeliveryOrderNo, --新SAP出库单号	
	                           a.SapNewOrderNo,--新SAP订单号
	                           a.SapOriginalDeliveryOrderNo, --原SAP出库单号
	                           a.SapOriginalOrderNo, --原SAP订单号
	                           a.IsBackConfirm, --电子回单确认状态
	                           a.IsSoundRecordConfirm, --录音文件状态确认
	                           b.RKCERTIFICATE, --凭证
	                           a.ForwarderName, --物流商名称
	                           e.unit_desc UnitDesc, --基本单位
	                           e.req_qty ReqQty,--基本单位交货数量
	                           d.ActualNum, --实收数量
	                           d.ReceivingGoodsDeal, --收货数量不符处理措施
							   hh.Value ReceivingGoodsDealName,
	                           a.TransportType,      --运输方式
	                           b.ENDTIME EndTime, --出库日期
	                           b.TIMECOST TimeCost, --应到货日期
	                           b.TIMEArrival, --应返单日期
	                           d.ActualReturnTime, --实际返单日期
	                           b.ToContact, --收货人姓名/电话
	                           d.ActualSigner, --实际签收人
	                           d.ActualSignerIsMatch,--签收人是否相符
							   cc.Value ActualSignerIsMatchName,
	                           d.SignedPersonNotMatchDeal, --签收人不符处理措施
							   ff.Value  SignedPersonNotMatchDealName,
	                           d.LogisticsResponsibilityDefinition, --物流责任界定
							   gg.Value LogisticsResponsibilityDefinitionName,
	                           d.ActualSignerNotMatchReult, --签收人不符最后确定结果
							   ee.Value  ActualSignerNotMatchReultName,
	                           d.ModifyDate OprateTime, --操作日期
                               b.SaleOfficeDesc, --销售办公室
	                           d.Remark, --备注
	                           b.UPLOADFILE--上传附件";
            string tableName = @"[dbo].[HY_FreightCalculationTable] a
                                 left join [dbo].[HY_Sign] d on d.FreightCalculationTableId=a.ID
                                 left join [dbo].[qx_InAndOutBoundOrder] b on a.OrderNo=b.[INORDERNO]
                                 left join DicItem c on c.Code=a.STATUS and c.CodeType='OrderStatus'
								 left join DicItem cc on cc.Code=d.ActualSignerIsMatch and cc.CodeType='ActualSignerIsMatch'
								 left join DicItem ee on ee.Code=d.ActualSignerNotMatchReult and ee.CodeType='ActualSignerNotMatchReult'
								 left join DicItem ff on ff.Code=d.SignedPersonNotMatchDeal and ff.CodeType='SignedPersonNotMatchDeal'
								 left join DicItem hh on hh.Code=d.ReceivingGoodsDeal and hh.CodeType='ReceivingGoodsDeal'
								 left join DicItem gg on gg.Code=d.LogisticsResponsibilityDefinition and gg.CodeType='LogisticsResponsibilityDefinition'
                                 left join qx_InAndOutBoundOrder_SAP as e on a.SapNewOrderNo = e.SAPORDERNO and a.SapItemNo=e.SAPITEMNO";
            List<Conditions> conditions = where.GetModel<List<Conditions>>();
            where = conditions.toSqlString();
            where += " and ISNULL(a.IsHide,0)=0 and ISNULL(b.STATUS,0) IN(20,21,30,40) "; //and ISNULL(c.Code,0) IN(20,21,30,40) 
            if (qx_User.CurrentUser.FACTORYNO != "" && qx_User.CurrentUser.FACTORYNO != "-1")
            {
                where += " and a.DeliveryFactory='" + qx_User.CurrentUser.FACTORYNO + "'";
            }
            return DB<HY_FreightCalculationTable>.GetPageList(files, tableName, where, orderby, pageIndex, pageSize, out total);
        }


        /// <summary>
        /// 删除转点对应的结费信息
        /// </summary>
        /// <param name="tansportTurningPointId"></param>
        /// <returns></returns>
        public bool DeleteFreightCalculation(DbConnection connection, DbTransaction transaction)
        {
            string sql = "delete HY_FreightCalculationTable where TansportTurningPointId=@TansportTurningPointId AND (SettlementNo='' or SettlementNo is null)";
            return DB<int>.Execute(sql, new { TansportTurningPointId = this.TansportTurningPointId }, connection, transaction) > 0;
        }

        /// <summary>
        /// 删除运单对应的结费信息
        /// </summary>
        /// <param name="tansportTurningPointId"></param>
        /// <returns></returns>
        public bool DeleteFreightCalculationByOrderNo(DbConnection connection, DbTransaction transaction)
        {
            string sql = "delete HY_FreightCalculationTable where OrderNo=@OrderNo AND (SettlementNo='' or SettlementNo is null)";
            return DB<int>.Execute(sql, new { OrderNo = this.OrderNo , connection , transaction }) > 0;
        }


        /// <summary>
        /// 修改转点结费数据
        /// </summary>
        /// <returns></returns>
        public bool UpdateTransPointSettmentFeeData(DbConnection connection, DbTransaction transaction)
        {
            string sql = "UPDATE HY_FreightCalculationTable SET ArriveProvince=@ArriveProvince,ArriveOneCity=@ArriveOneCity,ArriveSecondCity=@ArriveSecondCity,Modify=@Modify,ModifyDate=ModifyDate WHERE TansportTurningPointId=@TansportTurningPointId";
            return DB<int>.Execute(sql, this, connection, transaction) > 0;
        }

    }
}

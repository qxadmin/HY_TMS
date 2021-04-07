using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    /// <summary>
    /// 运单表
    /// </summary>
    [Serializable]
    public partial class QX_ORDERHEAD
    {
        #region Model

        /// <summary>
        /// ID
        /// </summary>
        public long ID { set; get; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string INORDERNO { set; get; }

        /// <summary>
        /// sap订单号
        /// </summary>
        public string SAPORDERNO { set; get; }

        /// <summary>
        /// 销售单类型
        /// </summary>
        public string ZORDERTYPE { set; get; }

        /// <summary>
        /// 计划交货时间
        /// </summary>
        public string DELIVERTIME { set; get; }

        /// <summary>
        /// 发货工厂
        /// </summary>
        public string FROMPLANT { set; get; }

        /// <summary>
        /// 发货工厂备注
        /// </summary>
        public string FROMPLANTDESC { set; get; }

        /// <summary>
        /// 发货仓库
        /// </summary>
        public string FROMWAREHOUSE { set; get; }

        /// <summary>
        /// 发货仓库备注
        /// </summary>
        public string FROMWAREHOUSEDESC { set; get; }

        /// <summary>
        /// 采购组织
        /// </summary>
        public string STROKORG { set; get; }

        /// <summary>
        /// 采购组，采购订单使用
        /// </summary>
        public string STROKGROUP { set; get; }

        /// <summary>
        /// 采购订单凭证日期
        /// </summary>
        public string BUYTIME { set; get; }

        /// <summary>
        /// 分销渠道
        /// </summary>
        public string SALEGROUP { set; get; }

        /// <summary>
        /// 分销渠道描述
        /// </summary>
        public string SALEGROUPDESC { set; get; }

        /// <summary>
        /// 销售办公室
        /// </summary>
        public string SALEOFFICE { set; get; }

        /// <summary>
        /// 销售办公室描述
        /// </summary>
        public string SALEOFFICEDESC { set; get; }

        /// <summary>
        /// 客户  销售客户或者供货商客户
        /// </summary>
        public string CUSTOMER { set; get; }

        /// <summary>
        /// 客户描述    销售客户或者供货商客户描述
        /// </summary>
        public string CUSTOMERDESC { set; get; }

        /// <summary>
        /// 购买工厂
        /// </summary>
        public string TOPLANT { set; get; }

        /// <summary>
        /// 购买工厂备注
        /// </summary>
        public string TOPLANTDESC { set; get; }

        /// <summary>
        /// 收货仓库   采购入库收货仓库
        /// </summary>
        public string TOWAREHOUSE { set; get; }

        /// <summary>
        /// 收货仓库备注   收货工厂
        /// </summary>
        public string TOWAREHOUSEDESC { set; get; }

        /// <summary>
        /// 销售人员/项目负责人
        /// </summary>
        public string SALESACCOUNT { set; get; }

        /// <summary>
        /// 销售人员/项目负责人电话
        /// </summary>
        public string SALESPHONE { set; get; }

        /// <summary>
        /// 收货单位（企业、公司）
        /// </summary>
        public string RECEIVECONPANY { set; get; }

        /// <summary>
        /// 收货单位描述
        /// </summary>
        public string RECEIVECONPANYDESC { set; get; }

        /// <summary>
        /// 到货地址
        /// </summary>
        public string TOADDRESS { set; get; }

        /// <summary>
        /// 到货放联系信息
        /// </summary>
        public string TOCONTACT { set; get; }

        /// <summary>
        /// 到达省份
        /// </summary>
        public string TOPROVINCES { set; get; }

        /// <summary>
        /// 到货市
        /// </summary>
        public string TOCITY1 { set; get; }

        /// <summary>
        /// 到货二级市
        /// </summary>
        public string TOCITY2 { set; get; }

        /// <summary>
        /// 订单类型
        /// </summary>
        public string ORDERTYPE { set; get; }

        /// <summary>
        /// 订单类型描述
        /// </summary>
        public string ORDERTYPEDESC { set; get; }

        /// <summary>
        /// 订单状态
        /// </summary>
        public string STATUS { set; get; }

        /// <summary>
        /// 是否分配发运计划
        /// </summary>
        public string ISINDICAT { set; get; }

        /// <summary>
        /// TMS订单备注
        /// </summary>
        public string REMARKS { set; get; }

        /// <summary>
        /// 确认出库的结束时间，车辆发车时间等
        /// </summary>
        public string ENDTIME { set; get; }

        /// <summary>
        /// 出（入）库确认时 sap返回，取消时传回给sap取消
        /// </summary>
        public string RKCERTIFICATE { set; get; }

        /// <summary>
        /// WBS号
        /// </summary>
        public string WBSNO { set; get; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string WBSNAME { set; get; }

        /// <summary>
        /// 运费承担方式
        /// </summary>
        public string TRANSPORT { set; get; }

        /// <summary>
        /// 合格证
        /// </summary>
        public string CERTIF { set; get; }

        /// <summary>
        /// 备注
        /// </summary>
        public string SAPREMARKS { set; get; }

        /// <summary>
        /// 文本
        /// </summary>
        public string TEXTDESC { set; get; }

        /// <summary>
        /// 退货标记
        /// </summary>
        public string RETURNFLG { set; get; }

        /// <summary>
        /// 发运计划创建人
        /// </summary>
        public string CREATEUSER { set; get; }

        /// <summary>
        /// 发运计划创建时间
        /// </summary>
        public string CREATETIME { set; get; }

        /// <summary>
        /// sap订单创建人
        /// </summary>
        public string SAPCREATEUSER { set; get; }

        /// <summary>
        /// sap订单创建时间
        /// </summary>
        public string SAPCREATETIME { set; get; }

        /// <summary>
        /// 交货单号
        /// </summary>
        public string DELIVERY { set; get; }

        /// <summary>
        /// 过账日期
        /// </summary>
        public string OVERTIME { set; get; }

        /// <summary>
        /// 计划装车时间
        /// </summary>
        public string PLANLOADDATE { set; get; }

        /// <summary>
        /// 一级物流运输商编号
        /// </summary>
        public string LOGISTICS { set; get; }
        /// <summary>
        /// 二级物流运输商编号
        /// </summary>
        public string TWOLOGISTICS { set; get; }
        /// <summary>
        /// 二级物流运输商编号
        /// </summary>
        public string TWOLOGISTICSNAME { set; get; }

        /// <summary>
        /// 车牌号
        /// </summary>
        public string CARNUMBER { set; get; }

        /// <summary>
        /// 是否结算标识
        /// </summary>
        public string ISAUDIT { set; get; }

        /// <summary>
        /// 结算备注
        /// </summary>
        public string AUDITREMARKS { set; get; }

        /// <summary>
        /// 特殊备注
        /// </summary>
        public string SREMARK { set; get; }

        /// <summary>
        /// 预估费用
        /// </summary>
        public decimal PREPRICE { set; get; }

        /// <summary>
        /// 结算费用
        /// </summary>
        public decimal REALPRICE { set; get; }

        /// <summary>
        /// 凭据图片，可多张
        /// </summary>
        public string UPLOADFILE { set; get; }

        /// <summary>
        /// 录音文件路径
        /// </summary>
        public string RECFILE { set; get; }

        /// <summary>
        /// 派车合同号
        /// </summary>
        public string CONTRACTNO { set; get; }

        /// <summary>
        /// 司机驾驶证编号
        /// </summary>
        public string DRIVER { set; get; }

        /// <summary>
        /// 司机名称
        /// </summary>
        public string DRIVERNAME { set; get; }

        /// <summary>
        /// 司机电话号码
        /// </summary>
        public string DriverPhone { set; get; }

        /// <summary>
        /// 司机身份证
        /// </summary>
        public string DriverIDCard { set; get; }

        /// <summary>
        /// ISDEL
        /// </summary>
        public string ISDEL { set; get; }

        /// <summary>
        /// ISCANCEL
        /// </summary>
        public string ISCANCEL { set; get; }


        /// <summary>
        /// 车辆到厂时间
        /// </summary>
        public string ARRIVEPLANTDATE { set; get; }
        /// <summary>
        /// 要求车辆到厂时间
        /// </summary>
        public string PLANARRIVEPLANTDATE { set; get; }

        /// <summary>
        /// 预计到厂时间
        /// </summary>
        public string DIRVERPLANARRIVEDATE { set; get; }


        /// <summary>
        /// 出厂时间
        /// </summary>
        public string OUTPLANTDATE { set; get; }

        /// <summary>
        /// 车辆签到时间
        /// </summary>
        public string TRUCKSIGNDATE { set; get; }

        /// <summary>
        /// 确认到厂时间
        /// </summary>
        public string CONFIRMDATE { set; get; }

        /// <summary>
        /// 要求车型
        /// </summary>
        public string ForecastModels { set; get; }

        /// <summary>
        /// 实际派车车型
        /// </summary>
        public string ActualModels { set; get; }

        /// <summary>
        /// 实际派车车型吨位
        /// </summary>
        public string ActualModel_num { set; get; }

        /// <summary>
        /// 上传附件
        /// </summary>
        public string UPLOADFILEFJ { set; get; }

        /// <summary>
        /// 包车名称
        /// </summary>
        public string CharteredName { set; get; }

        /// <summary>
        /// 包车价格
        /// </summary>
        public double CharteredPrice { set; get; }

        /// <summary>
        /// 是否劫持入场
        /// </summary>
        public int ISAllowInFactory { set; get; }


        /// <summary>
        /// 是否发送短信
        /// </summary>
        public int SendSMS { set; get; }

        /// <summary>
        /// 运输方式
        /// </summary>
        public string TRANSPORTMODE { set; get; }

        /// <summary>
        /// 重量
        /// </summary>
        public string WEIGHT { set; get; }

        /// <summary>
        /// 在途天数
        /// </summary>
        public int ZTDay { set; get; }


        /// <summary>
        /// 发运类型
        /// </summary>
        public string ForwardingType { set; get; }
        #endregion
        /// <summary>
        /// 订单行项目
        /// </summary>
        public List<qx_InAndOutOrderProductDetail> details { get; set; }

        public string Uid { get; set; }
    }



}

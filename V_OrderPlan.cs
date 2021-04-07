
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HY.TMS.ORM;
namespace HY.TMS.Model
{
    /// <summary>
    /// 发运计划
    /// </summary>
    public partial class V_OrderPlan
    {

        /// <summary>
        /// 状态名称
        /// </summary>
        public string StatusName { get; set; }

        /// <summary>
        /// 是否允许入厂名称
        /// </summary>
        private string _iSAllowInFactoryName = null;
        public string ISAllowInFactoryName
        {
            get
            {
                if (_iSAllowInFactoryName == null)
                {
                    _iSAllowInFactoryName = ISAllowInFactory > 0 ? "是" : "否";
                }
                return _iSAllowInFactoryName;
            }
        }

        /// <summary>
        /// 在途截至时间
        /// </summary>
        private string _endTime = null;

        public string endTime
        {
            get
            {
                if (_endTime == null)
                {
                    if (ZTDay.HasValue)
                    {
                        return !ENDTIME.HasValue ? "" : (ENDTIME.Value.AddDays(ZTDay.Value) == DateTime.MinValue ? "" : ENDTIME.Value.AddDays(ZTDay.Value).ToString("yyyy-MM-dd"));
                    }
                }
                return string.Empty;
            }
        }

        public string _isSignName = null;

        /// <summary>
        /// 是否签收
        /// </summary>
        public string isSignName 
        {
            get 
            {
                if (string.IsNullOrWhiteSpace(_isSignName)) 
                {
                   return _isSignName =this.ISSIGN=="01"?"是":"否";
                }
                return "";
            }
        }

        public string _isindicatName = null;
        /// <summary>
        /// 是否分配
        /// </summary>
        public string isindicatName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_isindicatName))
                {
                    return _isindicatName = this.ISINDICAT == "01" ? "是" : "否";
                }
                return "";
            }
        }

        public string _uploadfileName = null;
        /// <summary>
        /// 是否上传回单
        /// </summary>
        public string uploadfileName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_uploadfileName))
                {
                    return _uploadfileName = !string.IsNullOrWhiteSpace(this.UPLOADFILE)? "是" : "否";
                }
                return "";
            }
        }

        /// <summary>
        /// 发运地
        /// </summary>
        public string ShipmentPlace { get; set; }

        /// <summary>
        /// 获取发运计划分页数据
        /// </summary>
        /// <returns></returns>
        public List<V_OrderPlan> GetOrderPlanList(string where, string orderby, int pageIndex, int pageSize, out int total)
        {
            string files = "*,Value StatusName,factory.city ShipmentPlace";
            string tableName = "V_OrderPlan a left join DicItem on DicItem.Code=a.status and DicItem.CodeType='OrderStatus' left join (select factoryNo,city from V_FACTORY) factory on factory.factoryNo=a.FROMPLANT";
            return DB<V_OrderPlan>.GetPageList(files, tableName, where, orderby, pageIndex, pageSize, out total);
        }


        /// <summary>
        /// 获取订单转点第一条信息的运输信息
        /// </summary>
        /// <returns></returns>
        public V_OrderPlan GetOrderTransPointData() 
        {
            string sql = @" select a.CARNUMBER,b.DriverName [DRIVERNAME],b.[DRIVERIDCARD],b.DriverMobile DRIVERTEL,a.ActualModels,a.ActualModel_num,a.ForecastModels,a.ForwardingType,a.TWOLOGISTICS,a.TWOLOGISTICSNAME 
                             ,a.TRANSPORTMODE,c.SVALUE,a.TOPROVINCES,a.TOCITY1,a.TOCITY2
                            from qx_InAndOutBoundOrder a 
                            left join [dbo].[HY_DriverInfo] b on b.DRIVERNO=a.DRIVER
                            left join qx_KeyValue c on c.[TYPENO]=1 and c.SKEY=a.TRANSPORTMODE 
                            where [INORDERNO]=@INORDERNO";
            return DB<V_OrderPlan>.QueryFirstOrDefault(sql,new { INORDERNO =this.INORDERNO});
        }
    }
}

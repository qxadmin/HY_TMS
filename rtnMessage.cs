using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    public class rtnMessage
    {
        /// <summary>
        /// 返回值  0:成功 1:失败
        /// </summary>
        public char rtnValue { get; set; }
        /// <summary>
        /// 返回运费金额，如果出错，则返回0
        /// </summary>
        public double transFee { get; set; }

        /// <summary>
        /// 运价地表ID
        /// </summary>
        public int TansportPriceTableId { get; set; }
        /// <summary>
        /// 在途时间(天数)
        /// </summary>
        public int timeCost { get; set; }
        /// <summary>
        /// 应返单时间(天数)
        /// </summary>
        public int returnCost { get; set; }
        /// <summary>
        /// 错误信息，程序错误时返回错误内容
        /// </summary>
        public string rtnMsg { get; set; }
        /// <summary>
        /// 运费计算内容,成功时，返回备注
        /// </summary>
        public string rtnDesc { get; set; }
        /// <summary>
        /// 目的地 省份
        /// </summary>
        public string tProvince { get; set; }
        /// <summary>
        /// 目的地 一级市
        /// </summary>
        public string tCity1 { get; set; }
        /// <summary>
        /// 目的地 二级市
        /// </summary>
        public string tCity2 { get; set; }
        /// <summary>
        /// 工厂编号
        /// </summary>
        public string fFactory { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 到货地址
        /// </summary>
        public string ToAddress { get; set; }
        /// <summary>
        /// 发货日期，如果为空，默认为当天
        /// </summary>
        public DateTime sendDate { get; set; }
        /// <summary>
        /// 运输商编号
        /// </summary>
        public string Logistics { get; set; }
        /// <summary>
        /// 合并单号
        /// </summary>
        public int mergeOrder { get; set; }
        /// <summary>
        /// 承担方式
        /// </summary>
        public string TRANSPORT { get; set; }
        /// <summary>
        /// 重量
        /// </summary>
        public float zhongliang { get; set; }
        /// <summary>
        /// 配送费
        /// </summary>
        public float psnum { get; set; }
        /// <summary>
        /// 距离
        /// </summary>
        public float MILES { get; set; }
        /// <summary>
        /// 出库单重量
        /// </summary>
        public float rkqty { get; set; }



        /****************lukw 用于接口一返回值********************/
        /// <summary>
        /// 行项目
        /// </summary>
        public string SAPItemNo { get; set; }

        /// <summary>
        /// 物料编号
        /// </summary>
        public string ProductNo { get; set; }

        /****************cs 用于最后的财务报表********************/
        /// <summary>
        /// 结算吨位
        /// </summary>
        public double js_qty { get; set; }
        /// <summary>
        /// 行项目编号
        /// </summary>
        public string js_orderno { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public float js_Price { get; set; }
        /// <summary>
        /// 危险品
        /// </summary>
        public double js_Dangerous { get; set; }
        /// <summary>
        /// 保温品
        /// </summary>
        public double js_preservation { get; set; }
        /// <summary>
        /// 横放
        /// </summary>
        public double js_Horizontal { get; set; }
        /// <summary>
        /// 卸货费
        /// </summary>
        public double js_cargo { get; set; }
        /// <summary>
        /// 危险品重量
        /// </summary>
        public double js_Dangerous_qty { get; set; }
        /// <summary>
        /// 危险品上浮比例
        /// </summary>
        public float js_Dangerous_Proportion { get; set; }
        /// <summary>
        /// 危险品单价（上浮之前）
        /// </summary>
        public float js_Dangerous_Price { get; set; }
        /// <summary>
        /// 保温品重量
        /// </summary>
        public double js_preservation_qty { get; set; }
        /// <summary>
        /// 保温品上浮比例
        /// </summary>
        public float js_preservation_Proportion { get; set; }
        /// <summary>
        /// 保温品单价（上浮之前）
        /// </summary>
        public float js_preservation_Price { get; set; }
        /// <summary>
        /// 横放重量
        /// </summary>
        public double js_Horizontal_qty { get; set; }
        /// <summary>
        /// 横放上浮比例
        /// </summary>
        public float js_Horizontal_Proportion { get; set; }
        /// <summary>
        /// 横放单价（上浮之前）
        /// </summary>
        public float js_Horizontal_Price { get; set; }
    }
}

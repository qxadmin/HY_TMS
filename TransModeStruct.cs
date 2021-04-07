using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    public class TransModeStruct
    {
        /// <summary>
        /// 运输方式，中文
        /// </summary>
        public string transMode { get; set; }
        /// <summary>
        /// 出发地工厂编号，转换大写进行判断
        /// </summary>
        public string fFactory { get; set; }
        /// <summary>
        /// 目的地 身份
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
        /// 重量，需要转换成(吨)
        /// </summary>
        public float qty { get; set; }
        /// <summary>
        /// 特殊方式，0:无特殊 1:危险品 2:保温品 3:卷材
        /// </summary>
        public char feeMode { get; set; }
        /// <summary>
        /// 是否卸货，0:不卸货 1:卸货；默认为0
        /// </summary>
        public char isLoad { get; set; }
        /// <summary>
        /// 卸货点个数，如果为空，默认为0
        /// </summary>
        public int loadPoint { get; set; }
        /// <summary>
        /// 发货日期，如果为空，默认为当天
        /// </summary>
        public DateTime sendDate { get; set; }
        /// <summary>
        /// 合并订单后的重量
        /// </summary>
        public float weight { get; set; }

        /// <summary>
        /// 工厂编号集合
        /// </summary>
        public List<string> fFactorys { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string orderNo { get; set; }
        /// <summary>
        /// 到货地址
        /// </summary>
        public string ToAddress { get; set; }
        /// <summary>
        /// 运输商编号
        /// </summary>
        public string Logistics { get; set; }
        /// <summary>
        /// 合并单号
        /// </summary>
        public int mergeOrder { get; set; }
        /// <summary>
        /// 出库单重量
        /// </summary>
        public float rkqty { get; set; }
        /// <summary>
        /// 承担方式
        /// </summary>
        public string TRANSPORT { get; set; }

        /// <summary>
        /// 放大重量
        /// </summary>
        public float fangdaWeight { get; set; }

        /// <summary>
        /// 危险品合并重量
        /// </summary>
        public float wxweight { get; set; }
        /// <summary>
        /// 是否退货
        /// </summary>
        public string returngos { get; set; }
        /// <summary>
        /// 是否从低
        /// </summary>
        public bool not_cd { get; set; }
        /// <summary>
        /// 保温品合并重量
        /// </summary>
        public float bwweight { get; set; }
        /// <summary>
        /// 距离
        /// </summary>
        public float MILES { get; set; }
        /// <summary>
        /// 出库单号
        /// </summary>
        public string rks { get; set; }

        /****************2017年11月3日 13:30:14海运物流用********************/
        /// <summary>
        /// 航线
        /// </summary>
        public string port { get; set; }
        /// <summary>
        /// 码头到客户距离
        /// </summary>
        public float port_miles { get; set; }

        /****************2018年1月3日 16:38:20客户名称********************/

        /// <summary>
        /// 客户名称
        /// </summary>
        public string Customerdesc { get; set; }



        /****************lukw 用于接口一返回值********************/
        /// <summary>
        /// 行项目
        /// </summary>
        public string SAPItemNo { get; set; }

        /// <summary>
        /// 物料编号
        /// </summary>
        public string ProductNo { get; set; }
        /// <summary>
        /// 行项目编号
        /// </summary>
        public string js_orderno { get; set; }

        /****************cs ADD********************/
        /// <summary>
        /// 起运地
        /// </summary>
        public string origintransport { get; set; }
    }
}

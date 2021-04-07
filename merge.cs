using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    public class merge
    {
        public string Provinces { get; set; }
        public string tCity1 { get; set; }
        public string tCity2 { get; set; }
        public float weight { get; set; }
        public DateTime ENDTIME { get; set; }
        public char feeMode { get; set; }//运输方式
        public int mergeOrder { get; set; }//合并单号
        public float wxweight { get; set; }
        public string returnflg { get; set; }//是否退货
        public float bwweight { get; set; }//保温品重量
    }
}

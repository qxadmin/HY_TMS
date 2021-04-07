using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    public class DictionaryModel
    {
        /// <summary>
        /// 键(因为Key在SQL中是关键字，所以只能取其他名称)
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public string value { get; set; }
    }
}

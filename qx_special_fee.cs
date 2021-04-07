using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    public partial class qx_special_fee
    {
        /// <summary>
        /// 获取危险品和保温品上浮比例
        /// </summary>
        /// <param name="weinum">重量标识</param>
        /// <param name="type">1.危险品；2.保温品</param>
        /// <returns></returns>
        public float GetSpecialFee(string weinum, string type)
        {
            float num = 0;
            string sql = "select * from qx_special_fee where factory =@factory and logistics =@logistics";
            var specialFee = DB<qx_special_fee>.QueryFirstOrDefault(sql, new { factory = this.factory, logistics = this.logistics });
            if (type == "1")
            {
                if (weinum == "1")
                {
                    num = specialFee.DANGER_One.HasValue ? float.Parse(specialFee.DANGER_One.Value.ToString()) : 0;
                }
                else if (weinum == "2")
                {
                    num = specialFee.DANGER_Two.HasValue ? float.Parse(specialFee.DANGER_Two.Value.ToString()) : 0;
                }
                else if (weinum == "3")
                {
                    num = specialFee.DANGER_Three.HasValue ? float.Parse(specialFee.DANGER_Three.Value.ToString()) : 0;
                }
                else if (weinum == "4")
                {
                    num = specialFee.DANGER_Four.HasValue ? float.Parse(specialFee.DANGER_Four.Value.ToString()) : 0;
                }
                else if (weinum == "5")
                {
                    num = specialFee.DANGER_Five.HasValue ? float.Parse(specialFee.DANGER_Five.Value.ToString()) : 0;
                }
            }
            else 
            {
                if (weinum == "1")
                {
                    num = specialFee.preservation_One.HasValue ? float.Parse(specialFee.preservation_One.Value.ToString()) : 0;
                }
                else if (weinum == "2")
                {
                    num = specialFee.preservation_Two.HasValue ? float.Parse(specialFee.preservation_Two.Value.ToString()) : 0;
                }
                else if (weinum == "3")
                {
                    num = specialFee.preservation_Three.HasValue ? float.Parse(specialFee.preservation_Three.Value.ToString()) : 0;
                }
                else if (weinum == "4")
                {
                    num = specialFee.preservation_Four.HasValue ? float.Parse(specialFee.preservation_Four.Value.ToString()) : 0;
                }
                else if (weinum == "5")
                {
                    num = specialFee.preservation_Five.HasValue ? float.Parse(specialFee.preservation_Five.Value.ToString()) : 0;
                }
            }
            return num;
        }
    }
}

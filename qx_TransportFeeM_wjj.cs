using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    public partial class qx_TransportFeeM_wjj
    {
        public qx_TransportFeeM_wjj GetWjjFee(string qty, string miles, string logistics, string origintransport)
        {
            qx_TransportFeeM_wjj fwjj = new qx_TransportFeeM_wjj();
                string sql = @"select feetype,price from qx_TransportFeeM_wjj where @needmaxqty > needmaxqty   and @needminqty <= needminqty 
and @Needmaxmiles >Needmaxmiles and @Needmaxmiles<=Needminmiles and logistics =@logistics and origintransport =@origintransport";
            return DB<qx_TransportFeeM_wjj>.QueryFirstOrDefault(sql,new { needmaxqty=qty, Needmaxmiles= miles , logistics = logistics , origintransport = origintransport });


        }
    }
}

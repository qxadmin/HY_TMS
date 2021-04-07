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
    /// 物流信息
    /// </summary>
    public partial class qx_OrderTracing
    {
        public string area_name { get; set; }

        public string _isSignName = "";

        /// <summary>
        /// 是否签收
        /// </summary>
        public string isSignName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_isSignName))
                {
                    return _isSignName = this.ISSIGN == "1" ? "是" : "否";
                }
                return "";
            }
        }
        /// <summary>
        /// 根据TMS单号获取物流信息
        /// </summary>
        /// <param name="driVERNo">TNS单号</param>
        /// <returns></returns>
        public List<qx_OrderTracing> GetOrderTracingByOrderNo()
        {
            string sql = @"select *,(select area_name from qx_GCBLog where external_note = TMSORDERNO and CREATETIME >DATEADD(HH,1,TRACETIME)
                            and CREATETIME<DATEADD(HH,4,TRACETIME))  Location_gcb
                            from qx_OrderTracing t left join qx_KeyValue k
                            on t.dattitude = k.SKEY and k.TYPENO = '3' where TMSORDERNO = @TMSORDERNO";
            return DB<qx_OrderTracing>.Query(sql, new { TMSORDERNO = this.TMSORDERNO });
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool InsertSingle(DbConnection connection, DbTransaction dbTransaction)
        {
            string sql = @"INSERT INTO qx_OrderTracing(WORDERNO,TMSORDERNO,TRACETIME,LOCATION,CONTENT,ISSIGN,REMARK,CREATETIME,CREATEUSER,UPDATETIME,UPDATEUSER,Dattitude)
                           VALUES (@WORDERNO,@TMSORDERNO,@TRACETIME,@LOCATION,@CONTENT,@ISSIGN,@REMARK,@CREATETIME,@CREATEUSER,@UPDATETIME,@UPDATEUSER,@Dattitude)";
            return DB<int>.Execute(sql, this, connection, dbTransaction) > 0;
        }



        /// <summary>
        /// 修改一条数据
        /// </summary>
        /// <returns></returns>
        public bool UpdateSingle(DbConnection connection, DbTransaction dbTransaction)
        {
            string sql = @"UPDATE qx_OrderTracing SET WORDERNO=@WORDERNO,TRACETIME=@TRACETIME,LOCATION=@LOCATION,CONTENT=@CONTENT,ISSIGN=@ISSIGN
                           ,REMARK=@REMARK,UPDATETIME=@UPDATETIME,UPDATEUSER=@UPDATEUSER,Dattitude=@Dattitude WHERE TMSORDERNO=@TMSORDERNO and CONTENT=@CONTENT and CONVERT(varchar(100),TRACETIME,23)=CONVERT(varchar(100),GETDATE(),23)";
            return DB<int>.Execute(sql, this, connection, dbTransaction) > 0;
        }

        /// <summary>
        /// 获取是否签收和时间
        /// </summary>
        /// <returns></returns>
        public qx_OrderTracing getTRACETIMEOrISSIGN()
        {
            string sql = @"select top 1 ISSIGN,TRACETIME from qx_OrderTracing WITH (NOLOCK)  where TMSORDERNO = @inorder order by TRACETIME desc";
            return DB<qx_OrderTracing>.QuerySingleOrDefault(sql, new { inorder = this.TMSORDERNO });
        }

        /// <summary>
        /// 判断按钮是否显示
        /// </summary>
        /// <returns></returns>
        public bool IsOnly()
        {
            string sql = string.Empty;
            if (this.ISSIGN == "1")
            {
                sql = "select count(0) from qx_OrderTracing where (TMSORDERNO=@TMSORDERNO and CONTENT=@CONTENT and CONVERT(varchar(100),TRACETIME,23)=CONVERT(varchar(100),GETDATE(),23))";
            }
            else
            {
                sql = "select count(0) from qx_OrderTracing where TMSORDERNO=@TMSORDERNO and CONTENT=@CONTENT and ISSIGN=@ISSIGN and CONVERT(varchar(100),TRACETIME,23)=CONVERT(varchar(100),GETDATE(),23)";
            }
            return DB<int>.QuerySingleOrDefault(sql, new { CONTENT = this.CONTENT, TMSORDERNO = this.TMSORDERNO }) > 0;
        }
    }
}

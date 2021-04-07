using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    public partial class HY_Forwarder
    {

        public int RowNum { get; set; }

        public string FactoryNo { get; set; }

        public string FactoryForwarder
        {
            get
            {
                return this.FactoryNo + "/" + this.FORWARDERNO;
            }
        }

        /// <summary>
        /// 获取承运商分页数据
        /// </summary>
        /// <returns></returns>
        public List<HY_Forwarder> GetForwarderInfoList(string where, string orderby, int pageIndex, int pageSize, out int total)
        {
            string files = @"f.ID,f.FORWARDERNO,f.FORWARDERNAME,f.FORWARDADDRESS
                            ,f.FORWORDZIP,f.FORWORDCONTACT,f.FORWORDTEL,f.FORWORDSIGNTIME,f.FORWORDSTATUS,f.FORWORDREMARKS,
							f.ModifyDate,l.[FactoryNo]";
            string tableName = "HY_Forwarder f left join [dbo].[HY_LogistRelation] l on l.LogisticsNo=f.FORWARDERNO";
            return GetForwarderInfoInfo(files, tableName, where, orderby, pageIndex, pageSize, out total);
        }

        /// <summary>
        /// 获取承运商管理中心分页数据
        /// </summary>
        /// <returns></returns>
        public List<HY_Forwarder> GetForwarderCenterInfoList(string where, string orderby, int pageIndex, int pageSize, out int total)
        {
            string files = @"f.*,l.FactoryNo";
            string tableName = "HY_Forwarder f left join [dbo].[HY_LogistRelation] l on l.LogisticsNo=f.FORWARDERNO";
            return GetForwarderInfoInfo(files, tableName, where, orderby, pageIndex, pageSize, out total);
        }

        /// <summary>
        /// 获取承运商分页数据
        /// </summary>
        /// <returns></returns>
        public List<HY_Forwarder> GetForwarderInfoInfo(string files, string tableName, string where, string orderby, int pageIndex, int pageSize, out int total)
        {
            return DB<HY_Forwarder>.GetPageList(files, tableName, where, orderby, pageIndex, pageSize, out total);
        }


        /// <summary>
        /// 查询最大编号
        /// </summary>
        /// <returns></returns>
        public string MaxNo()
        {
            string MaxNo = "YS21";
            string sql = string.Format("select MAX(cast(substring(FORWARDERNO,5,len(FORWARDERNO))as int)) FORWARDERNO  from [dbo].[HY_Forwarder]");
            var maxNo = DB<HY_Forwarder>.QueryFirstOrDefault(sql, this);
            if (maxNo != null)
            {
                if (!string.IsNullOrWhiteSpace(maxNo.FORWARDERNO))
                {
                    MaxNo += (Convert.ToInt32(Convert.ToInt32(maxNo.FORWARDERNO) + 1)).ToString("000000");
                    return MaxNo;
                }
            }
            return "YS21000001";
        }

        /// <summary>
        /// 获取承运商信息
        /// </summary>
        /// <returns></returns>
        public HY_Forwarder GetById()
        {
            string sql = @"SELECT * from HY_Forwarder with(nolock) WHERE ID=@ID ";
            return DB<HY_Forwarder>.QuerySingleOrDefault(sql, new { ID = this.ID });
        }

        /// <summary>
        /// 获取承运商信息
        /// </summary>
        /// <returns></returns>
        public HY_Forwarder GetByName()
        {
            string sql = @"SELECT * from HY_Forwarder with(nolock) WHERE FORWARDERNAME=@FORWARDERNAME ";
            return DB<HY_Forwarder>.QuerySingleOrDefault(sql, new { FORWARDERNAME = this.FORWARDERNAME });
        }

        /// <summary>
        /// 获取承运商信息
        /// </summary>
        /// <returns></returns>
        public HY_Forwarder GetByForwarderNo()
        {
            string sql = @"SELECT * from HY_Forwarder with(nolock) WHERE FORWARDERNO=@FORWARDERNO";
            return DB<HY_Forwarder>.QuerySingleOrDefault(sql, new { FORWARDERNO = this.FORWARDERNO });
        }

        /// <summary>
        /// 新增承运商信息
        /// </summary>
        /// <returns></returns>
        public bool Insert()
        {
            string sql = @"insert into [dbo].[HY_Forwarder]
                            (
                              FORWARDERNO, FORWARDERNAME, FORWARDADDRESS, FORWORDZIP, FORWORDCONTACT, FORWORDTEL, FORWORDSIGNTIME, FORWORDSTATUS, FORWORDLICENSE, FORWORDLICENSEPIC
                              , FORWORDORGANIZATIONCODE, FORWORDORGANIZATIONCODEPIC, FORWORDNATIONALTAX, FORWORDNATIONALTAXPIC, FORWORDLANDTAX, FORWORDLANDTAXPIC, FORWORDROADLISENCE
                              , FORWORDROADLISENCEPIC, FORWORDIDCARD, FORWORDIDCARDPIC, FORWORDSTAFF, FORWORDREMARKS, ISDEL,CreateUser, CreateDate,Modify,ModifyDate,COMPANYID
                              , CONDITIONCODE, FORWORDORGANIZATIONCODE_DATE, FORWORDLICENSE_DATE, FORWORDNATIONALTAX_DATE, FORWORDLANDTAX_DATE, FORWORDROADLISENCE_DATE, FORWORDIDCARD_DATE
                            )
                            values
                            (
                             @FORWARDERNO, @FORWARDERNAME, @FORWARDADDRESS, @FORWORDZIP, @FORWORDCONTACT, @FORWORDTEL, @FORWORDSIGNTIME, @FORWORDSTATUS, @FORWORDLICENSE, @FORWORDLICENSEPIC
                            ,@FORWORDORGANIZATIONCODE, @FORWORDORGANIZATIONCODEPIC, @FORWORDNATIONALTAX, @FORWORDNATIONALTAXPIC, @FORWORDLANDTAX, @FORWORDLANDTAXPIC, @FORWORDROADLISENCE
                             ,@FORWORDROADLISENCEPIC, @FORWORDIDCARD, @FORWORDIDCARDPIC, @FORWORDSTAFF, @FORWORDREMARKS, 
                             @ISDEL,@CreateUser, @CreateDate,@Modify,@ModifyDate,@COMPANYID, @CONDITIONCODE,@FORWORDORGANIZATIONCODE_DATE
                            , @FORWORDLICENSE_DATE, @FORWORDNATIONALTAX_DATE, @FORWORDLANDTAX_DATE, @FORWORDROADLISENCE_DATE, @FORWORDIDCARD_DATE
                            )";
            return DB<int>.Execute(sql, this) > 0;
        }

        /// <summary>
        /// 修改承运商信息
        /// </summary>
        /// <returns></returns>
        public bool Update()
        {
            string sql = @"UPDATE HY_Forwarder SET FORWARDERNO=@FORWARDERNO, FORWARDERNAME=@FORWARDERNAME, FORWARDADDRESS=@FORWARDADDRESS, FORWORDZIP=@FORWORDZIP, FORWORDCONTACT=@FORWORDCONTACT
                           , FORWORDTEL=@FORWORDTEL, FORWORDSIGNTIME=@FORWORDSIGNTIME,FORWORDSTATUS=@FORWORDSTATUS, FORWORDLICENSE=@FORWORDLICENSE, FORWORDLICENSEPIC=@FORWORDLICENSEPIC
                           , FORWORDORGANIZATIONCODE=@FORWORDORGANIZATIONCODE, FORWORDORGANIZATIONCODEPIC=@FORWORDORGANIZATIONCODEPIC, FORWORDNATIONALTAX=@FORWORDNATIONALTAX,
                           FORWORDNATIONALTAXPIC=@FORWORDNATIONALTAXPIC, FORWORDLANDTAX=@FORWORDLANDTAX, FORWORDLANDTAXPIC=@FORWORDLANDTAXPIC, FORWORDROADLISENCE=@FORWORDROADLISENCE
                           , FORWORDROADLISENCEPIC=@FORWORDROADLISENCEPIC, FORWORDIDCARD=@FORWORDIDCARD,FORWORDIDCARDPIC=@FORWORDIDCARDPIC, FORWORDSTAFF=@FORWORDSTAFF
                           , FORWORDREMARKS=@FORWORDREMARKS, ISDEL=@ISDEL, ModifyDate=@ModifyDate, Modify=@Modify, 
                           COMPANYID=@COMPANYID, CONDITIONCODE=@CONDITIONCODE, FORWORDORGANIZATIONCODE_DATE=@FORWORDORGANIZATIONCODE_DATE, FORWORDLICENSE_DATE=@FORWORDLICENSE_DATE,
                           FORWORDNATIONALTAX_DATE=@FORWORDNATIONALTAX_DATE, FORWORDLANDTAX_DATE=@FORWORDLANDTAX_DATE, FORWORDROADLISENCE_DATE=@FORWORDROADLISENCE_DATE
                           , FORWORDIDCARD_DATE=@FORWORDIDCARD_DATE WHERE ID=@Id;";
            return DB<int>.Execute(sql, this) > 0;
        }

        /// <summary>
        /// 删除承运商信息
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {
            string sql = @"DELETE HY_Forwarder WHERE ID=@Id;";
            return DB<int>.Execute(sql, new { Id = this.ID }) > 0;
        }

        /// <summary>
        /// 禁用/启用承运商信息
        /// </summary>
        /// <returns></returns>
        public bool EnableForwarder()
        {
            string sql = @"UPDATE HY_Forwarder SET FORWORDSTATUS=@FORWORDSTATUS WHERE ID=@Id;";
            return DB<int>.Execute(sql, new { FORWORDSTATUS = this.FORWORDSTATUS, Id = this.ID }) > 0;
        }

        /// <summary>
        /// 订单物流商是否是共生平台或虹运物流
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public bool LogisticsIsGsOrHYPlatFrom(string INORDERNO)
        {
            string sql = @"SELECT *
                          FROM [dbo].[qx_InAndOutBoundOrder] o witH(nolock)
                          left join [TMS_DB].[dbo].[qx_Forwarder] f with(nolock) on f.FORWARDERNO=o.LOGISTICS
                          where o.INORDERNO=@INORDERNO
                          and (FORWARDERNO='YS12000170' OR FORWARDERNO='YS12000162')";
            return DB<qx_InAndOutBoundOrder>.QuerySingleOrDefault(sql, new { INORDERNO = INORDERNO }) != null;
        }

        /// <summary>
        /// 获取承运商
        /// </summary>
        /// <param name="FACTORYNO">工厂编号</param>
        /// <param name="FORWARDERNO">物流商编号</param>
        /// <returns></returns>
        public List<DictionaryModel> GetForwarder(string factoryNo, string forwarderNo)
        {
            string sql = @"select distinct FORWARDERNO Code,FORWARDERNAME value from (select * from HY_Forwarder f left  join (select FACTORYNO,LOGISTICSNO from HY_LogistRelation
                           group by FACTORYNO,LOGISTICSNO) l on f.FORWARDERNO=l.LOGISTICSNO)a  where FORWORDSTATUS=1";
            if (!string.IsNullOrWhiteSpace(factoryNo) && factoryNo != "-1")
            {
                sql += " and (FACTORYNO=@FACTORYNO or FORWARDERNO='YS12000070' or FORWARDERNO='YS12000066'";
                if (factoryNo == "YH10")
                {
                    sql += " or FORWARDERNO='YS12000083' or FORWARDERNO='YS12000081'";
                }
                sql += ")";
            }
            if (forwarderNo != "-1")
            {
                sql += " and FORWARDERNO=@FORWARDERNO";
            }
            return DB<DictionaryModel>.Query(sql, new { FACTORYNO = factoryNo, FORWARDERNO = forwarderNo });
        }

        /// <summary>
        /// 是否存在此物流商
        /// </summary>
        /// <param name="orderNo"></param>
        /// <returns></returns>
        public bool IsExists()
        {
            string sql = @" select Count(1) from  HY_Forwarder  where [FORWARDERNAME]=@FORWARDERNAME";
            return DB<int>.QueryFirstOrDefault(sql, new { FORWARDERNAME = this.FORWARDERNAME }) > 0;
        }
    }
}

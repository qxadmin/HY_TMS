using HY.TMS.ORM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HY.TMS.Model
{
    public class ExpressCont
    {
        /// <summary>
        /// 运费计算接口
        /// </summary>
        /// <param name="listtms">传入的信息集合</param>
        /// <returns></returns>
        public List<rtnMessage> TransportFeeInterface(List<TransModeStruct> listtms)
        {
            //创建最终返回的集合
            List<rtnMessage> listrm = new List<rtnMessage>();

            foreach (TransModeStruct tms in listtms)
            {
                rtnMessage rm = new rtnMessage();
                rm.rkqty = tms.rkqty;
                rm.OrderNo = tms.orderNo;
                rm.mergeOrder = tms.mergeOrder;
                rm.Logistics = tms.Logistics;
                rm.TRANSPORT = tms.TRANSPORT;
                rm.ToAddress = tms.ToAddress;
                rm.js_orderno = tms.js_orderno;
                //来记录，五吨一下是否从低（为了看是否给配送费）
                int cdps = 0;
                //来记录是否从低，如果从低则为1  不从低为0 （用于判断横放是否需要用大吨位）
                int cdps1 = 0;
                #region 验证部分
                ValiData(rm, tms, listrm);
                if (!string.IsNullOrWhiteSpace(rm.rtnMsg)) { continue; }
                #endregion

                #region 计算重量区间
                //存放转换后的重量
                float weight = 0;
                //当计算了LOWPRICERANGE后  如果需要则使用weights计算
                int weights = 0;
                tms.weight = tms.weight / 1000;
                tms.qty = tms.qty / 1000;
                rm.zhongliang = tms.qty;
                if (tms.weight > 0) { greaterThanWeight(tms, ref weight, ref weights); }
                else { lessThan(tms, ref weight, ref weights); }
                #region 获取最低运费的工厂
                float yunfei = 0;
                HY_TansportPriceTable tansportPriceTable = new HY_TansportPriceTable();
                tansportPriceTable.ArriveProvince = tms.tProvince;
                tansportPriceTable.ArriveCity = tms.tCity2;
                tansportPriceTable.WeightRangeSign = Convert.ToInt32(weight);
                var transportFee = tansportPriceTable.GetPrice(tms.fFactorys);
                if (transportFee != null)
                {
                    yunfei = float.Parse(!transportFee.Price.HasValue ? "0" : transportFee.Price.Value.ToString());
                    tms.fFactory = transportFee.FactoryNo;
                }
                #endregion

                if (string.IsNullOrEmpty(tms.fFactory))
                {
                    rm.rtnValue = '1';
                    rm.rtnMsg = "输入信息有误，请重新确认！";
                    rm.transFee = 0;
                    listrm.Add(rm);
                    continue;
                }
                else
                {
                    rm.fFactory = tms.fFactory;
                }
                #endregion

                //转换发货时间
                string sd = tms.sendDate.ToString("yyyy-MM-dd");

                #region 获取当条运费基表信息
                //计算从低到高的运价
                HY_TansportPriceTable minThanMaxTransportFeeM = congdi(tms, sd, weight);

                #endregion
                float price = 0;
                if (tms.TRANSPORT != "-2")
                {
                    #region 小于一吨
                    if (tms.weight < 1)
                    {
                        minThanMaxTransportFeeM = congdi(tms, sd, weight);
                        #region 当判断之后若重量小于0.2或0.5或1T
                        //当判断之后若重量小于0.2或0.5或1T
                        getfangdaWeight(tms);
                        #endregion

                        if (minThanMaxTransportFeeM != null)
                        {
                            if (float.Parse(!minThanMaxTransportFeeM.Price.HasValue ? "0" : minThanMaxTransportFeeM.Price.Value.ToString()) > 0)
                            {
                                price = float.Parse(minThanMaxTransportFeeM.Price.Value.ToString());
                                rm.rtnValue = '0';

                                rm.timeCost = minThanMaxTransportFeeM.TravelDay.Value;
                                rm.returnCost = minThanMaxTransportFeeM.ReturnOrderDay.Value;
                                rm.tProvince = tms.tProvince;
                                rm.tCity1 = tms.tCity1;
                                rm.tCity2 = tms.tCity2;
                                rm.OrderNo = tms.orderNo;
                                rm.ToAddress = tms.ToAddress;
                                rm.sendDate = tms.sendDate;
                            }
                            else
                            {
                                rm.rtnValue = '1';
                                rm.rtnMsg = "无价格规定";
                                rm.transFee = 0;
                                listrm.Add(rm);
                                continue;
                            }

                            double nums = Math.Round(tms.qty / tms.weight, 3);

                            rm.transFee = Math.Round(price * tms.fangdaWeight * nums, 2);
                            rm.rtnDesc = "计算方式：(放大判断逻辑运算) （单价）" + price + " * (重量) " + tms.fangdaWeight + " T*" + nums + " 到" + tms.tCity2;
                            rm.js_qty = tms.fangdaWeight * nums;
                            rm.js_Price = price;
                            if (tms.feeMode == '1')//危险品
                            {
                                nums = assignmentDangerousGoods(tms, rm, ref price);
                            }
                            else if (tms.feeMode == '2')//保温品
                            {
                                assignmentThermalInsulation(tms, rm, ref price, ref nums);
                            }
                            if (tms.feeMode == '3')//自粘卷材
                            {
                                assignmentSelfAdhesiveCoil(tms, rm, price, nums);
                            }
                        }
                        else
                        {
                            rm.rtnValue = '1';
                            rm.rtnMsg = "输入信息有误，请重新确认！";
                            rm.transFee = 0;
                            listrm.Add(rm);
                            continue;
                        }

                    }
                    #endregion
                }
                //存放元/吨的数值
                if (!(tms.fangdaWeight > 0))
                {
                    if (minThanMaxTransportFeeM != null)
                    {
                        if (float.Parse(!minThanMaxTransportFeeM.Price.HasValue ? "0" : minThanMaxTransportFeeM.Price.Value.ToString()) > 0)
                        {
                            price = float.Parse(!minThanMaxTransportFeeM.Price.HasValue ? "0" : minThanMaxTransportFeeM.Price.Value.ToString());
                            rm.rtnValue = '0';
                            rm.timeCost = int.Parse(!minThanMaxTransportFeeM.TravelDay.HasValue ? "0" : minThanMaxTransportFeeM.TravelDay.Value.ToString());
                            rm.returnCost = int.Parse(!minThanMaxTransportFeeM.ReturnOrderDay.HasValue ? "0" : minThanMaxTransportFeeM.ReturnOrderDay.Value.ToString());
                            rm.tProvince = tms.tProvince;
                            rm.tCity1 = tms.tCity1;
                            rm.tCity2 = tms.tCity2;
                            rm.OrderNo = tms.orderNo;
                            rm.ToAddress = tms.ToAddress;
                            rm.sendDate = tms.sendDate;
                        }
                        else
                        {
                            rm.rtnValue = '1';
                            rm.rtnMsg = "无价格规定";
                            rm.transFee = 0;
                            listrm.Add(rm);
                            continue;
                        }

                        if (float.Parse(!minThanMaxTransportFeeM.ReturnOrderDay.HasValue ? "0" : minThanMaxTransportFeeM.ReturnOrderDay.Value.ToString()) > 0)
                        {
                            if (tms.weight <= tms.qty)
                            {
                                if (tms.qty > float.Parse(!minThanMaxTransportFeeM.ReturnOrderDay.HasValue ? "0" : minThanMaxTransportFeeM.ReturnOrderDay.ToString()))
                                {
                                    if (tms.TRANSPORT != "-2")
                                    {
                                        //计算从低后的运价
                                        if ((tms.fFactory == "YH50" || tms.fFactory == "YH53" || tms.fFactory == "YH51") && weight == 3)
                                        {
                                            minThanMaxTransportFeeM = congdi(tms, sd, weight + 2);
                                            weights = 25;
                                            cdps1 = 1;
                                        }
                                        else
                                        {
                                            minThanMaxTransportFeeM = congdi(tms, sd, weight + 1);
                                            cdps = 1;
                                            cdps1 = 1;
                                        }
                                    }

                                    if (minThanMaxTransportFeeM != null)
                                    {
                                        price = float.Parse(!minThanMaxTransportFeeM.Price.HasValue ? "0" : minThanMaxTransportFeeM.Price.Value.ToString());
                                    }
                                    rm.transFee = Math.Round(price * weights, 2);
                                    rm.rtnDesc = "计算方式：(普通单价) " + price + " * (重量) " + weights + " T 到" + tms.tCity2 + " ";
                                    rm.js_qty = weights;
                                    rm.js_Price = price;
                                }
                                else
                                {
                                    rm.transFee = Math.Round(price * tms.qty, 2);
                                    rm.rtnDesc = "计算方式：(普通单价) " + price + " * (重量) " + tms.qty + " T 到" + tms.tCity2 + " ";
                                    rm.js_qty = tms.qty;
                                    rm.js_Price = price;
                                }
                            }
                            else
                            {

                                if (tms.weight > float.Parse(minThanMaxTransportFeeM.ReturnOrderDay.HasValue ? "0" : minThanMaxTransportFeeM.ReturnOrderDay.Value.ToString()))
                                {

                                    if (tms.TRANSPORT != "-2")
                                    {
                                        //计算从低后的运价
                                        if ((tms.fFactory == "YH50" || tms.fFactory == "YH53" || tms.fFactory == "YH51") && weight == 3)
                                        {
                                            minThanMaxTransportFeeM = congdi(tms, sd, weight + 2);
                                            weights = 25;
                                            cdps1 = 1;
                                        }
                                        else
                                        {
                                            minThanMaxTransportFeeM = congdi(tms, sd, weight + 1);
                                            cdps = 1;
                                            cdps1 = 1;
                                        }
                                    }
                                    price = float.Parse(!minThanMaxTransportFeeM.Price.HasValue ? "0" : minThanMaxTransportFeeM.Price.ToString());
                                    double proportion = Math.Round(tms.qty / tms.weight, 3);
                                    rm.transFee = Math.Round(price * weights * proportion, 2);
                                    rm.rtnDesc = "计算方式：(普通单价) " + price + " * (重量) " + weights + "*" + proportion + " T 到" + tms.tCity2 + " ";
                                    rm.js_qty = weights * proportion;
                                    rm.js_Price = price;
                                }
                                else
                                {
                                    rm.transFee = Math.Round(price * tms.qty, 2);
                                    rm.rtnDesc = "计算方式：(普通单价) " + price + " * (重量) " + tms.qty + " T 到" + tms.tCity2 + " ";
                                    rm.js_qty = tms.qty;
                                    rm.js_Price = price;
                                }
                            }
                        }
                        else
                        {
                            if (tms.weight <= tms.qty)
                            {

                                rm.transFee = Math.Round(price * tms.qty, 2);
                                rm.rtnDesc = "计算方式：(普通单价) " + price + " * (重量) " + tms.qty + " T 到" + tms.tCity2 + " ";
                                rm.js_qty = tms.qty;
                                rm.js_Price = price;
                            }
                            else
                            {
                                rm.transFee = Math.Round(price * tms.qty, 2);
                                rm.rtnDesc = "计算方式：(普通单价) " + price + " * (重量) " + tms.qty + " T 到" + tms.tCity2 + " ";
                                rm.js_qty = tms.qty;
                                rm.js_Price = price;
                            }
                        }
                    }
                    else
                    {
                        rm.rtnValue = '1';
                        rm.rtnMsg = "输入信息有误，请重新确认！";
                        rm.transFee = 0;
                        listrm.Add(rm);
                        continue;
                    }


                    //}
                    if (tms.feeMode == '1')//危险品
                    {
                        tms.wxweight = tms.wxweight / 1000;
                        if (0 < tms.wxweight && tms.wxweight < 5)
                        {
                            weight = 1;
                        }
                        else if (5 <= tms.wxweight && tms.wxweight < 10)
                        {
                            weight = 2;
                        }
                        else if (10 <= tms.wxweight && tms.wxweight < 20)
                        {
                            weight = 3;
                        }
                        else if (20 <= tms.wxweight && tms.wxweight < 25)
                        {
                            weight = 4;
                        }
                        else if (25 <= tms.wxweight)
                        {
                            weight = 5;
                        }
                        //计算没从低后的运价
                        minThanMaxTransportFeeM = congdi(tms, sd, weight);

                        if (minThanMaxTransportFeeM != null)
                        {
                            if (float.Parse(!minThanMaxTransportFeeM.Price.HasValue ? "0" : minThanMaxTransportFeeM.Price.Value.ToString()) > 0)
                            {
                                price = float.Parse(!minThanMaxTransportFeeM.Price.HasValue ? "0" : minThanMaxTransportFeeM.Price.Value.ToString());
                                rm.js_Dangerous_Price = price;
                                rm.rtnValue = '0';
                                rm.timeCost = Convert.ToInt32(minThanMaxTransportFeeM.TravelDay);
                                rm.returnCost = Convert.ToInt32(minThanMaxTransportFeeM.ReturnOrderDay);
                                rm.tProvince = tms.tProvince;
                                rm.tCity1 = tms.tCity1;
                                rm.tCity2 = tms.tCity2;
                                rm.OrderNo = tms.orderNo;
                                rm.ToAddress = tms.ToAddress;
                                rm.sendDate = tms.sendDate;
                            }
                            else
                            {
                                rm.rtnValue = '1';
                                rm.rtnMsg = "无价格规定";
                                rm.transFee = 0;
                                listrm.Add(rm);
                                continue;
                            }

                            if (weight == 1)
                            {
                                price = price * 0.6f;
                                rm.js_Dangerous_Proportion = 0.6f;
                            }
                            else if (weight == 2)
                            {
                                price = price * 0.4f;
                                rm.js_Dangerous_Proportion = 0.4f;
                            }
                            else if (weight == 3 || weight == 4)
                            {
                                price = price * 0.3f;
                                rm.js_Dangerous_Proportion = 0.3f;
                            }
                            else if (weight == 5)
                            {
                                price = price * 0.2f;
                                rm.js_Dangerous_Proportion = 0.2f;
                            }

                            if (tms.wxweight >= tms.qty && tms.wxweight <= 1)
                            {
                                double proportion = Math.Round(tms.qty / tms.wxweight, 3);
                                if (tms.TRANSPORT != "-2")
                                {
                                    if (tms.wxweight <= 0.2 && tms.wxweight > 0)
                                    {
                                        tms.wxweight = 0.2f;
                                    }
                                    else if (tms.wxweight > 0.2 && tms.wxweight <= 0.5)
                                    {
                                        tms.wxweight = 0.5f;
                                    }
                                    else if (tms.wxweight > 0.5 && tms.wxweight <= 1)
                                    {
                                        tms.wxweight = 1;
                                    }
                                }
                                rm.transFee += Math.Round(price * tms.wxweight * proportion, 2);
                                rm.rtnDesc += "计算方式：(危险品) " + price + " * (重量) " + tms.wxweight + " T * " + proportion + " 到" + tms.tCity2 + " ";
                                rm.js_Dangerous = Math.Round(price * tms.wxweight * proportion, 2);
                                rm.js_Dangerous_qty = Math.Round(tms.wxweight * proportion, 3);
                            }
                            else
                            {
                                if (tms.wxweight <= tms.qty)
                                {
                                    if (tms.TRANSPORT != "-2")
                                    {
                                        if (tms.qty <= 0.2 && tms.qty > 0)
                                        {
                                            tms.qty = 0.2f;
                                        }
                                        else if (tms.qty > 0.2 && tms.qty <= 0.5)
                                        {
                                            tms.qty = 0.5f;
                                        }
                                        else if (tms.qty > 0.5 && tms.qty <= 1)
                                        {
                                            tms.qty = 1;
                                        }
                                    }
                                }
                                rm.transFee += Math.Round(price * tms.qty, 2);
                                rm.rtnDesc += "计算方式：(危险品) " + price + " * (重量) " + tms.qty + " T 到" + tms.tCity2 + " ";
                                rm.js_Dangerous = Math.Round(price * tms.qty, 2);
                                rm.js_Dangerous_qty = Math.Round(tms.qty, 3);
                            }
                        }
                        else
                        {
                            rm.rtnValue = '1';
                            rm.rtnMsg = "输入信息有误，请重新确认！";
                            rm.transFee = 0;
                            listrm.Add(rm);
                            continue;
                        }
                    }
                    else if (tms.feeMode == '2')//保温品
                    {
                        //存放元/吨的数值

                        HY_HoldingTime holdingTime = new HY_HoldingTime();
                        holdingTime.FirstTime = DateTime.Now;
                        holdingTime.FactoryNo = tms.fFactory;
                        if (holdingTime.IsHaveHoldingTime())
                        {
                            tms.bwweight = tms.bwweight / 1000;
                            if (0 < tms.bwweight && tms.bwweight < 5)
                            {
                                weight = 1;
                            }
                            else if (5 <= tms.bwweight && tms.bwweight < 10)
                            {
                                weight = 2;
                            }
                            else if (10 <= tms.bwweight && tms.bwweight < 20)
                            {
                                weight = 3;
                            }
                            else if (20 <= tms.bwweight && tms.bwweight < 25)
                            {
                                weight = 4;
                            }
                            else if (25 <= tms.bwweight)
                            {
                                weight = 5;
                            }
                            if (minThanMaxTransportFeeM != null)
                            {
                                //计算没从低后的运价
                                minThanMaxTransportFeeM = congdi(tms, sd, weight);

                                if (float.Parse(!minThanMaxTransportFeeM.Price.HasValue ? "0" : minThanMaxTransportFeeM.Price.Value.ToString()) > 0)
                                {
                                    price = float.Parse(!minThanMaxTransportFeeM.Price.HasValue ? "0" : minThanMaxTransportFeeM.Price.Value.ToString());
                                    rm.js_preservation_Price = price;
                                    rm.rtnValue = '0';
                                    rm.timeCost = Convert.ToInt32(minThanMaxTransportFeeM.TravelDay);
                                    rm.returnCost = Convert.ToInt32(minThanMaxTransportFeeM.ReturnOrderDay);
                                    rm.tProvince = tms.tProvince;
                                    rm.tCity1 = tms.tCity1;
                                    rm.tCity2 = tms.tCity2;
                                    rm.OrderNo = tms.orderNo;
                                    rm.ToAddress = tms.ToAddress;
                                    rm.sendDate = tms.sendDate;
                                }
                                else
                                {
                                    rm.rtnValue = '1';
                                    rm.rtnMsg = "无价格规定";
                                    rm.transFee = 0;
                                    listrm.Add(rm);
                                    continue;
                                }

                                if (weight == 1)
                                {
                                    price = price * 1;
                                    rm.js_preservation_Proportion = 1;
                                }
                                else if (weight == 2)
                                {
                                    price = price * 0.6f;
                                    rm.js_preservation_Proportion = 0.6f;
                                }
                                else if (weight == 3 || weight == 4)
                                {
                                    price = price * 0.4f;
                                    rm.js_preservation_Proportion = 0.4f;
                                }
                                else if (weight == 5)
                                {
                                    price = price * 0.3f;
                                    rm.js_preservation_Proportion = 0.3f;
                                }

                                if (tms.bwweight >= tms.qty && tms.bwweight <= 1)
                                {
                                    double proportion = Math.Round(tms.qty / tms.bwweight, 3);
                                    if (tms.TRANSPORT != "-2")
                                    {
                                        if (tms.bwweight <= 0.2 && tms.bwweight > 0)
                                        {
                                            tms.bwweight = 0.2f;
                                        }
                                        else if (tms.bwweight > 0.2 && tms.bwweight <= 0.5)
                                        {
                                            tms.bwweight = 0.5f;
                                        }
                                        else if (tms.bwweight > 0.5 && tms.bwweight <= 1)
                                        {
                                            tms.bwweight = 1;
                                        }
                                    }

                                    rm.transFee += Math.Round(price * tms.bwweight * proportion, 2);
                                    rm.rtnDesc += "计算方式：(保温品) " + price + " * (重量) " + tms.bwweight + " T * " + proportion + " 到" + tms.tCity2 + " ";

                                    rm.js_preservation = Math.Round(price * tms.bwweight * proportion, 2);
                                    rm.js_preservation_qty = Math.Round(tms.bwweight * proportion, 3);
                                }
                                else
                                {
                                    float qty = 0;
                                    if (tms.bwweight <= tms.qty)
                                    {
                                        if (tms.TRANSPORT != "-2")
                                        {
                                            if (tms.qty <= 0.2 && tms.qty > 0)
                                            {
                                                qty = 0.2f;
                                            }
                                            else if (tms.qty > 0.2 && tms.qty <= 0.5)
                                            {
                                                qty = 0.5f;
                                            }
                                            else if (tms.qty > 0.5 && tms.qty <= 1)
                                            {
                                                qty = 1;
                                            }
                                            else
                                            {
                                                qty = tms.qty;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        qty = tms.qty;
                                    }


                                    rm.transFee += Math.Round(price * qty, 2);
                                    rm.rtnDesc += "计算方式：(保温品) " + price + " * (重量) " + qty + " T 到" + tms.tCity2 + " ";
                                    rm.js_preservation = Math.Round(price * qty, 2);
                                    rm.js_preservation_qty = Math.Round(qty, 3);
                                }

                            }
                            else
                            {
                                rm.rtnValue = '1';
                                rm.rtnMsg = "输入信息有误，请重新确认！";
                                rm.transFee = 0;
                                listrm.Add(rm);
                                continue;
                            }
                        }
                    }
                    else if (tms.feeMode == '3')//自粘卷材
                    {
                        if (tms.fFactory != "YH80" && tms.fFactory != "YH51" && tms.fFactory != "YH70")
                        {
                            //存放元/吨的数值
                            if (minThanMaxTransportFeeM != null)
                            {

                                if (float.Parse(!minThanMaxTransportFeeM.Price.HasValue ? "0" : minThanMaxTransportFeeM.Price.Value.ToString()) > 0)
                                {
                                    price = float.Parse(!minThanMaxTransportFeeM.Price.HasValue ? "0" : minThanMaxTransportFeeM.Price.Value.ToString());
                                    rm.js_Horizontal_Price = price;
                                    rm.rtnValue = '0';
                                    rm.timeCost = Convert.ToInt32(minThanMaxTransportFeeM.TravelDay);
                                    rm.returnCost = Convert.ToInt32(minThanMaxTransportFeeM.ReturnOrderDay);
                                    rm.tProvince = tms.tProvince;
                                    rm.tCity1 = tms.tCity1;
                                    rm.tCity2 = tms.tCity2;
                                    rm.OrderNo = tms.orderNo;
                                    rm.ToAddress = tms.ToAddress;
                                    rm.sendDate = tms.sendDate;
                                }
                                else
                                {
                                    rm.rtnValue = '1';
                                    rm.rtnMsg = "无价格规定";
                                    rm.transFee = 0;
                                    listrm.Add(rm);
                                    continue;
                                }

                                if (tms.qty <= 0.2 && tms.qty > 0)
                                {
                                    tms.qty = 0.2f;
                                }
                                else if (tms.qty > 0.2 && tms.qty <= 0.5)
                                {
                                    tms.qty = 0.5f;
                                }
                                else if (tms.qty > 0.5 && tms.qty <= 1)
                                {
                                    tms.qty = 1;
                                }
                                if (cdps1 == 1 & tms.qty == tms.weight)
                                {
                                    rm.rtnDesc += "计算方式：(自粘卷材) " + price + "*" + weights + " * 12.25% ";
                                    rm.transFee += price * weights * 0.1225f;
                                    rm.js_Horizontal = price * weights * 0.1225f;
                                    rm.js_Horizontal_Proportion = 0.1225f;
                                    rm.js_Horizontal_qty = weights;
                                }
                                else
                                {
                                    rm.rtnDesc += "计算方式：(自粘卷材) " + price + "*" + tms.qty + " * 12.25% ";
                                    rm.transFee += price * tms.qty * 0.1225f;
                                    rm.js_Horizontal = price * tms.qty * 0.1225f;
                                    rm.js_Horizontal_Proportion = 0.1225f;
                                    rm.js_Horizontal_qty = tms.qty;
                                }

                            }
                        }

                    }
                    //else
                    //{
                    //    rm.rtnValue = '1';
                    //    rm.rtnMsg = "特殊方式输入有误,请检查";
                    //    rm.transFee = 0;
                    //    listrm.Add(rm);
                    //    continue;
                    //}
                }
                if (tms.isLoad == '1')
                {
                    float qtys = 0;

                    qtys = tms.qty;

                    for (int i = 0; i < listrm.Count; i++)
                    {
                        if (listtms[i].ToAddress.TrimEnd('.').TrimEnd('.').TrimEnd('/').TrimEnd('/') == tms.ToAddress.TrimEnd('.').TrimEnd('.').TrimEnd('/').TrimEnd('/'))
                        {
                            qtys += listtms[i].qty;
                        }
                    }
                    for (int i = listrm.Count + 1; i < listtms.Count; i++)
                    {
                        if (listtms[i].ToAddress.TrimEnd('.').TrimEnd('.').TrimEnd('/').TrimEnd('/') == tms.ToAddress.TrimEnd('.').TrimEnd('.').TrimEnd('/').TrimEnd('/'))
                        {
                            qtys += listtms[i].qty / 1000;
                        }
                    }


                    if (qtys > 0)
                    {
                        if (0 < qtys && qtys <= 5)
                        {
                            rm.transFee += 24.77 * qtys * (tms.qty / qtys);
                            rm.rtnDesc += "+(特殊费用)卸货 24.77 *" + qtys * (tms.qty / qtys) + "T";
                            rm.js_cargo = 24.77 * qtys * (tms.qty / qtys);
                        }
                        else
                        {
                            rm.transFee += 19.82 * qtys * (tms.qty / qtys);
                            rm.rtnDesc += "+(特殊费用)卸货 19.82 *" + qtys * (tms.qty / qtys) + "T";
                            rm.js_cargo = 19.82 * qtys * (tms.qty / qtys);
                        }
                    }
                }

                //是否退货
                if (tms.returngos == "1")
                {
                    double tgos = 0;
                    tgos = rm.transFee * 0.1;
                    rm.transFee = rm.transFee * 1.1;

                    if (tms.qty < 10 && tms.qty > 0)
                    {
                        rm.transFee += 200;
                        tgos += 200;
                        rm.rtnDesc += " + (退货)" + tgos;
                    }
                    else if (tms.qty >= 10)
                    {
                        rm.rtnDesc += " + (退货)" + tgos;
                    }
                }


                int index = 0;//计数
                int index1 = 0;
                for (int i = 0; i < listrm.Count; i++)
                {

                    if (listrm[i].mergeOrder == tms.mergeOrder && listrm[i].ToAddress.TrimEnd('.').TrimEnd('.').TrimEnd('/').TrimEnd('/') != tms.ToAddress.TrimEnd('.').TrimEnd('.').TrimEnd('/').TrimEnd('/'))
                    {
                        index++;
                    }
                    if (listrm[i].mergeOrder == tms.mergeOrder && listrm[i].ToAddress.TrimEnd('.').TrimEnd('.').TrimEnd('/').TrimEnd('/') == tms.ToAddress.TrimEnd('.').TrimEnd('.').TrimEnd('/').TrimEnd('/'))
                    {
                        index1 = 1;
                    }
                    if (tms.rkqty >= 25 && listrm[i].rkqty >= 25 && listrm[i].mergeOrder == tms.mergeOrder)
                    {
                        index1 = 1;
                    }
                    if (cdps == 0 && tms.weight < 5)
                    {
                        index1 = 1;
                    }

                }

                if (index > 0 && index1 != 1)
                {
                    if (tms.fFactory == "YH50")
                    {
                        HY_Specialcity_Maintain specialcityMaintain = new HY_Specialcity_Maintain();
                        specialcityMaintain.City = tms.tCity1;
                        if (specialcityMaintain.IsSpecialCityByCity())
                        {
                            rm.transFee += 59.46;
                            rm.rtnDesc += ",派送费：59.46";
                            rm.psnum = 59.46f;
                        }
                        else
                        {
                            rm.transFee += 118.92;
                            rm.rtnDesc += ",派送费：118.92";
                            rm.psnum = 118.92f;
                        }
                    }
                    else if (tms.fFactory == "YH80")
                    {
                        if (tms.tCity1.IndexOf("北京") >= 0 || tms.tCity1.IndexOf("上海") >= 0 || tms.tCity1.IndexOf("深圳") >= 0 || tms.tCity1.IndexOf("广州") >= 0 || tms.tCity1.IndexOf("天津") >= 0 || tms.tCity1.IndexOf("重庆") >= 0)
                        {
                            rm.transFee += 59.46;
                            rm.rtnDesc += ",派送费：59.46";
                            rm.psnum = 59.46f;
                        }
                        else
                        {
                            rm.transFee += 118.92;
                            rm.rtnDesc += ",派送费：118.92";
                            rm.psnum = 118.92f;
                        }
                    }
                    else if (tms.fFactory == "YH51")
                    {
                        if (tms.tCity1.IndexOf("上海") >= 0 || tms.tCity1.IndexOf("深圳") >= 0 || tms.tCity1.IndexOf("广州") >= 0 || tms.tCity1.IndexOf("天津") >= 0 || tms.tCity1.IndexOf("重庆") >= 0)
                        {
                            rm.transFee += 59.46;
                            rm.rtnDesc += ",派送费：59.46";
                            rm.psnum = 59.46f;
                        }
                        else if (tms.tCity1.IndexOf("北京") >= 0)
                        {
                            rm.transFee += 59;
                            rm.rtnDesc += ",派送费：59";
                            rm.psnum = 59f;
                        }
                        else
                        {
                            rm.transFee += 118.92;
                            rm.rtnDesc += ",派送费：118.92";
                            rm.psnum = 118.92f;
                        }
                    }
                    else
                    {
                        if (tms.tCity1.IndexOf("北京") >= 0 || tms.tCity1.IndexOf("上海") >= 0 || tms.tCity1.IndexOf("深圳") >= 0 || tms.tCity1.IndexOf("广州") >= 0 || tms.tCity1.IndexOf("天津") >= 0 || tms.tCity1.IndexOf("重庆") >= 0)
                        {
                            rm.transFee += 59.46;
                            rm.rtnDesc += ",派送费：59.46";
                            rm.psnum = 59.46f;
                        }
                        else
                        {
                            rm.transFee += 118.92;
                            rm.rtnDesc += ",派送费：118.92";
                            rm.psnum = 118.92f;
                        }
                    }
                }
                else
                {
                    index1 = 0;
                }
                listrm.Add(rm);

            }
            return listrm;
        }

        #region 新逻辑
        /// <summary>
        /// 运费计算新逻辑
        /// </summary>
        /// <param name="listtms">传入的信息集合</param>
        /// <returns></returns>
        public List<rtnMessage> TransportFeeInterface_yh25(List<TransModeStruct> listtms)
        {
            //创建最终返回的集合
            List<rtnMessage> listrm = new List<rtnMessage>();

            foreach (TransModeStruct tms in listtms)
            {
                rtnMessage rm = new rtnMessage();
                rm.rkqty = tms.rkqty;
                rm.OrderNo = tms.orderNo;
                rm.mergeOrder = tms.mergeOrder;
                rm.Logistics = tms.Logistics;
                rm.TRANSPORT = tms.TRANSPORT;
                rm.ToAddress = tms.ToAddress;
                rm.js_orderno = tms.js_orderno;
                //来记录，五吨一下是否从低（为了看是否给配送费）
                int cdps = 0;
                //来记录是否从低，如果从低则为1  不从低为0 （用于判断横放是否需要用大吨位）
                int cdps1 = 0;
                #region 验证部分
                if (string.IsNullOrEmpty(tms.transMode))
                {
                    rm.rtnValue = '1';
                    rm.rtnMsg = "运输方式为空,请检查";
                    rm.transFee = 0;
                    listrm.Add(rm);
                    continue;
                }
                if (string.IsNullOrEmpty(tms.fFactory))
                {
                    rm.rtnValue = '1';
                    rm.rtnMsg = "出发地工厂编号为空,请检查";
                    rm.transFee = 0;
                    listrm.Add(rm);
                    continue;
                }
                if (string.IsNullOrEmpty(tms.tProvince))
                {
                    rm.rtnValue = '1';
                    rm.rtnMsg = "目的地 省份为空,请检查";
                    rm.transFee = 0;
                    listrm.Add(rm);
                    continue;
                }
                if (string.IsNullOrEmpty(tms.tCity1))
                {
                    rm.rtnValue = '1';
                    rm.rtnMsg = "目的地 一级市为空,请检查";
                    rm.transFee = 0;
                    listrm.Add(rm);
                    continue;
                }
                if (string.IsNullOrEmpty(tms.tCity2))
                {
                    rm.rtnValue = '1';
                    rm.rtnMsg = "目的地 二级市为空,请检查";
                    rm.transFee = 0;
                    listrm.Add(rm);
                    continue;
                }
                if (float.IsNaN(tms.qty) || tms.qty <= 0)
                {
                    rm.rtnValue = '1';
                    rm.rtnMsg = "重量输入有误,请检查";
                    rm.transFee = 0;
                    listrm.Add(rm);
                    continue;
                }

                #endregion

                #region 计算重量区间
                //存放转换后的重量
                int weight = 0;
                //当计算了LOWPRICERANGE后  如果需要则使用weights计算
                int weights = 0;
                tms.weight = tms.weight;
                tms.qty = tms.qty;
                rm.zhongliang = tms.qty;
                if (tms.weight > 0)
                {
                    if (0 < tms.weight && tms.weight < 5)
                    {
                        weight = 1;
                        weights = 5;
                    }
                    else if (5 <= tms.weight && tms.weight < 10)
                    {
                        weight = 2;
                        weights = 10;
                    }
                    else if (10 <= tms.weight && tms.weight < 20)
                    {
                        weight = 3;
                        weights = 20;
                    }
                    else if (20 <= tms.weight && tms.weight < 25)
                    {
                        weight = 4;
                        weights = 25;
                    }
                    else if (25 <= tms.weight)
                    {
                        weight = 5;
                    }
                }
                else
                {
                    if (0 < tms.qty && tms.qty < 5)
                    {
                        weight = 1;
                        weights = 5;
                    }
                    else if (5 <= tms.qty && tms.qty < 10)
                    {
                        weight = 2;
                        weights = 10;
                    }
                    else if (10 <= tms.qty && tms.qty < 20)
                    {
                        weight = 3;
                        weights = 20;
                    }
                    else if (20 <= tms.qty && tms.qty < 25)
                    {
                        weight = 4;
                        weights = 25;
                    }
                    else if (25 <= tms.qty)
                    {
                        weight = 5;
                    }
                }



                #region 获取最低运费的工厂
                float yunfei = 0;
                HY_TansportPriceTable tansportPriceTable = new HY_TansportPriceTable();
                tansportPriceTable.ArriveProvince = tms.tProvince;
                tansportPriceTable.ArriveCity = tms.tCity2;
                tansportPriceTable.WeightRangeSign = Convert.ToInt32(weight);
                var transportFee = tansportPriceTable.GetPrice(tms.fFactorys);
                if (transportFee != null)
                {
                    yunfei = float.Parse(!transportFee.Price.HasValue ? "0" : transportFee.Price.Value.ToString());
                    tms.fFactory = transportFee.FactoryNo;
                }
                #endregion

                if (string.IsNullOrEmpty(tms.fFactory))
                {
                    rm.rtnValue = '1';
                    rm.rtnMsg = "输入信息有误，请重新确认！";
                    rm.transFee = 0;
                    listrm.Add(rm);
                    continue;
                }
                else
                {
                    rm.fFactory = tms.fFactory;
                }
                #endregion
                #endregion

                //转换发货时间
                string sd = tms.sendDate != DateTime.MinValue ? tms.sendDate.ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd");



                #region 获取当条运费基表信息
                int index_yh25;//存放是否为二级市没有价格
                float num_yh25;//存放计算距离
                var lst = congdi_yh25(tms, sd, weight, out index_yh25, out num_yh25);
                if (num_yh25 == 0 && index_yh25 == 1)
                {
                    rm.rtnValue = '1';
                    rm.rtnMsg = "未维护距离";
                    rm.transFee = 0;
                    listrm.Add(rm);
                    continue;
                }
                #endregion
                float price = 0;
                if (tms.TRANSPORT != "-2")
                {

                    if (tms.weight < 1)
                    {
                        lst = congdi_yh25(tms, sd, weight, out index_yh25, out num_yh25);
                        if (num_yh25 == 0 && index_yh25 == 1)
                        {
                            rm.rtnValue = '1';
                            rm.rtnMsg = "未维护距离";
                            rm.transFee = 0;
                            listrm.Add(rm);
                            continue;
                        }
                        #region 当判断之后若重量小于0.2或0.5或1T
                        //当判断之后若重量小于0.2或0.5或1T
                        if (tms.Logistics == "YS12000128")
                        {
                            if (tms.weight > 0 && tms.weight <= 0.5f)
                            {
                                tms.fangdaWeight = 0.5f;
                            }
                            else if (tms.weight > 0.5f && tms.weight <= 1)
                            {
                                tms.fangdaWeight = 1;
                            }
                        }
                        else
                        {
                            if (tms.weight <= 0.2f && tms.weight > 0)
                            {
                                tms.fangdaWeight = 0.2f;
                            }
                            else if (tms.weight > 0.2f && tms.weight <= 0.5f)
                            {
                                tms.fangdaWeight = 0.5f;
                            }
                            else if (tms.weight > 0.5f && tms.weight <= 1)
                            {
                                tms.fangdaWeight = 1;
                            }
                        }

                        #endregion

                        if (lst.Count > 0)
                        {
                            if (float.Parse(!lst.FirstOrDefault().Price.HasValue ? "0" : lst.FirstOrDefault().Price.ToString()) > 0)
                            {
                                price = float.Parse(lst.FirstOrDefault().Price.Value.ToString()); ;
                                rm.rtnValue = '0';
                                rm.timeCost = lst.FirstOrDefault().TravelDay.HasValue ? lst.FirstOrDefault().TravelDay.Value : 0;
                                rm.returnCost = lst.FirstOrDefault().ReturnOrderDay.HasValue ? lst.FirstOrDefault().ReturnOrderDay.Value : 0;
                                rm.tProvince = tms.tProvince;
                                rm.tCity1 = tms.tCity1;
                                rm.tCity2 = tms.tCity2;
                                rm.OrderNo = tms.orderNo;
                                rm.ToAddress = tms.ToAddress;
                                rm.sendDate = tms.sendDate;
                            }
                            else
                            {
                                rm.rtnValue = '1';
                                rm.rtnMsg = "无价格规定";
                                rm.transFee = 0;
                                listrm.Add(rm);
                                continue;
                            }
                            double nums = Math.Round(tms.qty / tms.weight, 3);
                            if (index_yh25 == 0)
                            {
                                rm.transFee = Math.Round(price * tms.fangdaWeight * nums, 2);
                                rm.rtnDesc = "计算方式：(放大判断逻辑运算) （单价）" + price + " * (重量) " + tms.fangdaWeight + " T*" + nums + " 到" + tms.tCity2;
                                rm.js_qty = tms.fangdaWeight * nums;
                                rm.js_Price = price;
                            }
                            else
                            {
                                //按照吨公里计算
                                rm.transFee = Math.Round(price * tms.fangdaWeight * nums * num_yh25, 2);
                                rm.rtnDesc = "计算方式：(放大判断逻辑运算) （单价）" + price + " * (重量) " + tms.fangdaWeight + " T*" + nums + "（距离） " + num_yh25 + " 到" + tms.tCity2;
                                rm.js_qty = tms.fangdaWeight * nums;
                                rm.js_Price = price * num_yh25;
                            }
                            if (tms.feeMode == '1')//危险品
                            {
                                int indexbw_no = 0;
                                HY_FreightasDeterminepx freightasDeterminepx = new HY_FreightasDeterminepx();
                                var dangerousFoods = freightasDeterminepx.GetDangerousFoods();
                                if (dangerousFoods > 0)
                                {
                                    indexbw_no = 1;
                                }
                                if (indexbw_no == 0)
                                {
                                    lst = congdi_yh25(tms, sd, 1, out index_yh25, out num_yh25);
                                    float weixianfee = GetTeshuFee(tms, "1", "1");
                                    tms.wxweight = tms.wxweight / 1000;
                                    nums = Math.Round(tms.qty / tms.wxweight, 3);
                                    rm.js_Dangerous_Price = price;
                                    price = price * weixianfee;
                                    if (tms.wxweight <= 0.2 && tms.wxweight > 0)
                                    {
                                        tms.wxweight = 0.2f;
                                    }
                                    else if (tms.wxweight > 0.2 && tms.wxweight <= 0.5)
                                    {
                                        tms.wxweight = 0.5f;
                                    }
                                    else if (tms.wxweight > 0.5 && tms.wxweight <= 1)
                                    {
                                        tms.wxweight = 1;
                                    }

                                    if (index_yh25 == 0)
                                    {
                                        rm.transFee += Math.Round(price * tms.wxweight * nums, 2);
                                        rm.rtnDesc += "计算方式：(危险品放大判断逻辑运算) （单价）" + price + " * (重量) " + tms.wxweight + " T*" + nums + " 到" + tms.tCity2;
                                        rm.js_Dangerous = Math.Round(price * tms.wxweight * nums, 2);
                                        rm.js_Dangerous_qty = Math.Round(tms.wxweight * nums, 3);
                                        rm.js_Dangerous_Proportion = 0.6f;
                                    }
                                    else
                                    {
                                        rm.transFee += Math.Round(price * tms.wxweight * nums * num_yh25, 2);
                                        rm.rtnDesc += "计算方式：(危险品放大判断逻辑运算) （单价）" + price + " * (重量) " + tms.wxweight + " T*" + nums + "（距离） " + num_yh25 + " 到" + tms.tCity2;
                                        rm.js_Dangerous = Math.Round(price * tms.wxweight * nums * num_yh25, 2);
                                        rm.js_Dangerous_qty = Math.Round(tms.wxweight * nums, 3);
                                        rm.js_Dangerous_Proportion = 0.6f;
                                    }
                                }
                            }
                            else if (tms.feeMode == '2')
                            {
                                HY_HoldingTime holdingTime = new HY_HoldingTime();
                                holdingTime.FirstTime = DateTime.Now;
                                holdingTime.FactoryNo = tms.fFactory;
                                //是否有保温时间
                                if (holdingTime.IsHaveHoldingTime())
                                {
                                    tms.bwweight = tms.bwweight / 1000;
                                    nums = Math.Round(tms.qty / tms.bwweight, 3);

                                    float baowenfee = GetTeshuFee(tms, "1", "1");
                                    rm.js_preservation_Price = price;
                                    price = price * baowenfee;
                                    if (tms.bwweight <= 0.2 && tms.bwweight > 0)
                                    {
                                        tms.bwweight = 0.2f;
                                    }
                                    else if (tms.bwweight > 0.2 && tms.bwweight <= 0.5)
                                    {
                                        tms.bwweight = 0.5f;
                                    }
                                    else if (tms.bwweight > 0.5 && tms.bwweight <= 1)
                                    {
                                        tms.bwweight = 1;
                                    }

                                    if (index_yh25 == 0)
                                    {
                                        rm.transFee += Math.Round(price * tms.bwweight * nums, 2);
                                        rm.rtnDesc += "计算方式：(保温品放大判断逻辑运算) （单价）" + price + " * (重量) " + tms.bwweight + " T*" + nums + " 到" + tms.tCity2;
                                        rm.js_preservation = Math.Round(price * tms.bwweight * nums, 2);
                                        rm.js_preservation_qty = Math.Round(tms.bwweight * nums, 3);
                                        rm.js_preservation_Proportion = 0.6f;
                                    }
                                    else
                                    {
                                        rm.transFee += Math.Round(price * tms.bwweight * nums * num_yh25, 2);
                                        rm.rtnDesc += "计算方式：(保温品放大判断逻辑运算) （单价）" + price + " * (重量) " + tms.bwweight + " T*" + nums + "（距离） " + num_yh25 + " 到" + tms.tCity2;
                                        rm.js_preservation = Math.Round(price * tms.bwweight * nums * num_yh25, 2);
                                        rm.js_preservation_qty = Math.Round(tms.bwweight * nums, 3);
                                        rm.js_preservation_Proportion = 0.6f;
                                    }

                                }
                            }
                            if (tms.feeMode == '3')//自粘卷材
                            {
                                if (tms.fFactory != "YH80" && tms.fFactory != "YH51" && tms.fFactory != "YH56" && tms.fFactory != "YH70" && tms.fFactory != "YH54" && tms.fFactory != "YH25" && tms.fFactory != "YH58")
                                {
                                    if (tms.fFactory == "YH50")
                                    {
                                        if (index_yh25 == 0)
                                        {
                                            rm.transFee = Math.Round(price * tms.fangdaWeight * nums, 2) * 1.04f;
                                            rm.rtnDesc = "计算方式：(放大判断逻辑运算) （单价）" + price + " * (重量) " + tms.fangdaWeight + " T*" + nums + "*1.04 到" + tms.tCity2;
                                            //*1.31f;
                                            rm.js_Horizontal = Math.Round(price * tms.fangdaWeight * nums, 2) * 0.04f;
                                            rm.js_Horizontal_Proportion = 0.04f;
                                            rm.js_Horizontal_qty = Math.Round(tms.fangdaWeight * nums, 3);
                                        }
                                        else
                                        {
                                            rm.transFee = Math.Round(price * tms.fangdaWeight * nums * num_yh25, 2) * 1.04f;
                                            rm.rtnDesc = "计算方式：(放大判断逻辑运算) （单价）" + price + " * (重量) " + tms.fangdaWeight + " T*" + nums + "（距离） " + num_yh25 + " *1.04 到" + tms.tCity2;
                                            //*1.31f;
                                            rm.js_Horizontal = Math.Round(price * tms.fangdaWeight * nums * num_yh25, 2) * 0.04f;
                                            rm.js_Horizontal_Proportion = 0.04f;
                                            rm.js_Horizontal_qty = Math.Round(tms.fangdaWeight * nums, 3);
                                        }
                                    }
                                    else
                                    {
                                        if (index_yh25 == 0)
                                        {
                                            rm.transFee = Math.Round(price * tms.fangdaWeight * nums, 2) * 1.1225f;
                                            rm.rtnDesc = "计算方式：(放大判断逻辑运算) （单价）" + price + " * (重量) " + tms.fangdaWeight + " T*" + nums + "*1.1225 到" + tms.tCity2;
                                            //*1.31f;
                                            rm.js_Horizontal = Math.Round(price * tms.fangdaWeight * nums, 2) * 0.1225f;
                                            rm.js_Horizontal_Proportion = 0.1225f;
                                            rm.js_Horizontal_qty = Math.Round(tms.fangdaWeight * nums, 3);
                                        }
                                        else
                                        {
                                            rm.transFee = Math.Round(price * tms.fangdaWeight * nums * num_yh25, 2) * 1.1225f;
                                            rm.rtnDesc = "计算方式：(放大判断逻辑运算) （单价）" + price + " * (重量) " + tms.fangdaWeight + " T*" + nums + "（距离） " + num_yh25 + " *1.1225 到" + tms.tCity2;
                                            //*1.31f;
                                            rm.js_Horizontal = Math.Round(price * tms.fangdaWeight * nums * num_yh25, 2) * 0.1225f;
                                            rm.js_Horizontal_Proportion = 0.1225f;
                                            rm.js_Horizontal_qty = Math.Round(tms.fangdaWeight * nums, 3);
                                        }
                                    }

                                }
                            }
                        }
                        else
                        {
                            rm.rtnValue = '1';
                            rm.rtnMsg = "输入信息有误，请重新确认！";
                            rm.transFee = 0;
                            listrm.Add(rm);
                            continue;
                        }

                    }
                }
                //存放元/吨的数值

                if (!(tms.fangdaWeight > 0))
                {
                    if (lst != null && lst.Count > 0)
                    {
                        var prices = float.Parse(!lst.FirstOrDefault().Price.HasValue ? "0" : lst.FirstOrDefault().Price.ToString());
                        if (prices > 0)
                        {
                            price = prices;
                            rm.rtnValue = '0';
                            rm.timeCost = lst.FirstOrDefault().TravelDay.HasValue ? lst.FirstOrDefault().TravelDay.Value : 0;
                            rm.returnCost = lst.FirstOrDefault().ReturnOrderDay.HasValue ? lst.FirstOrDefault().ReturnOrderDay.Value : 0;
                            rm.tProvince = tms.tProvince;
                            rm.tCity1 = tms.tCity1;
                            rm.tCity2 = tms.tCity2;
                            rm.OrderNo = tms.orderNo;
                            rm.ToAddress = tms.ToAddress;
                            rm.sendDate = tms.sendDate;
                        }
                        else
                        {
                            rm.rtnValue = '1';
                            rm.rtnMsg = "无价格规定";
                            rm.transFee = 0;
                            listrm.Add(rm);
                            continue;
                        }



                        if (index_yh25 == 0)
                        {
                            rm.transFee = Math.Round(price * tms.qty , 2);
                            rm.rtnDesc = "计算方式：(普通单价) " + price + " * (重量) " + tms.qty + " T 到" + tms.tCity2 + " ";
                            rm.js_qty = tms.qty ;
                            rm.js_Price = price;
                        }
                        else
                        {
                            rm.transFee = Math.Round(tms.qty * num_yh25, 2);
                            rm.rtnDesc = "计算方式： (重量) " + tms.qty  + "(距离) " + num_yh25 + " T 到" + tms.tCity2 + " ";
                            rm.js_qty = tms.qty ;
                            rm.js_Price = price * num_yh25;
                        }

                    }
                    else
                    {
                        rm.rtnValue = '1';
                        rm.rtnMsg = "输入信息有误，请重新确认！";
                        rm.transFee = 0;
                        listrm.Add(rm);
                        continue;
                    }
                    if (tms.feeMode == '1')//危险品
                    {
                        int indexbw_no = 0;
                        HY_FreightasDeterminepx freightasDeterminepx = new HY_FreightasDeterminepx();
                        var dangerousFoods = freightasDeterminepx.GetDangerousFoods();
                        if (dangerousFoods > 0)
                        {
                            indexbw_no = 1;
                        }
                        if (indexbw_no == 0)
                        {
                            //if (tms.Logistics != "YS12000127")
                            //{
                            tms.wxweight = tms.wxweight / 1000;
                            if (0 < tms.wxweight && tms.wxweight < 5)
                            {
                                weight = 1;
                            }
                            else if (5 <= tms.wxweight && tms.wxweight < 10)
                            {
                                weight = 2;
                            }
                            else if (10 <= tms.wxweight && tms.wxweight < 20)
                            {
                                weight = 3;
                            }
                            else if (20 <= tms.wxweight && tms.wxweight < 25)
                            {
                                weight = 4;
                            }
                            else if (25 <= tms.wxweight)
                            {
                                weight = 5;
                            }
                            //计算没从低后的运价
                            lst = congdi_yh25(tms, sd, weight, out index_yh25, out num_yh25);
                            if (num_yh25 == 0 && index_yh25 == 1)
                            {
                                rm.rtnValue = '1';
                                rm.rtnMsg = "未维护距离";
                                rm.transFee = 0;
                                listrm.Add(rm);
                                continue;
                            }
                            if (lst != null && lst.Count > 0)
                            {
                                var prices = float.Parse(!lst.FirstOrDefault().Price.HasValue ? "0" : lst.FirstOrDefault().Price.ToString());
                                if (prices > 0)
                                {
                                    price = prices;
                                    rm.js_Dangerous_Price = price;
                                    rm.rtnValue = '0';
                                    rm.timeCost = lst.FirstOrDefault().TravelDay.HasValue ? lst.FirstOrDefault().TravelDay.Value : 0;
                                    rm.returnCost = lst.FirstOrDefault().ReturnOrderDay.HasValue ? lst.FirstOrDefault().ReturnOrderDay.Value : 0;
                                    rm.tProvince = tms.tProvince;
                                    rm.tCity1 = tms.tCity1;
                                    rm.tCity2 = tms.tCity2;
                                    rm.OrderNo = tms.orderNo;
                                    rm.ToAddress = tms.ToAddress;
                                    rm.sendDate = tms.sendDate;
                                }
                                else
                                {
                                    rm.rtnValue = '1';
                                    rm.rtnMsg = "无价格规定";
                                    rm.transFee = 0;
                                    listrm.Add(rm);
                                    continue;
                                }
                                float weixianfee = GetTeshuFee(tms, weight.ToString(), "1");
                                price = price * weixianfee;
                                rm.js_Dangerous_Proportion = weixianfee;

                                if (tms.wxweight >= tms.qty  && tms.wxweight <= 1)
                                {
                                    double proportion = Math.Round(tms.qty / tms.wxweight, 3);
                                    if (tms.TRANSPORT != "-2")
                                    {
                                        if (tms.wxweight <= 0.2 && tms.wxweight > 0)
                                        {
                                            tms.wxweight = 0.2f;
                                        }
                                        else if (tms.wxweight > 0.2 && tms.wxweight <= 0.5)
                                        {
                                            tms.wxweight = 0.5f;
                                        }
                                        else if (tms.wxweight > 0.5 && tms.wxweight <= 1)
                                        {
                                            tms.wxweight = 1;
                                        }
                                    }

                                    if (index_yh25 == 0)
                                    {
                                        rm.transFee += Math.Round(price * tms.wxweight * proportion, 2);
                                        rm.rtnDesc += "计算方式：(危险品) " + price + " * (重量) " + tms.wxweight + " T * " + proportion + " 到" + tms.tCity2 + " ";
                                        rm.js_Dangerous = Math.Round(price * tms.wxweight * proportion, 2);
                                        rm.js_Dangerous_qty = Math.Round(tms.wxweight * proportion, 3);
                                    }
                                    else
                                    {
                                        rm.transFee += Math.Round(price * tms.wxweight * proportion * num_yh25, 2);
                                        rm.rtnDesc += "计算方式：(危险品) " + price + " * (重量) " + tms.wxweight + " T * " + proportion + " (距离) " + num_yh25 + " 到" + tms.tCity2 + " ";
                                        rm.js_Dangerous = Math.Round(price * tms.wxweight * proportion * num_yh25, 2);
                                        rm.js_Dangerous_qty = Math.Round(tms.wxweight * proportion, 3);
                                    }
                                }
                                else
                                {
                                    if (tms.wxweight <= tms.qty)
                                    {
                                        if (tms.TRANSPORT != "-2")
                                        {
                                            if (tms.qty <= 0.2 && tms.qty > 0)
                                            {
                                                tms.qty = 0.2f;
                                            }
                                            else if (tms.qty > 0.2 && tms.qty <= 0.5)
                                            {
                                                tms.qty = 0.5f;
                                            }
                                            else if (tms.qty > 0.5 && tms.qty <= 1)
                                            {
                                                tms.qty = 1;
                                            }
                                        }
                                    }

                                    if (index_yh25 == 0)
                                    {
                                        rm.transFee += Math.Round(price * tms.qty, 2);
                                        rm.rtnDesc += "计算方式：(危险品) " + price + " * (重量) " + tms.qty + " T 到" + tms.tCity2 + " ";
                                        rm.js_Dangerous = Math.Round(price * tms.qty, 2);
                                        rm.js_Dangerous_qty = Math.Round(tms.qty, 3);
                                    }
                                    else
                                    {
                                        rm.transFee += Math.Round(price * tms.qty * num_yh25, 2);
                                        rm.rtnDesc += "计算方式：(危险品) " + price + " * (重量) " + tms.qty + " T *(距离) " + num_yh25 + " 到" + tms.tCity2 + " ";
                                        rm.js_Dangerous = Math.Round(price * tms.qty * num_yh25, 2);
                                        rm.js_Dangerous_qty = Math.Round(tms.qty, 3);
                                    }

                                }
                            }
                            else
                            {
                                rm.rtnValue = '1';
                                rm.rtnMsg = "输入信息有误，请重新确认！";
                                rm.transFee = 0;
                                listrm.Add(rm);
                                continue;
                            }
                            //}
                        }
                    }
                    else if (tms.feeMode == '2')//保温品
                    {
                        //存放元/吨的数值
                        #region 保温品
                        int indexbw_no = 0;
                        HY_FreightasDeterminepx freightasDeterminepx = new HY_FreightasDeterminepx();
                        var insulationProducts = freightasDeterminepx.GetInsulationProducts();
                        if (insulationProducts > 0)
                        {
                            indexbw_no = 1;
                        }
                        if (indexbw_no == 0)
                        {
                            HY_HoldingTime holdingTime = new HY_HoldingTime();
                            holdingTime.FirstTime = DateTime.Now;
                            holdingTime.FactoryNo = tms.fFactory;
                            //是否有保温时间
                            if (holdingTime.IsHaveHoldingTime())
                            {
                                tms.bwweight = tms.bwweight / 1000;
                                if (0 < tms.bwweight && tms.bwweight < 5)
                                {
                                    weight = 1;
                                }
                                else if (5 <= tms.bwweight && tms.bwweight < 10)
                                {
                                    weight = 2;
                                }
                                else if (10 <= tms.bwweight && tms.bwweight < 20)
                                {
                                    weight = 3;
                                }
                                else if (20 <= tms.bwweight && tms.bwweight < 25)
                                {
                                    weight = 4;
                                }
                                else if (25 <= tms.bwweight)
                                {
                                    weight = 5;
                                }
                                if (lst != null && lst.Count > 0)
                                {
                                    //计算没从低后的运价
                                    lst = congdi_yh25(tms, sd, weight, out index_yh25, out num_yh25);
                                    if (num_yh25 == 0 && index_yh25 == 1)
                                    {
                                        rm.rtnValue = '1';
                                        rm.rtnMsg = "未维护距离";
                                        rm.transFee = 0;
                                        listrm.Add(rm);
                                        continue;
                                    }

                                    var prices = float.Parse(!lst.FirstOrDefault().Price.HasValue ? "0" : lst.FirstOrDefault().Price.ToString());
                                    if (prices > 0)
                                    {
                                        price = prices;
                                        rm.js_Dangerous_Price = price;
                                        rm.rtnValue = '0';
                                        rm.timeCost = lst.FirstOrDefault().TravelDay.HasValue ? lst.FirstOrDefault().TravelDay.Value : 0;
                                        rm.returnCost = lst.FirstOrDefault().ReturnOrderDay.HasValue ? lst.FirstOrDefault().ReturnOrderDay.Value : 0;
                                        rm.tProvince = tms.tProvince;
                                        rm.tCity1 = tms.tCity1;
                                        rm.tCity2 = tms.tCity2;
                                        rm.OrderNo = tms.orderNo;
                                        rm.ToAddress = tms.ToAddress;
                                        rm.sendDate = tms.sendDate;
                                    }
                                    else
                                    {
                                        rm.rtnValue = '1';
                                        rm.rtnMsg = "无价格规定";
                                        rm.transFee = 0;
                                        listrm.Add(rm);
                                        continue;
                                    }
                                    float baowenfee = GetTeshuFee(tms, weight.ToString(), "2");
                                    price = price * baowenfee;
                                    rm.js_preservation_Proportion = baowenfee;

                                    if (tms.bwweight >= tms.qty && tms.bwweight <= 1)
                                    {
                                        double proportion = Math.Round(tms.qty / tms.bwweight, 3);
                                        if (tms.TRANSPORT != "-2")
                                        {
                                            if (tms.bwweight <= 0.2 && tms.bwweight > 0)
                                            {
                                                tms.bwweight = 0.2f;
                                            }
                                            else if (tms.bwweight > 0.2 && tms.bwweight <= 0.5)
                                            {
                                                tms.bwweight = 0.5f;
                                            }
                                            else if (tms.bwweight > 0.5 && tms.bwweight <= 1)
                                            {
                                                tms.bwweight = 1;
                                            }
                                        }
                                        if (index_yh25 == 0)
                                        {
                                            rm.transFee += Math.Round(price * tms.bwweight * proportion, 2);
                                            rm.rtnDesc += "计算方式：(保温品) " + price + " * (重量) " + tms.bwweight + " T * " + proportion + " 到" + tms.tCity2 + " ";

                                            rm.js_preservation = Math.Round(price * tms.bwweight * proportion, 2);
                                            rm.js_preservation_qty = Math.Round(tms.bwweight * proportion, 3);
                                        }
                                        else
                                        {
                                            rm.transFee += Math.Round(price * tms.bwweight * proportion * num_yh25, 2);
                                            rm.rtnDesc += "计算方式：(保温品) " + price + " * (重量) " + tms.bwweight + " T * " + proportion + "*(距离) " + num_yh25 + " 到" + tms.tCity2 + " ";

                                            rm.js_preservation = Math.Round(price * tms.bwweight * proportion * num_yh25, 2);
                                            rm.js_preservation_qty = Math.Round(tms.bwweight * proportion, 3);
                                        }
                                    }
                                    else
                                    {
                                        float qty = 0;
                                        if (tms.bwweight <= tms.qty)
                                        {
                                            if (tms.TRANSPORT != "-2")
                                            {
                                                if (tms.qty <= 0.2 && tms.qty > 0)
                                                {
                                                    qty = 0.2f;
                                                }
                                                else if (tms.qty > 0.2 && tms.qty <= 0.5)
                                                {
                                                    qty = 0.5f;
                                                }
                                                else if (tms.qty > 0.5 && tms.qty <= 1)
                                                {
                                                    qty = 1;
                                                }
                                                else
                                                {
                                                    qty = tms.qty;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            qty = tms.qty;
                                        }

                                        if (index_yh25 == 0)
                                        {
                                            rm.transFee += Math.Round(price * qty, 2);
                                            rm.rtnDesc += "计算方式：(保温品) " + price + " * (重量) " + qty + " T 到" + tms.tCity2 + " ";
                                            rm.js_preservation = Math.Round(price * qty, 2);
                                            rm.js_preservation_qty = Math.Round(qty, 3);
                                        }
                                        else
                                        {
                                            rm.transFee += Math.Round(price * qty * num_yh25, 2);
                                            rm.rtnDesc += "计算方式：(保温品) " + price + " * (重量) " + qty + " T * (距离) " + num_yh25 + " 到" + tms.tCity2 + " ";
                                            rm.js_preservation = Math.Round(price * qty * num_yh25, 2);
                                            rm.js_preservation_qty = Math.Round(qty, 3);
                                        }
                                    }

                                }
                                else
                                {
                                    rm.rtnValue = '1';
                                    rm.rtnMsg = "输入信息有误，请重新确认！";
                                    rm.transFee = 0;
                                    listrm.Add(rm);
                                    continue;
                                }
                            }
                        }

                        #endregion

                    }
                    else if (tms.feeMode == '3')//自粘卷材
                    {
                        #region 自粘
                        HY_HorizontallyFee horizontallyFee = new HY_HorizontallyFee();
                        horizontallyFee.FactoryNo = tms.fFactory;
                        horizontallyFee.LogisticsNo = tms.Logistics;
                        float hnum = horizontallyFee.GetHorizontallyFee();
                        if (hnum != 0)
                        {
                            //存放元/吨的数值
                            if (lst != null && lst.Count > 0)
                            {

                                var prices = float.Parse(!lst.FirstOrDefault().Price.HasValue ? "0" : lst.FirstOrDefault().Price.ToString());
                                if (prices > 0)
                                {
                                    price = prices;
                                    rm.js_Dangerous_Price = price;
                                    rm.rtnValue = '0';
                                    rm.timeCost = lst.FirstOrDefault().TravelDay.HasValue ? lst.FirstOrDefault().TravelDay.Value : 0;
                                    rm.returnCost = lst.FirstOrDefault().ReturnOrderDay.HasValue ? lst.FirstOrDefault().ReturnOrderDay.Value : 0;
                                    rm.tProvince = tms.tProvince;
                                    rm.tCity1 = tms.tCity1;
                                    rm.tCity2 = tms.tCity2;
                                    rm.OrderNo = tms.orderNo;
                                    rm.ToAddress = tms.ToAddress;
                                    rm.sendDate = tms.sendDate;
                                }
                                else
                                {
                                    rm.rtnValue = '1';
                                    rm.rtnMsg = "无价格规定";
                                    rm.transFee = 0;
                                    listrm.Add(rm);
                                    continue;
                                }

                                float hfzong = 0;
                                for (int i = 0; i < listtms.Count; i++)
                                {
                                    if (tms.mergeOrder == listtms[i].mergeOrder && listtms[i].feeMode == '3')
                                    {
                                        hfzong += listtms[i].qty;
                                    }
                                }
                                if (hfzong == tms.qty)
                                {
                                    if (tms.qty <= 0.2 && tms.qty > 0)
                                    {
                                        tms.qty = 0.2f;
                                    }
                                    else if (tms.qty > 0.2 && tms.qty <= 0.5)
                                    {
                                        tms.qty = 0.5f;
                                    }
                                    else if (tms.qty > 0.5 && tms.qty <= 1)
                                    {
                                        tms.qty = 1;
                                    }
                                }
                                if (tms.fFactory == "YH50")
                                {
                                    if (index_yh25 == 0)
                                    {
                                        rm.rtnDesc += "计算方式：(自粘卷材) " + price + "*" + tms.qty + " *  " + hnum;
                                        rm.transFee += price * tms.qty * hnum;
                                        rm.js_Horizontal = price * tms.qty * hnum;
                                        rm.js_Horizontal_Proportion = hnum;
                                        rm.js_Horizontal_qty = tms.qty;
                                    }
                                    else
                                    {
                                        rm.rtnDesc += "计算方式：(自粘卷材) " + price + "*" + tms.qty + "*" + num_yh25 + " *  " + hnum;
                                        rm.transFee += price * tms.qty * hnum * num_yh25;
                                        rm.js_Horizontal = price * tms.qty * hnum * num_yh25;
                                        rm.js_Horizontal_Proportion = hnum;
                                        rm.js_Horizontal_qty = tms.qty;
                                    }
                                }
                                else
                                {
                                    if (index_yh25 == 0)
                                    {
                                        rm.rtnDesc += "计算方式：(自粘卷材) " + price + "*" + tms.qty + " * " + hnum;
                                        rm.transFee += price * tms.qty * hnum;
                                        rm.js_Horizontal = price * tms.qty * hnum;
                                        rm.js_Horizontal_Proportion = hnum;
                                        rm.js_Horizontal_qty = tms.qty;
                                    }
                                    else
                                    {
                                        rm.rtnDesc += "计算方式：(自粘卷材) " + price + "*" + tms.qty + "*" + num_yh25 + " * " + hnum;
                                        rm.transFee += price * tms.qty * hnum * num_yh25;
                                        rm.js_Horizontal = price * tms.qty * hnum * num_yh25;
                                        rm.js_Horizontal_Proportion = hnum;
                                        rm.js_Horizontal_qty = tms.qty;
                                    }
                                }

                            }
                        }
                        #endregion
                    }
                }
                HY_UnloadingTransshipment transshipment = new HY_UnloadingTransshipment();
                transshipment.FactoryNo = tms.fFactory;
                transshipment.LogisticsNo = tms.Logistics;
                var transshipments = transshipment.getDischargeFee();
                if (tms.isLoad == '1')
                {
                    float qtys = 0;

                    qtys = tms.qty ;

                    for (int i = 0; i < listrm.Count; i++)
                    {
                        if (listtms[i].ToAddress.TrimEnd('.').TrimEnd('.').TrimEnd('/').TrimEnd('/') == tms.ToAddress.TrimEnd('.').TrimEnd('.').TrimEnd('/').TrimEnd('/'))
                        {
                            qtys += listtms[i].qty;
                        }
                    }
                    for (int i = listrm.Count + 1; i < listtms.Count; i++)
                    {
                        if (listtms[i].ToAddress.TrimEnd('.').TrimEnd('.').TrimEnd('/').TrimEnd('/') == tms.ToAddress.TrimEnd('.').TrimEnd('.').TrimEnd('/').TrimEnd('/'))
                        {
                            qtys += listtms[i].qty;
                        }
                    }


                    if (qtys > 0)
                    {
                        if (transshipments != null && transshipments.ID > 0)
                        {
                            if (0 < qtys && qtys <= 5)
                            {
                                var mixDischargeFee = transshipments.mixDischarge_fee.HasValue ? float.Parse(transshipments.mixDischarge_fee.ToString()) : 0;
                                rm.transFee += mixDischargeFee * qtys * (tms.qty / qtys);
                                rm.rtnDesc += "+(特殊费用)卸货 " + mixDischargeFee + " *" + qtys * (tms.qty / qtys) + "T";
                                rm.js_cargo = mixDischargeFee * qtys * (tms.qty / qtys);
                            }
                            else
                            {
                                var maxDischargeFee = transshipments.maxDischarge_fee.HasValue ? float.Parse(transshipments.maxDischarge_fee.ToString()) : 0;
                                rm.transFee += maxDischargeFee * qtys * (tms.qty / qtys);
                                rm.rtnDesc += "+(特殊费用)卸货 " + maxDischargeFee + " *" + qtys * (tms.qty / qtys) + "T";
                                rm.js_cargo = maxDischargeFee * qtys * (tms.qty / qtys);
                            }
                        }
                    }
                }

                //是否退货
                if (tms.returngos == "1")
                {
                    double tgos = 0;
                    tgos = rm.transFee * 0.1;
                    rm.transFee = rm.transFee * 1.1;

                    if (tms.qty  < 10 && tms.qty> 0)
                    {
                        rm.transFee += 200;
                        tgos += 200;
                        rm.rtnDesc += " + (退货)" + tgos;
                    }
                    else if (tms.qty  >= 10)
                    {
                        rm.rtnDesc += " + (退货)" + tgos;
                    }
                }


                int index = 0;//计数
                int index1 = 0;
                for (int i = 0; i < listrm.Count; i++)
                {

                    if (listrm[i].mergeOrder == tms.mergeOrder && listrm[i].ToAddress.TrimEnd('.').TrimEnd('.').TrimEnd('/').TrimEnd('/') != tms.ToAddress.TrimEnd('.').TrimEnd('.').TrimEnd('/').TrimEnd('/'))
                    {
                        index++;
                    }
                    if (listrm[i].mergeOrder == tms.mergeOrder && listrm[i].ToAddress.TrimEnd('.').TrimEnd('.').TrimEnd('/').TrimEnd('/') == tms.ToAddress.TrimEnd('.').TrimEnd('.').TrimEnd('/').TrimEnd('/'))
                    {
                        index1 = 1;
                    }
                    if (tms.rkqty >= 25 && listrm[i].rkqty >= 25 && listrm[i].mergeOrder == tms.mergeOrder)
                    {
                        index1 = 1;
                    }
                    if (cdps == 0 && tms.weight < 5)
                    {
                        index1 = 1;
                    }
                    if (tms.Logistics == "YS12000097" || tms.Logistics == "YS12000128")
                    {
                        index1 = 1;
                    }

                }

                if (index > 0 && index1 != 1)
                {
                    HY_Specialcity_Maintain specialcityMaintain = new HY_Specialcity_Maintain();
                    specialcityMaintain.City = tms.tCity1;
                    if (transshipments.ID > 0)
                    {
                        if (specialcityMaintain.IsSpecialCityByCity())
                        {
                            var specialCity = transshipments.Special_city.HasValue ? transshipments.Special_city.Value : 0;
                            rm.transFee += float.Parse(specialCity.ToString());
                            rm.rtnDesc += ",派送费：" + specialCity.ToString();
                            rm.psnum = float.Parse(specialCity.ToString());
                        }
                        else
                        {
                            var OrdinaryCity = transshipments.Ordinary_city.HasValue ? transshipments.Ordinary_city.Value : 0;
                            rm.transFee += float.Parse(OrdinaryCity.ToString());
                            rm.rtnDesc += ",派送费：" + OrdinaryCity.ToString();
                            rm.psnum = float.Parse(OrdinaryCity.ToString());
                        }
                    }
                }
                else
                {
                    index1 = 0;
                }
                rm.TansportPriceTableId = lst.FirstOrDefault().Id;
                listrm.Add(rm);
            }
            return listrm;
        }



        /// <summary>
        /// 徐州工厂计算过程
        /// </summary>
        public List<int> magguochen_yh25(List<int> IDLst, string forwarderNo)
        {
            #region 跳转到减水剂计算逻辑

            //if (get_Logistics_wei(forwarderNo))
            //{
            //    return jsjjisuan(Codes, forwarderNo, qx_User.CurrentUser.FACTORYNO);
            //}
            #endregion
            //else
            //{

                List<HY_FreightCalculationTable> freightCalculationTableLst = new List<HY_FreightCalculationTable>();
                //存储合并后的重量
                List<merge> listmg = new List<merge>();
                //存储计算需求数据
                List<TransModeStruct> listtms = new List<TransModeStruct>();
                //存储计算结果
                List<rtnMessage> listrm2 = new List<rtnMessage>();
                HY_FreightCalculationTable freightCalculationTable = new HY_FreightCalculationTable();
                //同步二级市为计费城市
                freightCalculationTable.SyncTwoCity(IDLst);

                freightCalculationTableLst = freightCalculationTable.GetFreightCalculationTableList(IDLst);
                int index_order = 0;
                var productTypeCode = "";
                decimal wight = 0;
                decimal count = 0;
                #region 合并重量
                foreach (var item in freightCalculationTableLst)
                {
                    productTypeCode = !string.IsNullOrEmpty(item.ProductTypeCode) ? item.ProductTypeCode : "";
                    wight = item.ProductWeight.HasValue ? item.ProductWeight.Value : 0;     //物料重量
                    count = item.Number.HasValue ? item.Number.Value : 0; //物料数量
                    //用于做判定的标示  true则说明在集合中没有可合并的订单，false则反之
                    bool index = true;
                    bool index1 = true;
                    merge mg_d = new merge();
                    //判断是否有需要合并的订单
                    for (int j = 0; j < listmg.Count; j++)
                    {
                        //根据循环，逐条来判断，在基本信息集合里，是否存在可以合并的订单
                        merge mg = new merge();
                        //逐条取出信息
                        mg = listmg[j];
                        //如果到货一级市为六大市则按照二级市合并
                        if (mg.returnflg != "1")
                        {

                            HY_Specialcity_Maintain specialcityMaintain = new HY_Specialcity_Maintain();
                            specialcityMaintain.City = item.ArriveOneCity;
                            if (specialcityMaintain.IsSpecialCityByCity())
                            {
                                var calculationDate = item.SapPostingDate.HasValue ? item.SapPostingDate.Value.ToString("yyyy-MM-dd") : "";
                                //通过到货省份，一级市，发货日期来确定是否需要合并
                                if (item.ArriveProvince.Trim() == mg.Provinces && item.ArriveSecondCity.Trim() == mg.tCity2 && mg.ENDTIME.ToString("yyyy-MM-dd") == calculationDate)
                                {

                                    //将重量取出放入float数据类型中
                                    float num = float.Parse((wight * count).ToString()) / 1000;

                                    mg.weight += num;

                                    if (productTypeCode.ToUpper().Trim() == "D00" && mg.feeMode == '1')
                                    {
                                        mg.wxweight += num;
                                        index1 = false;
                                    }
                                    if (productTypeCode.ToUpper().Trim() == "D01" && mg.feeMode == '2')
                                    {
                                        mg.bwweight += num;
                                        index1 = false;
                                    }

                                    mg_d = mg;
                                    //将合并信息再次存入
                                    listmg[j] = mg;
                                    //将判定标示改为false
                                    index = false;

                                }

                                if (productTypeCode.ToUpper().Trim() == "D00" && j == listmg.Count - 1 && index1 == true)
                                {
                                    merge mgs = new merge();
                                    mgs.Provinces = item.ArriveProvince.Trim();
                                    mgs.tCity1 = item.ArriveOneCity.Trim();
                                    mgs.tCity2 = item.ArriveSecondCity.Trim();
                                    mgs.feeMode = '1';

                                    if (item.SapPostingDate.HasValue)
                                    {
                                        mgs.ENDTIME = item.SapPostingDate.Value;

                                    }
                                    mgs.wxweight = (item.ProductWeight.HasValue ? float.Parse(item.ProductWeight.Value.ToString()) : 0) / 1000;

                                    mgs.mergeOrder = mg_d.mergeOrder;
                                    index1 = true;
                                    if (mg_d.mergeOrder != 0)
                                    {
                                        listmg.Add(mgs);
                                    }
                                    j = listmg.Count;
                                }

                                if (productTypeCode.ToUpper().Trim() == "D01" && j == listmg.Count - 1 && index1 == true)
                                {
                                    merge mgs = new merge();
                                    mgs.Provinces = item.ArriveProvince.Trim();
                                    mgs.tCity1 = item.ArriveOneCity.Trim();
                                    mgs.tCity2 = item.ArriveSecondCity.Trim();
                                    mgs.feeMode = '2';
                                    if (item.SapPostingDate.HasValue)
                                    {
                                        mgs.ENDTIME = item.SapPostingDate.Value;
                                    }
                                    mgs.bwweight = (item.ProductWeight.HasValue ? float.Parse(item.ProductWeight.Value.ToString()) : 0) / 1000;

                                    mgs.mergeOrder = mg_d.mergeOrder;
                                    index1 = true;
                                    if (mg_d.mergeOrder != 0)
                                    {
                                        listmg.Add(mgs);
                                    }
                                    j = listmg.Count;
                                }

                            }
                            else
                            {
                                var calculationDate = item.SapPostingDate.HasValue ? item.SapPostingDate.Value.ToString("yyyy-MM-dd") : "";
                                //通过到货省份，一级市，发货日期来确定是否需要合并
                                if (item.ArriveProvince.Trim() == mg.Provinces && item.ArriveSecondCity.Trim() == mg.tCity2 && mg.ENDTIME.ToString("yyyy-MM-dd") == calculationDate)
                                {
                                    //将重量取出放入float数据类型中
                                    float num = float.Parse((wight * count).ToString()) / 1000;

                                    mg.weight += num ;
                                    if (productTypeCode.ToUpper().Trim() == "D00" && mg.feeMode == '1')
                                    {
                                        mg.wxweight += num;
                                        index1 = false;
                                    }
                                    if (productTypeCode.ToUpper().Trim() == "D01" && mg.feeMode == '2')
                                    {
                                        mg.bwweight += num;
                                        index1 = false;
                                    }
                                    mg_d = mg;
                                    //将合并信息再次存入
                                    listmg[j] = mg;
                                    //将判定标示改为false
                                    index = false;

                                }

                                if (productTypeCode.ToUpper().Trim() == "D00" && j == listmg.Count - 1 && index1 == true)
                                {
                                    merge mgs = new merge();
                                    mgs.Provinces = item.ArriveProvince.Trim();
                                    mgs.tCity1 = item.ArriveOneCity.Trim();
                                    mgs.tCity2 = item.ArriveSecondCity.Trim();
                                    mgs.feeMode = '1';

                                    if (item.SapPostingDate.HasValue)
                                    {
                                        mgs.ENDTIME = item.SapPostingDate.Value;
                                    }
                                    mgs.wxweight = float.Parse((wight * count).ToString()) / 1000;

                                    mgs.mergeOrder = mg_d.mergeOrder;
                                    index1 = true;
                                    if (mg_d.mergeOrder != 0)
                                    {
                                        listmg.Add(mgs);
                                    }
                                    j = listmg.Count;
                                }

                                if (productTypeCode.ToUpper().Trim() == "D01" && j == listmg.Count - 1 && index1 == true)
                                {
                                    merge mgs = new merge();
                                    mgs.Provinces = item.ArriveProvince.Trim();
                                    mgs.tCity1 = item.ArriveOneCity.Trim();
                                    mgs.tCity2 = item.ArriveSecondCity.Trim();
                                    mgs.feeMode = '2';
                                    if (item.SapPostingDate.HasValue)
                                    {
                                        mgs.ENDTIME = item.SapPostingDate.Value;
                                    }
                                    mgs.bwweight = (item.ProductWeight.HasValue ? float.Parse(item.ProductWeight.Value.ToString()) : 0) / 1000;

                                    mgs.mergeOrder = mg_d.mergeOrder;
                                    index1 = true;
                                    if (mg_d.mergeOrder != 0)
                                    {
                                        listmg.Add(mgs);
                                    }
                                    j = listmg.Count;
                                }
                            }
                            //}
                        }
                    }
                    #region 如果没有就向合并后的基本信息集合中添加一条信息
                    if (index)
                    {
                        index_order++;
                        merge mg = new merge();
                        mg.Provinces = item.ArriveProvince.Trim();
                        mg.weight = float.Parse((wight * count).ToString()) / 1000;
                        mg.tCity1 = item.ArriveOneCity.Trim();
                        mg.tCity2 = item.ArriveSecondCity.Trim();
                        //默认为普货
                        char feemode = '0';
                        //危险品                    
                        if (productTypeCode.ToUpper().Trim() == "D00")
                        {
                            feemode = '1';
                        }
                        if (productTypeCode.ToUpper().Trim() == "D01")
                        {
                            feemode = '2';
                        }
                        mg.feeMode = feemode;
                        if (item.SapPostingDate.HasValue)
                        {
                            mg.ENDTIME = item.SapPostingDate.Value;
                        }
                        if (!string.IsNullOrWhiteSpace(productTypeCode) && feemode == '1')
                        {
                            mg.wxweight = float.Parse((wight * count).ToString()) / 1000;
                        }
                        if (!string.IsNullOrWhiteSpace(productTypeCode) && feemode == '2')
                        {
                            mg.bwweight = float.Parse((wight * count).ToString()) / 1000;
                        }
                        mg.mergeOrder = index_order;
                        listmg.Add(mg);
                    }
                    #endregion
                }
                #endregion


                foreach (var item in freightCalculationTableLst)
                {
                    TransModeStruct tms = new TransModeStruct();

                    tms.transMode = item.TransportType;
                    tms.SAPItemNo = item.ID.ToString();
                    tms.fFactory = item.DeliveryFactory;
                    tms.fFactorys = new List<string> { item.DeliveryFactory };
                    tms.tProvince = item.ArriveProvince.Trim();
                    tms.tCity1 = item.ArriveOneCity.Trim();
                    tms.tCity2 = item.ArriveSecondCity.ToString().Trim();
                    tms.orderNo = item.OrderNo;
                    tms.TRANSPORT = item.BearMode.ToString();
                    tms.Logistics = item.ForwarderNo;
                    tms.js_orderno = item.ID.ToString();
                    //默认为普货
                    char feemode = '0';
                    productTypeCode = !string.IsNullOrWhiteSpace(item.ProductTypeCode) ? item.ProductTypeCode.ToUpper().Trim() : "";
                    //危险品
                    //string ceshi = item.ProductTypeCode.ToUpper();
                    if (productTypeCode == "D00")
                    {
                        feemode = '1';
                    }
                    //保温品
                    else if (productTypeCode == "D01")
                    {
                        feemode = '2';
                    }
                    //卷材
                    else if (productTypeCode == "D02")
                    {
                        feemode = '3';
                    }
                    tms.feeMode = feemode;
                    tms.isLoad = char.Parse(item.IsUnloadingGoods.HasValue ? item.IsUnloadingGoods.Value.ToString() : "0");
                    tms.returngos = "0";
                    if (item.SapPostingDate.HasValue)
                    {
                        tms.sendDate = item.SapPostingDate.Value;
                    }
                    tms.ToAddress = item.ArriveAddress;
                    tms.qty = float.Parse(((item.ProductWeight.HasValue? item.ProductWeight.Value:0) * (item.Number.HasValue ? item.Number.Value : 0)).ToString()) / 1000;
                    for (int j = 0; j < listmg.Count; j++)
                    {
                        HY_Specialcity_Maintain specialcityMaintain = new HY_Specialcity_Maintain();
                        if (item.DeliveryFactory.ToString() == "YH50")
                        {

                            specialcityMaintain.City = tms.tCity1;
                            if (specialcityMaintain.IsSpecialCityByCity())
                            {
                                if (tms.tProvince == listmg[j].Provinces && tms.tCity2 == listmg[j].tCity2 && tms.sendDate == listmg[j].ENDTIME && tms.returngos == listmg[j].returnflg)
                                {
                                    if (tms.weight < listmg[j].weight)
                                    {
                                        tms.weight = listmg[j].weight;
                                    }
                                    if (tms.feeMode == '1' && listmg[j].feeMode == '1')
                                    {
                                        tms.wxweight = listmg[j].wxweight;
                                    }
                                    if (tms.feeMode == '2' && listmg[j].feeMode == '2')
                                    {
                                        tms.bwweight = listmg[j].bwweight;
                                    }
                                }

                                if (tms.tProvince == listmg[j].Provinces && tms.tCity2 == listmg[j].tCity2 && tms.sendDate == listmg[j].ENDTIME && tms.returngos == listmg[j].returnflg)
                                {
                                    tms.mergeOrder = listmg[j].mergeOrder;
                                }
                            }
                            else
                            {
                                if (tms.tProvince == listmg[j].Provinces && tms.tCity2 == listmg[j].tCity2 && tms.sendDate == listmg[j].ENDTIME && tms.returngos == listmg[j].returnflg)
                                {
                                    if (tms.weight < listmg[j].weight)
                                    {
                                        tms.weight = listmg[j].weight;
                                    }
                                    if (tms.feeMode == '1' && listmg[j].feeMode == '1')
                                    {
                                        tms.wxweight = listmg[j].wxweight;
                                    }
                                    if (tms.feeMode == '2' && listmg[j].feeMode == '2')
                                    {
                                        tms.bwweight = listmg[j].bwweight;
                                    }
                                }

                                if (tms.tProvince == listmg[j].Provinces && tms.tCity2 == listmg[j].tCity2 && tms.sendDate == listmg[j].ENDTIME && tms.returngos == listmg[j].returnflg)
                                {
                                    tms.mergeOrder = listmg[j].mergeOrder;
                                }
                            }
                        }
                        else
                        {
                            specialcityMaintain.City = tms.tCity1;
                            if (specialcityMaintain.IsSpecialCityByCity())
                            {
                                if (tms.tProvince == listmg[j].Provinces && tms.tCity2 == listmg[j].tCity2 && tms.sendDate == listmg[j].ENDTIME)
                                {
                                    if (tms.weight < listmg[j].weight)
                                    {
                                        tms.weight = listmg[j].weight;
                                    }
                                    if (tms.feeMode == '1' && listmg[j].feeMode == '1')
                                    {
                                        tms.wxweight = listmg[j].wxweight;
                                    }
                                    if (tms.feeMode == '2' && listmg[j].feeMode == '2')
                                    {
                                        tms.bwweight = listmg[j].bwweight;
                                    }
                                }

                                if (tms.tProvince == listmg[j].Provinces && tms.tCity2 == listmg[j].tCity2 && tms.sendDate == listmg[j].ENDTIME)
                                {
                                    tms.mergeOrder = listmg[j].mergeOrder;
                                }
                            }
                            else
                            {
                                if (tms.tProvince == listmg[j].Provinces && tms.tCity2 == listmg[j].tCity2 && tms.sendDate == listmg[j].ENDTIME)
                                {
                                    if (tms.weight < listmg[j].weight)
                                    {
                                        tms.weight = listmg[j].weight;
                                    }
                                    if (tms.feeMode == '1' && listmg[j].feeMode == '1')
                                    {
                                        tms.wxweight = listmg[j].wxweight;
                                    }
                                    if (tms.feeMode == '2' && listmg[j].feeMode == '2')
                                    {
                                        tms.bwweight = listmg[j].bwweight;
                                    }
                                }

                                if (tms.tProvince == listmg[j].Provinces && tms.tCity2 == listmg[j].tCity2 && tms.sendDate == listmg[j].ENDTIME && tms.returngos == Convert.ToInt32(listmg[j].returnflg).ToString())
                                {
                                    tms.mergeOrder = listmg[j].mergeOrder;
                                }
                            }
                        }
                    }
                    bool add = true;
                    if (add)
                    {
                        listtms.Add(tms);
                    }
                }

                for (int i = 0; i < listtms.Count; i++)
                {
                    for (int j = 0; j < listtms.Count; j++)
                    {
                        if (listtms[i].orderNo == listtms[j].orderNo)
                        {
                            listtms[i].rkqty += listtms[j].qty;
                        }
                    }
                }

                ExpressCont ct = new ExpressCont();
                listrm2 = ct.TransportFeeInterface_yh25(listtms);
                string sql_process = "";

                int failureNumber = 0;        //记录失败条数
                #region 循环判断是否计算运费成功，并将失败的item从集合中移除  
                List<rtnMessage> listrm = new List<rtnMessage>();
                foreach (rtnMessage item in listrm2)
                {
                    if (item.transFee == 0)
                    {
                        failureNumber++;
                    }
                    else
                    {
                        listrm.Add(item);
                    }
                }
                #endregion
                #region 存入数据库

                HY_SettlementPrice price = new HY_SettlementPrice();
                for (int i = 0; i < listrm.Count; i++)
                {
                    rtnMessage rm = listrm[i];


                    string sql = "select * from HY_SettlementPrice WITH(NOLOCK) where ItemId=@ItemId";
                    var settlementPrice = DB<HY_SettlementPrice>.QueryFirstOrDefault(sql, new { ItemId = rm.js_orderno });
                    if (settlementPrice != null)
                    {
                        price.SettlementPrice = Convert.ToDecimal(Math.Round(rm.js_Price, 3));
                        price.SettlementFormulaDesc = rm.rtnDesc;
                        price.DischargeCargoPrice = Convert.ToDecimal(Math.Round(rm.js_cargo, 3));
                        price.DistributionPrice = Convert.ToDecimal(Math.Round(rm.psnum, 3));
                        price.DangerousWeight = Convert.ToDecimal(Math.Round(rm.js_Dangerous_qty, 2));
                        price.DangerousPrice = Convert.ToDecimal(Math.Round(rm.js_Dangerous_Price, 3));
                        price.DangerousUpRate = Convert.ToDecimal(Math.Round(rm.js_Dangerous_Proportion, 3));
                        price.DangerousSumPrice = Convert.ToDecimal(Math.Round(rm.js_Dangerous, 3));
                        price.KeepWarmPrice = Convert.ToDecimal(Math.Round(rm.js_preservation_Price, 3));
                        price.KeepWarmWeight = Convert.ToDecimal(Math.Round(rm.js_preservation_qty, 2));
                        price.KeepWarmUpRate = Convert.ToDecimal(Math.Round(rm.js_preservation_Proportion, 3));
                        price.KeepWarmSumPrice = Convert.ToDecimal(Math.Round(rm.js_preservation, 3));
                        price.HorizontalPrice = Convert.ToDecimal(Math.Round(rm.js_Horizontal_Price, 3));
                        price.HorizontalWeight = Convert.ToDecimal(Math.Round(rm.js_Horizontal_qty, 2));
                        price.HorizontalUpRate = Convert.ToDecimal(Math.Round(rm.js_Horizontal_Proportion, 3));
                        price.HorizontalSumPrice = Convert.ToDecimal(Math.Round(rm.js_Horizontal, 3));
                        price.Modify = qx_User.CurrentUser != null ? qx_User.CurrentUser.UNAME : "";
                        price.Update();
                    }
                    else
                    {
                        price.SettlementPrice = Convert.ToDecimal(Math.Round(rm.js_Price, 3));
                        price.SettlementFormulaDesc = rm.rtnDesc;
                        price.DischargeCargoPrice = Convert.ToDecimal(Math.Round(rm.js_cargo, 3));
                        price.DistributionPrice = Convert.ToDecimal(Math.Round(rm.psnum, 3));
                        price.DangerousWeight = Convert.ToDecimal(Math.Round(rm.js_Dangerous_qty, 2));
                        price.DangerousPrice = Convert.ToDecimal(Math.Round(rm.js_Dangerous_Price, 3));
                        price.DangerousUpRate = Convert.ToDecimal(Math.Round(rm.js_Dangerous_Proportion, 3));
                        price.DangerousSumPrice = Convert.ToDecimal(Math.Round(rm.js_Dangerous, 3));
                        price.KeepWarmPrice = Convert.ToDecimal(Math.Round(rm.js_preservation_Price, 3));
                        price.KeepWarmWeight = Convert.ToDecimal(Math.Round(rm.js_preservation_qty, 2));
                        price.KeepWarmUpRate = Convert.ToDecimal(Math.Round(rm.js_preservation_Proportion, 3));
                        price.KeepWarmSumPrice = Convert.ToDecimal(Math.Round(rm.js_preservation, 3));
                        price.HorizontalPrice = Convert.ToDecimal(Math.Round(rm.js_Horizontal_Price, 3));
                        price.HorizontalWeight = Convert.ToDecimal(Math.Round(rm.js_Horizontal_qty, 2));
                        price.HorizontalUpRate = Convert.ToDecimal(Math.Round(rm.js_Horizontal_Proportion, 3));
                        price.HorizontalSumPrice = Convert.ToDecimal(Math.Round(rm.js_Horizontal, 3));
                        price.Modify = qx_User.CurrentUser != null ? qx_User.CurrentUser.UNAME : "";
                        price.ModifyDate = DateTime.Now;
                        price.ItemId = Convert.ToInt32(rm.js_orderno);
                        price.TansportPriceTableId = Convert.ToInt32(rm.js_orderno);
                        price.Insert();
                    }
                }
                //存储最后合并成订单的结果
                List<rtnMessage> listrm_order = new List<rtnMessage>();
                for (int i = 0; i < listrm.Count; i++)
                {
                    bool index = true;
                    rtnMessage rm = listrm[i];
                    listrm_order.Add(rm);
                    //for (int j = 0; j < listrm_order.Count; j++)
                    //{
                    //if(listrm_order[j].OrderNo == rm.OrderNo)
                    //{
                    //listrm_order[j].transFee = rm.transFee;
                    //listrm_order[j].rtnDesc  = rm.rtnDesc;
                    //index = false;
                    //}
                    //}
                    //if (index)
                    //{
                    //    listrm_order.Add(rm);
                    //}
                }



                string FACTORYNO = "";
                if (!string.IsNullOrEmpty("") && "" != "-1")
                {
                    FACTORYNO = "";
                }

                string MoneyId = "" + FACTORYNO + DateTime.Now.ToString("yyMMddmmss");
                int index2 = 0;

                foreach (rtnMessage rm in listrm_order)
                {
                    if (rm.rtnValue == '0')
                    {
                        DateTime dtm = rm.sendDate ==DateTime.MinValue?DateTime.Now:rm.sendDate;
                        freightCalculationTable.ID = Convert.ToInt32(rm.js_orderno); //运费计算表ID
                        freightCalculationTable.RealPrice = Convert.ToDecimal(rm.transFee); //实际费用
                        freightCalculationTable.OutboundOrderTotalPrice = Convert.ToDecimal(rm.transFee); //出库单总费用
                        freightCalculationTable.CalculationProcess = rm.rtnDesc;//计算方式
                        freightCalculationTable.SettlementNo = MoneyId; //结算单号
                        freightCalculationTable.OnTheWayDay = dtm.AddDays(rm.timeCost); //在途天数
                        freightCalculationTable.ReturnOrderDay = dtm.AddDays(rm.timeCost);//应返单天数
                        freightCalculationTable.MergeOrderNo = rm.mergeOrder.ToString();  //合并单号
                        freightCalculationTable.CalculationDate = DateTime.Now;
                        freightCalculationTable.Modify = qx_User.CurrentUser != null ? qx_User.CurrentUser.UNAME : "";
                        freightCalculationTable.ModifyDate = DateTime.Now;
                        if (!freightCalculationTable.Update())
                        {
                            failureNumber++;
                            continue;
                        }

                        //string sql_num = string.Format("update qx_InAndOutBoundOrderfee set REALPRICE = '{0}',REALPRICE_total = '{0}',AUDITREMARKS = '{1}',mforder='{3}',TIMECOST='{4}',TIMEArrival='{5}',mergeOrder='{6}' where INORDERNO = '{2}'", rm.transFee, rm.rtnDesc, rm.OrderNo, MoneyId, dtm.AddDays(rm.timeCost), dtm.AddDays(rm.returnCost).AddDays(rm.timeCost), rm.mergeOrder);
                        //if (!(DB.ExecuteNonQuery(sql_num) > 0))
                        //{
                        //    failureNumber++;
                        //    continue;
                        //}
                    }
                    else
                    {
                        freightCalculationTable.ID = Convert.ToInt32(rm.js_orderno); //运费计算表ID
                        freightCalculationTable.RealPrice = Convert.ToDecimal(rm.transFee);
                        freightCalculationTable.CalculationProcess = rm.rtnMsg;
                        freightCalculationTable.OrderNo = rm.OrderNo;
                        freightCalculationTable.SettlementNo = MoneyId;
                        freightCalculationTable.MergeOrderNo = rm.mergeOrder.ToString();
                        freightCalculationTable.CalculationDate = DateTime.Now;
                        freightCalculationTable.Modify = qx_User.CurrentUser != null ? qx_User.CurrentUser.UNAME : "";
                        freightCalculationTable.ModifyDate = DateTime.Now;
                        if (!freightCalculationTable.UpdateCalculationFailData())
                        {
                            failureNumber++;
                            continue;
                        }
                        //string sql_num = string.Format("update qx_InAndOutBoundOrderfee set REALPRICE = '{0}',REALPRICE_total = '{0}',AUDITREMARKS = '{1}',mforder='{3}',mergeOrder='{4}' where INORDERNO = '{2}'", rm.transFee, rm.rtnMsg, rm.OrderNo, MoneyId, rm.mergeOrder);
                        //if (!(DB.ExecuteNonQuery(sql_num) > 0))
                        //{
                        //    failureNumber++;
                        //}
                    }

                    index2++;
                }
                #endregion


                return new List<int> { index2, failureNumber };
                //return 0;
                //}
            //}

        }


        #region 外加剂逻辑
        /// <summary>
        /// 减水剂计算(正达物流)
        /// </summary>
        public List<int> jsjjisuan(List<string> Codes, string forwarderNo, string factoryNo)
        {
            DataTable dt = new DataTable();
            //存储合并后的重量
            List<merge> listmg = new List<merge>();
            //存储计算需求数据
            List<TransModeStruct> listtms = new List<TransModeStruct>();
            //存储计算结果
            List<rtnMessage> listrm2 = new List<rtnMessage>();
            List<HY_FreightCalculationTable> freightCalculationTableLst = new List<HY_FreightCalculationTable>();

            HY_FreightCalculationTable freightCalculationTable = new HY_FreightCalculationTable();
            //freightCalculationTableLst = freightCalculationTable.GetFreightCalculationTableList(Codes);

            foreach (var item in freightCalculationTableLst)
            {
                TransModeStruct tms = new TransModeStruct();

                tms.transMode = "公路";
                tms.fFactory = item.DeliveryFactory;
                tms.fFactorys = new List<string> { item.DeliveryFactory };
                tms.tProvince = item.ArriveProvince;
                tms.tCity1 = item.ArriveOneCity;
                tms.tCity2 = item.ArriveSecondCity;
                tms.orderNo = item.OrderNo;
                tms.TRANSPORT = item.TransportType;
                tms.rks = item.SapDeliveryOrderNo;
                tms.Customerdesc = item.CustomerName;
                tms.Logistics = item.ForwarderNo;
                tms.origintransport = item.ShipmentPlace;

                if (item.CalculationDate.HasValue)
                {
                    tms.sendDate = item.CalculationDate.Value;
                }
                tms.ToAddress = item.ArriveAddress;
                tms.qty = item.ProductWeight.HasValue ? float.Parse(item.ProductWeight.Value.ToString()) : 0;

                //如果吨位大于21吨，按照30吨计算
                if (tms.qty >= 21 && forwarderNo == "YS12000076")
                {
                    tms.qty = 30;
                }

                HY_MilesWai mILESWai = new HY_MilesWai();
                mILESWai.FactoryNo = item.DeliveryFactory;
                mILESWai.Province = item.ArriveProvince;
                mILESWai.City = item.ArriveSecondCity;
                mILESWai.CustomerName = item.CustomerName;
                mILESWai.PlaceOfShipment = item.ShipmentPlace;
                var miles = mILESWai.GetMILES();
                if (miles != null)
                {
                    tms.MILES = miles.MILES.HasValue ? float.Parse(miles.MILES.Value.ToString()) : 0;
                    if (tms.qty >= 30 && tms.MILES < 300)
                    {
                        tms.MILES = 300;
                    }
                }
                listtms.Add(tms);

            }
            int num = 1;
            int index = 0;
            for (int i = 0; i < listtms.Count; i++)
            {
                TransModeStruct tms = listtms[i];
                rtnMessage rm = new rtnMessage();
                rm.OrderNo = tms.orderNo;
                rm.sendDate = tms.sendDate;
                //存放运价
                double price = 0;
                //转换发货时间
                string sd = tms.sendDate.ToString("yyyy-MM-dd");
                rm.rtnValue = '0';
                rm.timeCost = 3;
                rm.returnCost = 10;

                rm.mergeOrder = num;
                num += 1;
                HY_TransportFeeMWJJ fwjj = new HY_TransportFeeMWJJ();
                fwjj = fwjj.GetWjjFee(tms.qty.ToString(), tms.MILES.ToString(), tms.Logistics, tms.origintransport);
                price = fwjj.Price.HasValue ? Convert.ToDouble(fwjj.Price) : 0;
                if (fwjj.FeeType.HasValue && fwjj.FeeType.Value == 1)
                {
                    rm.transFee = price;
                    rm.rtnDesc += "计算方式：(外加剂)（重量）" + tms.qty + "(距离)" + tms.MILES + "km 小于40KM  到" + tms.tCity2;
                }
                else if (fwjj.FeeType == 2)
                {
                    rm.transFee = Math.Round(tms.qty * price * tms.MILES, 2);
                    rm.rtnDesc += "计算方式：(外加剂)" + price + "*（重量）" + tms.qty + "*(距离)" + tms.MILES + " 到" + tms.tCity2;
                }
                else
                {
                    rm.rtnValue = '1';
                    rm.rtnMsg = "重量有误（正达物流逻辑）";
                    rm.transFee = 0;
                    listrm2.Add(rm);
                    continue;
                }
                listrm2.Add(rm);
            }

            string FACTORYNO = "";
            if (!string.IsNullOrEmpty(factoryNo) && factoryNo != "-1")
            {
                FACTORYNO = factoryNo;
            }

            string MoneyId = "" + FACTORYNO + DateTime.Now.ToString("yyMMddmmss");

            int failureNumber = 0;        //记录失败条数
            #region 循环判断是否计算运费成功，并将失败的item从集合中移除    
            List<rtnMessage> listrm = new List<rtnMessage>();
            foreach (rtnMessage item in listrm2)
            {
                if (item.transFee == 0 && !get_Logistics_wei(forwarderNo))
                {
                    failureNumber++;
                }
                else
                {
                    listrm.Add(item);
                }
            }
            #endregion
            foreach (rtnMessage rm in listrm)
            {
                DateTime dtm = rm.sendDate;
                if (rm.rtnValue == '0')
                {
                    freightCalculationTable.ID = Convert.ToInt32(rm.js_orderno);//运费计算表ID
                    freightCalculationTable.RealPrice = Convert.ToDecimal(rm.transFee);
                    freightCalculationTable.OutboundOrderTotalPrice = Convert.ToDecimal(rm.transFee);
                    freightCalculationTable.CalculationProcess = rm.rtnDesc;
                    freightCalculationTable.SettlementNo = MoneyId;
                    freightCalculationTable.OnTheWayDay = dtm.AddDays(rm.timeCost);
                    freightCalculationTable.ReturnOrderDay = dtm.AddDays(rm.timeCost);
                    freightCalculationTable.MergeOrderNo = rm.mergeOrder.ToString();
                    if (!freightCalculationTable.Update())
                    {
                        failureNumber++;
                        continue;
                    }
                }
                else
                {
                    if (!freightCalculationTable.UpdateCalculationFailData())
                    {
                        failureNumber++;
                        continue;
                    }
                }

                index++;
            }

            return new List<int> { index, failureNumber };
        }
        #endregion


        /// <summary>
        /// 判断物流商是否为外加剂运输商
        /// </summary>
        /// <param name="Logistics">运输商编号</param>
        /// <returns></returns>
        public bool get_Logistics_wei(string logisticsNo)
        {
            Logistics_wei logistics = new Logistics_wei();
            logistics.Factory = "";
            logistics.Logistics = logisticsNo;
            return logistics.IsLogisticsW();
        }

        private static float GetTeshuFee(TransModeStruct tms, string weight, string type)
        {
            HY_SpecialFee fee = new HY_SpecialFee();
            fee.FactoryNo = tms.fFactory;
            fee.LogisticsNo = tms.Logistics;
            float weixianfee = fee.GetSpecialFee(weight, type);
            return weixianfee;
        }




        /// <summary>
        /// 危险品计算
        /// </summary>
        /// <param name="tms"></param>
        /// <param name="rm"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        private static double assignmentDangerousGoods(TransModeStruct tms, rtnMessage rm, ref float price)
        {
            double nums;
            tms.wxweight = tms.wxweight / 1000;
            nums = Math.Round(tms.qty / tms.wxweight, 3);
            rm.js_Dangerous_Price = price;
            price = price * 0.6f;
            if (tms.wxweight <= 0.2 && tms.wxweight > 0)
            {
                tms.wxweight = 0.2f;
            }
            else if (tms.wxweight > 0.2 && tms.wxweight <= 0.5)
            {
                tms.wxweight = 0.5f;
            }
            else if (tms.wxweight > 0.5 && tms.wxweight <= 1)
            {
                tms.wxweight = 1;
            }

            rm.transFee += Math.Round(price * tms.wxweight * nums, 2);
            rm.rtnDesc += "计算方式：(危险品放大判断逻辑运算) （单价）" + price + " * (重量) " + tms.wxweight + " T*" + nums + " 到" + tms.tCity2;
            rm.js_Dangerous = Math.Round(price * tms.wxweight * nums, 2);
            rm.js_Dangerous_qty = Math.Round(tms.wxweight * nums, 3);
            rm.js_Dangerous_Proportion = 0.6f;
            return nums;
        }

        /// <summary>
        /// 自粘卷材计算
        /// </summary>
        /// <param name="tms"></param>
        /// <param name="rm"></param>
        /// <param name="price"></param>
        /// <param name="nums"></param>
        private static void assignmentSelfAdhesiveCoil(TransModeStruct tms, rtnMessage rm, float price, double nums)
        {
            if (tms.fFactory != "YH80" && tms.fFactory != "YH51" && tms.fFactory != "YH70")
            {
                rm.transFee = Math.Round(price * tms.fangdaWeight * nums, 2) * 1.1225f;
                rm.rtnDesc = "计算方式：(放大判断逻辑运算) （单价）" + price + " * (重量) " + tms.fangdaWeight + " T*" + nums + "*1.1225 到" + tms.tCity2;
                //*1.31f;
                rm.js_Horizontal = Math.Round(price * tms.fangdaWeight * nums, 2) * 0.1225f;
                rm.js_Horizontal_Proportion = 0.1225f;
                rm.js_Horizontal_qty = Math.Round(tms.fangdaWeight * nums, 3);
                rm.js_Horizontal_Price = price;
            }
        }

        /// <summary>
        /// 保温品计算
        /// </summary>
        /// <param name="tms"></param>
        /// <param name="rm"></param>
        /// <param name="price"></param>
        /// <param name="nums"></param>
        private static void assignmentThermalInsulation(TransModeStruct tms, rtnMessage rm, ref float price, ref double nums)
        {
            HY_HoldingTime holdingTime = new HY_HoldingTime();
            holdingTime.FirstTime = DateTime.Now;
            holdingTime.FactoryNo = tms.fFactory;
            if (holdingTime.IsHaveHoldingTime())
            {
                tms.bwweight = tms.bwweight / 1000;
                nums = Math.Round(tms.qty / tms.bwweight, 3);
                rm.js_preservation_Price = price;
                price = price * 1f;
                if (tms.bwweight <= 0.2 && tms.bwweight > 0)
                {
                    tms.bwweight = 0.2f;
                }
                else if (tms.bwweight > 0.2 && tms.bwweight <= 0.5)
                {
                    tms.bwweight = 0.5f;
                }
                else if (tms.bwweight > 0.5 && tms.bwweight <= 1)
                {
                    tms.bwweight = 1;
                }

                rm.transFee += Math.Round(price * tms.bwweight * nums, 2);
                rm.rtnDesc += "计算方式：(保温品放大判断逻辑运算) （单价）" + price + " * (重量) " + tms.bwweight + " T*" + nums + " 到" + tms.tCity2;
                rm.js_preservation = Math.Round(price * tms.bwweight * nums, 2);
                rm.js_preservation_qty = Math.Round(tms.bwweight * nums, 3);
                rm.js_preservation_Proportion = 0.6f;
            }
        }

        private static void getfangdaWeight(TransModeStruct tms)
        {
            if (tms.weight <= 0.2f && tms.weight > 0)
            {
                tms.fangdaWeight = 0.2f;
            }
            else if (tms.weight > 0.2f && tms.weight <= 0.5f)
            {
                tms.fangdaWeight = 0.5f;
            }
            else if (tms.weight > 0.5f && tms.weight <= 1)
            {
                tms.fangdaWeight = 1;
            }
        }

        //取运费从低后的
        public HY_TansportPriceTable congdi(TransModeStruct tms, string sd, float weight)
        {
            DataTable dt = new DataTable();
            HY_TansportPriceTable transportFeeM = new HY_TansportPriceTable();

            if (tms.tCity1 == "上海市" && tms.fFactory.ToUpper() == "YH10")
            {
                if (tms.weight <= 15)
                {
                    assignmentTransportFeeM("yh11", sd, tms.tProvince, tms.tCity1, tms.transMode, transportFeeM);
                    transportFeeM = transportFeeM.GetTransportFeeMInfo(1);
                    transportFeeM.Price = 424;
                }
                if (tms.weight > 15)
                {
                    assignmentTransportFeeM("yh11", sd, tms.tProvince, tms.tCity1, tms.transMode, transportFeeM);
                    transportFeeM = transportFeeM.GetTransportFeeMInfo(2);
                    transportFeeM.Price = 389;
                }
                return transportFeeM;
            }
            else
            {
                int weightType = Convert.ToInt32(weight);
                assignmentTransportFeeM(tms.fFactory.ToLowerInvariant(), sd, tms.tProvince, tms.tCity2, tms.transMode, transportFeeM);
                transportFeeM = transportFeeM.GetTransportFeeMInfo(weightType);
                //二级市取不到，取一级市
                if (transportFeeM == null)
                {
                    transportFeeM = new HY_TansportPriceTable();
                    assignmentTransportFeeM(tms.fFactory.ToLowerInvariant(), sd, tms.tProvince, tms.tCity1, tms.transMode, transportFeeM);
                    transportFeeM = transportFeeM.GetTransportFeeMInfo(weightType);
                }
                return transportFeeM;
            }
        }


        //取运费从低后的
        public List<HY_TansportPriceTable> congdi_yh25(TransModeStruct tms, string sd, int weight, out int index, out float num)
        {
            index = 0;
            num = 0;
            HY_TansportPriceTable tansportPriceTable = new HY_TansportPriceTable();
            tansportPriceTable.FactoryNo = tms.fFactory;
            tansportPriceTable.ArriveProvince = tms.tProvince;
            tansportPriceTable.ArriveCity = tms.tCity2;
            tansportPriceTable.TransportType = tms.transMode;
            tansportPriceTable.TransportBusinessNo = tms.Logistics;
            var weightRangeSign = tansportPriceTable.GetWeightRangeSign(weight);

            List<HY_TansportPriceTable> lst = new List<HY_TansportPriceTable>();
            if (weightRangeSign != null && weightRangeSign.Count > 0)
            {
                lst = GetTansportPriceTable(tms, weight, tansportPriceTable, weightRangeSign, tms.tCity2);
            }
            else
            {
                HY_Specialcity_Maintain specialcityMaintain = new HY_Specialcity_Maintain();
                //判断是否为特殊城市
                specialcityMaintain.City = tms.tCity1;
                if (specialcityMaintain.IsSpecialCityByCity())
                {
                    if (tms.tCity2.Contains("区"))
                    {
                        tansportPriceTable.ArriveCity = tms.tCity2;
                        weightRangeSign = tansportPriceTable.GetWeightRangeSign(weight);
                        lst = GetTansportPriceTable(tms, weight, tansportPriceTable, weightRangeSign, tms.tCity1);
                    }
                    else
                    {
                        tansportPriceTable.ArriveCity = tms.tCity2;
                        weightRangeSign = tansportPriceTable.GetWeightRangeSign(weight);
                        lst = GetTansportPriceTable(tms, weight, tansportPriceTable, weightRangeSign, tms.tCity1);
                    }
                }
            }
            if (!tms.tCity2.Contains("区") && lst.Count <= 0 || lst == null)
            {
                index = 1;
                var price = tansportPriceTable.GetRangePrice();
                if (price > 0)
                {
                    HY_CityDistance mILES = new HY_CityDistance();
                    mILES.City = tms.tCity2;
                    mILES.FactoryNo = tms.fFactory.ToLowerInvariant();
                    var range = mILES.GetMILES();
                    if (range > 0)
                    {
                        num = float.Parse(range.ToString()) * float.Parse(price.ToString());
                    }

                    tansportPriceTable.ArriveCity = tms.tCity1;
                    var tansportPriceTables = tansportPriceTable.GetTransportFeeMInfoLst(weight);
                    if (tansportPriceTables != null)
                    {
                        lst[0].ReturnOrderDay = tansportPriceTables.FirstOrDefault().ReturnOrderDay;
                    }
                }
                else
                {
                    return lst;
                }
            }
            return lst;
        }


        private static void assignmentTransportFeeM(string FACTORYNO, string sd, string PROVINCE, string CITY, string TRANSPORTID, HY_TansportPriceTable tansportPriceTable)
        {
            tansportPriceTable.FactoryNo = FACTORYNO;
            tansportPriceTable.ArriveProvince = PROVINCE;
            tansportPriceTable.ArriveCity = CITY;
            tansportPriceTable.TransportType = TRANSPORTID;
        }

        private static void lessThan(TransModeStruct tms, ref float weight, ref int weights)
        {
            if (0 < tms.qty && tms.qty < 5)
            {
                weight = 1;
                weights = 5;
            }
            else if (5 <= tms.qty && tms.qty < 10)
            {
                weight = 2;
                weights = 10;
            }
            else if (10 <= tms.qty && tms.qty < 20)
            {
                weight = 3;
                weights = 20;
            }
            else if (20 <= tms.qty && tms.qty < 25)
            {
                weight = 4;
                weights = 25;
            }
            else if (25 <= tms.qty)
            {
                weight = 5;
            }
        }

        private static void greaterThanWeight(TransModeStruct tms, ref float weight, ref int weights)
        {
            if (0 < tms.weight && tms.weight < 5)
            {
                weight = 1;
                weights = 5;
            }
            else if (5 <= tms.weight && tms.weight < 10)
            {
                weight = 2;
                weights = 10;
            }
            else if (10 <= tms.weight && tms.weight < 20)
            {
                weight = 3;
                weights = 20;
            }
            else if (20 <= tms.weight && tms.weight < 25)
            {
                weight = 4;
                weights = 25;
            }
            else if (25 <= tms.weight)
            {
                weight = 5;
            }
        }

        /// <summary>
        /// 校验数据
        /// </summary>
        /// <returns></returns>
        public void ValiData(rtnMessage rm, TransModeStruct tms, List<rtnMessage> listrm)
        {
            if (string.IsNullOrEmpty(tms.transMode))
            {
                rm.rtnValue = '1';
                rm.rtnMsg = "运输方式为空,请检查";
                rm.transFee = 0;
                listrm.Add(rm);
                return;
            }
            if (string.IsNullOrEmpty(tms.fFactory))
            {
                rm.rtnValue = '1';
                rm.rtnMsg = "出发地工厂编号为空,请检查";
                rm.transFee = 0;
                listrm.Add(rm);
            }
            if (string.IsNullOrEmpty(tms.tProvince))
            {
                rm.rtnValue = '1';
                rm.rtnMsg = "目的地 省份为空,请检查";
                rm.transFee = 0;
                listrm.Add(rm);
            }
            if (string.IsNullOrEmpty(tms.tCity1))
            {
                rm.rtnValue = '1';
                rm.rtnMsg = "目的地 一级市为空,请检查";
                rm.transFee = 0;
                listrm.Add(rm);

            }
            if (string.IsNullOrEmpty(tms.tCity2))
            {
                rm.rtnValue = '1';
                rm.rtnMsg = "目的地 二级市为空,请检查";
                rm.transFee = 0;
                listrm.Add(rm);
            }
            if (float.IsNaN(tms.qty) || tms.qty <= 0)
            {
                rm.rtnValue = '1';
                rm.rtnMsg = "重量输入有误,请检查";
                rm.transFee = 0;
                listrm.Add(rm);
            }
        }


        private static List<HY_TansportPriceTable> GetTansportPriceTable(TransModeStruct tms, int weight, HY_TansportPriceTable tansportPriceTable, List<HY_TansportPriceTable> weightRangeSign, string city)
        {
            //如果二级市包含区获取一级城市重量标识
            tansportPriceTable.ArriveCity = city;
            if (weightRangeSign != null && weightRangeSign.Count > 0)
            {
                //用来判断实际计算吨位档位
                int num_weight = 0;
                if (weight < weightRangeSign.FirstOrDefault().WeightRangeSign)
                {
                    //当实际吨位小于运价底表吨位时，以最小吨位为准
                    num_weight = weightRangeSign.FirstOrDefault().WeightRangeSign.Value;
                }
                for (int i = 0; i < weightRangeSign.Count; i++)
                {
                    //循环判断是否有与实际吨位相等的吨位范围
                    int weitype = weightRangeSign[i].WeightRangeSign.HasValue ? weightRangeSign[i].WeightRangeSign.Value : 0;
                    if (weight == weitype)
                    {
                        //存在相等的吨位范围
                        num_weight = weight;
                        break;
                    }
                }
                if (weight > weightRangeSign[weightRangeSign.Count - 1].WeightRangeSign.Value)
                {
                    //当实际吨位大于运价底表最大吨位范围时，以最大吨位为准
                    num_weight = weightRangeSign[weightRangeSign.Count - 1].WeightRangeSign.Value;
                }
                //开始取值
                return tansportPriceTable.GetTransportFeeMInfoLst(num_weight);
            }
            return null;
        }
    }
}
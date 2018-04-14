using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Test
{
   public class MsgHandler
    {
        private List<string> gamblingMsg; //群聊天监听到的信息
        private string orderList; //订单参数之一
        private Dictionary<int, int> gameList1=new Dictionary<int,int>();
        private Dictionary<int, int> gameList2 = new Dictionary<int, int>();
        private Dictionary<int, int> gameList3 = new Dictionary<int, int>();
        private Dictionary<int, int> gameList4 = new Dictionary<int, int>();
        private Dictionary<int, int> gameList5 = new Dictionary<int, int>();
        private Dictionary<int, int> gameList6 = new Dictionary<int, int>();
        private  List<string[]> oldInfos = new List<string[]>();
        private  string[] empty = { };

        public MsgHandler()
        {
            this.gamblingMsg = new List<string>();
            InitList();
            
        }

       //msg是群消息 当前金钱 当前利润 a是投注所需金钱
        public string GWJJHandler(string msg, string curMoney, string profit, out decimal a)
        {
            string[] info= GetInfos(msg,Settings.rules.GWJJInfoRules);


            if (info[0] == "" || info[1] == "" || info[2] == "")
            {
                a = 0;
                return Settings.error.ZZ_ERROR;
            }
            else
            {

                //重复订单
                if (Enumerable.SequenceEqual<string>(info, oldInfos[0]))
                {
                    a = 0;
                    return Settings.error.REPEAT_ERROR;
                }
                else
                {
                    oldInfos[0] = info;
                }

            }

            var curQi = info[0];
            var recNums = info[1];
            int recBei = int.Parse(info[2]);


            LogSystem.CountRecord(Settings.GameType.GWJJ, recBei);

            //c代表推荐的数字
            string c = "";
            for (int i = 0; i < recNums.Length - 1; i++)
            {
                c += recNums[i] + "|";
            }
            c += recNums[recNums.Length - 1];

            //t代表倍数  
            int t = GetFinalT(recBei,Settings.gwjj.startBeishu,Settings.gwjj.maxBeishu,Settings.gwjj.flag);
            if (t == 0) {
                a = 0;
                return Settings.error.TOOBIG_ERROR;
            } 

            //n代表下注个数
            int n = recNums.Length;

            //a代表下注金额
            a = GetFinalA(Settings.gwjj.m, t, n);

            //如果下注金额大于当前余额取消下注
            float curM = float.Parse(curMoney);
            if (a > (decimal)curM) {
                var mm = a;
                a = 0;
                return "下注失败\n下注所需金额：" + mm.ToString() + "，余额：" + curMoney;
            
            } 

            //日志记录
            var log = "[个位掘金]"+"\r\n期数:" + curQi + "  推荐数字:" + recNums + "  推荐倍数：" + recBei + "  实际倍数：" + t + "\r\n投注前余额:" + curMoney + "元,投注前利润:" + profit + "元";
            LogSystem.Record(log);

            if (Settings.gwjj.flag == 1)
            {
                if (recBei >Settings.gwjj.maxBeishu )
                    return "订单创建失败!\n推荐倍数:" + recBei + "\n设定最大倍数:" + Settings.gwjj.maxBeishu;
            }
            else if (Settings.gwjj.flag == 2)
            {
                if (t >Settings.gwjj.maxBeishu || recBei < Settings.gwjj.startBeishu )
                    return "订单创建失败!\n推荐倍数:" + recBei + "\n开始投注倍数:" + Settings.gwjj.startBeishu + "\n设定最大倍数:" + Settings.gwjj.maxBeishu;
            }
            
            orderList = "[{\"i\":21018,\"c\":\"" + c + "\",\"n\":" + n + ",\"t\":\"" + t + "\",\"k\":0,\"m\":"+Settings.gwjj.m+",\"a\":" + a + "}]";
           
            return orderList;
                
        }


        public string DDHandler(string msg, string curMoney, string profit, out decimal a)
        {
            string[] info = GetInfos(msg,Settings.rules.DDInfoRules2);
            if (info[0] == "" || info[1] == "")
            {
                info = GetInfos(msg, Settings.rules.DDInfoRules);
                if (info[0] == "" || info[1] == "")
                {
                    a = 0;
                    return Settings.error.ZZ_ERROR;
                }
            }
            else {
                //重复订单
                if (Enumerable.SequenceEqual<string>(info, oldInfos[1]))
                {
                    a = 0;
                    return Settings.error.REPEAT_ERROR;
                }
                else
                {
                    oldInfos[1] = info;
                }
            }

            var recNums = info[0];
            var recNum = recNums[0];
            var recBei = int.Parse(info[1]);

            LogSystem.CountRecord(Settings.GameType.DD, recBei);

            //c代表推荐的数字
            string c = "";
            c = recNum.ToString();

            //t代表倍数  
            int t = GetFinalT(recBei,Settings.dd.startBeishu,Settings.dd.maxBeishu,Settings.dd.flag);


            //n代表下注个数
            int n = 5;

            //a代表下注金额
            a = GetFinalA(Settings.dd.m,t,n);

            //如果下注金额大于当前余额取消下注
            float curM = float.Parse(curMoney);
            if (a > (decimal)curM)
            {
                var mm = a;
                a = 0;
                return "下注失败\n下注所需金额：" + mm.ToString() + "，余额：" + curMoney;

            }

            //日志记录
            var log = "[独胆]" + "\r\n推荐数字:" + recNums + "  推荐倍数：" + recBei + "  实际倍数：" + t + "\r\n投注前余额:" + curMoney + "元,投注前利润:" + profit + "元";
            LogSystem.Record(log);

            if (Settings.dd.flag == 1)
            {
                if (recBei > Settings.dd.maxBeishu)
                    return "订单创建失败!\n推荐倍数:" + recBei + "\n设定最大倍数:" + Settings.dd.maxBeishu;
            }
            else if (Settings.dd.flag == 2)
            {
                if (t > Settings.dd.maxBeishu || recBei < Settings.dd.startBeishu)
                    return "订单创建失败!\n推荐倍数:" + recBei + "\n开始投注倍数:" + Settings.dd.startBeishu + "\n设定最大倍数:" + Settings.dd.maxBeishu;
            }

            var tmpA = a / 5;
            n = n / 5;
            //  orderList = "";
            orderList = "[{\"i\":21014,\"c\":\"" + c + "\",\"n\":" + n + ",\"t\":" + t + ",\"k\":0,\"m\":" + Settings.dd.m + ",\"a\":" + tmpA + "},"
                + "{\"i\":21015,\"c\":\"" + c + "\",\"n\":" +n + ",\"t\":" + t + ",\"k\":0,\"m\":" + Settings.dd.m + ",\"a\":" + tmpA + "},"
                + "{\"i\":21016,\"c\":\"" + c + "\",\"n\":" + n + ",\"t\":" + t + ",\"k\":0,\"m\":" + Settings.dd.m + ",\"a\":" + tmpA + "},"
                + "{\"i\":21017,\"c\":\"" + c + "\",\"n\":" + n + ",\"t\":" + t + ",\"k\":0,\"m\":" + Settings.dd.m + ",\"a\":" + tmpA + "},"
                + "{\"i\":21018,\"c\":\"" + c + "\",\"n\":" + n + ",\"t\":" + t + ",\"k\":0,\"m\":" + Settings.dd.m + ",\"a\":" + tmpA + "}]";

            return orderList;

        }

        public string DDHandler2(string msg, string curMoney, string profit, out decimal a)
        {
            string[] info = GetInfos2(msg, Settings.rules.DDInfoRules2);
            if (info[0] == "" || info[1] == "")
            {
                a = 0;
                return Settings.error.ZZ_ERROR;
            }


            var recNums = info[0];
            var recNum = recNums[0];
            var recBei = int.Parse(info[1]);

            LogSystem.CountRecord(Settings.GameType.DD, recBei);

            //c代表推荐的数字
            string c = "";
            c = recNum.ToString();

            //t代表倍数  
            int t = GetFinalT(recBei, Settings.dd.startBeishu, Settings.dd.maxBeishu, Settings.dd.flag);


            //n代表下注个数
            int n = 5;

            //a代表下注金额
            a = GetFinalA(Settings.dd.m, t, n);

            //如果下注金额大于当前余额取消下注
            float curM = float.Parse(curMoney);
            if (a > (decimal)curM)
            {
                var mm = a;
                a = 0;
                return "下注失败\n下注所需金额：" + mm.ToString() + "，余额：" + curMoney;

            }

            //日志记录
            var log = "[独胆]" + "\r\n推荐数字:" + recNums + "  推荐倍数：" + recBei + "  实际倍数：" + t + "\r\n投注前余额:" + curMoney + "元,投注前利润:" + profit + "元";
            LogSystem.Record(log);

            if (Settings.dd.flag == 1)
            {
                if (recBei > Settings.dd.maxBeishu)
                    return "订单创建失败!\n推荐倍数:" + recBei + "\n设定最大倍数:" + Settings.dd.maxBeishu;
            }
            else if (Settings.dd.flag == 2)
            {
                if (t > Settings.dd.maxBeishu || recBei < Settings.dd.startBeishu)
                    return "订单创建失败!\n推荐倍数:" + recBei + "\n开始投注倍数:" + Settings.dd.startBeishu + "\n设定最大倍数:" + Settings.dd.maxBeishu;
            }

            var tmpA = a / 5;
            n = n / 5;
            //  orderList = "";
            orderList = "[{\"i\":21014,\"c\":\"" + c + "\",\"n\":" + n + ",\"t\":" + t + ",\"k\":0,\"m\":" + Settings.dd.m + ",\"a\":" + tmpA + "},"
                + "{\"i\":21015,\"c\":\"" + c + "\",\"n\":" + n + ",\"t\":" + t + ",\"k\":0,\"m\":" + Settings.dd.m + ",\"a\":" + tmpA + "},"
                + "{\"i\":21016,\"c\":\"" + c + "\",\"n\":" + n + ",\"t\":" + t + ",\"k\":0,\"m\":" + Settings.dd.m + ",\"a\":" + tmpA + "},"
                + "{\"i\":21017,\"c\":\"" + c + "\",\"n\":" + n + ",\"t\":" + t + ",\"k\":0,\"m\":" + Settings.dd.m + ",\"a\":" + tmpA + "},"
                + "{\"i\":21018,\"c\":\"" + c + "\",\"n\":" + n + ",\"t\":" + t + ",\"k\":0,\"m\":" + Settings.dd.m + ",\"a\":" + tmpA + "}]";

            return orderList;

        }


        public string HSZLHandler(string msg, string curMoney, string profit, out decimal a)
        {
            string[] info = GetInfos(msg,Settings.rules.HSZLInfoRules);
            if (info[0] == "" || info[1] == "")
            {
                a = 0;
                return Settings.error.ZZ_ERROR;
            }
            else {

                //重复订单
                if (Enumerable.SequenceEqual<string>(info, oldInfos[2]))
                {
                    a = 0;
                    return Settings.error.REPEAT_ERROR;
                }
                else
                {
                    oldInfos[2] = info;
                }
            }
            var notRecnum = int.Parse(info[0]);
            var recBei = int.Parse(info[1]);

            LogSystem.CountRecord(Settings.GameType.HSZL, recBei);

            //c代表推荐的数字
            string c = "";
            for (int i = 0; i <= 9; i++)
            {
                if(notRecnum!=i)
                c += i + "|";
            }
            c = c.Substring(0, 17);

            //t代表倍数  不同模式倍数不用
            int t = GetFinalT(recBei,Settings.hszl.startBeishu,Settings.hszl.maxBeishu,Settings.hszl.flag);


            //n代表下注个数
            int n = 84;

            //a代表下注金额
            a = GetFinalA(Settings.hszl.m,t,n);

            //如果下注金额大于当前余额取消下注
            float curM = float.Parse(curMoney);
            if (a > (decimal)curM)
            {
                var mm = a;
                a = 0;
                return "下注失败\n下注所需金额：" + mm.ToString() + "，余额：" + curMoney;

            }

            //日志记录
            var log = "[后三组六]" + "\r\n推荐数字:" + c + "  推荐倍数：" + recBei + "  实际倍数：" + t + "\r\n投注前余额:" + curMoney + "元,投注前利润:" + profit + "元";
            LogSystem.Record(log);

            if (Settings.hszl.flag == 1)
            {
                if (recBei > Settings.hszl.maxBeishu)
                    return "订单创建失败!\n推荐倍数:" + recBei + "\n设定最大倍数:" + Settings.hszl.maxBeishu;
            }
            else if (Settings.hszl.flag == 2)
            {
                if (t > Settings.hszl.maxBeishu || recBei < Settings.hszl.startBeishu)
                    return "订单创建失败!\n推荐倍数:" + recBei + "\n开始投注倍数:" + Settings.hszl.startBeishu + "\n设定最大倍数:" + Settings.hszl.maxBeishu;
            }

            //  orderList = "";
            orderList = "[{\"i\":20979,\"c\":\"" + c + "\",\"n\":" + n + ",\"t\":" + t + ",\"k\":0,\"m\":" + Settings.hszl.m + ",\"a\":" + a + "}]";
            return orderList;

        }


        public string BJSCHandler(string msg, string curMoney, string profit, out decimal a)
        {
            string[] info = GetInfos(msg,Settings.rules.BJSCInfoRules);

            if (info[0] == "" || info[1] == "" || info[2] == "")
            {
                a = 0;
                return Settings.error.ZZ_ERROR;
            }
            else {
                //重复订单
                if (Enumerable.SequenceEqual<string>(info, oldInfos[3]))
                {
                    a = 0;
                    return Settings.error.REPEAT_ERROR;
                }
                else
                {
                    oldInfos[3] = info;
                }
            }
            var curQi = info[0];
            var recNums = info[1];
            var recBei = int.Parse(info[2]);

            LogSystem.CountRecord(Settings.GameType.BJSC, recBei);


            //c代表推荐的数字
            string c = recNums;

            //t代表倍数  不同模式倍数不用
            int t = GetFinalT(recBei,Settings.bjsc.startBeishu,Settings.bjsc.maxBeishu,Settings.bjsc.flag);


            //n代表下注个数
            int n =5;

            //a代表下注金额
            a = GetFinalA(Settings.bjsc.m,t,n);

            //如果下注金额大于当前余额取消下注
            float curM = float.Parse(curMoney);
            if (a > (decimal)curM)
            {
                var mm = a;
                a = 0;
                return "下注失败\n下注所需金额：" + mm.ToString() + "，余额：" + curMoney;

            }

            //日志记录
            var log = "[北京赛车]" + "\r\n期数:" + curQi + "\r推荐数字:" + c + "  推荐倍数：" + recBei + "  实际倍数：" + t + "\r\n投注前余额:" + curMoney + "元,投注前利润:" + profit + "元";
            LogSystem.Record(log);

            if (Settings.bjsc.flag == 1)
            {
                if (recBei > Settings.bjsc.maxBeishu)
                    return "订单创建失败!\n推荐倍数:" + recBei + "\n设定最大倍数:" + Settings.bjsc.maxBeishu;
            }
            else if (Settings.bjsc.flag == 2)
            {
                if (t > Settings.bjsc.maxBeishu || recBei < Settings.bjsc.startBeishu)
                    return "订单创建失败!\n推荐倍数:" + recBei + "\n开始投注倍数:" + Settings.bjsc.startBeishu + "\n设定最大倍数:" + Settings.bjsc.maxBeishu;
            }

            //  orderList = "";
            orderList = "[{\"i\":21120,\"c\":\"" + c + "\",\"n\":" + n + ",\"t\":" + t + ",\"k\":0,\"m\":" + Settings.bjsc.m + ",\"a\":" + a + "}]";
            return orderList;

        }

       //传入单位大小，倍数，个数
        private decimal GetFinalA(int m,int t,int n) {
            decimal a = 0;
            float tmp = 0;
            switch (m)
            {
                case 1:
                    tmp = 2;
                    break;
                case 2:
                    tmp = 0.2f;
                    break;
                case 3:
                    tmp = 0.02f;
                    break;
                case 4:
                    tmp = 0.002f;
                    break;
            }
            a=(decimal)tmp * t * n;
            return a;
        }

       //得到最终倍数
        private int GetFinalT(int recBei,int startBei,int maxBei,int flag) {


          int t=recBei;
          switch (flag)
            {
                case 1:
                    if (recBei > maxBei)
                        t = 0; //大于最大倍数 则不下注
                    break;
                case 2:
                    if (startBei == 3)
                    {
                        if (recBei >= startBei)
                        gameList1.TryGetValue(recBei, out t);
                    else
                        t = recBei;

                    }
                    else if (startBei == 8)
                    {
                        if (recBei >= startBei)
                        gameList2.TryGetValue(recBei, out t);
                    else
                        t = recBei;
                    }
                    else if (startBei == 24)
                    {
                        if (recBei >= startBei)
                        gameList3.TryGetValue(recBei, out t);
                    else
                        t = recBei;
                    }

                    else if (startBei == 72)
                    {
                        if (recBei >= startBei)
                            gameList4.TryGetValue(recBei, out t);
                        else
                            t = recBei;
                    }

                    else if (startBei == 216)
                    {
                        if (recBei >= startBei)
                            gameList5.TryGetValue(recBei, out t);
                        else
                            t = recBei;
                    }

                    else if (startBei == 648)
                    {
                        if (recBei >= startBei)
                            gameList6.TryGetValue(recBei, out t);
                        else
                            t = recBei;
                    }
                    break;
                  
            }
            return t;
        }


       //传入内容和正则表达式
        private string[] GetInfos(string content,string rule)
        {
            gamblingMsg.Clear();


            Regex r = new Regex(rule, RegexOptions.None);
            Match mc = r.Match(content);
            string temp = mc.Groups[1].Value.Trim();
            gamblingMsg.Add(temp);
            temp = mc.Groups[2].Value.Trim();
            gamblingMsg.Add(temp);
            temp = mc.Groups[3].Value.Trim();
            gamblingMsg.Add(temp);
            temp = mc.Groups[4].Value.Trim();
            gamblingMsg.Add(temp);

            return gamblingMsg.ToArray();
        }


        private string[] GetInfos2(string content, string rule)
        {
            gamblingMsg.Clear();

            Regex r = new Regex(rule, RegexOptions.None);
            var mc = r.Matches(content);
            if (mc.Count <= 1) {
                gamblingMsg.Add("");
                gamblingMsg.Add("");
                return gamblingMsg.ToArray();
            } 
            string temp = mc[1].Groups[1].Value.ToString();
            gamblingMsg.Add(temp);
             temp = mc[1].Groups[2].Value.ToString();
            gamblingMsg.Add(temp);

            return gamblingMsg.ToArray();

        }


        public void InitList()
        {
            gameList1.Add(3, 1);
            gameList1.Add(8, 3);
            gameList1.Add(24, 8);
            gameList1.Add(72, 24);
            gameList1.Add(216, 72);
            gameList1.Add(648, 216);
            gameList1.Add(1944, 648);
            gameList1.Add(5832, 1944);


            gameList2.Add(8, 1);
            gameList2.Add(24, 3);
            gameList2.Add(72, 8);
            gameList2.Add(216, 24);
            gameList2.Add(648, 72);
            gameList2.Add(1944, 216);
            gameList2.Add(5832, 648);

            gameList3.Add(24, 1);
            gameList3.Add(72, 3);
            gameList3.Add(216, 8);
            gameList3.Add(648, 24);
            gameList3.Add(1944, 72);
            gameList3.Add(5832, 216);

            gameList4.Add(72, 1);
            gameList4.Add(216, 3);
            gameList4.Add(648, 8);
            gameList4.Add(1944, 24);
            gameList4.Add(5832, 72);
            gameList4.Add(17496, 216);

            gameList5.Add(216, 1);
            gameList5.Add(648, 3);
            gameList5.Add(1944, 8);
            gameList5.Add(5832, 24);
            gameList5.Add(17496, 72);

            gameList6.Add(648, 1);
            gameList6.Add(1944, 3);
            gameList6.Add(5832, 8);
            gameList6.Add(17496, 24);

            oldInfos.Add(empty);
            oldInfos.Add(empty);
            oldInfos.Add(empty);
            oldInfos.Add(empty);
        }

        public  void ClearOrders()
        {
            for (int i = 0; i < oldInfos.Count; i++)
            {
                oldInfos[i] = empty;
            }
        }
    }
}

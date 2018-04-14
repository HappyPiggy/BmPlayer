using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class Rules
    {
       public string GWJJInfoRules = "(.{3})期计划.([0-9]{5}).*等开【([0-9]{1,4}).{0,3}倍";
       public string OrderResultRules = "\"info\":\"(.*?)\",\"ins";
       public string LoginResultRules = "\"info\":\"(.*?)\",\"e";
       public string PlayerInfoRules = "账号:.*name\">(.*?)<[\\s\\S]*余额.*value\">(.*?)<";
       public string PeriodIDRules = "最新期数[\\s\\S]*\"fid.*(\\d{9,}).*fnum";
       public string DDInfoRules = "";
       public string HSZLInfoRules = "";
       public string BJSCInfoRules = "";
       public string DDInfoRules2 = "";
    }

    public class URL
    {
        public string PeriodURL = "https://111880022.com/OffcialOtherGame/Index/26#7.0.0";
        public string InfoURL = "https://111880022.com/?1521559386974";
        public string OrderURL = "https://111880022.com/OfficialAddOrders/AddOrders";
        public string LoginURL = "https://111880022.com/Home/login";
        public string ValidCodeURL = "https://111880022.com/Home/ValidateCode?1521481270436";
        public string PeriodURL2 = "";
    }


    //个位掘金
    public class GWJJ {
        public  int startBeishu = 1; //开始跟的倍数
        public  int maxBeishu = 0;  //最大倍数
        public  int m = 3;//钱的大小  1 2 3 4 对应 元 角 分 厘
        public  int flag = 1; //之前的模式合二为一
    
    }

    public class DD
    {
        public int startBeishu = 1; //开始跟的倍数
        public int maxBeishu = 0;  //最大倍数
        public int m = 3;//钱的大小  1 2 3 4 对应 元 角 分 厘
        public int flag = 1; //之前的模式合二为一

    }

    public class HSZL
    {
        public int startBeishu = 1; //开始跟的倍数
        public int maxBeishu = 0;  //最大倍数
        public int m = 3;//钱的大小  1 2 3 4 对应 元 角 分 厘
        public int flag = 1; //之前的模式合二为一
    }

    public class BJSC
    {
        public int startBeishu = 1; //开始跟的倍数
        public int maxBeishu = 0;  //最大倍数
        public int m = 3;//钱的大小  1 2 3 4 对应 元 角 分 厘
        public int flag = 1; //之前的模式合二为一
    }

    public class Title
    {
        public string GWJJ = "[个位掘金]\n";
        public string DD = "[独胆]\n";
        public string HSZL = "[后三组六]\n";
        public string BJSC = "[北京赛车]\n";
    }

    public class ERROR
    {
        public string ZZ_ERROR = "正则解析失败";
        public string REPEAT_ERROR = "订单建立失败，订单重复";
        public string TOOBIG_ERROR = "订单建立失败，倍数过大";
        public string LOGIN_INVALID = "登录失败，验证过期";
        public string UNKNOWN = "未知错误";
    }

   public class Settings
    {

       public  enum GameType
       {
           GWJJ,
           DD,
           HSZL,
           BJSC
       }

        public static string groupKeyword = "";  //群关键字

        public static List<bool>  listenerList=new List<bool>(); //游戏玩法 1是个位掘金 2是独胆

        public static GWJJ gwjj=new GWJJ(); //个位掘金的配置
        public static DD dd = new DD(); //独胆的配置
        public static HSZL hszl = new HSZL();//后三组六
        public static BJSC bjsc = new BJSC();//北京赛车


        public static Rules rules=new Rules(); //各种正则配置
        public static URL url = new URL();  //各种地址
        public static Title title = new Title();
        public static ERROR error = new ERROR(); 

        private static List<string> rulesList = new List<string>();
        private static List<string> urlList = new List<string>();

        public Settings() {

            listenerList.Add(true); listenerList.Add(true);
            listenerList.Add(true); listenerList.Add(true);
            UpdateConfig();
        }

       public static  void UpdateConfig()
        {
            string line = "";
            using (StreamReader sr = new StreamReader("./config/URL.config"))
            {
                urlList.Clear();
                while ((line = sr.ReadLine()) != null)
                {
                    urlList.Add(line.Split('|')[1]);
                }
            }

            using (StreamReader sr = new StreamReader("./config/Rules.config"))
            {
                rulesList.Clear();
                while ((line = sr.ReadLine()) != null)
                {
                    rulesList.Add(line.Split('|')[1]);
                }
            }

            url.PeriodURL = urlList[0];
            url.InfoURL = urlList[1];
            url.OrderURL = urlList[2];
            url.LoginURL = urlList[3];
            url.ValidCodeURL = urlList[4];
            url.PeriodURL2 = urlList[5];

            rules.GWJJInfoRules = rulesList[0];
            rules.OrderResultRules = rulesList[1];
            rules.LoginResultRules = rulesList[2];
            rules.PlayerInfoRules = rulesList[3];
            rules.PeriodIDRules = rulesList[4];
            rules.DDInfoRules = rulesList[5];
            rules.HSZLInfoRules = rulesList[6];
            rules.BJSCInfoRules = rulesList[7];
            rules.DDInfoRules2 = rulesList[8];

        }
        
    }
}

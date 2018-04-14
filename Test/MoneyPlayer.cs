using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Test
{
    public class MoneyPlayer
    {
        public string username="";
        public string curMoney="0";
        public float startMoney=0;
        Dictionary<string, string> parameters =new Dictionary<string, string>();

        public CookieCollection cookieCollection;
        public string cookie;
        public string infoCookie;


        private List<string> infoList=new List<string>();

        public float Profit
        {
            get
            {
                if (curMoney == "") return -1f;
                var tmp = float.Parse(curMoney);
                return tmp - startMoney;
            }
        }

        //显示验证码
        public void DownLoadValidCodePic()
        {
            cookieCollection = HttpRequest.GetLoginCookie(Settings.url.ValidCodeURL);

        }







        public string UpdatePlayerInfo()
        {
            //infoList.Clear();
            var rules = Settings.rules.PlayerInfoRules;
            string res = HttpRequest.Get(Settings.url.InfoURL, cookieCollection);
            if (res.Contains("失败") || res.Contains("重新登录")) return Settings.error.LOGIN_INVALID;
            Regex r = new Regex(rules, RegexOptions.None);
            Match mc = r.Match(res);
            username = mc.Groups[1].Value.Trim();
            curMoney = mc.Groups[2].Value.Trim();

            return "信息获取成功！";
        }

        public string LoginAccount(string validateCode, string user = "cancer123", string pwd = "321123qw")
        {
            //username=wdppx123&password=e0f2714f8bf61900dcf52d80d0123915&validateCode=6126
            var password = GetMD5((GetMD5(pwd) + validateCode).ToLower());
            //   var text="username="+user+"&password="+password+"&validateCode="+validateCode;
            IDictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "username", System.Web.HttpUtility.UrlEncode(user) },
                { "password", System.Web.HttpUtility.UrlEncode(password) },
                { "validateCode", System.Web.HttpUtility.UrlEncode(validateCode) }
            };
            System.Net.HttpWebResponse res = HttpRequest.Post(Settings.url.LoginURL, parameters, cookieCollection);
            if (res == null)
            {
                // debugText.Text = "未获取到post返回的消息";
                return Settings.error.UNKNOWN;
            }
            else
            {
                cookieCollection = res.Cookies; //登录后返回的cookie
                var info= HttpRequest.GetResponseString(res);
              //  if (res.Contains("失败")) return res;
                var rules = Settings.rules.LoginResultRules;
                Regex r = new Regex(rules, RegexOptions.None);
                Match mc = r.Match(info);
                var ret = mc.Groups[1].Value.Trim();
                return ret;
            }
        }

        private  string GetMD5(string input)
        {
            // Use input string to calculate MD5 hash
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
                // To force the hex string to lower-case letters instead of
                // upper-case, use he following line instead:
                // sb.Append(hashBytes[i].ToString("x2")); 
            }
            return sb.ToString();
        }



        //设置订单信息 传入订单和游戏类型
        public string SetOrders(string orderList,Settings.GameType type)
        {
            parameters.Clear();

            string gameID = "";
            var periodID = "";

            //游戏类型不同  期数获取地址和gameid不同
            switch (type)
            {
                case Settings.GameType.GWJJ:
                    gameID = "26";
                periodID = GetPeriodID(Settings.url.PeriodURL);
                    break;

                case Settings.GameType.DD:
                    gameID = "26";
                periodID = GetPeriodID(Settings.url.PeriodURL);
                    break;

                case Settings.GameType.HSZL:
                    gameID = "26";
                periodID = GetPeriodID(Settings.url.PeriodURL);
                    break;

                case Settings.GameType.BJSC:
                     gameID = "29";
                periodID = GetPeriodID(Settings.url.PeriodURL2);
                    break;
            }
                
               

            if (periodID.Contains("失败")) return periodID;

            parameters.Add("periodId", System.Web.HttpUtility.UrlEncode(periodID));
            parameters.Add("orderList", System.Web.HttpUtility.UrlEncode(orderList));
            parameters.Add("gameId", System.Web.HttpUtility.UrlEncode(gameID));
            parameters.Add("isSingle", "false");
            parameters.Add("canAdvance", "false");

            var res = HttpRequest.Post(Settings.url.OrderURL, parameters, cookieCollection);
            if (res == null )
            {
                return Settings.error.UNKNOWN;
            }
            else
            {
                var info = HttpRequest.GetResponseString(res);
              //  if (res.Contains("失败")) return res;
                var rules = Settings.rules.OrderResultRules;
                Regex r = new Regex(rules, RegexOptions.None);
                Match mc = r.Match(info);
                var ret = mc.Groups[1].Value.Trim();
                if (ret.Contains("成功")) UpdatePlayerInfo();  //下注成功才更新个人信息
                if (ret.Contains("超时")) return Settings.error.LOGIN_INVALID;
                return ret;
            }
        }


        private string GetPeriodID(string url)
        {
            
            var rules = Settings.rules.PeriodIDRules;
            string res ="";
            res = HttpRequest.Get(url, cookieCollection);

            if (res.Contains("重新登录")) return Settings.error.LOGIN_INVALID;
            Regex r = new Regex(rules, RegexOptions.None);
            Match mc = r.Match(res);
            return mc.Groups[1].Value.Trim();

        }

        //设定初始金额
        public void SetStartMoney()
        {
            if (curMoney == "") return;
            startMoney =float.Parse(curMoney) ;
        }


    
    }
}

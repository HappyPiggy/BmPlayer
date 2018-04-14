using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class LogSystem
    {
        private static string LogPath=@".\Log.txt";
        private static string gwjjCountPath = @".\count\gwjjCount.txt";
        private static string ddCountPath = @".\count\ddCount.txt";
        private static string hszlCountPath = @".\count\hszlCount.txt";
        private static string bjscCountPath = @".\count\bjscCount.txt";

        private static Dictionary<int, int> gwjjCount = new Dictionary<int, int>();
        private static Dictionary<int, int> ddCount = new Dictionary<int, int>();
        private static Dictionary<int, int> hszlCount = new Dictionary<int, int>();
        private static Dictionary<int, int> bjscCount = new Dictionary<int, int>();

        private static Dictionary<int, int> curList = new Dictionary<int, int>();

        public static void Record(string log){

            if (!System.IO.File.Exists(LogPath))
            {
                System.IO.File.Create(LogPath).Close();   
            }
            FileStream fs = new FileStream(LogPath, FileMode.Append, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine(DateTime.Now.ToString());
            sw.WriteLine(log);
            sw.WriteLine("");
            sw.Flush();
            sw.Close();
            sw.Dispose();
            fs.Close();
            fs.Dispose();
        }


        public static void CountRecord(Settings.GameType gt,int curBei)
        {
            curList.Clear();
            var path = "";
            //先读取
            switch (gt){
                case Settings.GameType.GWJJ:
                    curList = gwjjCount;
                    path = gwjjCountPath;
                    break;
                case Settings.GameType.DD:
                    curList = ddCount;
                    path = ddCountPath;
                    break;
                case Settings.GameType.HSZL:
                    curList = hszlCount;
                    path = hszlCountPath;
                    break;
                case Settings.GameType.BJSC:
                    curList = bjscCount;
                    path = bjscCountPath;
                    break;
            }

            LoadCount(path);

            int num = curList[curBei]+1;
            curList[curBei] = num;

            WriteCount(path);
          



        }

        private static void LoadCount(string path)
        {
            string line = "";
            
            using (StreamReader sr = new StreamReader(path))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    var key = int.Parse(line.Split(':')[0]);
                    var value = int.Parse(line.Split(':')[1]);
                    curList[key] = value;
                }
            }
        }

        private static void WriteCount(string path)
        {

            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);

            foreach (int key in curList.Keys)
            {
                var line = key + ":" + curList[key];
                sw.WriteLine(line);
            }
            
            sw.Flush();
            sw.Close();
            sw.Dispose();
            fs.Close();
            fs.Dispose();
        }

    }
}

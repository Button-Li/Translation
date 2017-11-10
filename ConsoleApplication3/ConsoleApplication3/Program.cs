using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;


namespace ConsoleApplication3
{
    using System;
    using System.Collections.Specialized;
    using System.IO;
    using System.Net;
    using System.Text;
    using System.Web;
    using Newtonsoft.Json;
    using System.Security.Cryptography;

    
    class Program
    {
        
        static void Main(string[] args)
        {
            string filename = "entities";
            string res=Read("C:\\Users\\llei\\Desktop\\农业项目目录\\"+filename+".json");
            //Console.WriteLine(res); 
            string tmp = res;
            int count = tmp.Length - tmp.Replace("name", "").Length;
            Console.WriteLine(count/4);
            tmp = res;
            int i = res.IndexOf("name");
            string[,] nameT = new string[count/4,2];
            int k = 0;
            for (; i < res.Length;)
            {
                int len = res.Length;
                int j = i + 8;
                string name = "";
                for (; res[j] != '"'; j++)
                {
                    name += res[j];
                }
                Console.Write(name+"--");
                string chName = getCHN(name);
                Console.WriteLine(chName);
                nameT[k,0] = name;
                nameT[k,1] = chName;
                k++;
                //tmp=tmp.Replace(name,chName);
                res = res.Substring(j, len-j);
                i = res.IndexOf("name");
                if (i == -1)
                {
                    /*
                    Console.WriteLine("---------------------");
                    for(int m = 0; m < k; m++)
                    {
                        Console.WriteLine(nameT[m,0]+"==="+nameT[m,1]);
                    }
                    */
                    break;
                }
            }
            //string[] names = res.Split("name");
            //res=Replace1(nameT,tmp,k);
            for(int j = 0; j < k; j++)
            {
                string former = nameT[j,0];
                string later = nameT[j, 1];
                int index = tmp.IndexOf(former);
                if (index != -1)
                {
                    tmp = tmp.Remove(index,former.Length);
                    tmp = tmp.Insert(index,later);
                }
            }
            Console.WriteLine("finished......");
            Write(tmp,filename);
            Console.ReadKey();
        }

        public static string Replace1(string[,] nameT,string tmp,int k)
        {
            for (int j = 0; j < k; j++)
            {
                if (nameT[j, 0] != null && nameT[j, 1] != null)
                {
                    string former = nameT[j, 0];
                    string later = nameT[j, 1];
                    if (j < k - 1 && nameT[j + 1, 0].Contains(nameT[j, 0]))
                    {
                        string tmpName = nameT[j, 0];
                        string tmpCHname = nameT[j, 1];
                        tmp = tmp.Replace(former, later);
                        j++;
                        while (nameT[j, 0].Contains(tmpName))
                        {
                            former = nameT[j, 0].Substring(0, nameT[j, 0].Length - tmpName.Length);
                            later = nameT[j, 1].Substring(0, nameT[j, 1].Length - tmpCHname.Length);
                            Console.WriteLine(former + "===" + later);
                            tmp = tmp.Replace(former, later);
                            j++;
                        }
                        j--;

                    }
                    Console.WriteLine(nameT[j, 0] + "===" + nameT[j, 1]);
                    tmp = tmp.Replace(former, later);
                }

            }
            return tmp;
        }

        public static string getCHN(string enName)
        {
            string q = enName;
            string appKey = "7c92908b95920a38";
            string to = "zh-CHS";
            string from = "en";
            string salt = DateTime.Now.Millisecond.ToString();
            string appSecret = "21pa10VLTXtJi9pwfqhbCMDLR80mBcTR";
            MD5 md5 = new MD5CryptoServiceProvider();
            string md5Str = appKey + q + salt + appSecret;
            byte[] output = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(md5Str));
            string sign = BitConverter.ToString(output).Replace("-", "");

            string url = string.Format("http://openapi.youdao.com/api?appKey={0}&q={1}&from={2}&to={3}&sign={4}&salt={5}", appKey, System.Web.HttpUtility.UrlDecode(q, System.Text.Encoding.GetEncoding("UTF-8")), from, to, sign, salt);
            WebRequest translationWebRequest = WebRequest.Create(url);

            WebResponse response = null;

            response = translationWebRequest.GetResponse();
            Stream stream = response.GetResponseStream();

            Encoding encode = Encoding.GetEncoding("utf-8");

            StreamReader reader = new StreamReader(stream, encode);
            string result = reader.ReadToEnd();
            //Console.WriteLine(result);
            int start = result.IndexOf("translation");
            int end = result.IndexOf("errorCode");
            string str = result.Substring(start + 15, end - start - 19);
            return str;
        }
       
        public static string Read(string path)
        {
            StreamReader sr = new StreamReader(path, Encoding.Default);
            String line;
            String res = "";
            while ((line = sr.ReadLine()) != null)
            {
                res =res + line +"\n";                
            }
            return res;
        }

        public static void Write(string res,string filename)
        {
            FileStream fs = new FileStream("C:\\Users\\llei\\Desktop\\农业项目目录\\"+filename+"-cn.json", FileMode.Create);
            //获得字节数组
            byte[] data = System.Text.Encoding.Default.GetBytes(res);
            //开始写入
            fs.Write(data, 0, data.Length);
            //清空缓冲区、关闭流
            fs.Flush();
            fs.Close();
        }
    }
}

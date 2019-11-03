using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;

namespace DWSerializecs
{
    class Program
    {
        private const string dwIndexString = "あいうえおかきくけこさしすせそたちつてとなにぬねのはひふへほまみむめもやゆよらりるれろわをんかぎぐげござじずぜぞだぢづでどばびぶべぼぱぴぷぺぽアイウエオカキクケコサシスセソタチツテトナニヌネノハヒフヘホマミムメモヤユヨラリルレロワヲンカギグゲゴザジズゼゾダヂヅデドバビブベボパピプペポ";
        private const string ScramblePattern = "None";
        static void Main(string[] args)
        {
            string basetext = "";
            basetext = "河野太郎";

            string result;
            
            result = TextEncode_dw(basetext);
            Console.WriteLine(result);
            result = TextDecode_dw(result);
            Console.WriteLine(result);
        }

        private static string TextDecode_dw(string text)
        {
            /*
             * 手順
             * 1.テキストを読み込む
             * 2.テキストを半角スペースで区切って読み込み、対応する数値と区切り文字へ変換
             * 3.数字をバイト列に変換して読み込む（区切り文字は破棄する）
             * 4.バイト列に読み込んだら、0データを取り除いてUTF-8へ変換
             */

            //スペース切り詰めて2文字単位で抽出して変換・復元する


            string[] decodebytes;
            // splitで区切らないで2文字ずつ抽出していくとどうか
            decodebytes = MySplit(text.Replace(" ",""),2);


            byte[] inbytes = new byte[decodebytes.Length];

            string decodestring = "";

            for (int i = 0; i < decodebytes.Length; i++)
            {
                //Console.Write(GetStringFromBitString_dwp2(decodebytes[i]) + " ");
                decodestring += GetStringFromBitString_dwp2(decodebytes[i]) + " ";
            }

            string result = "";
            byte[] blist = new byte[] { };
            int c = 0;
            // decodestringからバイト列に戻して、UTF-8のバイト列にする
            foreach (var s in decodestring.Trim().Split(' '))
            {
                if (s == "") continue;
                int num = int.Parse(s);
                byte[] b = BitConverter.GetBytes(num);

                //blist = blist.Concat(b);
                Array.Resize(ref blist, blist.Length + b.Length);
                b.CopyTo(blist, blist.Length - b.Length);


            }

            //バイト列から0を抜き取る
            blist = blist.Where(s => s != 0).ToArray();

            //もし追加で復号化等の処理をする場合、ここで blist に対して行う
            //byte[]を渡してbyte[] で返る
            blist = ExecuteMethod(ScramblePattern + "_Decode", blist);

            result += System.Text.Encoding.UTF8.GetString(blist);

            return result;
          


        }

        private static string  TextEncode_dw(string inText)
        {

            /*
             * 手順
             * 1.テキストを読み込む
             * 2.UTF-8のバイト列にに変換
             * 3.バイト列から対応する数字に変換、区切り文字を付加
             * 4.数字を対応するビットに変換。区切り文字も変換。
             */

            //スペース切り詰めて2文字単位で抽出して変換・復元する


            List<byte[]> lsttext = new List<byte[]>();
            string encodetext = "";
            string encodetext_dw = "";

            byte[] inBytes = System.Text.Encoding.UTF8.GetBytes(inText);

            //もし追加で暗号化等の処理をする場合、ここで　inBytes に対して行う
            //byte[]を渡してbyte[] で返る

            inBytes = ExecuteMethod(ScramblePattern + "_Encode", inBytes);
            
            //output(debug)
            for (int i = 0; i < inBytes.Length; i++)
            {
                //Console.Write(inBytes[i].ToString() + ".");
                encodetext += inBytes[i].ToString() + ".";
            }


            //Dragon Warrior Pattern

            for (int i = 0; i < inBytes.Length; i++)
            {
                //Console.Write(GetBitStringFromString_dwp2(inBytes[i]) );
                encodetext_dw += GetBitStringFromString_dwp2(inBytes[i]);
            }

            return encodetext_dw;
            //generatebitdata = sbitarray;
        }

        private static string GetBitStringFromString(string v)
        {
            switch (v)
            {
                case "0":
                    return "0000";
                case "1":
                    return "0001";
                case "2":
                    return "0010";
                case "3":
                    return "0011";
                case "4":
                    return "0100";
                case "5":
                    return "0101";
                case "6":
                    return "0110";
                case "7":
                    return "0111";
                case "8":
                    return "1000";
                case "9":
                    return "1001";
                case ".":
                    return "1111";
                default:
                    return "";
            }

        }

        private static string GetBitStringFromString_dw(byte v)
        {
            int index = v;
            int count = 0;
            string s = "";

            s += dwIndexString.Substring(v, 1);

            return s;
        }

        private static string GetBitStringFromString_dwp2(byte v)
        {
            int index = v;
            int count = 1;
            string s = "";
            string bs = v.ToString();

            foreach (var d in MySplit(bs, 2))
            {
                //Console.WriteLine($"{bs},{d}");
                s += dwIndexString.Substring(int.Parse(d), 1);

            }

            return s;
        }

        private static string GetStringFromBitString_dwp2(string v)
        {
            //int index = v;
            int count = 1;
            string s = "";
            string bs = v.ToString();

            foreach (var d in MySplit(v, 1))
            {
                s = s + dwIndexString.IndexOf(d, 0, StringComparison.Ordinal);
            }

            return s;
        }




        //from:https://www.sejuku.net/blog/44242#i-7
        // 拡張メソッド
        private static string[] MySplit(string str, int count)
        {
            var list = new List<string>();
            int length = (int)Math.Ceiling((double)str.Length / count);

            for (int i = 0; i < length; i++)
            {
                int start = count * i;
                if (str.Length <= start)
                {
                    break;
                }
                if (str.Length < start + count)
                {
                    list.Add(str.Substring(start));
                }
                else
                {
                    list.Add(str.Substring(start, count));
                }
            }

            return list.ToArray();
        }

        private static byte[] ExecuteMethod(string v, byte[] inBytes)
        {
            Scramble.cScramble sc = new Scramble.cScramble();
            Type t = sc.GetType();

            MethodInfo mi = t.GetMethod(v);

            object o = mi.Invoke(sc, new object[] { inBytes });

            byte[] ret = (byte[])o;

            return ret;
        }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Text2BinConv
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void tsbEncode_Click(object sender, EventArgs e)
        {
            TextEncode(txtIn.Text);
        }

        private void tsbDecode_Click(object sender, EventArgs e)
        {
            TextDecode(txtIn.Text);
        }

        private void TextDecode(string text)
        {
           /*
            * 手順
            * 1.テキストを読み込む
            * 2.ビット列を4文字ずつ区切って読み込み、対応する数値と区切り文字へ変換
            * 3.数字をバイト列に変換して読み込む（区切り文字は破棄する）
            * 4.バイト列に読み込んだら、0データを取り除いてUTF-8へ変換
            */

            //ビット列を読み込む
            //4ビット分ずつ

            txtOut.Clear();

            string[] decodebytes;
            decodebytes = StringExtensions.SubstringAtCount(text, 4);


            byte[] inbytes = new byte[decodebytes.Length];

            string decodestring = "";

            for (int i = 0; i < decodebytes.Length; i++)
            {
                txtOut.AppendText(GetStringFromBitString(decodebytes[i]));
                decodestring += GetStringFromBitString(decodebytes[i]);
            }

            //txtOut.AppendText("\n");
            

            string result = "";
            byte[] blist= new byte[]{};
            int c = 0;
            // バイト列になったので、これを戻す
            foreach (var s in decodestring.Trim().Split('.'))
            {
                if (s == "") continue;
                int num = int.Parse(s);
                byte[] b = BitConverter.GetBytes(num);

                //blist = blist.Concat(b);
                Array.Resize(ref blist,blist.Length + b.Length);
                b.CopyTo(blist,blist.Length-b.Length);


            }
            //バイト列から0を抜き取る
            blist = blist.Where(s => s != 0).ToArray();

                result += System.Text.Encoding.UTF8.GetString(blist);
            
            txtOut.Text = result;


        }

        private void TextEncode(string inText)
        {
            txtOut.Clear();

            /*
             * 手順
             * 1.テキストを読み込む
             * 2.UTF-8のバイト列にに変換
             * 3.バイト列から対応する数字に変換、区切り文字を付加
             * 4.数字を対応するビットに変換。区切り文字も変換。
             */

            List<byte[]> lsttext = new List<byte[]>();
            string encodetext = "";

            byte[] inBytes = System.Text.Encoding.UTF8.GetBytes(inText);

            //output(debug)
            for (int i = 0; i < inBytes.Length; i++)
            {
                txtOut.AppendText(inBytes[i].ToString() + ".");
                encodetext += inBytes[i].ToString() + ".";
            }
            /*
            // バイト列から戻してみる
            string dbgtxt = System.Text.Encoding.UTF8.GetString(inBytes);
            txtOut.AppendText(dbgtxt);
            */

            //バイト列になったので、これを仮想ビット列に変換

            string[] sBytes = new string[encodetext.Length*4];

            for (int i = 0; i < encodetext.Length; i++)
            {
                sBytes[i] = encodetext.Substring(i, 1);
            }

            //debug
            //for (int i = 0; i < sBytes.Length; i++)
            //{
            //    txtOut.AppendText(sBytes[i]);
            //}

            // 読み込んだらtxtOutを一旦クリア
            txtOut.Clear();

            string sbitarray = "";
            for (int i = 0; i < sBytes.Length; i++)
            {
                txtOut.AppendText(GetBitStringFromString(sBytes[i]));
                sbitarray += GetBitStringFromString(sBytes[i]);
        }
        }

        private string GetBitStringFromString(string v)
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

        private string GetStringFromBitString(string v)
        {
            switch (v)
            {
                case "0000":
                    return "0";
                case "0001":
                    return "1";
                case "0010":
                    return "2";
                case "0011":
                    return "3";
                case "0100":
                    return "4";
                case "0101":
                    return "5";
                case "0110":
                    return "6";
                case "0111":
                    return "7";
                case "1000":
                    return "8";
                case "1001":
                    return "9";
                case "1111":
                    return ".";
                default:
                    return "";
            }
        }
    }
    public static class StringExtensions
    {
        public static string[] SubstringAtCount(this string self, int count)
        {
            var result = new List<string>();
            var length = (int)Math.Ceiling((double)self.Length / count);

            for (int i = 0; i < length; i++)
            {
                int start = count * i;
                if (self.Length <= start)
                {
                    break;
                }
                if (self.Length < start + count)
                {
                    result.Add(self.Substring(start));
                }
                else
                {
                    result.Add(self.Substring(start, count));
                }
            }

            return result.ToArray();
        }
    }
}

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
            //ビット列を読み込む
            //4ビット分ずつ

            txtOut.Clear();

            string[] decodebytes;
            decodebytes = StringExtensions.SubstringAtCount(text, 4);


            byte[] inbytes = new byte[decodebytes.Length];

            for (int i = 0; i < decodebytes.Length; i++)
            {
                txtOut.AppendText(GetStringFromBitString(decodebytes[i]));
            }



            //// バイト列になったので、これを戻す
            //string dbgtxt = System.Text.Encoding.UTF8.GetString(inBytes);
            //txtOut.AppendText(dbgtxt);





        }

        private void TextEncode(string inText)
        {
            txtOut.Clear();

            /*
             * 手順
             * 1.テキストを読み込む
             * 2.UTF-8のバイナリに変換
             * 3.そのバイナリをバイト列に変換
             * 4.バイト列をビットに変換(8文字ずつ区切る)
             */



            byte[] inBytes = System.Text.Encoding.UTF8.GetBytes(inText);

            //output(debug)
            for (int i = 0; i < inBytes.Length; i++)
            {
                txtOut.AppendText(inBytes[i].ToString());
            }
            /*
            // バイト列から戻してみる
            string dbgtxt = System.Text.Encoding.UTF8.GetString(inBytes);
            txtOut.AppendText(dbgtxt);
            */

            //バイト列になったので、これをビット列に変換

            string bytedata = txtOut.Text;

            string[] sBytes = new string[bytedata.Length];

            for (int i = 0; i < bytedata.Length; i++)
            {
                sBytes[i] = bytedata.Substring(i, 1);
            }

            //debug
            for (int i = 0; i < sBytes.Length; i++)
            {
                txtOut.AppendText(sBytes[i]);
            }



            // 読み込んだらtxtOutを一旦クリア
            txtOut.AppendText("\n");

            for (int i = 0; i < inBytes.Length; i++)
            {
                txtOut.AppendText(GetBitStringFromString(sBytes[i]));
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

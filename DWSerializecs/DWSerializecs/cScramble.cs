using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Scramble
{
    class cScramble
    {
        // 3DES Cryot Key (16bit or 24bit)
        const string TripleDES_Crypto_KEY = "u49#kol2l)03l*32";

        // 3DES Initial Vector(8bit)
        const string TripleDES_Vector = "i3'94+12";

        public byte[] None_Encode(byte[] inBytes)
        {
            return inBytes;
        }

        public byte[] None_Decode(byte[] inBytes)
        {
            return inBytes;
        }

        public byte[] LeftBitShift2_Encode(byte[] inBytes)
        {
            throw new NotImplementedException();

        }

        public byte[] LeftBitShift2_Decode(byte[] inBytes)
        {
            throw new NotImplementedException();
        }

        public byte[] RightBitShift2_Encode(byte[] inBytes)
        {
            throw new NotImplementedException();

        }

        public byte[] RightBitShift2_Decode(byte[] inBytes)
        {
            throw new NotImplementedException();
        }

        public byte[] TripleDES_Encode(byte[] inBytes)
        {

            byte[] desKey = Encoding.UTF8.GetBytes(TripleDES_Crypto_KEY);
            byte[] desIV = Encoding.UTF8.GetBytes(TripleDES_Vector);

            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms,des.CreateEncryptor(desKey,desIV),CryptoStreamMode.Write);

            cs.Write(inBytes,0,inBytes.Length);
            cs.Close();

            byte[] cryptodata = ms.ToArray();
            ms.Close();

            return cryptodata;
        }

        public byte[] TripleDES_Decode(byte[] inBytes)
        {
            byte[] desKey = Encoding.UTF8.GetBytes(TripleDES_Crypto_KEY);
            byte[] desIV = Encoding.UTF8.GetBytes(TripleDES_Vector);

            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(desKey, desIV), CryptoStreamMode.Write);

            cs.Write(inBytes, 0, inBytes.Length);
            cs.Close();

            byte[] decryptdata = ms.ToArray();
            ms.Close();

            return decryptdata;
        }


    }
}

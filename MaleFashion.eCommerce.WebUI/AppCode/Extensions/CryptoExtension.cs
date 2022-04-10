using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MaleFashion.eCommerce.WebUI.AppCode.Extensions
{
    public static partial class Extension
    {
        public const string secretKey = "!Development@P313~";

        public static string ToMD5Static(this string value)
        {
            using (MD5 provider = MD5.Create())
            {
                byte[] buffer = Encoding.UTF8.GetBytes(value);

                buffer = provider.ComputeHash(buffer);

                StringBuilder sb = new StringBuilder();

                foreach (byte item in buffer)
                {
                    sb.Append(item.ToString("x2"));
                }

                return sb.ToString();

                //return string.Join("", buffer.Select(b => b.ToString("x2")));
            }
        }

        public static string ToMD5(this string value)
        {
            using (MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider())
            {
                byte[] buffer = Encoding.UTF8.GetBytes(value);

                buffer = provider.ComputeHash(buffer);

                //StringBuilder sb = new StringBuilder();

                //foreach (byte item in buffer)
                //{
                //    sb.Append(item.ToString("x2"));
                //}

                //return sb.ToString();

                return string.Join("", buffer.Select(b => b.ToString("x2")));
            }
        }

        public static string Encrypt(this string value, string key)
        {
            using (TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider())
            using (MD5CryptoServiceProvider md5Provider = new MD5CryptoServiceProvider())
            {
                byte[] keyValue = Encoding.UTF8.GetBytes($"!{key}@");
                byte[] ivValue = Encoding.UTF8.GetBytes($"?{key}#");

                keyValue = md5Provider.ComputeHash(keyValue);
                ivValue = md5Provider.ComputeHash(ivValue);

                ICryptoTransform transform = provider.CreateEncryptor(keyValue, ivValue);

                using (MemoryStream ms = new MemoryStream())
                using (CryptoStream cs = new CryptoStream(ms, transform, CryptoStreamMode.Write))
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(value);

                    cs.Write(buffer, 0, buffer.Length);
                    cs.FlushFinalBlock();

                    ms.Position = 0;
                    byte[] result = new byte[ms.Length];
                    ms.Read(result, 0, result.Length);

                    return Convert.ToBase64String(result);
                }
            }
        }

        public static string Encrypt(this string value)
        {
            return Encrypt(value, secretKey.ToMD5());
        }

        public static string Decrypt(this string value, string key)
        {
            using (TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider())
            using (MD5CryptoServiceProvider md5Provider = new MD5CryptoServiceProvider())
            {
                byte[] keyValue = Encoding.UTF8.GetBytes($"!{key}@");
                byte[] ivValue = Encoding.UTF8.GetBytes($"?{key}#");

                keyValue = md5Provider.ComputeHash(keyValue);
                ivValue = md5Provider.ComputeHash(ivValue);

                ICryptoTransform transform = provider.CreateEncryptor(keyValue, ivValue);

                using (MemoryStream ms = new MemoryStream())
                using (CryptoStream cs = new CryptoStream(ms, transform, CryptoStreamMode.Write))
                {
                    byte[] buffer = Convert.FromBase64String(value);

                    cs.Write(buffer, 0, buffer.Length);
                    cs.FlushFinalBlock();

                    ms.Position = 0;
                    byte[] result = new byte[ms.Length];
                    ms.Read(result, 0, result.Length);

                    return Encoding.UTF8.GetString(result);
                }
            }
        }

        public static string Decrypt(this string value)
        {
            return Decrypt(value, secretKey.ToMD5());
        }
    }
}

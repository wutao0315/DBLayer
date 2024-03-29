﻿using System.Security.Cryptography;
using System.Text;

namespace DBLayer.Core.Utilities;

/// <summary> 
/// 加密
/// </summary> 
public class AES
{
    //默认密钥向量
    private static readonly byte[] keys = { 0x41, 0x72, 0x65, 0x79, 0x6F, 0x75, 0x6D, 0x79, 0x53, 0x6E, 0x6F, 0x77, 0x6D, 0x61, 0x6E, 0x3F };
    private static readonly int keyLength = 32;
    public static string Encode(string encryptString, string encryptKey)
    {
        if (encryptKey.Length > keyLength)
        {
            encryptKey = encryptKey.Substring(0, keyLength);
        }
        encryptKey = encryptKey.PadRight(keyLength, ' ');

        RijndaelManaged rijndaelProvider = new RijndaelManaged();
        rijndaelProvider.Key = Encoding.UTF8.GetBytes(encryptKey.Substring(0, keyLength));
        rijndaelProvider.IV = keys;
        ICryptoTransform rijndaelEncrypt = rijndaelProvider.CreateEncryptor();

        byte[] inputData = Encoding.UTF8.GetBytes(encryptString);
        byte[] encryptedData = rijndaelEncrypt.TransformFinalBlock(inputData, 0, inputData.Length);

        return Convert.ToBase64String(encryptedData);
    }

    public static string Decode(string decryptString, string decryptKey)
    {
        try
        {
            if (decryptKey.Length > keyLength)
            {
                decryptKey = decryptKey.Substring(0, keyLength);
            }
            decryptKey = decryptKey.PadRight(keyLength, ' ');

            RijndaelManaged rijndaelProvider = new RijndaelManaged();
            rijndaelProvider.Key = Encoding.UTF8.GetBytes(decryptKey);
            rijndaelProvider.IV = keys;
            ICryptoTransform rijndaelDecrypt = rijndaelProvider.CreateDecryptor();

            byte[] inputData = Convert.FromBase64String(decryptString);
            byte[] decryptedData = rijndaelDecrypt.TransformFinalBlock(inputData, 0, inputData.Length);

            return Encoding.UTF8.GetString(decryptedData);
        }
        catch
        {
            return "";
        }

    }

}

/// <summary> 
/// 加密
/// </summary> 
public class DES
{
    //默认密钥向量
    private static readonly byte[] keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

    private static readonly int keyLength = 8;
    /// <summary>
    /// DES加密字符串
    /// </summary>
    /// <param name="encryptString">待加密的字符串</param>
    /// <param name="encryptKey">加密密钥,要求为8位</param>
    /// <returns>加密成功返回加密后的字符串,失败返回源串</returns>
    public static string Encode(string encryptString, string encryptKey)
    {
        if (encryptKey.Length > keyLength)
        {
            encryptKey = encryptKey.Substring(0, keyLength);
        }
        encryptKey = encryptKey.PadRight(keyLength, ' ');
        byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, keyLength));
        byte[] rgbIV = keys;
        byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
        DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
        MemoryStream mStream = new MemoryStream();
        CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
        cStream.Write(inputByteArray, 0, inputByteArray.Length);
        cStream.FlushFinalBlock();
        return Convert.ToBase64String(mStream.ToArray());

    }

    /// <summary>
    /// DES解密字符串
    /// </summary>
    /// <param name="decryptString">待解密的字符串</param>
    /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
    /// <returns>解密成功返回解密后的字符串,失败返源串</returns>
    public static string Decode(string decryptString, string decryptKey)
    {
        try
        {
            if (decryptKey.Length > keyLength)
            {
                decryptKey = decryptKey.Substring(0, keyLength);
            }
            decryptKey = decryptKey.PadRight(keyLength, ' ');
            byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
            byte[] rgbIV = keys;
            byte[] inputByteArray = Convert.FromBase64String(decryptString);
            DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();

            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Encoding.UTF8.GetString(mStream.ToArray());
        }
        catch
        {
            return "";
        }
    }
} 

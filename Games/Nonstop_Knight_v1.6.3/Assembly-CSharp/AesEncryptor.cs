using System;
using System.Security.Cryptography;
using System.Text;

public static class AesEncryptor
{
    private static readonly byte[] Iv = new byte[] { 0x21, 0xda, 0xfb, 0, 0xad, 0xad, 0x7a, 0xee, 0xc5, 0xfe, 0x17, 0xaf, 0x4d, 3, 0x23, 0x2c };
    private const int IvLength = 0x10;
    private static readonly byte[] Key = new byte[] { 0x16, 0xac, 0x1f, 4, 3, 0xed, 0x2a, 0xfe, 0xc5, 0xfe, 7, 0xab, 0x4d, 5, 0x12, 60 };
    private static RijndaelManaged Rijndael = new RijndaelManaged();
    private static ICryptoTransform StaticDecryptor;
    private static UTF8Encoding StaticEncoder;
    private static ICryptoTransform StaticEncryptor;

    static AesEncryptor()
    {
        Rijndael.Key = Key;
        Rijndael.BlockSize = 0x80;
        StaticEncryptor = Rijndael.CreateEncryptor(Key, Iv);
        StaticDecryptor = Rijndael.CreateDecryptor(Key, Iv);
        StaticEncoder = new UTF8Encoding();
    }

    public static byte[] Decrypt(byte[] buffer)
    {
        if (StaticDecryptor.CanReuseTransform)
        {
            return StaticDecryptor.TransformFinalBlock(buffer, 0, buffer.Length);
        }
        using (StaticDecryptor = Rijndael.CreateDecryptor(Key, Iv))
        {
            return StaticDecryptor.TransformFinalBlock(buffer, 0, buffer.Length);
        }
    }

    public static string Decrypt(string encrypted)
    {
        return StaticEncoder.GetString(Decrypt(Convert.FromBase64String(encrypted)));
    }

    public static byte[] Encrypt(byte[] buffer)
    {
        if (StaticEncryptor.CanReuseTransform)
        {
            return StaticEncryptor.TransformFinalBlock(buffer, 0, buffer.Length);
        }
        using (StaticEncryptor = Rijndael.CreateEncryptor(Key, Iv))
        {
            return StaticEncryptor.TransformFinalBlock(buffer, 0, buffer.Length);
        }
    }

    public static string Encrypt(string unencrypted)
    {
        return Convert.ToBase64String(Encrypt(StaticEncoder.GetBytes(unencrypted)));
    }
}


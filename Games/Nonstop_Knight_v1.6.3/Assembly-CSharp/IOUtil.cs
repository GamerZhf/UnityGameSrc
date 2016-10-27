using App;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public static class IOUtil
{
    public static bool AppendTextToPersistentStorage(string text, string filename)
    {
        if (ConfigApp.PersistentStorageEncryptionEnabled)
        {
            Debug.LogError("Encryption not implemented for this function.");
            return false;
        }
        try
        {
            string fileName = Path.Combine(Application.persistentDataPath, filename);
            Directory.CreateDirectory(new FileInfo(fileName).Directory.FullName);
            File.AppendAllText(fileName, text);
        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }

    public static void CopyFileInEditorDataPath(string sourceFilename, string targetFilename, bool overwrite)
    {
        string sourceFileName = Path.Combine(Application.dataPath, sourceFilename);
        string destFileName = Path.Combine(Application.dataPath, targetFilename);
        File.Copy(sourceFileName, destFileName, overwrite);
    }

    public static bool DeleteFileFromPersistentStorage(string filename)
    {
        try
        {
            File.Delete(Path.Combine(Application.persistentDataPath, filename));
        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }

    public static T DeserializeJsonObjectFromEditorDataPath<T>(string filename)
    {
        return JsonUtils.Deserialize<T>(LoadFromEditorDataPath(filename, false), true);
    }

    public static T DeserializeJsonObjectFromPersistentStorage<T>(string filename)
    {
        return JsonUtils.Deserialize<T>(LoadFromPersistentStorage(filename), true);
    }

    public static bool FileExistsInEditorDataPath(string filename)
    {
        return File.Exists(Path.Combine(Application.dataPath, filename));
    }

    public static bool FileExistsInPersistentStorage(string filename)
    {
        return File.Exists(Path.Combine(Application.persistentDataPath, filename));
    }

    public static List<string> GetFilesInDirectoryFromPersistentStorage(string directory)
    {
        List<string> list = new List<string>();
        try
        {
            foreach (string str2 in Directory.GetFiles(Path.Combine(Application.persistentDataPath, directory)))
            {
                list.Add(Path.GetFileName(str2));
            }
        }
        catch (Exception)
        {
            return null;
        }
        return list;
    }

    public static byte[] LoadBytesFromPersistentStorage(string filename)
    {
        try
        {
            string path = Path.Combine(Application.persistentDataPath, filename);
            byte[] buffer = null;
            if (ConfigApp.PersistentStorageEncryptionEnabled)
            {
                buffer = AesEncryptor.Decrypt(File.ReadAllBytes(path));
            }
            else
            {
                buffer = File.ReadAllBytes(path);
            }
            return buffer;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public static string LoadFromEditorDataPath(string filename, bool encrypted)
    {
        try
        {
            string path = Path.Combine(Application.dataPath, filename);
            byte[] bytes = null;
            if (encrypted)
            {
                bytes = AesEncryptor.Decrypt(File.ReadAllBytes(path));
            }
            else
            {
                bytes = File.ReadAllBytes(path);
            }
            return Encoding.UTF8.GetString(bytes);
        }
        catch (Exception exception)
        {
            Debug.LogError(string.Concat(new object[] { "Cannot load '", filename, "' from editor data path: ", exception }));
        }
        return null;
    }

    public static string LoadFromPersistentStorage(string filename)
    {
        byte[] bytes = LoadBytesFromPersistentStorage(filename);
        return ((bytes == null) ? null : Encoding.UTF8.GetString(bytes));
    }

    public static Texture2D LoadRawTextureFromPersistentStorage(string filename)
    {
        RawTextureMetaData data = DeserializeJsonObjectFromPersistentStorage<RawTextureMetaData>(filename + ".meta");
        if (data == null)
        {
            return null;
        }
        byte[] buffer = LoadBytesFromPersistentStorage(filename);
        if (buffer == null)
        {
            return null;
        }
        Texture2D textured = data.getTexture2D();
        textured.LoadRawTextureData(buffer);
        textured.Apply();
        return textured;
    }

    public static bool RenameFileInPersistentStorage(string filename, string newFilename)
    {
        try
        {
            string sourceFileName = Path.Combine(Application.persistentDataPath, filename);
            string destFileName = Path.Combine(Application.persistentDataPath, newFilename);
            File.Move(sourceFileName, destFileName);
        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }

    public static bool SaveRawTextureToPersistentStorage(Texture2D texture, string filename, bool overwrite)
    {
        if (!SerializeJsonObjectToPersistentStorage<RawTextureMetaData>(new RawTextureMetaData(texture), filename + ".meta"))
        {
            return false;
        }
        if (!SaveToPersistentStorage(texture.GetRawTextureData(), filename, ConfigApp.PersistentStorageEncryptionEnabled, overwrite))
        {
            return false;
        }
        return true;
    }

    public static bool SaveToEditorDataPath(string content, string filename, bool encrypt, bool overwrite)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(content);
        try
        {
            string path = Path.Combine(Application.dataPath, filename);
            if (!overwrite && File.Exists(path))
            {
                return false;
            }
            FileInfo info = new FileInfo(path);
            if (info.Directory == null)
            {
                throw new Exception();
            }
            Directory.CreateDirectory(info.Directory.FullName);
            if (encrypt)
            {
                File.WriteAllBytes(path, AesEncryptor.Encrypt(bytes));
            }
            else
            {
                File.WriteAllBytes(path, bytes);
            }
        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }

    public static bool SaveToPersistentStorage(byte[] bytes, string filename, bool encrypt, bool overwrite)
    {
        try
        {
            string path = Path.Combine(Application.persistentDataPath, filename);
            if (!overwrite && File.Exists(path))
            {
                return false;
            }
            FileInfo info = new FileInfo(path);
            if (info.Directory == null)
            {
                throw new Exception();
            }
            Directory.CreateDirectory(info.Directory.FullName);
            if (encrypt)
            {
                File.WriteAllBytes(path, AesEncryptor.Encrypt(bytes));
            }
            else
            {
                File.WriteAllBytes(path, bytes);
            }
        }
        catch (Exception)
        {
            return false;
        }
        return true;
    }

    public static bool SaveToPersistentStorage(string content, string filename, bool encrypt, bool overwrite)
    {
        return SaveToPersistentStorage(Encoding.UTF8.GetBytes(content), filename, encrypt, overwrite);
    }

    public static bool SerializeJsonObjectToPersistentStorage<T>(T dataObject, string filename)
    {
        bool flag = SaveToPersistentStorage(JsonUtils.Serialize(dataObject), filename, ConfigApp.PersistentStorageEncryptionEnabled, true);
        if (!flag)
        {
            Debug.LogError("Cannot serialize data object '" + filename + "' into persistent storage.");
        }
        return flag;
    }

    private class RawTextureMetaData
    {
        public int Height;
        public bool Mipmap;
        public UnityEngine.TextureFormat TextureFormat;
        public int Width;

        public RawTextureMetaData()
        {
        }

        public RawTextureMetaData(Texture2D texture)
        {
            this.Width = texture.width;
            this.Height = texture.height;
            this.TextureFormat = texture.format;
            this.Mipmap = texture.mipmapCount > 1;
        }

        public Texture2D getTexture2D()
        {
            return new Texture2D(this.Width, this.Height, this.TextureFormat, this.Mipmap);
        }
    }
}


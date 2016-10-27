using System;
using System.IO;

[AttributeUsage(AttributeTargets.Class, AllowMultiple=false)]
public class IntegrationTestAttribute : Attribute
{
    private readonly string m_Path;

    public IntegrationTestAttribute(string path)
    {
        if (path.EndsWith(".unity"))
        {
            path = path.Substring(0, path.Length - ".unity".Length);
        }
        this.m_Path = path;
    }

    public bool IncludeOnScene(string scenePath)
    {
        return ((scenePath == this.m_Path) || (Path.GetFileNameWithoutExtension(scenePath) == this.m_Path));
    }
}


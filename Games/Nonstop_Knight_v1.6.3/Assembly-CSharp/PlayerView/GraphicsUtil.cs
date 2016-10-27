namespace PlayerView
{
    using System;
    using System.IO;
    using UnityEngine;

    public static class GraphicsUtil
    {
        public static Material ClearAlphaMaterial;

        public static RenderTexture RenderCameraToRenderTexture(Camera cam)
        {
            RenderTexture texture = new RenderTexture(cam.pixelWidth, cam.pixelHeight, 0x18);
            cam.targetTexture = texture;
            cam.Render();
            cam.targetTexture = null;
            return texture;
        }

        public static Texture2D RenderCameraToTexture2d(Camera cam)
        {
            RenderTexture active = RenderTexture.active;
            RenderTexture source = RenderTexture.GetTemporary(cam.pixelWidth, cam.pixelHeight, 0x18);
            Graphics.Blit(source, source, ClearAlphaMaterial, 0);
            cam.targetTexture = source;
            RenderTexture.active = cam.targetTexture;
            cam.Render();
            Texture2D textured = new Texture2D(cam.targetTexture.width, cam.targetTexture.height);
            textured.ReadPixels(new Rect(0f, 0f, (float) cam.targetTexture.width, (float) cam.targetTexture.height), 0, 0);
            textured.Apply();
            cam.targetTexture = null;
            RenderTexture.active = active;
            RenderTexture.ReleaseTemporary(source);
            return textured;
        }

        public static Texture2D RenderScreenToTexture2d()
        {
            Texture2D textured = new Texture2D(Screen.width, Screen.height);
            textured.ReadPixels(new Rect(0f, 0f, (float) Screen.width, (float) Screen.height), 0, 0);
            textured.Apply();
            return textured;
        }

        public static Texture2D RenderTextureToTexture2d(RenderTexture rt)
        {
            RenderTexture active = RenderTexture.active;
            RenderTexture.active = rt;
            Texture2D textured = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false);
            textured.ReadPixels(new Rect(0f, 0f, (float) rt.width, (float) rt.height), 0, 0);
            textured.Apply();
            RenderTexture.active = active;
            return textured;
        }

        public static void SaveRenderTextureAsPng(RenderTexture rt, string fileName)
        {
            int width = rt.width;
            int height = rt.height;
            Texture2D textured = new Texture2D(width, height, TextureFormat.ARGB32, false);
            textured.ReadPixels(new Rect(0f, 0f, (float) width, (float) height), 0, 0);
            textured.Apply();
            byte[] bytes = textured.EncodeToPNG();
            UnityEngine.Object.Destroy(textured);
            File.WriteAllBytes(Application.dataPath + "/../" + fileName, bytes);
        }

        public static void SaveTextureAsPng(Texture2D tex2d, string fileName)
        {
            byte[] bytes = tex2d.EncodeToPNG();
            File.WriteAllBytes(Application.dataPath + "/../" + fileName, bytes);
        }
    }
}


using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Fizzle
{
    public static class FizzleUtil
    {
        static MethodInfo playClip;

        public static void PlayClip(AudioClip clip)
        {
            playClip.Invoke(null, new object[] { clip });
        }

        static FizzleUtil()
        {
            var unityEditorAssembly = typeof(AudioImporter).Assembly;
            var audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
            playClip = audioUtilClass.GetMethod("PlayClip", BindingFlags.Static | BindingFlags.Public, null, new System.Type[] { typeof(AudioClip) }, null);
        }

        static Texture2D WaveformTexture(float[] data, int width, int height)
        {
            var waveVisual = new Texture2D(width, height, TextureFormat.ARGB32, false);
            var pixels = new Color[width * height];
            var sections = (data.Length / 2) / width;
            var LY = height / 4;
            var RY = (height / 4) * 3;
            var H = height / 4;
            var di = 0;
            for (int x = 0; x < width; x++)
            {
                var smpL = 0f;
                var smpR = 0f;
                for (var sx = 0; sx < sections; sx++, di += 2)
                {
                    smpL += data[di];
                    smpR += data[di + 1];
                }
                smpL /= sections;
                smpR /= sections;
                var y = LY + Mathf.FloorToInt(H * smpL);
                y = y >= height ? height - 1 : y;
                y = y < 0 ? 0 : y;
                if (y > LY)
                    for (var yp = LY; yp <= y; yp++)
                        pixels[(yp * width) + x] = Color.green;
                else if (y < LY)
                    for (var yp = LY; yp >= y; yp--)
                        pixels[(yp * width) + x] = Color.green;
                y = RY + Mathf.FloorToInt(H * smpR);
                y = y >= height ? height - 1 : y;
                y = y < 0 ? 0 : y;
                if (y > RY)
                    for (var yp = RY; yp <= y; yp++)
                        pixels[(yp * width) + x] = Color.green;
                else if (y < RY)
                    for (var yp = RY; yp >= y; yp--)
                        pixels[(yp * width) + x] = Color.green;

                pixels[LY * width + x] = Color.white * 0.8f;
                pixels[0 * width + x] = Color.white;
                pixels[(H * 2) * width + x] = Color.white;
                pixels[(height - 1) * width + x] = Color.white;

            }
            waveVisual.SetPixels(pixels);
            waveVisual.Apply();
            return waveVisual;
        }
    }
}
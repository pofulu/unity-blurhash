
using Blurhash.Core;
using UnityEngine;

namespace Blurhash.Unity
{
    public static class BlurHash
    {
        public static string EncodeToBlurHash(Texture2D texture2d, int componentsX = 4, int componentsY = 3)
        {
            return BlurHashEncoder.Instance.EncodeToBlurHash(texture2d, componentsX, componentsY);
        }

        public static Texture2D DecodeToTexture2D(string blurhash, int outputWidth, int outputHeight, double punch = 1.0)
        {
            return BlurHashDecoder.Instance.DecodeToTexture2D(blurhash, outputWidth, outputHeight, punch);
        }

        public static Color32[] DecodeToColor32(string blurhash, int outputWidth, int outputHeight, double punch = 1.0)
        {
            return BlurHashDecoder.Instance.DecodeToColor32(blurhash, outputWidth, outputHeight, punch);
        }

        class BlurHashEncoder : CoreEncoder
        {
            private static BlurHashEncoder instance;
            public static BlurHashEncoder Instance
            {
                get
                {
                    if (instance == null)
                    {
                        instance = new BlurHashEncoder();
                    }

                    return instance;
                }
            }

            public string EncodeToBlurHash(Texture2D texture2d, int componentsX, int componentsY)
            {
                var pixels = new Pixel[texture2d.width, texture2d.height];

                for (int y = 0; y < texture2d.height; y++)
                {
                    for (int x = 0; x < texture2d.width; x++)
                    {
                        var color = texture2d.GetPixel(x, texture2d.height - 1 - y);
                        pixels[x, y] = new Pixel(color.r, color.g, color.b);
                    }
                }

                return CoreEncode(pixels, componentsX, componentsY);
            }
        }

        class BlurHashDecoder : CoreDecoder
        {
            private static BlurHashDecoder instance;
            public static BlurHashDecoder Instance
            {
                get
                {
                    if (instance == null)
                    {
                        instance = new BlurHashDecoder();
                    }

                    return instance;
                }
            }

            public Texture2D DecodeToTexture2D(string blurhash, int outputWidth, int outputHeight, double punch = 1.0)
            {
                var texture = new Texture2D(outputWidth, outputHeight);
                texture.wrapMode = TextureWrapMode.Clamp;
                texture.SetPixels32(DecodeToColor32(blurhash, outputWidth, outputHeight, punch));
                texture.Apply();

                return texture;
            }

            public Color32[] DecodeToColor32(string blurhash, int outputWidth, int outputHeight, double punch = 1.0)
            {
                var result = CoreDecode(blurhash, outputWidth, outputHeight, punch);
                var colors = new Color32[outputWidth * outputHeight];

                for (int y = 0; y < outputHeight; y++)
                {
                    for (int x = 0; x < outputWidth; x++)
                    {
                        var color = result[x, outputHeight - y - 1];
                        colors[outputWidth * y + x] = new Color((float)color.Red, (float)color.Green, (float)color.Blue);
                    }
                }

                return colors;
            }
        }
    }
}

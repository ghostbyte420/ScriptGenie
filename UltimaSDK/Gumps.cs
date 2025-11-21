using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ScriptGenie.UltimaSDK
{
    public static class Gumps
    {
        private static FileIndex m_FileIndex = new FileIndex("gumpidx.mul", "gumpart.mul", 0x4000, 12);
        private static Bitmap[] m_Cache;
        private static bool[] m_Removed;
        private static bool m_Loaded = false;

        static Gumps()
        {
            m_Cache = new Bitmap[0x4000];
            m_Removed = new bool[0x4000];
            m_Loaded = true;
        }

        public static Bitmap GetGump(int index, int hue, bool onlyHueGrayPixels, out bool patched)
        {
            patched = false;
            index &= 0x3FFF;

            if (m_Removed[index])
                return null;

            if (m_Cache[index] != null)
                return m_Cache[index];

            int length, extra;
            bool patchedInternal;
            Stream stream = m_FileIndex.Seek(index, out length, out extra, out patchedInternal);
            patched = patchedInternal;

            if (stream == null)
                return null;

            Bitmap bmp = LoadGump(stream, length, hue, onlyHueGrayPixels);

            if (Files.CacheData)
                m_Cache[index] = bmp;

            return bmp;
        }

        private static Bitmap LoadGump(Stream stream, int length, int hue, bool onlyHueGrayPixels)
        {
            byte[] buffer = new byte[length];
            stream.Read(buffer, 0, length);

            int width = (buffer[0] << 8) | buffer[1];
            int height = (buffer[2] << 8) | buffer[3];

            Bitmap bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format16bppArgb1555);

            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, width, height), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format16bppArgb1555);

            unsafe
            {
                ushort* pBuffer = (ushort*)bd.Scan0;
                fixed (byte* pData = buffer)
                {
                    ushort* pDataPtr = (ushort*)(pData + 4);

                    Hue hueObj = Hues.GetHue(hue);

                    for (int y = 0; y < height; ++y)
                    {
                        for (int x = 0; x < width; ++x, ++pBuffer, ++pDataPtr)
                        {
                            ushort val = *pDataPtr;
                            if ((val & 0x8000) != 0)
                            {
                                int hueIndex = (val & 0x3FFF) - 1;
                                if (hueIndex >= 0)
                                {
                                    if (onlyHueGrayPixels && hueIndex != 0)
                                        continue;

                                    if (hueIndex < hueObj.Colors.Length)
                                    {
                                        *pBuffer = (ushort)(hueObj.Colors[hueIndex] | 0x8000);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            bmp.UnlockBits(bd);
            return bmp;
        }
    }
}

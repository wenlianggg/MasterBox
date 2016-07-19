using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace MasterBox.Auth {
    public class Stegano {

        internal static Image JumblePixels(Image img, out string hash) {
            GraphicsUnit unit = GraphicsUnit.Pixel;
            Bitmap bmp = new Bitmap(img);
            bmp = ScaleImage(bmp, 1920, 1080); // Max size 1080p image
            RectangleF rf = bmp.GetBounds(ref unit);

            Random rnd = new Random();
            int iterations = GenerateInt(30, 200, rnd);
            for (int i = 0; i < iterations; i++) {
                int xcoord = GenerateInt(0, (int) rf.Height, rnd);
                int ycoord = GenerateInt(0, (int) rf.Width, rnd);
                Color clr = bmp.GetPixel(xcoord, ycoord);
                Color newclr = Color.FromArgb(clr.R, clr.G, clr.B);
                bmp.SetPixel(xcoord, ycoord, GetNearRgb(clr.R, clr.B, clr.G, rnd));
            }

            hash = BitmapGetHash(bmp);
            return img;
        }

        private static string BitmapGetHash(Bitmap img) {
            byte[] toBeHashed;
            if (img == null)
                return "";
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream()) {
                bf.Serialize(ms, img);
                toBeHashed = ms.ToArray();
            }
            using (SHA512 sha = new SHA512Managed()) {
                return Convert.ToBase64String(toBeHashed);
            }
        }


        private static int GenerateInt(int min, int max, Random rnd) {
            return (int)(rnd.NextDouble() * (max - min)) + min;
        }

        private static Color GetNearRgb(int R, int G, int B, Random rnd) {
            R = GetNearSingleColor(R, rnd);
            G = GetNearSingleColor(G, rnd);
            B = GetNearSingleColor(B, rnd);
           return Color.FromArgb(R, G, B);
        }

        private static int GetNearSingleColor(int val, Random rnd) {
            if (val < 11) {
                val += GenerateInt(0, 15, rnd);
            } else if (val > 244) {
                val += GenerateInt(-15, 0, rnd);
            } else {
                val += GenerateInt(10, 10, rnd);
            }
            return val;
        }

        public static Bitmap ScaleImage(Bitmap image, int maxWidth, int maxHeight) {
            // Reference: https://stackoverflow.com/questions/6501797/resize-image-proportionally-with-maxheight-and-maxwidth-constraints
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;
        }

    }
}
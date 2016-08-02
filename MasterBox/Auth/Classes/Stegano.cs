using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Drawing.Imaging;

namespace MasterBox.Auth {
    public class Stegano : IDisposable {

        private Bitmap _bmp;
        private GraphicsUnit unit = GraphicsUnit.Pixel;
        private string _format;

        // Constructor
        internal Stegano(Stream imgstream, string format) {
            Image img = Image.FromStream(imgstream);
            Bitmap bmp = new Bitmap(img);
            _bmp = bmp;
            _format = format;
        }

        internal MemoryStream ImageData {
            get {
                MemoryStream stream = new MemoryStream();
                if (_format == "image/jpeg") {
                    _bmp.Save(stream, ImageFormat.Jpeg);
                } else if (_format == "image/png") {
                    _bmp.Save(stream, ImageFormat.Png);
                } else if (_format == "image/gif") {
                    _bmp.Save(stream, ImageFormat.Gif);
                } else if (_format == "image/bmp") {
                    _bmp.Save(stream, ImageFormat.Bmp);
                }
                stream.Position = 0;
                return stream;
            }
        }


        // Instance methods
        internal string JumblePixels() {
            ScaleImage(3840, 2160); // Max size 4K image
            RectangleF rf = _bmp.GetBounds(ref unit);
            Random rnd = new Random();
            int iterations = GenerateInt(30, 200, rnd);
            for (int i = 0; i < iterations; i++) {
                int xcoord = GenerateInt(0, (int) rf.Width, rnd);
                int ycoord = GenerateInt(0, (int) rf.Height, rnd);
                Color clr = _bmp.GetPixel(xcoord, ycoord);
                Color newclr = Color.FromArgb(clr.R, clr.G, clr.B);
                _bmp.SetPixel(xcoord, ycoord, GetNearRgb(clr.R, clr.B, clr.G, rnd));
            }
            return BitmapGetHash(_bmp);
        }

        internal string BitmapGetHash(Bitmap img) {
            byte[] toBeHashed;
            if (img == null)
                return "";
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream()) {
                bf.Serialize(ms, img);
                toBeHashed = ms.ToArray();
            }
            using (SHA512 sha = new SHA512Managed()) {
				byte[] hashresult = sha.ComputeHash(toBeHashed);
                return Convert.ToBase64String(hashresult);
            }
        }


        private int GenerateInt(int min, int max, Random rnd) {
            return (int)(rnd.NextDouble() * (max - min)) + min;
        }

        private Color GetNearRgb(int R, int G, int B, Random rnd) {
            R = GetNearSingleColor(R, rnd);
            G = GetNearSingleColor(G, rnd);
            B = GetNearSingleColor(B, rnd);
           return Color.FromArgb(R, G, B);
        }
        private void ScaleImage(int maxWidth, int maxHeight) {
            // Reference: https://stackoverflow.com/questions/6501797/resize-image-proportionally-with-maxheight-and-maxwidth-constraints
            var ratioX = (double)maxWidth / _bmp.Width;
            var ratioY = (double)maxHeight / _bmp.Height;
            var ratio = Math.Min(ratioX, ratioY);
            var newWidth = (int)(_bmp.Width * ratio);
            var newHeight = (int)(_bmp.Height * ratio);
            var newImage = new Bitmap(newWidth, newHeight);
            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(_bmp, 0, 0, newWidth, newHeight);
            _bmp = newImage;
        }

        private int GetNearSingleColor(int val, Random rnd) {
            if (val < 11) {
                val += GenerateInt(0, 15, rnd);
            } else if (val > 244) {
                val += GenerateInt(-15, 0, rnd);
            } else {
                val += GenerateInt(10, 10, rnd);
            }
            return val;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing) {
            if (!disposedValue) {
                if (disposing) {
                    
                }

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Stegano() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

    }
}
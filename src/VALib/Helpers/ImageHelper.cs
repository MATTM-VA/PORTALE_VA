using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace VALib.Helpers
{
    public static class ImageHelper
    {
        public static Bitmap Ritaglia(Bitmap img, int x1, int y1, int x2, int y2)
        {
            Bitmap immagineRitagliata = null;
            Graphics grpImmagineRitagliata = null;
            Rectangle rettangoloRitaglio;

            int w = x2 - x1;
            int h = y2 - y1;

            if (w != 0 && h != 0)
            {
                rettangoloRitaglio = new Rectangle(x1, y1, x2 - x1, y2 - y1);

                immagineRitagliata = new Bitmap(x2 - x1, y2 - y1);

                grpImmagineRitagliata = Graphics.FromImage((System.Drawing.Image)immagineRitagliata);

                grpImmagineRitagliata.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                grpImmagineRitagliata.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                grpImmagineRitagliata.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;


                grpImmagineRitagliata.DrawImage((System.Drawing.Image)img, new Rectangle(0, 0, x2 - x1, y2 - y1), rettangoloRitaglio, GraphicsUnit.Pixel);
                grpImmagineRitagliata.Dispose();
            }

            return immagineRitagliata;
        }

        public static Bitmap Ridimensiona(Bitmap img, int w, int h)
        {
            Bitmap immagineRidimensionata = null;
            Graphics grpImmagineRidimensionata = null;
            Rectangle rettangoloRidimensionata;

            if (w != 0 && h == 0)
                h = (int)Math.Round(((double)img.Height / img.Width) * w);
            else if (w == 0 && h != 0)
                w = (int)Math.Round(((double)img.Width / img.Height) * h);

            if (w != 0 && h != 0)
            {
                rettangoloRidimensionata = new Rectangle(0, 0, w, h);

                immagineRidimensionata = new Bitmap(w, h);

                grpImmagineRidimensionata = Graphics.FromImage((System.Drawing.Image)immagineRidimensionata);

                grpImmagineRidimensionata.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                grpImmagineRidimensionata.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                grpImmagineRidimensionata.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

                grpImmagineRidimensionata.DrawImage((System.Drawing.Image)img, rettangoloRidimensionata);
                grpImmagineRidimensionata.Dispose();
            }

            return immagineRidimensionata;
        }
    }
}

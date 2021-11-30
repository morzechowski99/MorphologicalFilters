using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorphologicalFilters.BitmapFilter
{
   public static class BitmapFilter
    {
        public static Bitmap ThresholdByValue(Bitmap bitmap, byte value)
        {
            Bitmap newBitmap = new(bitmap);
            for (int i = 0; i < bitmap.Width; i++)
                for (int j = 0; j < bitmap.Height; j++)
                {
                    var color = bitmap.GetPixel(i, j);
                    var newValue = (color.R + color.G + color.B) / 3 < value ? 0 : 255;
                    newBitmap.SetPixel(i, j, Color.FromArgb(newValue, newValue, newValue));
                }
            return newBitmap;
        }  
        
        public static Bitmap Dilation(Bitmap bitmap)
        {
            double[,] mask = new double[3, 3];

            mask[0, 0] = 1;
            mask[1, 0] = 1;
            mask[2, 0] = 1;
            mask[0, 1] = 1;
            mask[1, 1] = 1;
            mask[2, 1] = 1;
            mask[0, 2] = 1;
            mask[1, 2] = 1;
            mask[2, 2] = 1;
            var colors = getColors(bitmap);
            Bitmap newBitmap = new Bitmap(bitmap);

            for (int i = 1; i < bitmap.Width - 1; i++)
            {
                for (int j = 1; j < bitmap.Height - 1; j++)
                {
                    var has1 = false;
                    for(var x = i - 1; x <= i+1;x++)
                        for(var y = j - 1; y <= j + 1; y++)
                        {
                            if (x == i && y == j)
                                continue;
                            if(colors[x,y].R == 255)
                            {
                                has1 = true;
                                break;
                            }

                        }
                    if (has1)
                        newBitmap.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                    else
                        newBitmap.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                }
            }
            return newBitmap;
        } 
        
        public static Bitmap Erosion(Bitmap bitmap)
        {
            double[,] mask = new double[3, 3];

            mask[0, 0] = 1;
            mask[1, 0] = 1;
            mask[2, 0] = 1;
            mask[0, 1] = 1;
            mask[1, 1] = 1;
            mask[2, 1] = 1;
            mask[0, 2] = 1;
            mask[1, 2] = 1;
            mask[2, 2] = 1;
            var colors = getColors(bitmap);
            Bitmap newBitmap = new Bitmap(bitmap);

            for (int i = 1; i < bitmap.Width - 1; i++)
            {
                for (int j = 1; j < bitmap.Height - 1; j++)
                {
                    var has0 = false;
                    for(var x = i - 1; x <= i+1;x++)
                        for(var y = j - 1; y <= j + 1; y++)
                        {
                            if (x == i && y == j)
                                continue;
                            if(colors[x,y].R == 0)
                            {
                                has0 = true;
                                break;
                            }

                        }
                    if (!has0)
                        newBitmap.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                    else
                        newBitmap.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                }
            }
            return newBitmap;
        }
        
        public static Bitmap HitOrMiss(Bitmap bitmap,byte[,]mask)
        {
            var colors = getColors(bitmap);
            Bitmap newBitmap = new Bitmap(bitmap);

            for (int i = 1; i < bitmap.Width - 1; i++)
            {
                for (int j = 1; j < bitmap.Height - 1; j++)
                {
                    var miss = false;
                    var maskX = 0;
                    for (var x = i - 1; x <= i + 1; x++)
                    {
                        var maskY = 0;
                        for (var y = j - 1; y <= j + 1; y++)
                        {
                            var maskVal = mask[maskX, maskY];
                            if (maskVal  == 2)
                            {
                                maskY++;
                                continue;
                            }
                            byte colorValue = colors[x, y].R == 255 ? (byte)1 : (byte)0;
                            if(maskVal != colorValue)
                            {
                                miss = true;
                                break;
                            }
                            maskY++;
                        }
                        maskX++;
                    }

                    if (!miss)
                        newBitmap.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                    else
                        newBitmap.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                }
            }
            return newBitmap;
        } 
        
        public static Bitmap Opening(Bitmap bitmap)
        {
            return Dilation(Erosion(bitmap));
        } 
        
        public static Bitmap GetClosing(Bitmap bitmap)
        {
            return Erosion(Dilation(bitmap));
        }

        private static Color[,] getColors(Bitmap bitmap)
        {
            Color[,] colors = new Color[bitmap.Width, bitmap.Height];
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    colors[i, j] = bitmap.GetPixel(i, j);
                }
            }

            return colors;
        }
    }
}

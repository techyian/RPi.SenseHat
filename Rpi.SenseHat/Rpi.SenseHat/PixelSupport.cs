////////////////////////////////////////////////////////////////////////////
//
//  This file is part of Rpi.SenseHat
//
//  Copyright (c) 2017, Mattias Larsson
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy of 
//  this software and associated documentation files (the "Software"), to deal in 
//  the Software without restriction, including without limitation the rights to use, 
//  copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the 
//  Software, and to permit persons to whom the Software is furnished to do so, 
//  subject to the following conditions:
//
//  The above copyright notice and this permission notice shall be included in all 
//  copies or substantial portions of the Software.
//
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//  INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
//  PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
//  HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION 
//  OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE 
//  SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Image = SixLabors.ImageSharp.Image;

namespace Emmellsoft.IoT.Rpi.SenseHat
{
	public static class PixelSupport
	{
		/// <summary>
		/// Gets a 2-dimensional pixel array from an image.
		/// </summary>
		/// <param name="imageUri">The URI to the image.</param>
		public static Color[,] GetPixels(Uri imageUri)
		{
			Color[,] pixels = null;
            
            using (FileStream stream = File.OpenRead(imageUri.AbsolutePath))
		    using (Image<Rgba32> image = Image.Load(stream))
		    {
		        var pixelData = image.SavePixelData();

		        pixels = new Color[image.Width, image.Height];

                int pixelDataIndex = 0;
                for (int y = 0; y < image.Height; y++)
		        {
		            for (int x = 0; x < image.Width; x++)
		            {
		                byte b = pixelData[pixelDataIndex];
		                byte g = pixelData[pixelDataIndex + 1];
		                byte r = pixelData[pixelDataIndex + 2];
		                byte a = pixelData[pixelDataIndex + 3];

		                pixels[x, y] = Color.FromArgb(a, r, g, b);

		                pixelDataIndex += 4;
		            }
		        }
            }
            
			return pixels;
		}

		internal static void ConvertDirectionParameters(
			DisplayDirection direction,
			bool flipHorizontal,
			bool flipVertical,
			out bool leftToRight,
			out bool topToBottom,
			out bool flipAxis)
		{
			switch (direction)
			{
				case DisplayDirection.Deg0:
					if (!flipHorizontal && !flipVertical)
					{
						leftToRight = true;
						topToBottom = true;
					}
					else if (flipHorizontal && !flipVertical)
					{
						leftToRight = false;
						topToBottom = true;
					}
					else if (!flipHorizontal /* && flipVertical */)
					{
						leftToRight = true;
						topToBottom = false;
					}
					else /* if (flipHorizontal && flipVertical) */
					{
						leftToRight = false;
						topToBottom = false;
					}

					flipAxis = false;
					break;

				case DisplayDirection.Deg90:
					if (!flipHorizontal && !flipVertical)
					{
						leftToRight = false;
						topToBottom = true;
					}
					else if (flipHorizontal && !flipVertical)
					{
						leftToRight = true;
						topToBottom = true;
					}
					else if (!flipHorizontal /* && flipVertical */)
					{
						leftToRight = false;
						topToBottom = false;
					}
					else /* if (flipHorizontal && flipVertical) */
					{
						leftToRight = true;
						topToBottom = false;
					}

					flipAxis = true;
					break;

				case DisplayDirection.Deg180:
					if (!flipHorizontal && !flipVertical)
					{
						leftToRight = false;
						topToBottom = false;
					}
					else if (flipHorizontal && !flipVertical)
					{
						leftToRight = true;
						topToBottom = false;
					}
					else if (!flipHorizontal /* && flipVertical */)
					{
						leftToRight = false;
						topToBottom = true;
					}
					else /* if (flipHorizontal && flipVertical) */
					{
						leftToRight = true;
						topToBottom = true;
					}

					flipAxis = false;
					break;

				case DisplayDirection.Deg270:
					if (!flipHorizontal && !flipVertical)
					{
						leftToRight = true;
						topToBottom = false;
					}
					else if (flipHorizontal && !flipVertical)
					{
						leftToRight = false;
						topToBottom = false;
					}
					else if (!flipHorizontal /* && flipVertical */)
					{
						leftToRight = true;
						topToBottom = true;
					}
					else /* if (flipHorizontal && flipVertical) */
					{
						leftToRight = false;
						topToBottom = true;
					}

					flipAxis = true;
					break;

				default:
					throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
			}
		}

		public static Color[,] Convert1DTo2D(Color[] pixels)
		{
			if (pixels.Length != 64)
			{
				throw new ArgumentException("The pixel array must be 64 bytes long (i.e. 8x8).", nameof(pixels));
			}

			var pixels2D = new Color[8, 8];

			int i = 0;
			for (int y = 0; y < 8; y++)
			{
				for (int x = 0; x < 8; x++)
				{
					pixels2D[x, y] = pixels[i++];
				}
			}

			return pixels2D;
		}
	}
}
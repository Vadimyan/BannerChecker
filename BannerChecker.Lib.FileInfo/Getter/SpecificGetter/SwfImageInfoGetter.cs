using System.Drawing;
using System.IO;

namespace BannerChecker.Lib.FileInfo.Getter.SpecificGetter
{
	[ImageInfoGetter(".swf")]
	class SwfImageInfoGetter : ImageInfoGetterBase
	{
		protected override Size GetImageSize(string filePath)
		{
			var headerReader = new FlashHeaderReader(filePath);
			return new Size(headerReader.Width, headerReader.Height);
		}

		/// <summary>
		/// Summary description for FlashHeaderReader.
		/// </summary>
		private class FlashHeaderReader
		{
			internal int FrameRate { get; private set; }

			internal int FrameCount { get; private set; }

			internal int Width { get; private set; }

			internal int Height { get; private set; }

			internal bool Error { get; private set; }

			// To Parse Header
			private readonly byte[] data;
			private int currentIndex;


			public FlashHeaderReader(string filename)
			{
				currentIndex = 0;
				Error = false;
				using (var fs = File.OpenRead(filename))
				{
					data = new byte[fs.Length];
					fs.Read(data, 0, data.Length);
					fs.Close();
				}
				ParseHeader();
			}

			/*
			 *  Parse the header of the swf format
			 *  Format http://www.half-serious.com/swf/format/index.html#header
			 *  convert from php : http://www.zend.com/codex.php?id=1382&single=1   
			 */

			private void ParseHeader()
			{
				ReadSignature();
				GetNextByte();
				ReadFileLength();
				ReadFrameSize();
				ReadFrameRate();
				ReadFrameCount();
			}

			/*
			 * Read first caractere FWS or CWS
			 * File compressed are not supported
			 */

			private void ReadSignature()
			{
				var readingByte = GetNextByte();
				if (readingByte == 'C')
				{
					// Not supported
				}
				if (readingByte != 'F' && readingByte != 'C')
				{
					Error = true;
				}

				if (GetNextByte() != 'W')
					Error = true;
				if (GetNextByte() != 'S')
					Error = true;
			}

			/*
			 *  Read the RECT Structure : Frame size in twips   
			 *  This retrieve from byte the size.
			 *  the source php is difficult to translate I have made
			 *  something working...
			 */

			private void ReadFrameSize()
			{
				int cByte = GetNextByte();
				int nbBits = cByte >> 3;

				cByte &= 7;
				cByte <<= 5;

				int currentBit = 2;

				// Must get all 4 values in the RECT
				for (int numField = 0; numField < 4; numField++)
				{
					var currentValue = 0;
					int bitcount = 0;
					while (bitcount < nbBits)
					{
						if ((cByte & 128) == 128)
						{
							currentValue = currentValue + (1 << (nbBits - bitcount - 1));
						}
						cByte <<= 1;
						cByte &= 255;
						currentBit--;
						bitcount++;
						// We will be needing a new byte if we run out of bits
						if (currentBit < 0)
						{
							cByte = GetNextByte();
							currentBit = 7;
						}
					}

					// TWIPS to PIXELS
					currentValue /= 20;
					switch (numField)
					{
						case 0:
							Width = currentValue;
							break;
						case 1:
							Width = currentValue - Width;
							break;
						case 2:
							Height = currentValue;
							break;
						case 3:
							Height = currentValue - Height;
							break;
					}
				}
			}

			/*
			 * Read Frame delay in 8.8 fixed number of frames per second  
			 */

			private void ReadFrameRate()
			{
				// Frame rate
				var fpsDecimal = GetNextByte();
				var fpsInt = GetNextByte();
				FrameRate = fpsInt + fpsDecimal / 100;
			}

			private void ReadFrameCount()
			{
				for (int i = 0; i < 2; i++)
				{
					FrameCount += GetNextByte() << (8 * i);
				}
			}

			/*
			 *  Read  FileLength : Length of entire file in bytes   
			 *  Not implemented
			 */

			private void ReadFileLength()
			{
				//Read Size
				GetNextByte();
				GetNextByte();
				GetNextByte();
				GetNextByte();
			}

			/*
			 *  Retrieve one caractere of the buffer.
			 */

			private byte GetNextByte()
			{
				var result = data[currentIndex];
				currentIndex++;
				return result;
			}
		}
	}
}

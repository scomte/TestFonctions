using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace TestFonctions
{
    public static class FileFunctions
    {

        public static bool IsVariableSeqfile(string filePath, int fileLength, ref int RecordCount, ref int RecordMaxLength, ref int RecordMinLength)
        {
            bool variablefile = false;

            byte[] record;
            byte[] FileContent = null;
            FileContent = ReadFileContentsByte(filePath);
            //Encoding fileEncoding = Encoding.ASCII;
            //Encoding fileEncoding = Encoding.Default;

            Int32 l1 = 0;
            Int32 l2 = 0;
            Int32 l01 = 0;
            Int32 l02 = 0;
            int bytesreaded = 0;
            int bytesleft = 0;
            string sRrecordLength = string.Empty;
            Int32 recordLength = 0;
            byte[] header;
            RecordCount = 0;
            RecordMaxLength = 0;
            RecordMinLength = 0;

            using (FileStream ftest = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (BinaryReader reader = new BinaryReader(ftest))
                {
                    //Lecture de l'entete
                    reader.ReadBytes(126);
                    bytesreaded = 126;

                    bytesleft = fileLength - bytesreaded;

                    //while (reader.ReadBytes(recordLength).Length > 0)
                    while (bytesleft > 0)
                    //while (RecordCount < 10)
                    {
                        try
                        {
                            header = reader.ReadBytes(4);
                            bytesreaded = bytesreaded + 4;
                            bytesleft = fileLength - bytesreaded;

                            if (header.Length > 2)
                            {
                                RecordCount++;


                                l01 = Int32.Parse(header.ToArray()[0].ToString());
                                l02 = Int32.Parse(header.ToArray()[1].ToString());
                                l1 = Int32.Parse(header.ToArray()[2].ToString());
                                l2 = Int32.Parse(header.ToArray()[3].ToString());

                                if ((l01 == 0) && (l02 == 0))
                                //if (l2 > 0)
                                {
                                    sRrecordLength = "0" + (l1.ToString("x") + l2.ToString("x")).Substring(1);
                                    recordLength = Int32.Parse(sRrecordLength, System.Globalization.NumberStyles.HexNumber);

                                    record = reader.ReadBytes(recordLength);

                                    bytesreaded = bytesreaded + recordLength;
                                    bytesleft = fileLength - bytesreaded;
                                    variablefile = true;

                                    if (RecordCount == 1)
                                    {
                                        RecordMaxLength = recordLength;
                                        RecordMinLength = recordLength;
                                    }

                                    if (recordLength > RecordMaxLength)
                                    {
                                        RecordMaxLength = recordLength;
                                    }
                                    if (recordLength < RecordMinLength)
                                    {
                                        RecordMinLength = recordLength;
                                    }
                                }
                                else
                                {
                                    variablefile = false;
                                    break;
                                }
                            }
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            variablefile = false;
                            break;
                        }
                    }
                }
            }

            return variablefile;
        }

        public static bool IsLineSequentialfile(string filePath, ref int RecordCount, ref int RecordMaxLength, ref int RecordMinLength, int fileLength)
        {
            bool linesequentialfile = false;
            RecordCount = 0;
            RecordMaxLength = 0;
            RecordMinLength = 0;


            try
            {
                // Read each line of the file into a string array. Each element
                // of the array is one line of the file.
                int linecounter = 0;

                using (var streamReader = File.OpenText(filePath))
                {
                    var lines = streamReader.ReadToEnd().Split("\n".ToCharArray());
                    RecordCount = lines.Count();

                    if ((fileLength >= 10000) && (RecordCount == 1))
                    {
                        linesequentialfile = false;
                    }
                    else
                    {

                        if (RecordCount == 1)
                        {

                            List<int> primeFactorsList = primeFactors(fileLength);
                            //Console.WriteLine(primeFactorsList.Count);

                            if (primeFactorsList.Count > 0)
                            {
                                linesequentialfile = false;
                            }
                            else
                            {
                                linesequentialfile = true;
                            }
                        }
                        else
                        {
                            foreach (var line in lines)
                            {
                                linecounter++;

                                if (linecounter == 1)
                                {
                                    RecordMaxLength = line.Length; ;
                                    RecordMinLength = line.Length; ;
                                }

                                if (line.Length > RecordMaxLength)
                                {
                                    RecordMaxLength = line.Length;
                                }
                                if (line.Length < RecordMinLength)
                                {
                                    RecordMinLength = line.Length;
                                }
                            }
                            linesequentialfile = true;
                        }
                    }
                }

            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                linesequentialfile = false;
            }
            return linesequentialfile;
        }

        /// Reads the raw contents of the file.
        /// </summary>
        /// <param name="filePath">string:  The fully qualified path to the file.</param>
        /// <returns>An array of the raw byte information stored in the file.</returns>
        public static byte[] ReadFileContentsByte(string filePath)
        {
            byte[] returnValue = null;
            if (File.Exists(filePath))
            {
                returnValue = new byte[0];
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    byte[] buffer = new byte[fs.Length];
                    if (fs.Length > buffer.Length)
                    {
                        long remainingBytes = fs.Length;
                        while (remainingBytes > 0)
                        {
                            if (remainingBytes >= buffer.Length)
                            {
                                fs.Read(buffer, 0, buffer.Length);
                                int oldLength = returnValue.Length;
                                int newLength = returnValue.Length + buffer.Length;
                                Array.Resize<byte>(ref returnValue, newLength);
                                Array.Copy(buffer, 0, returnValue, oldLength, buffer.Length);
                            }
                            else
                            {
                                Array.Clear(buffer, 0, buffer.Length);
                                fs.Read(buffer, 0, (int)remainingBytes);
                                int oldLength = returnValue.Length;
                                int newLength = returnValue.Length + (int)remainingBytes;
                                Array.Resize<byte>(ref returnValue, newLength);
                                Array.Copy(buffer, 0, returnValue, oldLength, (int)remainingBytes);
                            }
                            remainingBytes -= buffer.Length;
                        }
                    }
                    else
                    {
                        fs.Read(buffer, 0, (int)fs.Length);
                        Array.Resize<byte>(ref returnValue, (int)fs.Length);
                        Array.Copy(buffer, 0, returnValue, 0, (int)fs.Length);
                    }
                }
            }
            return returnValue;
        } // end function ReadFileContents

        public static void WriteFileNoNewLine(string file, string line, bool append)
        {
            System.Text.Encoding encoding = System.Text.Encoding.GetEncoding(1252);
            StreamWriter sw_log = new StreamWriter(file, append, encoding);
            sw_log.Write(line);
            sw_log.Flush();
            sw_log.Dispose();
            sw_log.Close();
        }

        public static List<int> primeFactors(int InputInteger)
        {
            List<int> locPrimeFactors = new List<int>();

            // Print the number of 2s that divide InputInteger
            while (InputInteger % 2 == 0)
            {
                locPrimeFactors.Add(2);
                InputInteger /= 2;
            }
            // InputInteger must be odd at this point. So we can 
            // skip one element (Note i = i +2) 
            for (int i = 3; i <= Math.Sqrt(InputInteger); i += 2)
            {
                // While i divides n, print i and divide n 
                while (InputInteger % i == 0)
                {
                    locPrimeFactors.Add(i);
                    InputInteger /= i;
                }
            }
            // This condition is to handle the case when 
            // InputInteger is a prime number greater than 2 
            if (InputInteger > 2)
                locPrimeFactors.Add(InputInteger);

            return locPrimeFactors;
        }

        public static byte[] ConvertAsciiToEbcdic(byte[] asciiData)
        {
            // Create two different encodings.         
            Encoding ascii = Encoding.ASCII;
            Encoding ebcdic = Encoding.GetEncoding("IBM037");

            //Retutn Ebcdic Data
            return Encoding.Convert(ascii, ebcdic, asciiData);
        }

        public static byte[] ConvertEbcdicToAscii(byte[] ebcdicData)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            // Create two different encodings.      
            Encoding ascii = Encoding.ASCII;
            Encoding ebcdic = Encoding.GetEncoding(37);

            //Retutn Ascii Data 
            return Encoding.Convert(ebcdic, ascii, ebcdicData);
        }
        public static string strConvertEBCDOCToASCII(byte[] fileData)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            Encoding ascii = Encoding.ASCII;
            //Encoding ebcdic = Encoding.GetEncoding("IBM037");
            Encoding ebcdic = Encoding.GetEncoding(37);

            byte[] convertedByte = Encoding.Convert(ebcdic, ascii, fileData);

            return Encoding.ASCII.GetString(convertedByte);
        }

        public static string strConvertASCIIToUTF8(string inAsciiString)
        {

            // Create encoding ASCII.
            Encoding inAsciiEncoding = Encoding.ASCII;
            // Create encoding UTF8.
            Encoding outUTF8Encoding = Encoding.UTF8;

            // Convert the input string into a byte[].
            byte[] inAsciiBytes = inAsciiEncoding.GetBytes(inAsciiString);
            // Conversion string in ASCII encoding to UTF8 encoding byte array.
            byte[] outUTF8Bytes = Encoding.Convert(inAsciiEncoding, outUTF8Encoding, inAsciiBytes);

            // Convert the byte array into a char[] and then into a string.
            char[] inUTF8Chars = new

            char[outUTF8Encoding.GetCharCount(outUTF8Bytes, 0, outUTF8Bytes.Length)];

            outUTF8Encoding.GetChars(outUTF8Bytes, 0, outUTF8Bytes.Length, inUTF8Chars, 0);

            string outUTF8String = new string(inUTF8Chars);

            return outUTF8String;

        }

        public static byte[] byteConvertASCIIToUTF8(byte[] inAsciiBytes)
        {

            // Create encoding ASCII.
            Encoding inAsciiEncoding = Encoding.ASCII;
            // Create encoding UTF8.
            Encoding outUTF8Encoding = Encoding.UTF8;

            // Conversion string in ASCII encoding to UTF8 encoding byte array.
            byte[] outUTF8Bytes = Encoding.Convert(inAsciiEncoding, outUTF8Encoding, inAsciiBytes);

            return outUTF8Bytes;

        }

        public static byte[] ReadBinaryLineFromFile(string filePath, int recordLength, int lineToRead)
        {
            if (!File.Exists(filePath) || string.IsNullOrEmpty(filePath) || recordLength == 0 || lineToRead == 0)
            {
                return null;
            }
            else
            {
                byte[] ligne;
                int beforeReading = (lineToRead - 1) * recordLength;

                if (lineToRead == 1)
                {
                    using (FileStream ftest = File.OpenRead(filePath))
                    {
                        using (BinaryReader reader = new BinaryReader(ftest))
                        {
                            ligne = reader.ReadBytes(recordLength);
                        }
                    }
                }
                else
                {
                    using (FileStream ftest = File.OpenRead(filePath))
                    {
                        using (BinaryReader reader = new BinaryReader(ftest))
                        {
                            ligne = reader.ReadBytes(beforeReading);
                            ligne = reader.ReadBytes(recordLength);
                        }
                    }
                }

                return ligne;
            }
        }

        public static Tuple<int, int> GetPositionInFile (int AbsolutePosition, int recordLength)
        {
            if (AbsolutePosition == 0 || recordLength == 0)
            {
                return null;
            }
            else
            {
                int Row = 0;
                int Column = 0;

                Row = (AbsolutePosition / recordLength) + 1;
                Column = AbsolutePosition % recordLength;

                Tuple<int, int> FilePosition = new Tuple<int, int> (Row, Column);

                return FilePosition;

            }
        }
    }
}

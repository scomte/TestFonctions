using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace TestFonctions
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            //TestEBCDICRead();

            //TestReadFileLine();

            //TestFileHexa();

            //TestFilePosition();

            TestSplitFile();
        }


        static void TestEBCDICRead()
        {

            Console.WriteLine("** TestEBCDICRead Start **");

            string filePath = @"\\srvfilesgva02\users$\sbp\Projects\Tests\F01A-Sequential-154\F01A.QSAM";
            int RecordLength = 154;

            //string filePath = @"\\srvfilesgva02\users$\sbp\Projects\Tests\P41-Seq-155\F41A.QSAM";
            //int RecordLength = 155;

            byte[] ligneadjusted;
            byte[] ligne;
            //byte[] outUTF8Bytes;

            using (FileStream ftest = File.OpenRead(filePath))
            {
                using (BinaryReader reader = new BinaryReader(ftest, System.Text.Encoding.ASCII))
                {
                    //for (int i = 0; i < RecordCount; i++)
                    for (int i = 0; i < 5; i++)
                    {
                        ligne = reader.ReadBytes(RecordLength);
                        ligneadjusted = ligne;
                        CommonFunctions.adjustAsciibyte(ref ligneadjusted);


                        string ligneHex = BitConverter.ToString(ligne);
                        string ligneASCII = System.Text.Encoding.ASCII.GetString(ligne);
                        string ligneUTF8 = System.Text.Encoding.UTF8.GetString(ligne);
                        string ligneadjustedASCII = System.Text.Encoding.ASCII.GetString(ligneadjusted);
                        string ligneadjustedUTF8 = System.Text.Encoding.UTF8.GetString(ligneadjusted);

                        string HexaFirstLine = string.Empty;
                        string HexaSecondLine = string.Empty;
                        GestionHexa.HexaExtractLines(ligne, ref HexaFirstLine, ref HexaSecondLine);

                        //Console.WriteLine(Environment.NewLine + "*****************************************************************************");
                        //Console.WriteLine(Environment.NewLine + ligneASCII);
                        //Console.WriteLine(Environment.NewLine + ligneUTF8);
                        //Console.WriteLine(Environment.NewLine + ligneadjustedASCII);
                        //Console.WriteLine(Environment.NewLine + ligneadjustedUTF8);
                        //Console.WriteLine(Environment.NewLine + HexaFirstLine);
                        //Console.WriteLine(Environment.NewLine + HexaSecondLine);
                        //Console.WriteLine(Environment.NewLine + ligneHex);


                        Debug.Write(Environment.NewLine + "*****************************************************************************");
                        Debug.Write(Environment.NewLine + ligneASCII);
                        Debug.Write(Environment.NewLine + ligneUTF8);
                        Debug.Write(Environment.NewLine + ligneadjustedASCII);
                        Debug.Write(Environment.NewLine + ligneadjustedUTF8);
                        Debug.Write(Environment.NewLine + HexaFirstLine);
                        Debug.Write(Environment.NewLine + HexaSecondLine);
                        Debug.Write(Environment.NewLine + ligneHex);

                    }
                }
            }

        }

        static void TestReadFileLine ()
        {

            string filePath = @"\\srvfilesgva02\users$\sbp\Projects\Tests\F01A-Sequential-154\F01A.QSAM";
            int RecordLength = 154;

            byte[] ligneadjusted;
            byte[] ligne;
            string ligneadjustedASCII = string.Empty;

            //ligne = FileFunctions.ReadBinaryLineFromFile(filePath, RecordLength, 19);
            ligne = FileFunctions.ReadBinaryLineFromFile(filePath, RecordLength, 19);
            ligneadjusted = ligne;
            CommonFunctions.adjustAsciibyte(ref ligneadjusted);
            ligneadjustedASCII = System.Text.Encoding.ASCII.GetString(ligneadjusted);

            Debug.Write(Environment.NewLine + "*****************************************************************************");
            Debug.Write(Environment.NewLine + ligneadjustedASCII);

            //ligne = FileFunctions.ReadBinaryLineFromFile(filePath, RecordLength, 194);
            //ligneadjusted = ligne;
            //CommonFunctions.adjustAsciibyte(ref ligneadjusted);
            //ligneadjustedASCII = System.Text.Encoding.ASCII.GetString(ligneadjusted);

            //Debug.Write(Environment.NewLine + "*****************************************************************************");
            //Debug.Write(Environment.NewLine + ligneadjustedASCII);
        }

        static void TestFileHexa()
        {

            string filePath = @"\\srvfilesgva02\users$\sbp\Projects\Tests\F01A-Sequential-154\F01A.QSAM";
            //int RecordLength = 154;


            byte[] allBytes = File.ReadAllBytes(filePath);

            string hexafile1 = GestionHexa.ToHexString(allBytes);
            //string hexafile2 = GestionHexa.ByteToHexString(allBytes);

            Debug.Write(Environment.NewLine + "*****************************************************************************");
            Debug.Write(Environment.NewLine + hexafile1);
            //Debug.Write(Environment.NewLine + hexafile2);


            //503f
            string pattern = "503f";
            MatchCollection matches = Regex.Matches(hexafile1, @pattern);
            foreach (Match match in matches)
            {
                Debug.Write(Environment.NewLine + match.Index.ToString());
            }
            // Créer liste des index
        }

        static void TestFilePosition ()
        {

            Tuple<int, int> PositionInFile = FileFunctions.GetPositionInFile(323, 154);

            Debug.Write(Environment.NewLine + PositionInFile.Item1.ToString() + " | " + PositionInFile.Item2.ToString());

            PositionInFile = FileFunctions.GetPositionInFile(631, 154);

            Debug.Write(Environment.NewLine + PositionInFile.Item1.ToString() + " | " + PositionInFile.Item2.ToString());

            PositionInFile = FileFunctions.GetPositionInFile(14491, 154);

            Debug.Write(Environment.NewLine + PositionInFile.Item1.ToString() + " | " + PositionInFile.Item2.ToString());


        }

        static void TestSplitFile()
        {

            //string filePath = @"\\srvfilesgva02\users$\sbp\Projects\Tests\F01A-Sequential-154\F01A.QSAM";
            //int RecordLength = 154;

            string filePath = @"\\srvfilesgva02\users$\sbp\Projects\Tests\F151-Seq-146\F151.QSAM";
            int RecordLength = 146;


            // Split sequential file
            #region [Split sequential file]
            byte[] allBytes = File.ReadAllBytes(filePath);
            //byte[] ligneadjusted = allBytes;
            CommonFunctions.adjustAsciibyte(ref allBytes);
            string fileAdjustedASCII = System.Text.Encoding.ASCII.GetString(allBytes);

            //List<string> splitList = CommonFunctions.SplitByLength(fileAdjustedASCII, RecordLength);
            List<string> splitList = CommonFunctions.SplitByLength2(fileAdjustedASCII, RecordLength);

            #endregion [Split sequential file]

            // split line sequential file
            #region [Split line sequential file]

            //filePath = @"\\srvfilesgva02\users$\sbp\Projects\Tests\F01G2-Line-Sequential-160\F01G2.QSAM";

            //List<string> allLinesText = File.ReadAllLines(filePath).ToList();

            #endregion [Split line sequential file]

            Debug.Write(Environment.NewLine  + " List count < " + splitList.Count.ToString());

        }


    }
}

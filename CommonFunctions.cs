using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TestFonctions
{
    public static class CommonFunctions
    {
        public static string HexaToNumericXtend(string DataNum)
        {
            string dataint = string.Empty;
            bool boWrongHexa = false;

            try
            {
                for (int indice = 0; indice < DataNum.Length; ++indice)
                {
                    int ResteDiv = indice % 2;

                    switch (ResteDiv)
                    {
                        case 0:
                            if (DataNum[indice] != '3')
                            {
                                boWrongHexa = true;
                            }
                            break;
                        case 1:
                            if (boWrongHexa)
                            {
                                byte[] byteDataNum = Enumerable.Repeat((byte)0x00, 1).ToArray(); ;
                                dataint = dataint + Encoding.ASCII.GetString(byteDataNum);
                                boWrongHexa = false;
                            }
                            else
                            {
                                dataint = dataint + DataNum[indice];
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
#pragma warning disable CS0168 // The variable 'e' is declared but never used
            catch (Exception e)
#pragma warning restore CS0168 // The variable 'e' is declared but never used
            {
                //Console.WriteLine("The Directory Creation failed: {0}", e.ToString());
            }

            return dataint;
        }

        public static string NumericXtendToHexa(string DataNum)
        {
            string dataint = string.Empty;
            for (int indice = 0; indice < DataNum.Length; ++indice)
            {
                dataint = "3" + DataNum[indice];
            }

            return dataint;
        }

        public static int FNbMaxDigits(string strVarId, int NbDigitMax)
        {
            int NbDigit = CountDigits(Int32.Parse(strVarId));
            if (NbDigit > NbDigitMax)
            {
                NbDigitMax = NbDigit;
            }

            return NbDigitMax;
        }

        public static int CountDigits(int number)
        {
            // In case of negative numbers
            number = Math.Abs(number);

            if (number >= 10)
                return CountDigits(number / 10) + 1;
            return 1;
        }

        public static bool IsNumeric(string valueToCheck)
        {
            Regex regex = new Regex(@"^[-+]?\d*[.,]?\d*$");
            return regex.IsMatch(valueToCheck);
        }

        public static bool IsOnlyNumeric(string valueToCheck)
        {
            if (string.IsNullOrEmpty(valueToCheck))
            {
                return false;
            }
            else
            {
                Regex regex = new Regex(@"^\d*$");
                return regex.IsMatch(valueToCheck);
            }
        }

        public static bool IsDigitNumeric(string valueToCheck)
        {
            Regex regex = new Regex(@"^[ZB]?\d$");
            return regex.IsMatch(valueToCheck);
        }

        public static void adjustAsciibyte(ref byte[] bytes)
        {
            var exclusionList = Enumerable.Range(0, 32).ToList();
            var exclusionList2 = Enumerable.Range(127, 34).ToList();
            //var exclusionList3 = Enumerable.Range(80, 176).ToList();

            var allExclusions = exclusionList.Concat(exclusionList2).ToList();

            bool isExist = false;

            for (int i = 0; i < bytes.Length; i++)
            {
                isExist = allExclusions.Contains(bytes[i]);

                if (isExist)
                {
                    bytes[i] = 46;
                }
            }
        }

        public static List<string> SplitByLength(string item, int size)
        {
            if (item.Length <= size) return new List<string> { item };
            var temp = new List<string> { item.Substring(0, size) };
            temp.AddRange(SplitByLength(item.Substring(size), size));
            return temp;
        }

        public static List<string> SplitByLength2(string originalString, int chunkSize)
        {
            List<string> temp = new List<string>();
            for (int i = 0; i < originalString.Length; i = i + chunkSize)
            {
                if (originalString.Length - i >= chunkSize)
                    temp.Add(originalString.Substring(i, chunkSize));
                else
                    temp.Add(originalString.Substring(i, ((originalString.Length - i))));
            }
            return temp;
        }

    }
}

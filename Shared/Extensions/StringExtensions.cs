using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Extensions
{
    public static class StringExtensions
    {
        public static string FirstCharToUpper(this string input)
        {
            input = input.ToLower();
            input = input switch
            {
                null => throw new ArgumentNullException(nameof(input)),
                "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
                _ => input.First().ToString().ToUpper() + input.Substring(1)
            };

            return input;
        }
        private static Dictionary<int, string> numbersMap = null;
        public static string GetAlphabeticCurerncyValue(decimal value)
        {
            string alphabeticCurerncy = String.Empty;
            alphabeticCurerncy = GetTextByNumber((int)value) + " manat ";
            decimal floatPointPart = value - Math.Truncate(value);

            if (floatPointPart > 0m)
            {
                string valueTmp = value.ToString("G", CultureInfo.InvariantCulture);
                int floatPoinPart = int.Parse(valueTmp.Substring(valueTmp.IndexOf('.') + 1));
                alphabeticCurerncy += String.Format(" {0} qəpik", GetTextByNumber(floatPoinPart));
            }

            return alphabeticCurerncy;
        }


        public static string GetTextByNumber(int x)
        {
            if (numbersMap == null)
            {
                numbersMap = new Dictionary<int, string>(22);

                numbersMap.Add(1, "bir");
                numbersMap.Add(2, "iki");
                numbersMap.Add(3, "üç");
                numbersMap.Add(4, "dörd");
                numbersMap.Add(5, "beş");
                numbersMap.Add(6, "altı");
                numbersMap.Add(7, "yeddi");
                numbersMap.Add(8, "səkkiz");
                numbersMap.Add(9, "doqquz");
                numbersMap.Add(10, "on");
                numbersMap.Add(20, "iyirmi");
                numbersMap.Add(30, "otuz");
                numbersMap.Add(40, "qırx");
                numbersMap.Add(50, "əlli");
                numbersMap.Add(60, "altmış");
                numbersMap.Add(70, "yetmiş");
                numbersMap.Add(80, "səksən");
                numbersMap.Add(90, "doxsan");
                numbersMap.Add(100, "yüz");
                numbersMap.Add(1000, "min");
                numbersMap.Add(1000000, "milyon");
                numbersMap.Add(1000000000, "milyard");
            }

            string result = "";
            int groupIndex = 1000;
            int groupNumber = 1000;

            if (x == 0)
            {
                result = "sıfır";
            }
            else if (x > 0)
            {
                int modX;
                string bufGroupedNumberText;

                while (x > 0)
                {
                    bufGroupedNumberText = "";
                    modX = x % groupNumber;

                    //grouong by 100
                    if (modX >= 100)
                    {
                        bufGroupedNumberText += ((modX / 100 == 1) ? ("") : (numbersMap[modX / 100] + " ")) +
                                                numbersMap[100] + " ";
                        modX %= 100;
                    }

                    //grouping by 10
                    if (modX >= 10)
                    {
                        bufGroupedNumberText += numbersMap[(modX / 10) * 10] + " ";
                        modX %= 10;
                    }

                    if (modX >= 1)
                    {
                        bufGroupedNumberText += numbersMap[modX] + " ";
                    }

                    x /= groupNumber;
                    result = ((x % groupNumber > 0) ? (numbersMap[groupIndex] + " ") : ("")) + bufGroupedNumberText +
                             result;
                    if (x == 1) x = 0;
                    groupIndex *= groupNumber;
                }
            }
            else
            {
                result = "error";
            }

            return result.Trim();
        }

        /// <summary>
        /// Return sufix for year
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static string GetYearSufix(int year)
        {
            switch (year % 10)
            {
                case 1:
                case 2:
                case 5:
                case 7:
                case 8:
                    return "-ci";
                case 3:
                case 4:
                    return "-cü";

                case 6:
                    return "-cı";

                case 9:
                    return "-cu";

                case 0:
                    switch (year % 100)
                    {
                        case 10:
                        case 30:
                            return "-cu";
                        case 20:
                        case 50:
                        case 70:
                            return "-ci";
                        case 80:
                        case 40:
                        case 60:
                        case 90:
                            return "-cı";
                        case 0:
                            if (year % 1000 != 0)
                                return "-cü";
                            else
                                return "-ci";
                    }
                    break;
            }

            return String.Empty;
        }

        public static string GetOrgNameSufix(this string fullName)
        {
            string sufix = fullName.Substring(fullName.Length - 1, 1);
            if (sufix == "i")
                return String.Format(fullName + "-nin");
            else if (sufix == "s")
                return String.Format(fullName + "-in");
            else if (sufix == "İ")
                return String.Format(fullName + "-nin");
            else if (sufix == "u")
                return String.Format(fullName + "-nun");
            else if (sufix == "U")
                return String.Format(fullName + "-nun");
            else if (sufix == "ı")
                return String.Format(fullName + "-nın");
            else if (sufix == "A")
                return String.Format(fullName + "-nın");
            else if (sufix == "r")
                return String.Format(fullName + "-ın");
            else
                return String.Format(fullName + "-nin");

        }
        public static string GetOrgNameSufixNonSymbol(this string fullName)
        {
            string sufix = fullName.Substring(fullName.Length - 1, 1);
            if (sufix == "i")
                return String.Format(fullName + "nin");
            else if (sufix == "s")
                return String.Format(fullName + "in");
            else if (sufix == "İ")
                return String.Format(fullName + "nin");
            else if (sufix == "u")
                return String.Format(fullName + "nun");
            else if (sufix == "U")
                return String.Format(fullName + "nun");
            else if (sufix == "ı")
                return String.Format(fullName + "nın");
            else if (sufix == "A")
                return String.Format(fullName + "nın");
            else if (sufix == "r")
                return String.Format(fullName + "ın");
            else
                return String.Format(fullName + "nin");

        }

        public static string GetShortFullName(string fullName)
        {
            if (String.IsNullOrEmpty(fullName))
                return fullName;

            string[] st = fullName.ToUpper().Split(' ');

            fullName = st[1].Trim().Substring(0, 1);

            if (st[2] != null)
                fullName += ". " + st[2].Trim().Substring(0, 1);

            fullName += ". " + st[0].Trim();

            return fullName;
        }

        public static string GetShortFullName2(this string fullName)
        {
            if (String.IsNullOrEmpty(fullName))
                return fullName;

            string[] st = fullName.Split(' ');

            fullName = st[1].Trim().FirstCharToUpper() + " " + st[0].Trim().FirstCharToUpper();
            return fullName;
        }

        public static string GetSuffixSurname(this string surname)
        {
            if (String.IsNullOrEmpty(surname))
                return surname;

            surname = surname.Trim().FirstCharToUpper();

            string sufix = surname.ToLower().Substring(surname.Length - 1, 1);
            var result = surname;
            switch (sufix)
            {
                case "i": result += "yə"; break;
                case "e": result += "yə"; break;
                case "ə": result += "yə"; break;
                case "ö": result += "yə"; break;
                case "ü": result += "yə"; break;
                case "a": result += "ya"; break;
                case "ı": result += "ya"; break;
                case "u": result += "ya"; break;
                case "o": result += "ya"; break;
            };

            return result;
        }

        public static string ConvertMonthToString(int month)
        {

            string monthSt = "";
            switch (month.ToString())
            {
                case "1": monthSt = "Yanvar"; break;
                case "2": monthSt = "Fevral"; break;
                case "3": monthSt = "Mart"; break;
                case "4": monthSt = "Aprel"; break;
                case "5": monthSt = "May"; break;
                case "6": monthSt = "İyun"; break;
                case "7": monthSt = "İyul"; break;
                case "8": monthSt = "Avqust"; break;
                case "9": monthSt = "Sentyabr"; break;
                case "10": monthSt = "Oktyabr"; break;
                case "11": monthSt = "Noyabr"; break;
                case "12": monthSt = "Dekabr"; break;
            }



            return monthSt;
        }


        public static int ConvertStringToMonth(string monthName)
        {
            monthName = monthName.Trim().ToUpperInvariant();
#pragma warning disable CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive). For example, the pattern '""' is not covered.
            return monthName switch
            {
                "YANVAR" => 1,
                "FEVRAL" => 2,
                "MART" => 3,
                "APREL" => 4,
                "MAY" => 5,
                "İYUN" => 6,
                "IYUN" => 6,
                "İYUL" => 7,
                "IYUL" => 7,
                "AVQUST" => 8,
                "SENTYABR" => 9,
                "OKTYABR" => 10,
                "NOYABR" => 11,
                "DEKABR" => 12,
            };
#pragma warning restore CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive). For example, the pattern '""' is not covered.
        }

        public static string GetTidyBranch(this string branch)
        {
            if (String.IsNullOrEmpty(branch))
                return branch;

            branch = branch.Trim().FirstCharToUpper();
            string[] st = branch.Split(' ');

            //foreach (string item in st)
            //{
            //    if (item.Length>1 & item[0] == '\"')
            //    {
            //        char.ToUpper(item[1]);
            //    }
            //}

            switch (st[st.Length - 1])
            {
                case "mmc":
                case "asc":
                case "qsc":
                    {
                        st[st.Length - 1] = st[st.Length - 1].ToUpper();
                        return string.Join(" ", st).GetOrgNameSufix();
                    }
            }

            if (st.Length > 3)
            {
                for (int i = st.Length - 1; i > st.Length - 4; i--)
                {

                    if (st[i] == "məhdud" || st[i].Contains("cəmiyyət") || st[i] == "məsuliyyətli" || st[i] == "səhmdar" || st[i] == "açıq" || st[i] == "qapalı")
                    {
                        st[i] = st[i].FirstCharToUpper();
                    }
                }
            }

            return string.Join(" ", st).GetOrgNameSufix();

        }

        public static string GetPatronymicSuffix(string genderName)
        {
            genderName = genderName.ToLower();

#pragma warning disable CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive). For example, the pattern '""' is not covered.
            return genderName switch
            {
                "male" => " oğlu",
                "female" => " qızı",
            };
#pragma warning restore CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive). For example, the pattern '""' is not covered.
        }

        public static string GetPatronymicPossessiveCaseSuffix(string genderName)
        {
            genderName = genderName.ToLower();

#pragma warning disable CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive). For example, the pattern '""' is not covered.
            return genderName switch
            {
                "male" => " oğlunun",
                "female" => " qızının",
            };
#pragma warning restore CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive). For example, the pattern '""' is not covered.
        }
        /// <summary>
        /// Methodlarin adlarin yaxwi olar eng dilinde yazarsiz) men overload etmiwem
        /// </summary>
        /// <param name="patronymic"></param>
        /// <param name="genderName"></param>
        /// <returns></returns>
        public static string GetPatronymicPossessiveCaseSuffix(this string patronymic, string genderName)
        {
            genderName = genderName.ToLower();

            patronymic = patronymic.Trim();
            int startIndex = patronymic.Length - 5;
            if (startIndex > 0)
            {

                string forCheckFullName = patronymic.Substring(startIndex).ToLower();
                if (forCheckFullName == " oğlu" || forCheckFullName == " qızı")
                    patronymic = patronymic.Substring(0, patronymic.Length - 4);
            }
#pragma warning disable CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive). For example, the pattern '""' is not covered.
            return genderName switch
            {
                "male" => patronymic.FirstCharToUpper() + "oğlunun",
                "female" => patronymic.FirstCharToUpper() + "qızının",
            };
#pragma warning restore CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive). For example, the pattern '""' is not covered.
        }

        public static string GetFullNamePossessiveCaseSuffix(this string fullname, string genderName)
        {
            genderName = genderName.ToLower();
            fullname = fullname.Trim();
            int startIndex = fullname.Length - 5;
            if (startIndex > 0)
            {

                string forCheckFullName = fullname.Substring(startIndex).ToLower();
                if (forCheckFullName == " oğlu" || forCheckFullName == " qızı")
                    fullname = fullname.Substring(0, fullname.Length - 4);
            }
#pragma warning disable CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive). For example, the pattern '""' is not covered.
            return genderName switch
            {
                "male" => fullname.Trim() + " oğlunun",
                "female" => fullname.Trim() + " qızının",
            };
#pragma warning restore CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive). For example, the pattern '""' is not covered.
        }

        public static string GetPatronymicSuffix2(this string fullname, string genderName)
        {
            genderName = genderName.ToLower();
            fullname = fullname.Trim();
            int startIndex = fullname.Length - 5;
            if (startIndex > 0)
            {

                string forCheckFullName = fullname.Substring(startIndex).ToLower();
                if (forCheckFullName == " oğlu" || forCheckFullName == " qızı")
                    fullname = fullname.Substring(0, fullname.Length - 4);
            }
#pragma warning disable CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive). For example, the pattern '""' is not covered.
            return genderName switch
            {
                "male" => fullname.Trim() + " oğlu",
                "female" => fullname.Trim() + " qızı",
            };
#pragma warning restore CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive). For example, the pattern '""' is not covered.
        }

        public static string GetPatronymicSuffixForFullname(this string fullname)
        {
            fullname = fullname.Trim();
            string forCheckFullName = "";
            int startIndex = fullname.Length - 5;
            if (startIndex > 0)
            {
                forCheckFullName = fullname.Substring(startIndex).ToLower();
                if (forCheckFullName == " oğlu" || forCheckFullName == " qızı")
                    fullname = fullname.Substring(0, fullname.Length - 4);

                if (forCheckFullName == " oğlu")
                    forCheckFullName = " oğlunun";
                else if (forCheckFullName == " qızı")
                    forCheckFullName = " qızının";
            }
#pragma warning disable CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive). For example, the pattern '""' is not covered.
            return fullname + forCheckFullName;
#pragma warning restore CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive). For example, the pattern '""' is not covered.
        }


    }
}

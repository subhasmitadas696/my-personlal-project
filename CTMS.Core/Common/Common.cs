using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace CTMS.Core
{
    public static class  Common
    {
        #region Stopwatch timer
        /// <summary>
        /// Timer starts
        /// </summary>
        /// <returns></returns>
        public static Stopwatch TimerStart()
        {
            Stopwatch watch = new Stopwatch();
            watch.Reset();
            watch.Start();
            return watch;
        }
        /// <summary>
        /// Timer Over.
        /// </summary>
        /// <param name="watch"></param>
        /// <returns></returns>
        public static string TimerEnd(Stopwatch watch)
        {
            watch.Stop();
            double costtime = watch.ElapsedMilliseconds;
            return costtime.ToString();
        }
        #endregion

        #region Delete Duplication in an array Item
        /// <summary>
        /// Delete Duplication in an array Item
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string[] RemoveDup(string[] values)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < values.Length; i++)//Traversing array members
            {
                if (!list.Contains(values[i]))
                {
                    list.Add(values[i]);
                }
            }
            return list.ToArray();
        }
        #endregion

        #region Automatically generate Number
        /// <summary>
        /// Represents a globally unique identifier (GUID).
        /// </summary>
        /// <returns></returns>
        public static string GuId()
        {
            return Guid.NewGuid().ToString();
        }
        /// <summary>
        /// Automatically generate Number  201008251145409865
        /// </summary>
        /// <returns></returns>
        public static string CreateNo()
        {
            Random randomNo = new Random();
            string strRandom = randomNo.Next(1000, 10000).ToString(); //Generate Number 
            string code = DateTime.Now.ToString("yyyyMMddHHmmss") + strRandom;//Shaped like
            return code;
        }
        #endregion

        #region Generate 0-9 random numbers
        /// <summary>
        /// Generate 0-9 random numbers
        /// </summary>
        /// <param name="codeNum">Generating length</param>
        /// <returns></returns>
        public static string RndNum(int codeNum)
        {

            StringBuilder sb = new StringBuilder(codeNum);
            Random rand = new Random();
            for (int i = 1; i < codeNum + 1; i++)
            {
                int t = rand.Next(9);
                sb.AppendFormat("{0}", t);
            }
            return sb.ToString();

        }
        #endregion
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        #region Delete The character after the last character
        /// <summary>
        /// Delete The last ending of a tease No.
        /// </summary>
        public static string DelLastComma(string str)
        {
            return str.Substring(0, str.LastIndexOf(","));
        }
        /// <summary>
        /// Delete The character after the specified character at the end
        /// </summary>
        public static string DelLastChar(string str, string strchar)
        {
            return str.Substring(0, str.LastIndexOf(strchar));
        }
        /// <summary>
        /// Delete Last end length
        /// </summary>
        /// <param name="str"></param>
        /// <param name="Length"></param>
        /// <returns></returns>
        public static string DelLastLength(string str, int Length)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            str = str.Substring(0, str.Length - Length);
            return str;
        }
        #endregion


        //************************************* Server side Control Validation function ***************************************


        static string str = string.Empty;

        public static string check(string Ctrltxt, string strMand, string ctlname, int intSize, string allowchar = "")
        {

            string strMessage = "pass";
            try
            {
                if (strMand == "M")
                {
                    if (Ctrltxt == "")
                    {
                        strMessage = ctlname + " can not be left blank";
                        return strMessage;
                    }
                }
                if (Ctrltxt != "")
                {
                    string FirststChar = Ctrltxt.Substring(0, 1);
                    if (FirststChar == " ")
                    {
                        strMessage = ctlname + " does not allow White Space(s) in first place";
                        return strMessage;
                    }

                    else if (FirststChar == "=" || FirststChar == "," || FirststChar == "-" || FirststChar == "." || FirststChar == "\\" || FirststChar == "(" || FirststChar == ")" || FirststChar == "/" || FirststChar == " " || FirststChar == "_" || FirststChar == ":")
                    {
                        strMessage = ctlname + " does not allow " + FirststChar + " in first place";
                        return strMessage;
                    }
                    else if (Ctrltxt.Substring(Ctrltxt.Length - 1, 1) == " ")
                    {
                        strMessage = ctlname + " " + "does not allow White Space(s) in last place";
                        return strMessage;
                    }
                    else if (allowchar != "")
                    {

                        string spcialchar = "!@#$%^&*()_+=-{}[]';:|\\?/>.<,~|";
                        char[] allowcharecter = allowchar.ToCharArray();
                        foreach (char c in allowcharecter)
                        {
                            int index = spcialchar.IndexOf(c);
                            if (index != -1)
                            {
                                spcialchar = spcialchar.Remove(index, 1);
                            }

                        }
                        char[] specialchararray = spcialchar.ToCharArray();
                        foreach (char c in specialchararray)
                        {
                            if (Ctrltxt.Contains(c.ToString()))
                            {
                                strMessage = ctlname + " does not allow " + c.ToString() + "";
                                return strMessage;
                            }
                        }

                    }
                    else if (allowchar == "")
                    {

                        string spcialchar = "!@#$%^&*()_+=-{}[]';:|\\?/>.<,~|";
                        char[] specialchararray = spcialchar.ToCharArray();
                        foreach (char c in specialchararray)
                        {
                            if (Ctrltxt.Contains(c.ToString()))
                            {
                                strMessage = ctlname + " does not allow " + c.ToString() + "";
                                return strMessage;
                            }
                        }
                    }
                    else if (Ctrltxt.Length > intSize)
                    {
                        strMessage = ctlname + " does not allow more than " + intSize + " characters";
                        return strMessage;
                    }

                    else
                    {
                        return strMessage;
                    }
                }

            }
            catch
            {
                strMessage = "Fail";
            }
            return strMessage;

        }

        public static string ValidateTextbox_First_Char(string ctlValue, string lbltext)
        {
            try
            {
                if (ctlValue != "")
                {
                    if (ctlValue.Substring(0, 1) == " ")
                    {
                        str = "Does not allow White Space(s) in first place of " + lbltext + " !";
                        
                    }
                    else if (ctlValue.Substring(0, 1) == "!" || ctlValue.Substring(0, 1) == "@" || ctlValue.Substring(0, 1) == "#" || ctlValue.Substring(0, 1) == "$" || ctlValue.Substring(0, 1) == "%" || ctlValue.Substring(0, 1) == "^" || ctlValue.Substring(0, 1) == "&" || ctlValue.Substring(0, 1) == "*" || ctlValue.Substring(0, 1) == "(" || ctlValue.Substring(0, 1) == ")" || ctlValue.Substring(0, 1) == "-" || ctlValue.Substring(0, 1) == "_" || ctlValue.Substring(0, 1) == "+" || ctlValue.Substring(0, 1) == "=" || ctlValue.Substring(0, 1) == "{" || ctlValue.Substring(0, 1) == "}" || ctlValue.Substring(0, 1) == "[" || ctlValue.Substring(0, 1) == "]" || ctlValue.Substring(0, 1) == "|" || ctlValue.Substring(0, 1) == ";" || ctlValue.Substring(0, 1) == ":" || ctlValue.Substring(0, 1) == "<" || ctlValue.Substring(0, 1) == ">" || ctlValue.Substring(0, 1) == "?" || ctlValue.Substring(0, 1) == "." || ctlValue.Substring(0, 1) == "," || ctlValue.Substring(0, 1) == "/" || ctlValue.Substring(0, 1) == "\\" || ctlValue.Substring(0, 1) == "~" || ctlValue.Substring(0, 1) == "" || ctlValue.Substring(0, 1) == "\"" || ctlValue.Substring(0, 1) == "\'")
                    {
                        str = "Does not allow Special Characters in first place of " + lbltext + " !";
                        
                    }
                    else if (ctlValue.Substring(ctlValue.Length - 1, 1) == " ")
                    {
                        str = "Does not allow White Space(s) in last place of " + lbltext + " !";
                        
                    }
                    else if ((ctlValue.Contains('!')) || (ctlValue.Contains('%')) || (ctlValue.Contains('<')) || (ctlValue.Contains('>')) || (ctlValue.Contains('=')) || (ctlValue.Contains('\"')) || (ctlValue.Contains('\'')))
                    {
                        str = "Does not allow !,%,<,>,=,\", and single quote characters in " + lbltext + " !";
                         
                    }
                    else
                    {
                        str = "PASS";
                    }
                }
                else
                {
                    str = "PASS";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return str;
        }


        public static string ValidateCKEditor(string ctlValue, string ctlname)
        {
            try
            {

                
                if ((string.IsNullOrEmpty(ctlValue)))
                {
                    str = ctlname + " " + "can not be left blank";
                }
                else if (ctlValue.Substring(0, 1) == " ")
                {
                    str = ctlname + " " + "White Space(s) not allowed in first place";

                }

                else if (ctlValue.Substring(ctlValue.Length - 1, 1) == " ")
                {
                    str = ctlname + " " + "does not allow White Space(s) in last place";

                }

                else
                {
                    str = "PASS";
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return str;
        }


        public static string ValidateCKEditor_NonMandatory(string ctlValue, string ctlname)
        {
            try
            {

                
                if (ctlValue.Substring(0, 1) == " ")
                {
                    str = ctlname + " " + "White Space(s) not allowed in first place";

                }

                else if (ctlValue.Substring(ctlValue.Length - 1, 1) == " ")
                {
                    str = ctlname + " " + "does not allow White Space(s) in last place";

                }

                else
                {
                    str = "PASS";
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return str;
        }

        public static string ValidateTextbox_Mandatory_Numbers(string ctlValue, string ctlname, int sz, int minSize = 0)
        {
            try
            {
                int n;

                bool isNumeric = int.TryParse(ctlValue, out n);


                if ((string.IsNullOrEmpty(ctlValue)))
                {
                    str = ctlname + " " + "can not be left blank";

                }
                else if (ctlValue.Substring(0, 1) == " ")
                {
                    str = ctlname + " " + "White Space(s) not allowed in first place";

                }
                else if (ctlValue.Substring(0, 1) == "!" || ctlValue.Substring(0, 1) == "@" || ctlValue.Substring(0, 1) == "#" || ctlValue.Substring(0, 1) == "$" || ctlValue.Substring(0, 1) == "%" || ctlValue.Substring(0, 1) == "^" || ctlValue.Substring(0, 1) == "&" || ctlValue.Substring(0, 1) == "*" || ctlValue.Substring(0, 1) == "(" || ctlValue.Substring(0, 1) == ")" || ctlValue.Substring(0, 1) == "-" || ctlValue.Substring(0, 1) == "_" || ctlValue.Substring(0, 1) == "+" || ctlValue.Substring(0, 1) == "=" || ctlValue.Substring(0, 1) == "{" || ctlValue.Substring(0, 1) == "}" || ctlValue.Substring(0, 1) == "[" || ctlValue.Substring(0, 1) == "]" || ctlValue.Substring(0, 1) == "|" || ctlValue.Substring(0, 1) == ";" || ctlValue.Substring(0, 1) == ":" || ctlValue.Substring(0, 1) == "<" || ctlValue.Substring(0, 1) == ">" || ctlValue.Substring(0, 1) == "?" || ctlValue.Substring(0, 1) == "." || ctlValue.Substring(0, 1) == "," || ctlValue.Substring(0, 1) == "/" || ctlValue.Substring(0, 1) == "\\" || ctlValue.Substring(0, 1) == "~" || ctlValue.Substring(0, 1) == "" || ctlValue.Substring(0, 1) == "\"" || ctlValue.Substring(0, 1) == "\'" || ctlValue.Substring(0, 1) == "&")
                {
                    str = ctlname + " " + "does not allow Special Characters in first place";
                    

                }
                else if (ctlValue.Substring(ctlValue.Length - 1, 1) == " ")
                {
                    str = ctlname + " " + "does not allow White Space(s) in last place";

                }
                else if ((ctlValue == "'"))
                {
                    str = ctlname + " " + "does not allow Single Quote";
                     

                }

                else if ((ctlValue.Contains('\'')) || (ctlValue.Contains('!')) || (ctlValue.Contains('$')) || (ctlValue.Contains('%')) || (ctlValue.Contains('^')) || (ctlValue.Contains('*')) || (ctlValue.Contains('<')) || (ctlValue.Contains('>')) || (ctlValue.Contains('~')) || (ctlValue.Contains('+')) || (ctlValue.Contains('=')) || (ctlValue.Contains('{')) || (ctlValue.Contains('}')) || (ctlValue.Contains('[')) || (ctlValue.Contains(']')) || (ctlValue.Contains('|')) || (ctlValue.Contains(';')) || (ctlValue.Contains('`')) || (ctlValue.Contains('-')) || (ctlValue.Contains('_')) || (ctlValue.Contains('"')) || (ctlValue.Contains('\\')) || (ctlValue.Contains('/')) || (ctlValue.Contains('&')))
                {
                    str = ctlname + " " + "does not allow Special characters";
                    

                }
                else if ((minSize > 0) && ctlValue.Length < minSize)
                {
                    int cnt = 0;
                    cnt = ctlValue.Length;
                    if ((cnt < minSize))
                    {
                        str = ctlname + " " + "allows minimum" + " " + minSize.ToString() + " " + "character(s)";

                    }
                }
                else if ((sz > 0) && ctlValue.Length > sz)
                {
                    int cnt = 0;
                    cnt = ctlValue.Length;
                    if ((cnt > sz))
                    {
                        str = ctlname + " " + "allows Maximum" + " " + sz.ToString() + " " + "character(s)";

                    }
                }


                else if (ctlValue.Contains('.'))
                {
                    decimal value;
                    if (!Decimal.TryParse(ctlValue, out value))
                    {
                        str = ctlname + " " + "is not a numeric value";

                    }
                    else
                    {
                        str = "PASS";
                    }
                }
                else if (isNumeric == false)
                {
                    str = ctlname + " " + "is not a numeric value";


                }

                else
                {
                    str = "PASS";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return str;
        }
        public static string ValidateTextbox_NonMandatory_Numbers(string ctlValue, string ctlname, int sz, int minSize = 0)
        {
            try
            {
                int n;




                if (ctlValue != "")
                {
                    bool isNumeric = int.TryParse(ctlValue, out n);
                    if (ctlValue.Substring(0, 1) == " ")
                    {
                        str = ctlname + " " + "White Space(s) not allowed in first place";

                    }
                    else if (ctlValue.Substring(0, 1) == "!" || ctlValue.Substring(0, 1) == "@" || ctlValue.Substring(0, 1) == "#" || ctlValue.Substring(0, 1) == "$" || ctlValue.Substring(0, 1) == "%" || ctlValue.Substring(0, 1) == "^" || ctlValue.Substring(0, 1) == "&" || ctlValue.Substring(0, 1) == "*" || ctlValue.Substring(0, 1) == "(" || ctlValue.Substring(0, 1) == ")" || ctlValue.Substring(0, 1) == "-" || ctlValue.Substring(0, 1) == "_" || ctlValue.Substring(0, 1) == "+" || ctlValue.Substring(0, 1) == "=" || ctlValue.Substring(0, 1) == "{" || ctlValue.Substring(0, 1) == "}" || ctlValue.Substring(0, 1) == "[" || ctlValue.Substring(0, 1) == "]" || ctlValue.Substring(0, 1) == "|" || ctlValue.Substring(0, 1) == ";" || ctlValue.Substring(0, 1) == ":" || ctlValue.Substring(0, 1) == "<" || ctlValue.Substring(0, 1) == ">" || ctlValue.Substring(0, 1) == "?" || ctlValue.Substring(0, 1) == "." || ctlValue.Substring(0, 1) == "," || ctlValue.Substring(0, 1) == "/" || ctlValue.Substring(0, 1) == "\\" || ctlValue.Substring(0, 1) == "~" || ctlValue.Substring(0, 1) == "" || ctlValue.Substring(0, 1) == "\"" || ctlValue.Substring(0, 1) == "\'" || ctlValue.Substring(0, 1) == "&")
                    {
                        str = ctlname + " " + "does not allow Special Characters in first place";
                        

                    }
                    else if (ctlValue.Substring(ctlValue.Length - 1, 1) == " ")
                    {
                        str = ctlname + " " + "does not allow White Space(s) in last place";

                    }
                    else if ((ctlValue == "'"))
                    {
                        str = ctlname + " " + "does not allow Single Quote";
                         

                    }

                    else if ((ctlValue.Contains('\'')) || (ctlValue.Contains('!')) || (ctlValue.Contains('$')) || (ctlValue.Contains('%')) || (ctlValue.Contains('^')) || (ctlValue.Contains('*')) || (ctlValue.Contains('<')) || (ctlValue.Contains('>')) || (ctlValue.Contains('~')) || (ctlValue.Contains('+')) || (ctlValue.Contains('=')) || (ctlValue.Contains('{')) || (ctlValue.Contains('}')) || (ctlValue.Contains('[')) || (ctlValue.Contains(']')) || (ctlValue.Contains('|')) || (ctlValue.Contains(';')) || (ctlValue.Contains('`')) || (ctlValue.Contains('-')) || (ctlValue.Contains('_')) || (ctlValue.Contains('"')) || (ctlValue.Contains('\\')) || (ctlValue.Contains('/')) || (ctlValue.Contains('&')))
                    {
                        str = ctlname + " " + "does not allow Special characters";
                        

                    }
                    else if ((minSize > 0) && ctlValue.Length < minSize)
                    {
                        int cnt = 0;
                        cnt = ctlValue.Length;
                        if ((cnt < minSize))
                        {
                            str = ctlname + " " + "allows minimum" + " " + minSize.ToString() + " " + "character(s)";

                        }
                    }
                    else if ((sz > 0) && ctlValue.Length > sz)
                    {
                        int cnt = 0;
                        cnt = ctlValue.Length;
                        if ((cnt > sz))
                        {
                            str = ctlname + " " + "allows Maximum" + " " + sz.ToString() + " " + "character(s)";

                        }
                    }


                    else if (ctlValue.Contains('.'))
                    {
                        decimal value;
                        if (!Decimal.TryParse(ctlValue, out value))
                        {
                            str = ctlname + " " + "is not a numeric value";

                        }
                        else
                        {
                            str = "PASS";
                        }
                    }
                    else if (isNumeric == false)
                    {
                        str = ctlname + " " + "is not a numeric value";


                    }

                    else
                    {
                        str = "PASS";
                    }

                }
                else
                {
                    str = "PASS";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return str;
        }
        public static string ValidateTextbox_NonMandatory_AlphaNumeric(string ctlValue, string ctlname, int sz, int minSize = 0)
        {
            try
            {
                if (ctlValue != "")
                {
                    if (ctlValue.Substring(0, 1) == " ")
                    {
                        str = ctlname + " " + "does not allow White Space(s) in first place";

                    }
                    else if (ctlValue.Substring(0, 1) == "!" || ctlValue.Substring(0, 1) == "@" || ctlValue.Substring(0, 1) == "#" || ctlValue.Substring(0, 1) == "$" || ctlValue.Substring(0, 1) == "%" || ctlValue.Substring(0, 1) == "^" || ctlValue.Substring(0, 1) == "&" || ctlValue.Substring(0, 1) == "*" || ctlValue.Substring(0, 1) == "(" || ctlValue.Substring(0, 1) == ")" || ctlValue.Substring(0, 1) == "-" || ctlValue.Substring(0, 1) == "_" || ctlValue.Substring(0, 1) == "+" || ctlValue.Substring(0, 1) == "=" || ctlValue.Substring(0, 1) == "{" || ctlValue.Substring(0, 1) == "}" || ctlValue.Substring(0, 1) == "[" || ctlValue.Substring(0, 1) == "]" || ctlValue.Substring(0, 1) == "|" || ctlValue.Substring(0, 1) == ";" || ctlValue.Substring(0, 1) == ":" || ctlValue.Substring(0, 1) == "<" || ctlValue.Substring(0, 1) == ">" || ctlValue.Substring(0, 1) == "?" || ctlValue.Substring(0, 1) == "." || ctlValue.Substring(0, 1) == "," || ctlValue.Substring(0, 1) == "/" || ctlValue.Substring(0, 1) == "\\" || ctlValue.Substring(0, 1) == "~" || ctlValue.Substring(0, 1) == "" || ctlValue.Substring(0, 1) == "\"" || ctlValue.Substring(0, 1) == "\'" || ctlValue.Substring(0, 1) == "&")
                    {
                        str = ctlname + " " + "does not allow Special Characters in first place";
                        

                    }
                    else if (ctlValue.Substring(ctlValue.Length - 1, 1) == " ")
                    {
                        str = ctlname + " " + "does not allow White Space(s) in last place";

                    }
                    else if ((ctlValue == "'"))
                    {
                        str = ctlname + " " + "does not allow Single Quote ";
                         

                    }

                    else if ((ctlValue.Contains('!')) || (ctlValue.Contains('%')) || (ctlValue.Contains('<')) || (ctlValue.Contains('>')) || (ctlValue.Contains('=')) || (ctlValue.Contains("'")))
                    {
                        str = ctlname + " " + "does not allow Special character ";
                        

                    }
                    else if ((minSize > 0) && ctlValue.Length < minSize)
                    {
                        int cnt = 0;
                        cnt = ctlValue.Length;
                        if ((cnt < minSize))
                        {
                            str = ctlname + " " + "allows minimum" + " " + minSize.ToString() + " " + "character(s)";

                        }
                    }
                    else if ((sz > 0) && ctlValue.Length > sz)
                    {
                        int cnt = 0;
                        cnt = ctlValue.Length;
                        if ((cnt > sz))
                        {
                            str = ctlname + " " + "allows Maximum" + " " + sz.ToString() + " " + "character(s)";

                        }
                    }

                    else
                    {
                        str = "PASS";
                    }
                }
                else
                {
                    str = "PASS";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return str;
        }
        public static string ValidateTextbox_Mandatory_Alphabets(string ctlValue, string ctlname, int sz, int minSize = 0)
        {
            try
            {
                if ((string.IsNullOrEmpty(ctlValue)))
                {
                    str = ctlname + " can not be left blank";

                }
                else if (ctlValue.Substring(0, 1) == " ")
                {
                    str = ctlname + " does not allow White Space(s) in first place";

                }
                else if (ctlValue.Substring(0, 1) == "!" || ctlValue.Substring(0, 1) == "@" || ctlValue.Substring(0, 1) == "#" || ctlValue.Substring(0, 1) == "$" || ctlValue.Substring(0, 1) == "%" || ctlValue.Substring(0, 1) == "^" || ctlValue.Substring(0, 1) == "&" || ctlValue.Substring(0, 1) == "*" || ctlValue.Substring(0, 1) == "(" || ctlValue.Substring(0, 1) == ")" || ctlValue.Substring(0, 1) == "-" || ctlValue.Substring(0, 1) == "_" || ctlValue.Substring(0, 1) == "+" || ctlValue.Substring(0, 1) == "=" || ctlValue.Substring(0, 1) == "{" || ctlValue.Substring(0, 1) == "}" || ctlValue.Substring(0, 1) == "[" || ctlValue.Substring(0, 1) == "]" || ctlValue.Substring(0, 1) == "|" || ctlValue.Substring(0, 1) == ";" || ctlValue.Substring(0, 1) == ":" || ctlValue.Substring(0, 1) == "<" || ctlValue.Substring(0, 1) == ">" || ctlValue.Substring(0, 1) == "?" || ctlValue.Substring(0, 1) == "." || ctlValue.Substring(0, 1) == "," || ctlValue.Substring(0, 1) == "/" || ctlValue.Substring(0, 1) == "\\" || ctlValue.Substring(0, 1) == "~" || ctlValue.Substring(0, 1) == "" || ctlValue.Substring(0, 1) == "\"" || ctlValue.Substring(0, 1) == "\'" || ctlValue.Substring(0, 1) == "&")
                {
                    str = ctlname + " does not allow Special Characters in first place";
                    

                }
                else if (ctlValue.Substring(ctlValue.Length - 1, 1) == " ")
                {
                    str = ctlname + " does not allow White Space(s) in last place";

                }
                else if ((ctlValue == "'"))
                {
                    str = ctlname + " does not allow Single Quote";
                     

                }

                else if ((ctlValue.Contains('!')) || (ctlValue.Contains('%')) || (ctlValue.Contains('<')) || (ctlValue.Contains('>')) || (ctlValue.Contains('=')) || (ctlValue.Contains("'")))
                {
                    str = ctlname + " does not allow Special characters";
                    

                }
                else if (ctlValue.ToString().Contains('1') || ctlValue.ToString().Contains('2') || ctlValue.ToString().Contains('3') || ctlValue.ToString().Contains('4') || ctlValue.ToString().Contains('5') || ctlValue.ToString().Contains('6') || ctlValue.ToString().Contains('7') || ctlValue.ToString().Contains('8') || ctlValue.ToString().Contains('9') || ctlValue.ToString().Contains('0'))
                {
                    str = ctlname + " does not allow Numbers";

                }
                else if ((minSize > 0) && ctlValue.Length < minSize)
                {
                    int cnt = 0;
                    cnt = ctlValue.Length;
                    if ((cnt < minSize))
                    {
                        str = ctlname + " allows minimum " + minSize.ToString() + " character(s)";

                    }
                }
                else if ((sz > 0) && ctlValue.Length > sz)
                {
                    int cnt = 0;
                    cnt = ctlValue.Length;
                    if ((cnt > sz))
                    {
                        str = ctlname + " allows Maximum " + sz.ToString() + " character(s)";

                    }

                }

                else
                {
                    str = "PASS";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return str;
        }
        public static string ValidateTextbox_NonMandatory_Alphabets(string ctlValue, string ctlname, int sz, int minSize = 0)
        {
            try
            {
                if (ctlValue != "")
                {
                    if (ctlValue.Substring(0, 1) == " ")
                    {
                        str = ctlname + " does not allow White Space(s) in first place";

                    }
                    else if (ctlValue.Substring(0, 1) == "!" || ctlValue.Substring(0, 1) == "@" || ctlValue.Substring(0, 1) == "#" || ctlValue.Substring(0, 1) == "$" || ctlValue.Substring(0, 1) == "%" || ctlValue.Substring(0, 1) == "^" || ctlValue.Substring(0, 1) == "&" || ctlValue.Substring(0, 1) == "*" || ctlValue.Substring(0, 1) == "(" || ctlValue.Substring(0, 1) == ")" || ctlValue.Substring(0, 1) == "-" || ctlValue.Substring(0, 1) == "_" || ctlValue.Substring(0, 1) == "+" || ctlValue.Substring(0, 1) == "=" || ctlValue.Substring(0, 1) == "{" || ctlValue.Substring(0, 1) == "}" || ctlValue.Substring(0, 1) == "[" || ctlValue.Substring(0, 1) == "]" || ctlValue.Substring(0, 1) == "|" || ctlValue.Substring(0, 1) == ";" || ctlValue.Substring(0, 1) == ":" || ctlValue.Substring(0, 1) == "<" || ctlValue.Substring(0, 1) == ">" || ctlValue.Substring(0, 1) == "?" || ctlValue.Substring(0, 1) == "." || ctlValue.Substring(0, 1) == "," || ctlValue.Substring(0, 1) == "/" || ctlValue.Substring(0, 1) == "\\" || ctlValue.Substring(0, 1) == "~" || ctlValue.Substring(0, 1) == "" || ctlValue.Substring(0, 1) == "\"" || ctlValue.Substring(0, 1) == "\'" || ctlValue.Substring(0, 1) == "&")
                    {
                        str = ctlname + " does not allow Special Characters in first place";
                        

                    }
                    else if (ctlValue.Substring(ctlValue.Length - 1, 1) == " ")
                    {
                        str = ctlname + " does not allow White Space(s) in last place";

                    }
                    else if ((ctlValue == "'"))
                    {
                        str = ctlname + " does not allow Single Quote";
                         

                    }

                    else if ((ctlValue.Contains('!')) || (ctlValue.Contains('%')) || (ctlValue.Contains('<')) || (ctlValue.Contains('>')) || (ctlValue.Contains('=')) || (ctlValue.Contains("'")))
                    {
                        str = ctlname + " does not allow Special characters";
                        

                    }
                    else if (ctlValue.ToString().Contains('1') || ctlValue.ToString().Contains('2') || ctlValue.ToString().Contains('3') || ctlValue.ToString().Contains('4') || ctlValue.ToString().Contains('5') || ctlValue.ToString().Contains('6') || ctlValue.ToString().Contains('7') || ctlValue.ToString().Contains('8') || ctlValue.ToString().Contains('9') || ctlValue.ToString().Contains('0'))
                    {
                        str = ctlname + " does not allow Numbers";

                    }
                    else if ((minSize > 0) && ctlValue.Length < minSize)
                    {
                        int cnt = 0;
                        cnt = ctlValue.Length;
                        if ((cnt < minSize))
                        {
                            str = ctlname + " allows minimum " + minSize.ToString() + " character(s)";

                        }
                    }
                    else if ((sz > 0) && ctlValue.Length > sz)
                    {
                        int cnt = 0;
                        cnt = ctlValue.Length;
                        if ((cnt > sz))
                        {
                            str = ctlname + " allows Maximum " + sz.ToString() + " character(s)";

                        }

                    }

                    else
                    {
                        str = "PASS";
                    }
                }
                else
                {
                    str = "PASS";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return str;
        }
        public static string ValidateMobile(string ctlValue, string ctlname, string strMandate)
        {
            try
            {
                long n;


                if (strMandate.ToUpper() == "YES")
                {
                    if ((string.IsNullOrEmpty(ctlValue)))
                    {
                        str = ctlname + " can not be left blank";

                    }
                    else if ((ctlValue.Length < 10))
                    {

                        str = "Mobile Number should not be less than 10 digits";


                    }
                    else if (long.TryParse(ctlValue, out n) == false)
                    {
                        str = ctlname + " is not a numeric value";


                    }
                    else
                    {
                        str = "PASS";
                    }
                }
                else
                {
                    if (ctlValue != "")
                    {
                        if ((ctlValue.Length < 10))
                        {

                            str = "Mobile Number should not be less than 10 digits";


                        }
                        else if (long.TryParse(ctlValue, out n) == false)
                        {
                            str = ctlname + " is not a numeric value";


                        }
                        else
                        {
                            str = "PASS";
                        }
                    }
                    else
                    {
                        str = "PASS";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return str;
        }
        public static string ValidateEmailTextBox(string ctlValue, string ctlname, int sz)
        {
            try
            {
                if (ctlValue != "")
                {
                    
                    if (ctlValue.Substring(0, 1) == "!" || ctlValue.Substring(0, 1) == "@" || ctlValue.Substring(0, 1) == "#" || ctlValue.Substring(0, 1) == "$" || ctlValue.Substring(0, 1) == "%" || ctlValue.Substring(0, 1) == "^" || ctlValue.Substring(0, 1) == "&" || ctlValue.Substring(0, 1) == "*" || ctlValue.Substring(0, 1) == "(" || ctlValue.Substring(0, 1) == ")" || ctlValue.Substring(0, 1) == "-" || ctlValue.Substring(0, 1) == "_" || ctlValue.Substring(0, 1) == "+_" || ctlValue.Substring(0, 1) == "=" || ctlValue.Substring(0, 1) == "{" || ctlValue.Substring(0, 1) == "}" || ctlValue.Substring(0, 1) == "[" || ctlValue.Substring(0, 1) == "]" || ctlValue.Substring(0, 1) == "|" || ctlValue.Substring(0, 1) == ";" || ctlValue.Substring(0, 1) == ":" || ctlValue.Substring(0, 1) == "<" || ctlValue.Substring(0, 1) == ">" || ctlValue.Substring(0, 1) == "?" || ctlValue.Substring(0, 1) == "." || ctlValue.Substring(0, 1) == ",")
                    {
                        str = ctlname + " Special characters not allowed in first place";

                    }
                    
                    else if ((ctlValue == "'"))
                    {
                        str = ctlname + " Single Quote not allowed";

                    }

                    else if ((ctlValue.Contains('\'')) || (ctlValue.Contains('!')) || (ctlValue.Contains('%')) || (ctlValue.Contains('^')) || (ctlValue.Contains('&')) || (ctlValue.Contains('*')) || (ctlValue.Contains('(')) || (ctlValue.Contains(')')) || (ctlValue.Contains('<')) || (ctlValue.Contains('>')) || (ctlValue.Contains('~')) || (ctlValue.Contains(')')) || (ctlValue.Contains('-')) || (ctlValue.Contains('+')) || (ctlValue.Contains('=')) || (ctlValue.Contains('{')) || (ctlValue.Contains('}')) || (ctlValue.Contains(']')) || (ctlValue.Contains('[')) || (ctlValue.Contains('|')) || (ctlValue.Contains(';')) || (ctlValue.Contains(':')) || (ctlValue.Contains('?')) || (ctlValue.Contains(',')) || (ctlValue.Contains('/')) || (ctlValue.Contains('\\')) || (ctlValue.Contains('"')) || (ctlValue.Contains('`')) || (ctlValue.Contains('^')) || (ctlValue.Contains('~')) || (ctlValue.Contains('&')))
                    {
                        str = ctlname + " Special character not allowed";

                    }
                    else if (ctlValue.Contains("@") == false)
                    {
                        str = ctlname + " is not valid";

                    }
                    else if ((sz > 0) && ctlValue.Length > sz)
                    {
                        int cnt = 0;
                        cnt = ctlValue.Length;
                        if ((cnt > sz))
                        {
                            str = ctlname + "Maximum" + " " + sz + " character(s) allowed";

                        }
                    }
                    else
                    {
                        str = "PASS";
                    }
                }
                else
                {
                    str = "PASS";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return str;
        }
        public static string Validatedropdown(string ctlValue, string ctlname)
        {

            try
            {
                if ((ctlValue == "0"))
                {
                    str = "Please select" + " " + ctlname;

                }
                else
                {
                    str = "PASS";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return str;
        }
        public static string ValidateTextbox_Mandatory_AlphaNumeric(string ctlValue, string ctlname, int sz, int minSize = 0)
        {
            try
            {
                if ((string.IsNullOrEmpty(ctlValue)))
                {
                    str = ctlname + " can not be left blank";

                }
                else if (ctlValue.Substring(0, 1) == " ")
                {
                    str = ctlname + " does not allow White Space(s) in first place";

                }
                else if (ctlValue.Substring(0, 1) == "!" || ctlValue.Substring(0, 1) == "@" || ctlValue.Substring(0, 1) == "#" || ctlValue.Substring(0, 1) == "$" || ctlValue.Substring(0, 1) == "%" || ctlValue.Substring(0, 1) == "^" || ctlValue.Substring(0, 1) == "&" || ctlValue.Substring(0, 1) == "*" || ctlValue.Substring(0, 1) == "(" || ctlValue.Substring(0, 1) == ")" || ctlValue.Substring(0, 1) == "-" || ctlValue.Substring(0, 1) == "_" || ctlValue.Substring(0, 1) == "+" || ctlValue.Substring(0, 1) == "=" || ctlValue.Substring(0, 1) == "{" || ctlValue.Substring(0, 1) == "}" || ctlValue.Substring(0, 1) == "[" || ctlValue.Substring(0, 1) == "]" || ctlValue.Substring(0, 1) == "|" || ctlValue.Substring(0, 1) == ";" || ctlValue.Substring(0, 1) == ":" || ctlValue.Substring(0, 1) == "<" || ctlValue.Substring(0, 1) == ">" || ctlValue.Substring(0, 1) == "?" || ctlValue.Substring(0, 1) == "." || ctlValue.Substring(0, 1) == "," || ctlValue.Substring(0, 1) == "/" || ctlValue.Substring(0, 1) == "\\" || ctlValue.Substring(0, 1) == "~" || ctlValue.Substring(0, 1) == "`" || ctlValue.Substring(0, 1) == "\"" || ctlValue.Substring(0, 1) == "\'" || ctlValue.Substring(0, 1) == "&")
                {
                    str = ctlname + " does not allow Special Characters in first place";

                }
                else if (ctlValue.Substring(ctlValue.Length - 1, 1) == " ")
                {
                    str = ctlname + " does not allow White Space(s) in last place";

                }
                else if ((ctlValue == "'"))
                {
                    str = ctlname + " does not allow Single Quote";

                }

                else if ((ctlValue.Contains('!')) || (ctlValue.Contains('%')) || (ctlValue.Contains('<')) || (ctlValue.Contains('>')) || (ctlValue.Contains('=')) || (ctlValue.Contains("'")))
                {
                    str = ctlname + " does not allow Special character";

                }
                else if ((minSize > 0) && ctlValue.Length < minSize)
                {
                    int cnt = 0;
                    cnt = ctlValue.Length;
                    if ((cnt < minSize))
                    {
                        str = ctlname + " allows minimum " + minSize.ToString() + " character(s)";

                    }
                }
                else if ((sz > 0) && ctlValue.Length > sz)
                {
                    int cnt = 0;
                    cnt = ctlValue.Length;
                    if ((cnt > sz))
                    {
                        str = ctlname + " allows Maximum " + sz.ToString() + " character(s)";

                    }
                }
                else
                {
                    str = "PASS";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return str;
        }
        public static string ValidateRadioButtonList(string ctlValue, string ctlname, int intMaxRangeVal = 0)
        {
            //intMaxRangeVal is the maximum listItem value which is optional
            try
            {
                if ((ctlValue == "0") || (ctlValue == ""))
                {
                    str = "Please select" + " " + ctlname;

                }
                else if (intMaxRangeVal > 0)
                {
                    if (Convert.ToInt32(ctlValue) > intMaxRangeVal)
                    {
                        str = "Please select" + " " + ctlname;

                    }
                }
                else
                {
                    str = "PASS";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return str;
        }
        public static string ValidDateCurrentdate(string txtDate, string ctlname)
        {
            try
            {
                if (string.IsNullOrEmpty(txtDate))
                {
                    str = ctlname + " can not be left blank";
                }

                else if (Convert.ToDateTime(txtDate) > Convert.ToDateTime(DateTime.Now.Date.ToShortDateString()))
                {
                    str = ctlname + " can not be greater than current date";
                }
                else
                {
                    str = "PASS";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return str;
        }
        public static string CurrentDateLessValidator(string txtdateto, string ctlname)
        {
            try
            {
                if (string.IsNullOrEmpty(txtdateto))
                {
                    str = ctlname + " " + "can not be left blank";
                }

                else if (Convert.ToDateTime(txtdateto) < Convert.ToDateTime(DateTime.Now.Date.ToShortDateString()))
                {
                    str = ctlname + " " + "can not be before Current Date";
                }
                else
                {
                    str = "PASS";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return str;
        }
        public static string DateDifferenceValidator(string txtdatefrom, string txtdateto, string ctrlname)
        {
            try
            {

                if (Convert.ToDateTime(txtdatefrom) > Convert.ToDateTime(txtdateto))
                {
                    str = ctrlname + " " + "End date cannot before Start date ";
                }

                else
                {
                    str = "PASS";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return str;
        }
        public static string ValidateTextbox_Mandatory_Decimal(string ctlValue, string ctlname, int sz, int minSize = 0)
        {
            try
            {



                double outVar;
                bool isdecimal = double.TryParse(ctlValue, out outVar);

                if ((string.IsNullOrEmpty(ctlValue)))
                {
                    str = ctlname + " can not be left blank";

                }
                else if (outVar == 0.0)
                {
                    str = ctlname + " can not be left blank";

                }
                else if (ctlValue.Substring(0, 1) == " ")
                {
                    str = ctlname + " " + "White Space(s) not allowed in first place";

                }
                else if (ctlValue.Substring(0, 1) == "!" || ctlValue.Substring(0, 1) == "@" || ctlValue.Substring(0, 1) == "#" || ctlValue.Substring(0, 1) == "$" || ctlValue.Substring(0, 1) == "%" || ctlValue.Substring(0, 1) == "^" || ctlValue.Substring(0, 1) == "&" || ctlValue.Substring(0, 1) == "*" || ctlValue.Substring(0, 1) == "(" || ctlValue.Substring(0, 1) == ")" || ctlValue.Substring(0, 1) == "-" || ctlValue.Substring(0, 1) == "_" || ctlValue.Substring(0, 1) == "+" || ctlValue.Substring(0, 1) == "=" || ctlValue.Substring(0, 1) == "{" || ctlValue.Substring(0, 1) == "}" || ctlValue.Substring(0, 1) == "[" || ctlValue.Substring(0, 1) == "]" || ctlValue.Substring(0, 1) == "|" || ctlValue.Substring(0, 1) == ";" || ctlValue.Substring(0, 1) == ":" || ctlValue.Substring(0, 1) == "<" || ctlValue.Substring(0, 1) == ">" || ctlValue.Substring(0, 1) == "?" || ctlValue.Substring(0, 1) == "." || ctlValue.Substring(0, 1) == "," || ctlValue.Substring(0, 1) == "/" || ctlValue.Substring(0, 1) == "\\" || ctlValue.Substring(0, 1) == "~" || ctlValue.Substring(0, 1) == "" || ctlValue.Substring(0, 1) == "\"" || ctlValue.Substring(0, 1) == "\'" || ctlValue.Substring(0, 1) == "&")
                {
                    str = ctlname + " " + "does not allow Special Characters in first place";
                    

                }
                else if (ctlValue.Substring(ctlValue.Length - 1, 1) == " ")
                {
                    str = ctlname + " " + "does not allow White Space(s) in last place";

                }
                else if ((ctlValue == "'"))
                {
                    str = ctlname + " " + "does not allow Single Quote";
                     

                }

                else if ((ctlValue.Contains('\'')) || (ctlValue.Contains('!')) || (ctlValue.Contains('$')) || (ctlValue.Contains('%')) || (ctlValue.Contains('^')) || (ctlValue.Contains('*')) || (ctlValue.Contains('<')) || (ctlValue.Contains('>')) || (ctlValue.Contains('~')) || (ctlValue.Contains('+')) || (ctlValue.Contains('=')) || (ctlValue.Contains('{')) || (ctlValue.Contains('}')) || (ctlValue.Contains('[')) || (ctlValue.Contains(']')) || (ctlValue.Contains('|')) || (ctlValue.Contains(';')) || (ctlValue.Contains('`')) || (ctlValue.Contains('-')) || (ctlValue.Contains('_')) || (ctlValue.Contains('"')) || (ctlValue.Contains('\\')) || (ctlValue.Contains('/')) || (ctlValue.Contains('&')))
                {
                    str = ctlname + " " + "does not allow Special characters";
                    


                }


                else if ((minSize > 0) && ctlValue.Length < minSize)
                {
                    int cnt = 0;
                    cnt = ctlValue.Length;
                    if ((cnt < minSize))
                    {
                        str = ctlname + " " + "allows minimum" + " " + minSize.ToString() + " " + "character(s)";

                    }
                }
                else if ((sz > 0) && ctlValue.Length > sz)
                {
                    int cnt = 0;
                    cnt = ctlValue.Length;
                    if ((cnt > sz))
                    {
                        str = ctlname + " " + "allows maximum" + " " + sz.ToString() + " " + "character(s)";

                    }
                }
               

                else if ((ctlValue.ToUpper().Contains('A')) || (ctlValue.Contains('B')) || (ctlValue.Contains('C')) || (ctlValue.Contains('D')) || (ctlValue.Contains('E')) || (ctlValue.Contains('F')) || (ctlValue.Contains('G')) || (ctlValue.Contains('H')) || (ctlValue.Contains('I')) || (ctlValue.Contains('J')) || (ctlValue.Contains('K')) ||
                    (ctlValue.Contains('L')) || (ctlValue.Contains('M')) || (ctlValue.Contains('N')) || (ctlValue.Contains('O')) || (ctlValue.Contains('P')) || (ctlValue.Contains('Q')) || (ctlValue.Contains('R')) || (ctlValue.Contains('S')) || (ctlValue.Contains('T')) || (ctlValue.Contains('U')) || (ctlValue.Contains('V')) ||
                    (ctlValue.Contains('W')) || (ctlValue.Contains('X')) || (ctlValue.Contains('Y')) || (ctlValue.Contains('Z')))
                {
                    str = ctlname + " " + "is a decimal no. which does not allow alphabets ";
                    

                }
                else if (isdecimal == false)
                {
                    str = ctlname + " " + "is not a decimal value";


                }
                else
                {
                    str = "PASS";
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return str;
        }
        public static string ValidateTextbox_NonMandatory_Decimal(string ctlValue, string ctlname, int sz, int minSize = 0)
        {
            try
            {
                double outVar;
                bool isdecimal = double.TryParse(ctlValue, out outVar);

                if (ctlValue != "")
                {


                    if (ctlValue.Substring(0, 1) == " ")
                    {
                        str = ctlname + " " + "White Space(s) not allowed in first place";

                    }
                    else if (ctlValue.Substring(0, 1) == "!" || ctlValue.Substring(0, 1) == "@" || ctlValue.Substring(0, 1) == "#" || ctlValue.Substring(0, 1) == "$" || ctlValue.Substring(0, 1) == "%" || ctlValue.Substring(0, 1) == "^" || ctlValue.Substring(0, 1) == "&" || ctlValue.Substring(0, 1) == "*" || ctlValue.Substring(0, 1) == "(" || ctlValue.Substring(0, 1) == ")" || ctlValue.Substring(0, 1) == "-" || ctlValue.Substring(0, 1) == "_" || ctlValue.Substring(0, 1) == "+" || ctlValue.Substring(0, 1) == "=" || ctlValue.Substring(0, 1) == "{" || ctlValue.Substring(0, 1) == "}" || ctlValue.Substring(0, 1) == "[" || ctlValue.Substring(0, 1) == "]" || ctlValue.Substring(0, 1) == "|" || ctlValue.Substring(0, 1) == ";" || ctlValue.Substring(0, 1) == ":" || ctlValue.Substring(0, 1) == "<" || ctlValue.Substring(0, 1) == ">" || ctlValue.Substring(0, 1) == "?" || ctlValue.Substring(0, 1) == "." || ctlValue.Substring(0, 1) == "," || ctlValue.Substring(0, 1) == "/" || ctlValue.Substring(0, 1) == "\\" || ctlValue.Substring(0, 1) == "~" || ctlValue.Substring(0, 1) == "" || ctlValue.Substring(0, 1) == "\"" || ctlValue.Substring(0, 1) == "\'" || ctlValue.Substring(0, 1) == "&")
                    {
                        str = ctlname + " " + "does not allow Special Characters in first place";
                        

                    }
                    else if (ctlValue.Substring(ctlValue.Length - 1, 1) == " ")
                    {
                        str = ctlname + " " + "does not allow White Space(s) in last place";

                    }
                    else if ((ctlValue == "'"))
                    {
                        str = ctlname + " " + "does not allow Single Quote";
                         

                    }

                    else if ((ctlValue.Contains('\'')) || (ctlValue.Contains('!')) || (ctlValue.Contains('$')) || (ctlValue.Contains('%')) || (ctlValue.Contains('^')) || (ctlValue.Contains('*')) || (ctlValue.Contains('<')) || (ctlValue.Contains('>')) || (ctlValue.Contains('~')) || (ctlValue.Contains('+')) || (ctlValue.Contains('=')) || (ctlValue.Contains('{')) || (ctlValue.Contains('}')) || (ctlValue.Contains('[')) || (ctlValue.Contains(']')) || (ctlValue.Contains('|')) || (ctlValue.Contains(';')) || (ctlValue.Contains('`')) || (ctlValue.Contains('-')) || (ctlValue.Contains('_')) || (ctlValue.Contains('"')) || (ctlValue.Contains('\\')) || (ctlValue.Contains('/')) || (ctlValue.Contains('&')))
                    {
                        str = ctlname + " " + "does not allow Special characters";
                        


                    }


                    else if ((minSize > 0) && ctlValue.Length < minSize)
                    {
                        int cnt = 0;
                        cnt = ctlValue.Length;
                        if ((cnt < minSize))
                        {
                            str = ctlname + " " + "allows minimum" + " " + minSize.ToString() + " " + "character(s)";

                        }
                    }
                    else if ((sz > 0) && ctlValue.Length > sz)
                    {
                        int cnt = 0;
                        cnt = ctlValue.Length;
                        if ((cnt > sz))
                        {
                            str = ctlname + " " + "allows maximum" + " " + sz.ToString() + " " + "character(s)";

                        }
                    }
                    else if (!(ctlValue.Contains('.') || ctlValue.Contains('4')))
                    {
                        str = ctlname + " " + ". is not a decimal no.  ";
                        

                    }
                    else if ((ctlValue.ToUpper().Contains('A')) || (ctlValue.Contains('B')) || (ctlValue.Contains('C')) || (ctlValue.Contains('D')) || (ctlValue.Contains('E')) || (ctlValue.Contains('F')) || (ctlValue.Contains('G')) || (ctlValue.Contains('H')) || (ctlValue.Contains('I')) || (ctlValue.Contains('J')) || (ctlValue.Contains('K')) ||
                        (ctlValue.Contains('L')) || (ctlValue.Contains('M')) || (ctlValue.Contains('N')) || (ctlValue.Contains('O')) || (ctlValue.Contains('P')) || (ctlValue.Contains('Q')) || (ctlValue.Contains('R')) || (ctlValue.Contains('S')) || (ctlValue.Contains('T')) || (ctlValue.Contains('U')) || (ctlValue.Contains('V')) ||
                        (ctlValue.Contains('W')) || (ctlValue.Contains('X')) || (ctlValue.Contains('Y')) || (ctlValue.Contains('Z')))
                    {
                        str = ctlname + " " + "is a decimal no. which does not allow alphabets ";
                        

                    }
                    else if (isdecimal == false)
                    {
                        str = ctlname + " " + "is not a decimal value";


                    }

                    else
                    {
                        str = "PASS";
                    }
                }
                else
                {
                    str = "PASS";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return str;
        }
        #region "Check Valid Date"
        public static string CheckDate(string ctlValue, string ctlname)
        {
            try
            {
                if (ctlValue.Trim() != "")
                {
                    DateTime Temp;
                    if (DateTime.TryParse(ctlValue, out Temp) == true)
                    {
                        str = "PASS";
                    }
                    else
                    {
                        str = ctlname + " " + "is not a valid date ";

                    }
                }
                else
                {
                    str = ctlname + " " + "can not be left blank ";

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return str;
        }
        #endregion
        #region "Validate Date Range"
        public static string ValidateDateRange(string FrstCntrl, string ScndCntrl, string FrstCntrlName, string ScndCntrlName)
        {
            try
            {
                if ((FrstCntrl.Trim() != "") && (ScndCntrl.Trim() != ""))
                {
                    if (Convert.ToDateTime(FrstCntrl.Trim()) > Convert.ToDateTime(ScndCntrl.Trim()))
                    {
                        str = "Invalid Date Range! " + ScndCntrlName + " can not be before " + FrstCntrlName;
                    }
                    else
                    {
                        str = "PASS";
                    }
                }
                else
                {
                    str = FrstCntrlName + " " + "and " + ScndCntrlName + " can not be left blank ";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return str;
        }
        #endregion
        #region "Check Todays Date"
        public static string CheckTodaysDate(string txtDate, string ctlname)
        {
            try
            {
                if (string.IsNullOrEmpty(txtDate))
                {
                    str = ctlname + " can not be left blank";
                }

                else if (Convert.ToDateTime(txtDate) != Convert.ToDateTime(DateTime.Now.Date.ToShortDateString()))
                {
                    str = ctlname + " can not be less than or greater than current date";
                }
                else
                {
                    str = "PASS";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return str;
        }
        #endregion
        //********************End of Server side Control Validation function **********************************************************

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo? fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
    }
}

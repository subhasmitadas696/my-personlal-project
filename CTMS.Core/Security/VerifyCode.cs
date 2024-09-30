using System.Drawing;
using System.Drawing.Imaging;

namespace CTMS.Core
{
    public class VerifyCode
    {
        //https://www.cnblogs.com/yuangang/p/6000460.html
        public byte[] GetVerifyCode()
        {
            Color brushColor = Color.Black;
            // Creating object for bitmap
            Bitmap objBitmap = new Bitmap(300, 30);
            // Creating object for Graphics class
            Graphics objGraphics = Graphics.FromImage(objBitmap);
            objGraphics.Clear(Color.Transparent);
            // Creating object for Font class
            Font objFont = new Font("Calibri", 14, FontStyle.Regular);
            string inputNumberString = "";
            Random r = new Random();
            char[] str = { '*', '-', '+', '>', '<', 'F', 'L', 'M' };
            int[] array = new int[3];
            int ranlen = r.Next(str.Length);
            int a = 0, b = 0, c = 0, d = 0;
            switch (ranlen)
            {
                case 0:// FOr Multiplication.
                    a = r.Next(1, 10);
                    b = r.Next(1, 10);
                    c = a * b;
                    inputNumberString = "What is the Output ? " + a.ToString() + " * " + b.ToString() + "";
                    break;
                case 1:// FOr Subtraction.
                    a = r.Next(10, 99);
                    b = r.Next(0, 10);
                    c = a - b;
                    inputNumberString = "What is the Output ? " + a.ToString() + " - " + b.ToString() + "";
                    break;
                case 2:// FOr Addition.
                    a = r.Next(1, 10);
                    b = r.Next(1, 99);
                    c = a + b;
                    inputNumberString = "What is the Output ? " + a.ToString() + " + " + b.ToString() + "";
                    break;
                case 3:// FOr getting Greatest No.
                    a = r.Next(1, 10);
                    array[0] = a;
                    for (int j = 1; j < 3; j++)
                    {
                    Loop:
                        b = r.Next(1, 99);
                        for (int k = 0; k < array.Length; k++)
                        {
                            if (array[k] == b)
                            {
                                goto Loop;
                            }

                        }

                        array[j] = b;
                    }
                    c = array.Max();
                    inputNumberString = "Which is the greatest No. ? " + array[0].ToString() + "," + array[1].ToString() + "," + array[2].ToString() + "";
                    break;
                case 4:// FOr getting Smallest No.

                    a = r.Next(1, 10);
                    array[0] = a;

                    for (int j = 1; j < 3; j++)
                    {
                    Loop:
                        b = r.Next(1, 99);
                        for (int k = 0; k < array.Length; k++)
                        {
                            if (array[k] == b)
                            {
                                goto Loop;
                            }

                        }
                        array[j] = b;
                    }
                    c = array.Min();
                    inputNumberString = "Which is the smallest No. ? " + array[0].ToString() + " , " + array[1].ToString() + " , " + array[2].ToString() + "";
                    break;
                case 5:// FOr getting First No.
                    a = r.Next(1, 10);
                    b = r.Next(1, 99);
                    d = r.Next(1, 99);
                    c = a;
                    inputNumberString = "Which is the first No. ? " + a.ToString() + " , " + b.ToString() + " , " + d.ToString() + "";
                    break;
                case 6:// FOr getting Last No.
                    a = r.Next(1, 10);
                    b = r.Next(1, 99);
                    d = r.Next(1, 99);
                    c = d;
                    inputNumberString = "Which is the last No. ? " + a.ToString() + " , " + b.ToString() + " , " + d.ToString() + "";
                    break;
                case 7:// FOr getting Middle No.
                    a = r.Next(1, 10);
                    b = r.Next(1, 99);
                    d = r.Next(1, 99);
                    c = b;
                    inputNumberString = "Which is the Middle No. ? " + a.ToString() + " , " + b.ToString() + " , " + d.ToString() + "";
                    break;
            }

            //Storing the captcha value in the session
            WebHelper.WriteSession(ConstParameters.VerifyCodeKeyName, Md5Hash.Md5(c.ToString().ToLower(), 16));
            SolidBrush myBrush = new SolidBrush(brushColor);
            objGraphics.DrawString(inputNumberString, objFont, myBrush, 1, 3);

            //Write the captcha image to the memory stream and output it in" image/Png " format
            MemoryStream ms = new MemoryStream();
            try
            {
                objBitmap.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                objGraphics.Dispose();
                objBitmap.Dispose();
            }
        }
    }
}


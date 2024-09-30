using SkiaSharp;
using System;

namespace CTMS.Web
{
    public static class GenerateCaptcha
    {
        //public static string Captcha()
        //{
        //    string[] strArray = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "2", "3", "4", "5", "6", "7", "8", "9" };
        //    Random autoRand = new Random();
        //    string captchaCode = GenerateRandomCode(autoRand, 4, "123456789ABCDEFGHJKLMNPQRSTUVWXYZ");
        //    captchaCode=captchaCode.Replace('0', 'A').Replace('O', 'A');
        //    return captchaCode;
        //}
        public static string Captcha()
        {
            //string[] strArray = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "2", "3", "4", "5", "6", "7", "8", "9" };
            string[] strArray = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "L", "M", "N","1", "2", "3", "4", "5", "6", "7", "8", "9" };

            // Shuffling the array
            Shuffle(strArray);

            // Selecting the first four characters
            string captchaCode = string.Join("", strArray.Take(4));

            captchaCode = captchaCode.Replace('0', 'A').Replace('O', 'A');

            return captchaCode;
        }

        // Fisher-Yates shuffle algorithm
        public static void Shuffle<T>(T[] array)
        {
            Random rand = new Random();
            int n = array.Length;
            while (n > 1)
            {
                int k = rand.Next(n--);
                T temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }
        public static bool ValidateCaptchaCode(string userInputCaptcha, Microsoft.AspNetCore.Http.HttpContext context)
        {
            var isValid = userInputCaptcha == context.Session.GetString("CaptchaCode");
            context.Session.Remove("CaptchaCode");
            return isValid;
        }

        public static byte[] GenerateCaptchaImage(int width, int height, string captchaCode)
        {
            using (var surface = SKSurface.Create(new SKImageInfo(width, height)))
            {
                // Get the canvas from the surface
                var canvas = surface.Canvas;

                // Clear the canvas (remove the red background)
                canvas.Clear(SKColors.Transparent);  // Use Transparent instead of Red

               

                // Create a paint object for drawing text
                using (var paint = new SKPaint())
                {
                    paint.TextSize = 22;
                    paint.Color = SKColors.White;  // Set text color to white
                    // Use a bold typeface for the paint
                    paint.Typeface = SKTypeface.FromFamilyName(null, SKFontStyleWeight.Bold, SKFontStyleWidth.Normal, SKFontStyleSlant.Upright);

                    // Position to center the text
                    float x = (width - paint.MeasureText(captchaCode)) / 2;
                    float y = (height + paint.TextSize) / 2;

                    // Draw the text on the canvas
                    canvas.DrawText(captchaCode, x, y, paint);
                }

                // Save the generated image to a memory stream
                using (var image = surface.Snapshot())
                using (var stream = new MemoryStream())
                {
                    image.Encode(SKEncodedImageFormat.Png, 100).SaveTo(stream);
                    return stream.ToArray();
                }
            }
        }
        // Function to generate a random code excluding specific characters
        private static string GenerateRandomCode(Random random, int length, string characters)
        {
            char[] code = new char[length];
            for (int i = 0; i < length; i++)
            {
                code[i] = characters[random.Next(characters.Length)];
            }
            return new string(code);
        }
    }
}

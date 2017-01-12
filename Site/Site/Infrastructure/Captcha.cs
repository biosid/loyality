using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Vtb24.Site.Infrastructure
{
    public class CaptchaOptions
    {
        public string Text { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int FontSize { get; set; }
        public string FontName { get; set; }

        public const int NOIZE_SIZE = 2; 
        public const int MAX_WIDTH = 400;
        public const int MAX_HEIGHT = 200;
        public const int MIN_FONT_SIZE = 6;
        public const int MAX_FONT_SIZE = 36;

        public static readonly Color TextColor = ColorTranslator.FromHtml("#253A67");
        public static readonly Color NoizeColor = ColorTranslator.FromHtml("#E8ECF1");
        
        public void Validate()
        {
            if (string.IsNullOrEmpty(Text))
            {
                throw new InvalidOperationException("Параметр text обязателен!");
            }
            if (Width <= 0 || Width > MAX_WIDTH || Height <= 0 || Height > MAX_HEIGHT)
            {
                throw new InvalidOperationException("Заданы некорректные размеры капчи");
            }
            if(FontSize < MIN_FONT_SIZE || FontSize > MAX_FONT_SIZE)
            {
                throw new InvalidOperationException("Заданы некорректные размеры шрифта");
            }
        }
    }

    public class Captcha
    {                             
        public Guid Id { get; private set; }

        public Captcha(CaptchaOptions options)
        {            
            options.Validate();
            Id = Guid.NewGuid();

            Image = new Bitmap(options.Width, options.Height, PixelFormat.Format32bppArgb);

            var g = Graphics.FromImage(Image);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var rect = new Rectangle(0, 0, options.Width, options.Height);


            var hatchBrush = new HatchBrush(HatchStyle.SmallConfetti, CaptchaOptions.TextColor, CaptchaOptions.NoizeColor);
            g.FillRectangle(hatchBrush, rect);

            g.DrawString(options.Text, new Font(options.FontName, options.FontSize), new SolidBrush(CaptchaOptions.TextColor), new PointF(0, 0));

            var random = new Random();
            for (var i = 0; i < (int)(rect.Width * rect.Height / 30F); i++)
            {
                var x = random.Next(rect.Width);
                var y = random.Next(rect.Height);
                g.FillEllipse(new SolidBrush(CaptchaOptions.TextColor), x, y, CaptchaOptions.NOIZE_SIZE, CaptchaOptions.NOIZE_SIZE);
            }

            hatchBrush.Dispose();
            g.Dispose();
        }

        public Image Image { get; private set; }

        public static string GenerateRandomCode()
        {
            return Guid.NewGuid().ToString().Substring(0, 6);
        }
    }
}
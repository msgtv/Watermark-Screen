using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;

namespace WatermarkScreen
{
    public partial class Watermark : Form
    {
        // Добавляем свойство для хранения аргументов командной строки
        public string[] CommandLineArguments { get; set; }

        private System.Windows.Forms.Timer aTimer;
        public string TextToShow { get; set; }
        public string TimeAndDate { get; set; }

        Color TextColor = Color.Red;

        public Watermark()
        {
            InitializeComponent();

            SetFormLocation();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.DoubleBuffer, true);

            // Создаем и настраиваем таймер
            aTimer = new System.Windows.Forms.Timer();
            aTimer.Interval = 1000; // Устанавливаем интервал в 1 секунду
            aTimer.Tick += Timer_Tick; // Привязываем событие TimerTick к событию Tick таймера
            aTimer.Enabled = true; // Включаем таймер
        }

        private void SetFormLocation()
        {
            if (Screen.AllScreens.Length > 1)
            {
                this.Location = Screen.AllScreens[1].WorkingArea.Location;
            }
            else
            {
                this.Location = Screen.PrimaryScreen.WorkingArea.Location;
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle = cp.ExStyle | 0x20; // Define the Background Transparency without colors 
                cp.ExStyle |= 0x80; // Hide Borderless Form from Alt+Tab
                return cp;
            }
        }

        private void Watermark_Paint(object sender, PaintEventArgs e)
        {
            SizeF szF1 = e.Graphics.MeasureString(TextToShow + TimeAndDate, this.Font);

            //Returns the larger of two specified numbers.
            int max = Math.Max(this.Width, this.Height);

            //Is the Object used to draw lines, instead "Soolid Brush" we could use too classes "Pen", "Brush".
            var ToolDraw = new SolidBrush(Color.FromArgb(255, this.TextColor));

            double wid1 = (this.Width % szF1.Width) / 2;
            double wid2 = Math.Truncate(this.Width / szF1.Width);
            double hei1 = (this.Height % szF1.Height) / 2;
            double hei2 = Math.Truncate(this.Height / szF1.Height);

            // e.Graphics.TextRenderingHint = TextRenderingHint.AntiAlias;

            for (int i = 0; i < hei2; i++)
            {
                if ((i % 2) == 0)
                {
                    for (int j = 0; j < wid2; j = j + 3)
                    {
                        e.Graphics.DrawString(TextToShow + TimeAndDate, this.Font, ToolDraw, (float)(wid1 + szF1.Width * j), (float)(hei1 + szF1.Height * i));
                    }
                }
                else
                {
                    for (int j = 0; j < wid2 - 1; j = j + 4)
                    {
                        e.Graphics.DrawString(TextToShow + TimeAndDate, this.Font, ToolDraw, (float)(wid1 + szF1.Width * (j + 1)), (float)(hei1 + szF1.Height * i));
                    }
                }
            }

            e.Graphics.DrawString(Environment.MachineName, this.Font, ToolDraw, this.Width - 250, this.Height - 225);
        }

        private string GetTimeAndDate()
        {
            return DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }

        private void Watermark_Load(object sender, EventArgs e)
        {
            // Получаем все IP-адреса текущего компьютера
            IPAddress[] ipAddresses = Dns.GetHostAddresses(Dns.GetHostName());

            // Создаем строку для отображения IP-адресов
            string ipAddressesString = "";
            foreach (IPAddress ipAddress in ipAddresses)
            {
                ipAddressesString += ipAddress.ToString() + "\r\n";
            }


            // TextToShow = "Your Company Name here \r\n" + "Direitos reservados \r\n" + Environment.UserName + "\r\n";

            TextToShow = "КОНФИДЕНЦИАЛЬНО \r\n"
                + "Пользователь: " + Environment.UserName + "\r\n"
                + "Имя ПК: " + Environment.MachineName + "\r\n"
                + ipAddressesString + "\r\n";

            TimeAndDate = GetTimeAndDate();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TimeAndDate = GetTimeAndDate();
            Invalidate();
        }
    }
}
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace WatermarkScreen
{
    public partial class Watermark : Form
    {
        private System.Windows.Forms.Timer aTimer;
        public string TextToShow { get; set; }
        public string TimeAndDate { get; set; }

        public Watermark()
        {
            InitializeComponent();
            SetFormLocation();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.DoubleBuffer, true);

            // Создаем и настраиваем таймер
            aTimer = new System.Windows.Forms.Timer();
            aTimer.Interval = 1000; // Устанавливаем интервал в 1 секунду
            aTimer.Tick += Timer_Tick; // Привязываем событие Timer_Tick к событию Tick таймера
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
            using (var brush = new SolidBrush(Color.Green))
            {
                e.Graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
                e.Graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;

                var textSize = e.Graphics.MeasureString(TextToShow + TimeAndDate, this.Font);
                var x = (this.Width - textSize.Width) / 2;
                var y = (this.Height - textSize.Height) / 2;

                e.Graphics.DrawString(TextToShow + TimeAndDate, this.Font, brush, x, y);
            }
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
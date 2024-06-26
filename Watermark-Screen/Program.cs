﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace WatermarkScreen
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (Process.GetProcesses().Count(x => x.ProcessName == "Watermark-Screen") > 1)
            {
                Process.GetCurrentProcess().Kill();
            }                

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // Создаем новый экземпляр формы Watermark
            Watermark form = new Watermark();

            // Запускаем приложение с созданной формой
            Application.Run(form);
        }
    }
}

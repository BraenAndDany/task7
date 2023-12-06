using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using LibreHardwareMonitor.Hardware;

namespace task7
{
    public partial class Form1 : Form
    {
        class ProgramTask7
        {
            public string FindLargestFile(string directory)
            {
                // Проверяет, существует ли директория
                if (Directory.Exists(directory))
                {
                    // Получает все файлы в директории и их размеры
                    var files = Directory.GetFiles(directory);
                    var sizes = files.Select(f => new FileInfo(f).Length);

                    // Находит максимальный размер и соответствующий файл
                    var max = sizes.Max();
                    var index = sizes.ToList().IndexOf(max);
                    var file = files[index];

                    // Возвращает имя и размер файла в виде строки
                    return $"Самый большой файл в директории {directory} это {file}, его размер {max} байт";
                }
                else
                {
                    // Возвращает сообщение об ошибке, если директория не существует
                    return $"Директория {directory} не существует";
                }
            }

            // Переименовывает все файлы внутри какой-либо директории от 1 до N, где N – это количество файлов в директории
            public string RenameFiles(string directory)
            {
            // Проверяет, существует ли директория
                if (Directory.Exists(directory))
                {
                    // Получает все файлы в директории и их расширения
                    var files = Directory.GetFiles(directory);
                    var extensions = files.Select(f => Path.GetExtension(f));

                    // Переименовывает каждый файл с помощью цикла for
                    for (int i = 0; i < files.Length; i++)
                    {
                        // Формирует новое имя файла с помощью индекса и расширения
                        var newName = (i + 1).ToString() + extensions.ElementAt(i);

                    // Переименовывает файл с помощью метода Move
                    try
                    {
                        File.Move(files[i], Path.Combine(directory, newName));
                    }
                    catch { 
                    }
                    }

                    // Возвращает сообщение об успехе
                    return $"Все файлы в директории {directory} были переименованы от 1 до {files.Length}";
                }
                else
                {
                    // Возвращает сообщение об ошибке, если директория не существует
                    return $"Директория {directory} не существует";
                }          
            }

            static CpuInfo GetCpuInfo()
            {
                Computer computer = new Computer
                {
                    IsCpuEnabled = true // Включаем мониторинг CPU
                };
                computer.Open(); // Начинаем мониторинг

                CpuInfo cpuInfo = new CpuInfo();

                foreach (var hardwareItem in computer.Hardware)
                {
                    if (hardwareItem.HardwareType == HardwareType.Cpu)
                    {
                        hardwareItem.Update(); // Обновляем информацию о CPU
                        foreach (var sensor in hardwareItem.Sensors)
                        {
                            if (sensor.SensorType == SensorType.Temperature)
                            {
                                cpuInfo.Temperature = sensor.Value.HasValue ? sensor.Value.Value : 0;
                            }
                            else if (sensor.SensorType == SensorType.Load)
                            {
                                cpuInfo.Load = sensor.Value.HasValue ? sensor.Value.Value : 0;
                            }
                        }
                        cpuInfo.Name = hardwareItem.Name;
                    }
                }

                computer.Close(); // Заканчиваем мониторинг
                return cpuInfo;
            }       

            public class CpuInfo
            {
                public double Temperature { get; set; }
                public double Load { get; set; }
                public string Name { get; set; }
            }

            public string ain()
            {
                string StringCpuInfo = "";
                var cpuInfo = GetCpuInfo();
                Console.WriteLine("Температура процессора: {0} °C", cpuInfo.Temperature);
                Console.WriteLine("Загрузка процессора: {0} %", cpuInfo.Load);
                Console.WriteLine("Название процессора: {0}", cpuInfo.Name);
                StringCpuInfo = "Название процессора: " + cpuInfo.Name + "               Загрузка процессора: " + cpuInfo.Load + "%              Температура процессора: " + cpuInfo.Temperature;
                return StringCpuInfo;
            }

        }
        public Form1()
        {
            InitializeComponent();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            ProgramTask7 GetData = new ProgramTask7();
            richTextBox1.Text += Environment.NewLine;
            richTextBox1.Text += Environment.NewLine;
            richTextBox1.Text += GetData.RenameFiles(richTextBox2.Text);
            richTextBox1.Text += Environment.NewLine;
            richTextBox1.Text += Environment.NewLine;
            richTextBox1.Text = GetData.FindLargestFile(richTextBox2.Text);
            richTextBox3.Text = GetData.ain();

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

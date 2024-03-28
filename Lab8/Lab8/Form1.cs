using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Lab8
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            comboBox1.Items.Add("Change to");
            comboBox1.Items.Add("Delete");

            comboBox2.Items.Add(".txt");
            comboBox2.Items.Add(".bin");

            comboBox3.Items.Add(".txt");
            comboBox3.Items.Add(".bin");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string fileName = textBox2.Text;
            string content = textBox1.Text;
            string format = comboBox2.Text;

            if (string.IsNullOrWhiteSpace(fileName))
            {
                MessageBox.Show("Будь ласка, введіть назву файлу.");
                return;
            }
            fileName += format;

            try
            {
                File.WriteAllText(fileName, content);
                MessageBox.Show("Файл успішно створено.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Виникла помилка: {ex.Message}");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string fileName = textBox7.Text;
            string triggerSubtext = textBox4.Text;
            string action = comboBox1.Text;
            string changeTo = textBox5.Text;

            if (string.IsNullOrWhiteSpace(fileName))
            {
                MessageBox.Show("Будь ласка, введіть назву файлу.");
                return;
            }
            fileName += ".txt";

            if (!File.Exists(fileName))
            {
                MessageBox.Show("Файл не знайдено.");
                return;
            }

            // Зчитуємо вміст файлу
            string content = File.ReadAllText(fileName);

            // Пошук дат у тексті за допомогою регулярного виразу
            string pattern = @"\b(19\d\d|20\d\d)\.(0[1-9]|1[0-2])\.(0[1-9]|[12]\d|3[01])\.(0[0-9]|1\d|2[0-4]):(0[0-9]|[1-5]\d|60)\b";
            MatchCollection matches = Regex.Matches(content, pattern);

            // Створюємо новий рядок для запису дат
            string dates = "";
            foreach (Match match in matches)
            {
                dates += match.Value + Environment.NewLine;
            }

            // Оголошення списку для зберігання дат
            List<string> dateWords = new List<string>();

            // Додавання дат до списку
            dateWords.AddRange(dates.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries));

            // Враховуємо фільтри
            if (action == "Change to")
            {
                textBox5.Text = triggerSubtext;
                for (int i = 0; i < dateWords.Count; i++)
                {
                    textBox1.Text += $"`{dateWords[i]}` ";
                    if (dateWords[i] == triggerSubtext)
                    {
                        dateWords[i] = changeTo;
                    }
                }
            }
            else if (action == "Delete")
            {
                for (int i = 0; i < dateWords.Count; i++)
                {
                    if (dateWords[i] == triggerSubtext)
                    {
                        dateWords.RemoveAt(i);
                    }
                }
            }

            // Записуємо знайдені дати у той же файл
            File.WriteAllText(fileName, string.Join(Environment.NewLine, dateWords));

            textBox3.Text = dateWords.Count.ToString();

            MessageBox.Show("Дати успішно витягнуті та записані у файл.");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string fileName = textBox8.Text;
            string format = comboBox3.Text;

            if (string.IsNullOrWhiteSpace(fileName))
            {
                MessageBox.Show("Будь ласка, введіть назву файлу.");
                return;
            }
            fileName += format;

            if (!File.Exists(fileName))
            {
                MessageBox.Show("Файл не знайдено.");
                return;
            }

            string[] lines = File.ReadAllLines(fileName);
            textBox6.Text = string.Join(Environment.NewLine, lines);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string fileName = textBox9.Text;

            if (string.IsNullOrWhiteSpace(fileName))
            {
                MessageBox.Show("Будь ласка, введіть назву файлу.");
                return;
            }
            fileName += ".txt";

            if (!File.Exists(fileName))
            {
                MessageBox.Show("Файл не знайдено.");
                return;
            }

            // Отримати вміст файлу
            string text;
            try
            {
                text = File.ReadAllText(fileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Виникла помилка при читанні файлу: {ex.Message}");
                return;
            }

            // Розділити текст на окремі слова за пробілами
            string[] words = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // Створити список для зберігання зміненого тексту
            List<string> modifiedWords = new List<string>();

            // Пройтися по кожному слову і зробити необхідні заміни
            foreach (string word in words)
            {
                // Видалити слова з префіксами "re", "not" та "be"
                if (!(word.StartsWith("re") || word.StartsWith("not") || word.StartsWith("be")))
                {
                    // Замінити слова з префіксом "не" на "not"
                    if (word.StartsWith("не"))
                    {
                        modifiedWords.Add("not" + word.Substring(2));
                    }
                    else
                    {
                        modifiedWords.Add(word);
                    }
                }
            }

            // Об'єднати слова знову у текст
            string modifiedText = string.Join(" ", modifiedWords);

            // Записати змінений текст у файл
            try
            {
                File.WriteAllText(fileName, modifiedText);
                MessageBox.Show("Зміни успішно виконані та записані у файл.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Виникла помилка при записі у файл: {ex.Message}");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string inputFile = textBox10.Text;
            string outputFile = textBox11.Text;

            if (string.IsNullOrWhiteSpace(inputFile) && string.IsNullOrWhiteSpace(outputFile))
            {
                MessageBox.Show("Будь ласка, введіть назву файлів.");
                return;
            }
            
            inputFile += ".txt";
            outputFile += ".txt";

            if (!File.Exists(inputFile))
            {
                MessageBox.Show("Файл вхідного тексту не знайдено.");
                return;
            }

            if (!File.Exists(outputFile))
            {
                MessageBox.Show("Файл вихідного тексту не знайдено.");
                return;
            }

            try
            {
                // Зчитуємо тексти з файлів
                string inputText = File.ReadAllText(inputFile);
                string outputText = File.ReadAllText(outputFile);

                // Розділяємо тексти на слова
                string[] inputWords = inputText.Split(new char[] { ' ', ',', '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
                string[] outputWords = outputText.Split(new char[] { ' ', ',', '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);

                // Вилучаємо слова другого тексту, які є у першому тексті
                List<string> filteredOutputWords = outputWords.Except(inputWords).ToList();

                // Формуємо новий текст з вилученими словами
                string resultText = string.Join(" ", filteredOutputWords);

                // Записуємо результат у вихідний файл
                File.WriteAllText(outputFile, resultText);

                MessageBox.Show("Слова з першого тексту вилучено з другого тексту та результат записано у вихідний файл.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Виникла помилка: {ex.Message}");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string fileName = textBox12.Text;

            if (string.IsNullOrWhiteSpace(fileName))
            {
                MessageBox.Show("Будь ласка, введіть назву файлу.");
                return;
            }
            fileName += ".bin";

            if (!File.Exists(fileName))
            {
                MessageBox.Show("Файл не знайдено.");
                return;
            }

            try
            {
                string content = File.ReadAllText(fileName);

                // Сплітим файл
                string[] numbersStr = content.Split(' ');

                // Створюємо список для зберігання чисел
                List<double> numbers = new List<double>();

                // Робим із стрінга дабл
                foreach (string numberStr in numbersStr)
                {
                    if (double.TryParse(numberStr, out double number))
                    {
                        numbers.Add(number);
                    }
                }

                // Вибираємо числа на четних позиціях
                List<double> evenPositionNumbers = new List<double>();
                for (int i = 0; i < numbers.Count; i += 2)
                {
                    evenPositionNumbers.Add(numbers[i]);
                }
                double average = evenPositionNumbers.Count > 0 ? evenPositionNumbers.Average() : 0;

                textBox13.Text = average.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Виникла помилка: {ex.Message}");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            string path = @"D:\temp";

            if (!Directory.Exists(path))
            {
                // Створення папки
                Directory.CreateDirectory(path);
                MessageBox.Show("Папка створена успішно.");
            }
            else
            {
                MessageBox.Show("Папка вже існує.");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string path1 = @"D:\temp\Mudrak1";
            string path2 = @"D:\temp\Mudrak2";

            if (!Directory.Exists(path1))
            {
                Directory.CreateDirectory(path1);
                MessageBox.Show("Папка створена успішно.");
            }
            else
            {
                MessageBox.Show($"Папка Mudrak1 вже існує.");
            }

            if (!Directory.Exists(path2))
            {
                Directory.CreateDirectory(path2);
                MessageBox.Show("Папка створена успішно.");
            }
            else
            {
                MessageBox.Show($"Папка Mudrak2 вже існує.");
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string path1 = @"D:\temp\Mudrak1";

            string file1 = Path.Combine(path1, "t1.txt");
            string file2 = Path.Combine(path1, "t2.txt");

            string name1 = "Шевченко Степан Іванович, 2001";
            string name2 = "Комар Сергій Федорович, 2000";
            string city1 = "м.Суми";
            string city2 = "м.Київ";

            string content1 = $"{name1} року народження, місце проживання {city1}";
            string content2 = $"{name2} року народження, місце проживання {city2}";

            File.WriteAllText(file1, content1);
            File.WriteAllText(file2, content2);

            MessageBox.Show("Документи створені успішно.");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            string copyPath1 = @"D:\temp\Mudrak1\t1.txt";
            string copyPath2 = @"D:\temp\Mudrak1\t2.txt";
            string pastePath1 = @"D:\temp\Mudrak2\t3.txt";

            try
            {
                string content1 = File.ReadAllText(copyPath1);
                string content2 = File.ReadAllText(copyPath2);

                File.WriteAllText(pastePath1, content1 + Environment.NewLine + content2);

                MessageBox.Show("Вміст скопійовано успішно.");
            }
            catch (IOException error)
            {
                MessageBox.Show($"Помилка: {error.Message}");
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            string path1 = @"D:\temp\Mudrak1\t1.txt";
            string path2 = @"D:\temp\Mudrak1\t2.txt";
            string path3 = @"D:\temp\Mudrak2\t3.txt";

            try
            {
                // Побудова рядка із інформацією про файли
                StringBuilder fileInfo = new StringBuilder();
                fileInfo.AppendLine("Створені файли:");
                fileInfo.AppendLine($"Шлях першого файлу: {path1}");
                fileInfo.AppendLine($"Шлях другого файлу: {path2}");
                fileInfo.AppendLine($"Шлях третього файлу: {path3}");

                textBox14.Text = fileInfo.ToString();
            }
            catch (IOException error)
            {
                MessageBox.Show($"Помилка: {error.Message}");
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            string fromPath = @"D:\temp\Mudrak1\t2.txt";
            string toPath = @"D:\temp\Mudrak2";

            try
            {
                string fileName = Path.GetFileName(fromPath);
                string toFilePath = Path.Combine(toPath, fileName);

                File.Move(fromPath, toFilePath);

                MessageBox.Show("Документ перенесено успішно.");
            }
            catch (IOException error)
            {
                MessageBox.Show($"Помилка: {error.Message}");
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            string fromPath = @"D:\temp\Mudrak1\t1.txt";
            string toPath = @"D:\temp\Mudrak2";

            try
            {
                string fileName = Path.GetFileName(fromPath);
                string toFilePath = Path.Combine(toPath, fileName);

                File.Copy(fromPath, toFilePath);

                MessageBox.Show("Документ скопійовано успішно.");
            }
            catch (IOException error)
            {
                MessageBox.Show($"Помилка: {error.Message}");
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            string oldFolderPath = @"D:\temp";
            string newFolderPath = @"D:\ALL";

            try
            {
                Directory.Move(oldFolderPath, newFolderPath);

                Console.WriteLine("Назва папки змінена успішно.");
            }
            catch (IOException error)
            {
                Console.WriteLine($"Помилка: {error.Message}");
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            string folderPath = @"D:\ALL"; // Путь к вашей папке

            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
                int directoryCount = directoryInfo.GetDirectories().Length;
                string information = $"Number of Directories: {directoryCount}";
                textBox14.Text = information;

                int txtFileCount = CountTxtFiles(folderPath);
                string information1 = $"\r\nNumber of .txt Files: {txtFileCount}";
                textBox14.Text += information1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private int CountTxtFiles(string folderPath)
        {
            int txtFileCount = 0;

            if (!Directory.Exists(folderPath))
            {
                throw new DirectoryNotFoundException($"Directory \"{folderPath}\" not found.");
            }

            foreach (string filePath in Directory.EnumerateFiles(folderPath, "*.txt", SearchOption.AllDirectories))
            {
                txtFileCount++;
            }

            return txtFileCount;
        }
    }
}

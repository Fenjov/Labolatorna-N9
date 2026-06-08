using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Globalization;
using System.IO;

namespace Lab9Avalonia
{
    public partial class MainWindow : Window
    {
        // Залишаємо тільки вихідний файл для збереження результатів роботи програми
        private const string OutputFile = "output.txt";

        public MainWindow()
        {
            InitializeComponent();
        }

        // Головна логіка розрахунку при натисканні кнопки
        private void OnCalculateClick(object? sender, RoutedEventArgs e)
        {
            try
            {
                // 1. Перевірка порожніх полів перед зчитуванням
                if (string.IsNullOrWhiteSpace(txtWeight.Text) || string.IsNullOrWhiteSpace(txtHeight.Text))
                {
                    txtResult.Text = "Помилка: Будь ласка, заповніть усі поля!";
                    return;
                }

                // 2. Отримання даних з форми (захищено від помилок введення крапки/коми)
                double weight = ParseDoubleSafe(txtWeight.Text);
                double heightCm = ParseDoubleSafe(txtHeight.Text);

                // Перевірка валідності значень
                if (weight <= 0 || heightCm <= 0)
                {
                    txtResult.Text = "Помилка: Значення ваги та зросту повинні бути більшими за 0!";
                    return;
                }

                // 3. Розрахунок ІМТ
                double heightM = heightCm / 100.0; // Конвертуємо сантиметри в метри
                double bmi = weight / (heightM * heightM);
                bmi = Math.Round(bmi, 1); // Округлення до 1 знаку після коми

                // 4. Визначення категорії (switch-case конструкція)
                string category = bmi switch
                {
                    < 18.5 => "Недостатня вага",
                    >= 18.5 and < 25.0 => "Норма",
                    >= 25.0 and < 30.0 => "Надмірна вага",
                    _ => "Ожиріння"
                };

                // Формування фінального тексту
                string resultText = $"Вхідні дані:\nВага: {weight} кг\nЗріст: {heightCm} см\n\n" +
                                   $"Результат:\nІМТ = {bmi}\nКатегорія: {category}";

                // 5. Виведення на екран та збереження результату у файл
                txtResult.Text = resultText;
                File.WriteAllText(OutputFile, resultText);
            }
            catch (FormatException)
            {
                txtResult.Text = "Помилка: Некоректний формат чисел! Вводьте лише цифри.";
            }
            catch (Exception ex)
            {
                txtResult.Text = $"Непередбачувана помилка: {ex.Message}";
            }
        }

        // Допоміжний метод для безпечної конвертації рядка в дробове число
        private double ParseDoubleSafe(string input)
        {
            string cleanInput = input.Trim().Replace(',', '.');
            return double.Parse(cleanInput, CultureInfo.InvariantCulture);
        }
    }
}
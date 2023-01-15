using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GuessTheNumber
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            GenerateWinningNumber("Novice");
            Loaded += Window_Loaded;
        }

        private void GenerateWinningNumber(string difficulty)
        {
            Random random = new();
            int lowerBounds = 0;
            int upperBounds;
            switch (difficulty)
            {
                case "Novice":
                    upperBounds = 3;
                    break;
                case "Average":
                    upperBounds = 10;
                    break;
                case "Experienced":
                    upperBounds = 25;
                    break;
                case "Skilled":
                    upperBounds = 50;
                    break;
                case "Adept":
                    upperBounds = 75;
                    break;
                case "Masterful":
                    upperBounds = 100;
                    break;
                case "Inhuman":
                    upperBounds = 150;
                    break;
                case "Godlike":
                    upperBounds = 250;
                    break;
                default:
                    upperBounds = 3;
                    break;
            }

            int winningNumber = random.Next(lowerBounds, upperBounds + 1);
            WinningNumberTextBlock.Text = winningNumber.ToString();
        }

        private void KeypadButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string buttonContent = button.Content as string;

            if (buttonContent != null && GuessedNumberTextBlock.Text.Length < 3)
            {
                GuessedNumberTextBlock.Text += buttonContent;
            }
        }

        private void CheckMarkButton_Click(object sender, RoutedEventArgs e)
        {
            SubmitGuess();
        }

        private void DifficultyRadioButton_Click(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            RestartGame(radioButton.Name);
        }

        private void RestartGame(string difficulty)
        {
            WinningNumberTextBlock.Background = new SolidColorBrush(Colors.Black);
            GenerateWinningNumber(difficulty);
            GuessedNumberTextBlock.Text = null;
            GuessedNumberTextBlock.Background = new SolidColorBrush(Colors.White);
        }

        private void SubmitGuess()
        {
            if (!(String.IsNullOrEmpty(GuessedNumberTextBlock.Text)))
            {
                WinningNumberTextBlock.Background = new SolidColorBrush(Colors.White);
                if (GuessedNumberTextBlock.Text == WinningNumberTextBlock.Text)
                {
                    GuessedNumberTextBlock.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#A3BE8C");
                }
                else
                {
                    GuessedNumberTextBlock.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#BF616A");
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IEnumerable<Button> buttons = GetChildButtons(KeypadGrid);
            foreach (Button button in buttons)
            {
                if (button != null && button.Name is not "CheckMarkButton")
                {
                    button.Click += KeypadButton_Click;
                }
                else if (button != null)
                {
                    button.Click += CheckMarkButton_Click;
                }
            }

            IEnumerable<RadioButton> radioButtons = GetChildRadioButtons(DifficultiesGrid);
            foreach (RadioButton radioButton in radioButtons)
            {
                if (radioButton != null)
                {
                    radioButton.Click += DifficultyRadioButton_Click;
                }
            }
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<Button> GetChildButtons(DependencyObject parent)
        {
            var children = LogicalTreeHelper.GetChildren(parent);
            var buttons = new List<Button>();
            foreach (var child in children)
            {
                if (child is Button)
                {
                    buttons.Add((Button)child);
                }
                else if (child is DependencyObject)
                {
                    buttons.AddRange(GetChildButtons((DependencyObject)child));
                }
            }
            return buttons;
        }

        private IEnumerable<RadioButton> GetChildRadioButtons(DependencyObject parent)
        {
            var children = LogicalTreeHelper.GetChildren(parent);
            var radioButtons = new List<RadioButton>();
            foreach (var child in children)
            {
                if (child is RadioButton)
                {
                    radioButtons.Add((RadioButton)child);
                }
                else if (child is DependencyObject)
                {
                    radioButtons.AddRange(GetChildRadioButtons((DependencyObject)child));
                }
            }

            return radioButtons;
        }


    }
}

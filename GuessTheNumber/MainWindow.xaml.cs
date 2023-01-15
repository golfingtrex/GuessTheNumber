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
            GenerateWinningNumber("Novice"); // Starting difficulty of "Novice".
            Loaded += Window_Loaded;
        }

        /// <summary>
        /// Generates a random number between 0 and a positive value based on selected difficulty called winningNumber
        /// and sets the Text property of the WinningNumberTextBlock to winningNumber.
        /// </summary>
        /// <param name="difficulty">The difficulty setting of the current game.</param>
        private void GenerateWinningNumber(string difficulty)
        {
            Random random = new();
            int lowerBounds = 0;
            int upperBounds = difficulty switch
            {
                "Novice" => 3,
                "Average" => 10,
                "Experienced" => 25,
                "Skilled" => 50,
                "Adept" => 75,
                "Masterful" => 100,
                "Inhuman" => 150,
                "Godlike" => 250,
                _ => 3,
            };
            int winningNumber = random.Next(lowerBounds, upperBounds + 1);

            WinningNumberTextBlock.Text = winningNumber.ToString();
        }

        /// <summary>
        /// Sets the Click event for all Buttons and RadioButtons in the window.
        /// </summary>
        /// <param name="sender">The MainWindow.</param>
        /// <param name="e">TODO - idk much about this.</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IEnumerable<Button> buttons = GetChildButtons(KeypadGrid);
            foreach (Button button in buttons)
            {
                if (button != null && button.Name != "CheckMarkButton")
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

        /// <summary>
        /// Collects all Buttons in the window.
        /// </summary>
        /// <param name="parent"></param>
        /// <returns>An IEnumerable collection of Buttons.</returns>
        private static IEnumerable<Button> GetChildButtons(DependencyObject parent)
        {
            var children = LogicalTreeHelper.GetChildren(parent);
            var buttons = new List<Button>();
            foreach (var child in children)
            {
                if (child is Button button)
                {
                    buttons.Add(button);
                }
                else if (child is DependencyObject dependencyObject)
                {
                    buttons.AddRange(GetChildButtons(dependencyObject));
                }
            }
            return buttons;
        }

        /// <summary>
        /// Collects all RadioButtons in the window.
        /// </summary>
        /// <param name="parent"></param>
        /// <returns>An IEnumerable collection of RadioButtons.</returns>
        private static IEnumerable<RadioButton> GetChildRadioButtons(DependencyObject parent)
        {
            var children = LogicalTreeHelper.GetChildren(parent);
            var radioButtons = new List<RadioButton>();
            foreach (var child in children)
            {
                if (child is RadioButton button)
                {
                    radioButtons.Add(button);
                }
                else if (child is DependencyObject dependencyObject)
                {
                    radioButtons.AddRange(GetChildRadioButtons(dependencyObject));
                }
            }

            return radioButtons;
        }

        /// <summary>
        /// Appends the content of the keypad button that is clicked to the GuessedNumberTextBlock Text property,
        /// but only if the current length has three or less digits.
        /// </summary>
        /// <param name="sender">The keypad Button that was clicked.</param>
        /// <param name="e">TODO - idk much about this.</param>
        private void KeypadButton_Click(object sender, RoutedEventArgs e)
        {
            Button? button = sender as Button;

            if (button?.Content is string buttonContent && GuessedNumberTextBlock.Text.Length < 3)
            {
                GuessedNumberTextBlock.Text += buttonContent;
            }
        }

        /// <summary>
        /// Reveals the winning number and sets the background of the guessed number to green if it's a match, sets it to
        /// red if not.
        /// </summary>
        /// <param name="sender">The Button that was clicked. i.e. the green check mark Button.</param>
        /// <param name="e">TODO - idk much about this.</param>
        private void CheckMarkButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(String.IsNullOrEmpty(GuessedNumberTextBlock.Text)))
            {
                WinningNumberTextBlock.Background = new SolidColorBrush(Colors.White);
                if (GuessedNumberTextBlock.Text == WinningNumberTextBlock.Text)
                {
                    GuessedNumberTextBlock.Background = new BrushConverter().ConvertFromString("#A3BE8C") as SolidColorBrush;
                }
                else
                {
                    GuessedNumberTextBlock.Background = new BrushConverter().ConvertFromString("#BF616A") as SolidColorBrush;
                }
            }
        }

        /// <summary>
        /// Calls RestartGame with the Name of the RadioButton as the difficulty.
        /// </summary>
        /// <param name="sender">The RadioButton that was checked.</param>
        /// <param name="e">TODO - idk much about this.</param>
        private void DifficultyRadioButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radioButton)
            {
                RestartGame(radioButton.Name);
            }
        }

        /// <summary>
        /// Sets the Background property of the WinningNumberTextBlock to Black, generates a new winning number,
        /// clears out the user's guessed number and resets the Background of the GuessedNumberTextBlock to White.
        /// </summary>
        /// <param name="difficulty">The difficulty setting of the new game. Used in generating the winning number.</param>
        private void RestartGame(string difficulty)
        {
            WinningNumberTextBlock.Background = new SolidColorBrush(Colors.Black);
            GenerateWinningNumber(difficulty);
            GuessedNumberTextBlock.Text = null;
            GuessedNumberTextBlock.Background = new SolidColorBrush(Colors.White);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Xps;

namespace kalkulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int plusCounter = 0;
        private int minusCounter = 0;
        private int multiCounter = 0;
        private int divideCounter = 0;
        private bool isPlusSign = true;
        private bool operation;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CheckForSignAndEmptyTb()
        {
            if (isPlusSign)
            {
                if (tbwynik.Text == "0")
                {
                    tbwynik.Text = "";
                }
            }
            else
            {
                if (tbwynik.Text == "-0")
                {
                    tbwynik.Text = "-";
                }
            }
        }

        private void OnOperationExecuting()
        {
            if (operation)
            {
                tbwynik.Text = "";
            }
        }

        private void CheckToBlockButtons(bool shouldBlock, string type)
        {
            if (!shouldBlock && type == "divide")
            {
                ChangeFontOfTB(20);
                lpamiec.Content = "Nie wolno dzielić przez zero";
                tbwynik.Text = "Naciśnij C, aby kontynuować";
            }

            if (!shouldBlock && type == "sqrt")
            {
                ChangeFontOfTB(20);
                lpamiec.Content = "Nieprawidłowe dane wejściowe";
                tbwynik.Text = "Naciśnij C, aby kontynuować";
            }
            ZeroButton.IsEnabled = shouldBlock;
            OneButton.IsEnabled = shouldBlock;
            TwoButton.IsEnabled = shouldBlock;
            ThreeButton.IsEnabled = shouldBlock;
            FourButton.IsEnabled = shouldBlock;
            FiveButton.IsEnabled = shouldBlock;
            SixButton.IsEnabled = shouldBlock;
            SevenButton.IsEnabled = shouldBlock;
            EightButton.IsEnabled = shouldBlock;
            NineButton.IsEnabled = shouldBlock;
            BackButton.IsEnabled = shouldBlock;
            CEButton.IsEnabled = shouldBlock;
            PlusMinusButton.IsEnabled = shouldBlock;
            SqrtButton.IsEnabled = shouldBlock;
            DivideButton.IsEnabled = shouldBlock;
            PercentButton.IsEnabled = shouldBlock;
            MultiplyButton.IsEnabled = shouldBlock;
            OneByXButton.IsEnabled = shouldBlock;
            MinusButton.IsEnabled = shouldBlock;
            PlusButton.IsEnabled = shouldBlock;
            DotButton.IsEnabled = shouldBlock;
            EqualsButton.IsEnabled = shouldBlock;
        }

        private void ChangeFontOfTB(int fontSize)
        {
            tbwynik.FontSize = fontSize;
        }

        private bool CheckForTBLength()
        {
            return tbwynik.Text.Length >= 15;
        }

        private void CutStringOnLimitBroke(bool isLimitBroken)
        {
            if (isLimitBroken)
            {
                tbwynik.Text = tbwynik.Text.Substring(0, 15);
            }
        }

        private bool CheckIfContainsSign()
        {
            char sign = lpamiec.Content.ToString()[^1];
            return sign == (char)43 || sign == (char)45 || sign == (char)215 || sign == (char)247;
        }

        private string ResultOfOperation(double firstValue, double secondValue, char sign, bool isEqualClick)
        {
            double result = 0;
            switch (sign)
            {
                case (char)43:
                    result = firstValue + secondValue;
                    break;
                case (char)45:
                    result = firstValue - secondValue;
                    break;
                case (char)215:
                    result = firstValue * secondValue;
                    break;
                case (char)247:
                    if (secondValue == 0f)
                    {
                        CheckToBlockButtons(false, "divide");
                    }

                    result = firstValue / secondValue;
                    break;
            }


            if (result.ToString().Length > 15)
            {
                if (result.ToString().Contains('E'))
                {
                    string resultAsString = result.ToString();
                    string[] resultParted = resultAsString.Split('E');
                    resultParted[1] = resultParted[1].Insert(0, "E");
                    int lengthToSubString = 15 - resultParted[1].Length;
                    string resultWithE = resultParted[0].Substring(0, lengthToSubString) + resultParted[1];
                    return isEqualClick ? resultWithE : resultWithE + sign;
                }

                if (!isPlusSign)
                {
                    return isEqualClick ? result.ToString("E", CultureInfo.InvariantCulture).Substring(0, 14) : result.ToString("E", CultureInfo.InvariantCulture).Substring(0, 14) + sign;
                }
                return isEqualClick ? result.ToString("E", CultureInfo.InvariantCulture) : result.ToString("E", CultureInfo.InvariantCulture) + sign;   
            }
            return isEqualClick ? result.ToString() : result.ToString() + sign;
        }

        private double ChangeDotToCommaAndParseToDouble(string text)
        {
            if (text.Contains("."))
            {
                text = text.Replace(".", ",");
            }
            return double.Parse(text);
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            if (tbwynik.Text == "0" || tbwynik.Text == "-0") return;
            tbwynik.Text = tbwynik.Text.Remove(tbwynik.Text.Length - 1, 1);
            if (!isPlusSign)
            {
                if (tbwynik.Text.Length == 1 && tbwynik.Text.Contains("-"))
                {
                    tbwynik.Text = "-0";
                }
            }
            else
            {
                if (tbwynik.Text.Length == 0)
                {
                    tbwynik.Text = "0";
                }
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TBKeyDown(object sender, KeyEventArgs e)
        {
            // if (!isPlusSign && tbwynik.Text.Contains("0") && tbwynik.Text.Length == 2)
            // {
            //     tbwynik.Text = "-";
            //     tbwynik.Select(tbwynik.Text.Length, 0);
            // }
            // if (tbwynik.Text == "0")
            // {
            //     tbwynik.Text = "";
            // }
            
            string labelValue;
            double firstValue;
            double secondValue;
            switch (e.Key)
            {
                case Key.NumPad0:
                case Key.NumPad1:
                case Key.NumPad2:
                case Key.NumPad3:
                case Key.NumPad4:
                case Key.NumPad5:
                case Key.NumPad6:
                case Key.NumPad7:
                case Key.NumPad8:
                case Key.NumPad9:
                    CheckForSignAndEmptyTb();
                    OnOperationExecuting();
                    if (CheckForTBLength())
                    {
                        return;
                    }
                    operation = false;
                    break;
                case Key.Add:
                    if ((string)lpamiec.Content == null)
                    {
                        lpamiec.Content = tbwynik.Text + "+";
                    }
                    else
                    {
                        if (CheckIfContainsSign() && (lpamiec.Content.ToString()[^1] != (char)43))
                        {
                            lpamiec.Content = lpamiec.Content.ToString().Replace(lpamiec.Content.ToString()[^1], (char)43);
                            return;
                        }
                        labelValue = lpamiec.Content.ToString().Remove(lpamiec.Content.ToString().Length - 1);
                        firstValue = double.Parse(labelValue, CultureInfo.InvariantCulture);
                        secondValue = double.Parse(tbwynik.Text, CultureInfo.InvariantCulture);
                        lpamiec.Content = ResultOfOperation(firstValue, secondValue, (char)43, false);
                    }
                    operation = true;
                    break;
                case Key.Subtract:
                    if ((string)lpamiec.Content == null)
                    {
                        lpamiec.Content = tbwynik.Text + "-";
                    }
                    else
                    {
                        if (CheckIfContainsSign() && (lpamiec.Content.ToString()[^1] != (char)45))
                        {
                            lpamiec.Content = lpamiec.Content.ToString().Replace(lpamiec.Content.ToString()[^1], (char)45);
                            return;
                        }
                        labelValue = lpamiec.Content.ToString().Remove(lpamiec.Content.ToString().Length - 1);
                        firstValue = double.Parse(labelValue, CultureInfo.InvariantCulture);
                        secondValue = double.Parse(tbwynik.Text, CultureInfo.InvariantCulture);
                        lpamiec.Content = ResultOfOperation(firstValue, secondValue, (char)45, false);
                    }
                    operation = true;
                    break;
                case Key.Multiply:
                    if ((string)lpamiec.Content == null)
                    {
                        lpamiec.Content = tbwynik.Text + "\u00D7";
                    }
                    else
                    {
                        if (CheckIfContainsSign() && (lpamiec.Content.ToString()[^1] != (char)215))
                        {
                            lpamiec.Content = lpamiec.Content.ToString().Replace(lpamiec.Content.ToString()[^1], (char)215);
                            return;
                        }
                        labelValue = lpamiec.Content.ToString().Remove(lpamiec.Content.ToString().Length - 1);
                        firstValue = double.Parse(labelValue, CultureInfo.InvariantCulture);
                        secondValue = double.Parse(tbwynik.Text, CultureInfo.InvariantCulture);
                        lpamiec.Content = ResultOfOperation(firstValue, secondValue, (char)215, false);
                    }
                    operation = true;
                    break;
                case Key.Divide:
                    if ((string)lpamiec.Content == null)
                    {
                        lpamiec.Content = tbwynik.Text + "\u00F7";
                    }
                    else
                    {
                        if (CheckIfContainsSign() && (lpamiec.Content.ToString()[^1] != (char)247))
                        {
                            lpamiec.Content = lpamiec.Content.ToString().Replace(lpamiec.Content.ToString()[^1], (char)247);
                            return;
                        }
                        labelValue = lpamiec.Content.ToString().Remove(lpamiec.Content.ToString().Length - 1);
                        firstValue = double.Parse(labelValue, CultureInfo.InvariantCulture);
                        secondValue = double.Parse(tbwynik.Text, CultureInfo.InvariantCulture);
                        lpamiec.Content = ResultOfOperation(firstValue, secondValue, (char)247, false);
                    }
                    operation = true;
                    break;
                case Key.Enter:
                    double result = 0;
                    operation = false;
                    if (lpamiec.Content == null) return;
                    char sign = lpamiec.Content.ToString()[^1];
                    labelValue = lpamiec.Content.ToString().Remove(lpamiec.Content.ToString().Length - 1);
                    // if (labelValue.Contains("1" + "\u00F7" + "x")) return;
                    firstValue = ChangeDotToCommaAndParseToDouble(labelValue);
                    secondValue = ChangeDotToCommaAndParseToDouble(tbwynik.Text);
                    tbwynik.Text = ResultOfOperation(firstValue, secondValue, sign, true);
                    lpamiec.Content = null;
                    tbwynik.Select(tbwynik.Text.Length, 0);
                    break;
                case Key.Decimal:
                    if (tbwynik.Text.Contains(",")) return;
                    tbwynik.Text = tbwynik.Text.Insert(tbwynik.Text.Length, ",");
                    break;
            }
        }

        private void TBKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back || e.Key == Key.Delete)
            {
                if (!isPlusSign && tbwynik.Text.Length == 0)
                {
                    tbwynik.Text = "-0";
                    tbwynik.Select(tbwynik.Text.Length, 0);
                }
                else if (tbwynik.Text == "")
                {
                    tbwynik.Text = "0";
                    tbwynik.Select(tbwynik.Text.Length, 0);
                }
            }

            CutStringOnLimitBroke(CheckForTBLength());
            tbwynik.Select(tbwynik.Text.Length, 0);
            
        }

        private void CEClick(object sender, RoutedEventArgs e)
        {
            tbwynik.Text = "0";
            isPlusSign = true;
            operation = false;
        }

        private void CClick(object sender, RoutedEventArgs e)
        {
            tbwynik.Text = "0";
            lpamiec.Content = null;
            isPlusSign = true;
            operation = false;
            CheckToBlockButtons(true, "nothing");
            ChangeFontOfTB(30);
        }

        private void PlusMinusClick(object sender, RoutedEventArgs e)
        {
            if (isPlusSign)
            {
                tbwynik.Text = tbwynik.Text.Insert(0, "-");
            }
            else
            {
                if (tbwynik.Text.Contains("-"))
                {
                    tbwynik.Text = tbwynik.Text.Remove(0, 1);
                }
            }

            isPlusSign = !isPlusSign;
        }

        private void SevenClick(object sender, RoutedEventArgs e)
        {
            CheckForSignAndEmptyTb();
            OnOperationExecuting();
            if (CheckForTBLength()) return;
            tbwynik.Text = tbwynik.Text.Insert(tbwynik.Text.Length, "7");
            operation = false;
        }

        private void EightClick(object sender, RoutedEventArgs e)
        {
            CheckForSignAndEmptyTb();
            OnOperationExecuting();
            if (CheckForTBLength()) return;
            tbwynik.Text = tbwynik.Text.Insert(tbwynik.Text.Length, "8");
            operation = false;
        }

        private void NineClick(object sender, RoutedEventArgs e)
        {
            CheckForSignAndEmptyTb();
            OnOperationExecuting();
            if (CheckForTBLength()) return;
            tbwynik.Text = tbwynik.Text.Insert(tbwynik.Text.Length, "9");
            operation = false;
        }

        private void FourClick(object sender, RoutedEventArgs e)
        {
            CheckForSignAndEmptyTb();
            OnOperationExecuting();
            if (CheckForTBLength()) return;
            tbwynik.Text = tbwynik.Text.Insert(tbwynik.Text.Length, "4");
            operation = false;
        }

        private void FiveClick(object sender, RoutedEventArgs e)
        {
            CheckForSignAndEmptyTb();
            OnOperationExecuting();
            if (CheckForTBLength()) return;
            tbwynik.Text = tbwynik.Text.Insert(tbwynik.Text.Length, "5");
            operation = false;
        }

        private void SixClick(object sender, RoutedEventArgs e)
        {
            CheckForSignAndEmptyTb();
            OnOperationExecuting();
            if (CheckForTBLength()) return;
            tbwynik.Text = tbwynik.Text.Insert(tbwynik.Text.Length, "6");
            operation = false;
        }

        private void OneClick(object sender, RoutedEventArgs e)
        {
            CheckForSignAndEmptyTb();
            OnOperationExecuting();
            if (CheckForTBLength()) return;
            tbwynik.Text = tbwynik.Text.Insert(tbwynik.Text.Length, "1");
            operation = false;
        }

        private void TwoClick(object sender, RoutedEventArgs e)
        {
            CheckForSignAndEmptyTb();
            OnOperationExecuting();
            if (CheckForTBLength()) return;
            tbwynik.Text = tbwynik.Text.Insert(tbwynik.Text.Length, "2");
            operation = false;
        }

        private void ThreeClick(object sender, RoutedEventArgs e)
        {
            CheckForSignAndEmptyTb();
            OnOperationExecuting();
            if (CheckForTBLength()) return;
            tbwynik.Text = tbwynik.Text.Insert(tbwynik.Text.Length, "3");
            operation = false;
        }

        private void ZeroClick(object sender, RoutedEventArgs e)
        {
            CheckForSignAndEmptyTb();
            OnOperationExecuting();
            if (CheckForTBLength()) return;
            tbwynik.Text = tbwynik.Text.Insert(tbwynik.Text.Length, "0");
            operation = false;
        }

        private void DotClick(object sender, RoutedEventArgs e)
        {
            if (tbwynik.Text.Contains(",")) return;
            tbwynik.Text = tbwynik.Text.Insert(tbwynik.Text.Length, ",");
        }

        private void SqrtClick(object sender, RoutedEventArgs e)
        {
            var wynik = ChangeDotToCommaAndParseToDouble(tbwynik.Text);
            if (double.IsNegative(wynik))
            {
                CheckToBlockButtons(false, "sqrt");
                return;
            }

            tbwynik.Text = Math.Sqrt(wynik).ToString(CultureInfo.InvariantCulture);
            CutStringOnLimitBroke(CheckForTBLength());
        }

        private void PercentClick(object sender, RoutedEventArgs e)
        {
            var wynik = ChangeDotToCommaAndParseToDouble(tbwynik.Text);
            // lpamiec.Content = "\u0025" + "(" + tbwynik.Text + ")";
            tbwynik.Text = (wynik / 100).ToString(CultureInfo.InvariantCulture);
            CutStringOnLimitBroke(CheckForTBLength());
        }

        private void OneByXClick(object sender, RoutedEventArgs e)
        {
            var wynik = ChangeDotToCommaAndParseToDouble(tbwynik.Text);
            tbwynik.Text = (1 / wynik).ToString(CultureInfo.InvariantCulture);
            CutStringOnLimitBroke(CheckForTBLength());

        }

        private void DivideClick(object sender, RoutedEventArgs e)
        {
            if ((string)lpamiec.Content == null)
            {
                lpamiec.Content = tbwynik.Text + "\u00F7";
            }
            else
            {
                if (CheckIfContainsSign() && (lpamiec.Content.ToString()[^1] != (char)247))
                {
                    lpamiec.Content = lpamiec.Content.ToString().Replace(lpamiec.Content.ToString()[^1], (char)247);
                    return;
                }
                string labelValue = lpamiec.Content.ToString().Remove(lpamiec.Content.ToString().Length - 1);
                double firstValue = double.Parse(labelValue, CultureInfo.InvariantCulture);
                double secondValue = double.Parse(tbwynik.Text, CultureInfo.InvariantCulture);
                lpamiec.Content = ResultOfOperation(firstValue, secondValue, (char)247, false);
            }
            operation = true;
        }

        private void MultiplyClick(object sender, RoutedEventArgs e)
        {
            if ((string)lpamiec.Content == null)
            {
                lpamiec.Content = tbwynik.Text + "\u00D7";
            }
            else
            {
                if (CheckIfContainsSign() && (lpamiec.Content.ToString()[^1] != (char)215))
                {
                    lpamiec.Content = lpamiec.Content.ToString().Replace(lpamiec.Content.ToString()[^1], (char)215);
                    return;
                }
                string labelValue = lpamiec.Content.ToString().Remove(lpamiec.Content.ToString().Length - 1);
                double firstValue = double.Parse(labelValue, CultureInfo.InvariantCulture);
                double secondValue = double.Parse(tbwynik.Text, CultureInfo.InvariantCulture);
                lpamiec.Content = ResultOfOperation(firstValue, secondValue, (char)215, false);
            }
            operation = true;
        }

        private void MinusClick(object sender, RoutedEventArgs e)
        {

            if ((string)lpamiec.Content == null)
            {
                lpamiec.Content = tbwynik.Text + "-";
            }
            else
            {
                if (CheckIfContainsSign() && (lpamiec.Content.ToString()[^1] != (char)45))
                {
                    lpamiec.Content = lpamiec.Content.ToString().Replace(lpamiec.Content.ToString()[^1], (char)45);
                    return;
                }
                string labelValue = lpamiec.Content.ToString().Remove(lpamiec.Content.ToString().Length - 1);
                double firstValue = double.Parse(labelValue, CultureInfo.InvariantCulture);
                double secondValue = double.Parse(tbwynik.Text, CultureInfo.InvariantCulture);
                lpamiec.Content = ResultOfOperation(firstValue, secondValue, (char)45, false);
            }
            operation = true;
        }

        private void PlusClick(object sender, RoutedEventArgs e)
        {
            if ((string)lpamiec.Content == null)
            {
                lpamiec.Content = tbwynik.Text + "+";
            }
            else
            {
                if (CheckIfContainsSign() && (lpamiec.Content.ToString()[^1] != (char)43))
                {
                    lpamiec.Content = lpamiec.Content.ToString().Replace(lpamiec.Content.ToString()[^1], (char)43);
                    return;
                }
                string labelValue = lpamiec.Content.ToString().Remove(lpamiec.Content.ToString().Length - 1);
                double firstValue = double.Parse(labelValue, CultureInfo.InvariantCulture);
                double secondValue = double.Parse(tbwynik.Text, CultureInfo.InvariantCulture);
                lpamiec.Content = ResultOfOperation(firstValue, secondValue, (char)43, false);
            }
            operation = true;
        }

        private void EqualsClick(object sender, RoutedEventArgs e)
        {
            double result = 0;
            operation = false;
            if (lpamiec.Content == null) return;
            char sign = lpamiec.Content.ToString()[^1];
            string labelValue = lpamiec.Content.ToString().Remove(lpamiec.Content.ToString().Length - 1);
            // if (labelValue.Contains("1" + "\u00F7" + "x")) return;
            double firstValue = ChangeDotToCommaAndParseToDouble(labelValue);
            double secondValue = ChangeDotToCommaAndParseToDouble(tbwynik.Text);
            tbwynik.Text = ResultOfOperation(firstValue, secondValue, sign, true);
            lpamiec.Content = null;
        }
    }
}
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ElectronicShop.App.Behaviors
{
    public static class NumericInputBehavior
    {
        public static readonly DependencyProperty AllowDecimalProperty =
            DependencyProperty.RegisterAttached(
                "AllowDecimal",
                typeof(bool),
                typeof(NumericInputBehavior),
                new PropertyMetadata(false, OnAllowDecimalChanged));

        public static bool GetAllowDecimal(DependencyObject obj) => (bool)obj.GetValue(AllowDecimalProperty);
        public static void SetAllowDecimal(DependencyObject obj, bool value) => obj.SetValue(AllowDecimalProperty, value);

        private static void OnAllowDecimalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not TextBox textBox) return;

            textBox.PreviewTextInput -= TextBox_PreviewTextInput;
            textBox.PreviewTextInput += TextBox_PreviewTextInput;

            DataObject.RemovePastingHandler(textBox, OnPaste);
            DataObject.AddPastingHandler(textBox, OnPaste);
        }

        private static void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = (TextBox)sender;
            var allowDecimal = GetAllowDecimal(textBox);
            var proposedText = GetProposedText(textBox, e.Text);

            e.Handled = !IsValid(proposedText, allowDecimal);
        }

        private static void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            var textBox = (TextBox)sender;
            var allowDecimal = GetAllowDecimal(textBox);

            if (e.DataObject.GetData(typeof(string)) is not string pastedText ||
                !IsValid(GetProposedText(textBox, pastedText), allowDecimal))
            {
                e.CancelCommand();
            }
        }

        private static string GetProposedText(TextBox textBox, string newText)
        {
            var text = textBox.Text;
            var selectionStart = textBox.SelectionStart;
            var selectionLength = textBox.SelectionLength;

            return text.Remove(selectionStart, selectionLength).Insert(selectionStart, newText);
        }

        private static bool IsValid(string text, bool allowDecimal)
        {
            if (string.IsNullOrEmpty(text)) return true;

            var pattern = allowDecimal ? @"^\d*\.?\d{0,2}$" : @"^\d*$";
            return Regex.IsMatch(text, pattern);
        }
    }
}
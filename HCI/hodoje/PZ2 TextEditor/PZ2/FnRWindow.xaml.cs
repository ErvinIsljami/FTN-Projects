using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Linq;
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
using System.Windows.Shapes;

namespace PZ2
{
    /// <summary>
    /// Interaction logic for FnRWindow.xaml
    /// </summary>
    public partial class FnRWindow : Window
    {
        private static List<int> fontSizes = MainWindow.fontSizes;

        private static List<RichTextBox> rtbList = MainWindow.rtbList;
        private static List<string> activeRtbFilePath = MainWindow.activeRtbFilePath;
        private static List<string> activeRtbFormatAsString = MainWindow.activeRtbFormatAsString;
        public static List<bool> activeRtbChanged = MainWindow.activeRtbChanged;

        private static int index = MainWindow.index;

        public FnRWindow()
        {
            InitializeComponent();
            FindTextBox.Focus();
        }

        private void FindAndReplaceButton_OnClick(object sender, RoutedEventArgs e)
        {
            string findWord = FindTextBox.Text;
            string replaceWord = ReplaceTextBox.Text;

            if (findWord.Equals(String.Empty) || replaceWord.Equals(String.Empty))
            {
                MessageBoxResult emptyInput = MessageBox.Show("You need to enter both 'Find' and 'Replace' arguments.",
                    "Find & Replace", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                TextRange text = new TextRange(rtbList[index].Document.ContentStart, rtbList[index].Document.ContentEnd);
                TextPointer current = text.Start.GetInsertionPosition(LogicalDirection.Forward);

                while (current != null)
                {
                    string textInRun = current.GetTextInRun(LogicalDirection.Forward);
                    if (!string.IsNullOrWhiteSpace(textInRun))
                    {
                        int idx = textInRun.IndexOf(findWord);
                        if (idx != -1)
                        {
                            TextPointer selectionStart = current.GetPositionAtOffset(idx, LogicalDirection.Forward);
                            TextPointer selectionEnd = selectionStart.GetPositionAtOffset(findWord.Length, LogicalDirection.Forward);
                            TextRange selection = new TextRange(selectionStart, selectionEnd);

                            object o = selection.GetPropertyValue(TextElement.FontFamilyProperty);                  // For some reason, it has to be like this
                            object o1 = selection.GetPropertyValue(TextElement.ForegroundProperty);
                            object o2 = selection.GetPropertyValue(Inline.TextDecorationsProperty);
                            object o3 = selection.GetPropertyValue(TextElement.FontWeightProperty);
                            object o4 = selection.GetPropertyValue(TextElement.FontStyleProperty);
                            object o5 = selection.GetPropertyValue(Inline.FontSizeProperty);

                            selection.Text = replaceWord;

                            selection.ApplyPropertyValue(TextElement.FontFamilyProperty, o);
                            selection.ApplyPropertyValue(TextElement.ForegroundProperty, o1);
                            selection.ApplyPropertyValue(Inline.TextDecorationsProperty, o2);
                            selection.ApplyPropertyValue(TextElement.FontWeightProperty, o3);
                            selection.ApplyPropertyValue(TextElement.FontStyleProperty, o4);
                            selection.ApplyPropertyValue(Inline.FontSizeProperty, o5);

                            rtbList[index].Focus();
                        }
                    }
                    current = current.GetNextContextPosition(LogicalDirection.Forward);
                }
            }
        }

        private void FnRWindow_OnClosing(object sender, CancelEventArgs e)
        {
            rtbList[index].Focus();
        }
    }
}

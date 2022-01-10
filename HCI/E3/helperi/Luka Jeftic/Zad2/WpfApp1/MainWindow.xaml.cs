using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public static string find;
        public static string replace;

        public MainWindow()
        {
            InitializeComponent();
            cmbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);

            RoutedCommand realDateCmd = new RoutedCommand();
            realDateCmd.InputGestures.Add(new KeyGesture(Key.F5, ModifierKeys.None));
            CommandBindings.Add(new CommandBinding(realDateCmd, Button_Date));

            for (double i = 6; i <= 12; ++i)
            {
                cmbFontSize.Items.Add(i);
            }
            cmbFontSize.Items.Add(14.0);
            cmbFontSize.Items.Add(16.0);
            cmbFontSize.Items.Add(18.0);
            cmbFontSize.Items.Add(20.0);
            cmbFontSize.Items.Add(22.0);
            cmbFontSize.Items.Add(24.0);
            cmbFontSize.Items.Add(36.0);
            cmbFontSize.Items.Add(48.0);
            cmbFontSize.Items.Add(72.0);

            cmbFontSize.Text = rtbEditor.FontSize.ToString();

            btn_ChangeColor.Background = rtbEditor.Foreground;



        }

        private void cmbFontFamily_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cmbFontFamily.SelectedItem != null)
            {
                rtbEditor.Focus();
                rtbEditor.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, cmbFontFamily.SelectedItem);
            }
        }

        private void cmbFontSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbFontSize.SelectedItem != null)
            {
                rtbEditor.Focus();
                rtbEditor.Selection.ApplyPropertyValue(RichTextBox.FontSizeProperty, cmbFontSize.SelectedItem);
            }
        }

        private void btn_ChangeColor_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.ColorDialog dialog = new System.Windows.Forms.ColorDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(dialog.Color.A, dialog.Color.R, dialog.Color.G, dialog.Color.B));
                rtbEditor.Selection.ApplyPropertyValue(Control.ForegroundProperty, brush);
                btn_ChangeColor.Background = brush;
            }
        }






        private void rtbEditor_SelectionChanged(object sender, RoutedEventArgs e)
        {
            object temp = rtbEditor.Selection.GetPropertyValue(Inline.FontWeightProperty);
            btnBold.IsChecked = (temp != DependencyProperty.UnsetValue) &&
                (temp.Equals(FontWeights.Bold));

            object temp1 = rtbEditor.Selection.GetPropertyValue(Inline.FontStyleProperty);
            btnItalic.IsChecked = (temp1 != DependencyProperty.UnsetValue) &&
                (temp1.Equals(FontStyles.Italic));

            object temp2 = rtbEditor.Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            btnUnderline.IsChecked = (temp2 != DependencyProperty.UnsetValue) &&
                (temp2.Equals(TextDecorations.Underline));

            object temp3 = rtbEditor.Selection.GetPropertyValue(Inline.FontFamilyProperty);
            cmbFontFamily.SelectedItem = temp3;

            string fileText = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd).Text;

            tb_WordCount.Text = WordCount(fileText).ToString();

            object size = rtbEditor.Selection.GetPropertyValue(RichTextBox.FontSizeProperty);
            if (size != DependencyProperty.UnsetValue)
                cmbFontSize.SelectedValue = (double)size;

            object color = rtbEditor.Selection.GetPropertyValue(Control.ForegroundProperty);
            if (color != DependencyProperty.UnsetValue)
                btn_ChangeColor.Background = (SolidColorBrush)color;


        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            string fileText = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd).Text;
            if (!string.IsNullOrWhiteSpace(fileText))
            {
                Window_Save ws = new Window_Save();
                ws.ShowDialog();
            }
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            (sender as Button).ContextMenu.IsEnabled = true;
            (sender as Button).ContextMenu.PlacementTarget = (sender as Button);
            (sender as Button).ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            (sender as Button).ContextMenu.IsOpen = true;

            
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            TextRange t = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
            if (!string.IsNullOrWhiteSpace(t.Text))
            {
                Window_Save ws = new Window_Save();
                ws.ShowDialog();
            }

            rtbEditor.Document.Blocks.Clear();



        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {

            TextRange t = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
            if (!string.IsNullOrWhiteSpace(t.Text))
            {
                Window_Save ws = new Window_Save();
                ws.ShowDialog();
            }


            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();



            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".rtf";
            dlg.Filter = "RTF Files (*.rtf)|*.rtf|Txt Files (*.txt)|*.txt";


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();


             
            if (result == true)
            {
                // Open document 
                /* string filename = dlg.FileName;
                 StreamReader reader = new StreamReader(filename);
                 rtbEditor.Document.Blocks.Clear();
                 rtbEditor.Document.Blocks.Add(new Paragraph(new Run(reader.ReadToEnd())));
                 reader.Close();*/
                FileStream file = new FileStream(dlg.FileName, FileMode.Open);
                t.Load(file, System.Windows.DataFormats.Rtf);
                file.Close();
            }

        }

        public  void Save_Click(object sender, RoutedEventArgs e)
        {
            TextRange t = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd); 

            SaveFileDialog dialog = new SaveFileDialog()
            {
                Filter = "Rtf Files(*.rtf)|*.rtf|Text Files(*.txt)|*.txt|All(*.*)|*"
            };

            if (dialog.ShowDialog() == true)
            {
                //File.WriteAllText(dialog.FileName, fileText);
                FileStream file = new FileStream(dialog.FileName, FileMode.Create);
                t.Save(file, System.Windows.DataFormats.Rtf);
                file.Close();
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            string fileText = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd).Text;
            if (string.IsNullOrWhiteSpace(fileText))
            {
                this.Close();
            }
            else
            {
                Window_Save ws = new Window_Save();
                ws.ShowDialog();
                this.Close();
            }
            
        }

        private void FR_Click(object sender, RoutedEventArgs e)
        {
            FRWindow fr = new FRWindow();
            fr.ShowDialog();
            if (find != "" && replace != "")
            {
                /*string str = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd).Text;
                str = str.Replace(find, replace);
                rtbEditor.Document.Blocks.Clear();
                rtbEditor.Document.Blocks.Add(new Paragraph(new Run(str)));*/


                TextRange text = new TextRange(rtbEditor.Document.ContentStart, rtbEditor.Document.ContentEnd);
                TextPointer current = text.Start.GetInsertionPosition(LogicalDirection.Forward);
                while (current != null)
                {
                    string textInRun = current.GetTextInRun(LogicalDirection.Forward);
                    if (!string.IsNullOrWhiteSpace(textInRun))
                    {
                        int index = textInRun.IndexOf(find);
                        if (index != -1)
                        {
                            TextPointer selectionStart = current.GetPositionAtOffset(index, LogicalDirection.Forward);
                            TextPointer selectionEnd = selectionStart.GetPositionAtOffset(find.Length, LogicalDirection.Forward);
                            TextRange selection = new TextRange(selectionStart, selectionEnd);
                            selection.Text = replace;
                            selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
                            rtbEditor.Selection.Select(selection.Start, selection.End);
                            rtbEditor.Focus();
                        }
                    }
                    current = current.GetNextContextPosition(LogicalDirection.Forward);
                }
            }

        }

        private void Button_Date(object sender, RoutedEventArgs e)
        {
            rtbEditor.CaretPosition.InsertTextInRun(DateTime.Now.ToString());
        }

        public int WordCount(string s)
        {
            return Regex.Matches(s, @"[\w']+").Count;
        }


    }


}

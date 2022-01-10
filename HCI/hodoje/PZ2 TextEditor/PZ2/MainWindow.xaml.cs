using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using Xceed.Wpf.Toolkit;
using Brushes = System.Windows.Media.Brushes;
using Color = System.Windows.Media.Color;
using ColorConverter = System.Windows.Media.ColorConverter;
using FontFamily = System.Windows.Media.FontFamily;
using MessageBox = System.Windows.MessageBox;
using RichTextBox = System.Windows.Controls.RichTextBox;

namespace PZ2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static List<int> fontSizes = new List<int>() { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 26, 28, 36, 48, 72 };

        public static List<RichTextBox> rtbList = new List<RichTextBox>();                     // List of RTBs
        public static List<string> activeRtbFilePath = new List<string>();                     // File path of active RTB (for Save, SaveAs)
        public static List<string> activeRtbFormatAsString = new List<string>();               // Format of active RTB
        public static List<bool> activeRtbChanged = new List<bool>();                          // Track active RTB changes

        public static int index;                                                               // Static index for tracking current RTB

        public MainWindow()
        {
            InitializeComponent();
            CmbFontFamily.ItemsSource = Fonts.SystemFontFamilies.OrderBy(f => f.Source);        // Initialization of font list
            CmbFontSize.ItemsSource = fontSizes;
            DataContext = this;

            TabItem tab = new TabItem();

            RichTextBox tabChild = new RichTextBox();

            tabChild.SelectionChanged += RtbEditor_SelectionChanged;
            tabChild.BorderThickness = new Thickness(1, 1, 1, 1);
            tabChild.Margin = new Thickness(0, 0, 0, 0);
            tabChild.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            tabChild.AcceptsTab = true;
            tabChild.Loaded += (senderr, ee) => tabChild.Focus();                               // Sets focus on loaded RTB
            tabChild.KeyUp += (senderr, ee) =>
            {
                if (ee.Key == Key.F5)
                {
                    var txtRange = new TextRange(rtbList[index].CaretPosition, rtbList[index].CaretPosition);
                    var selekcija = rtbList[index].Selection;
                    var ceoTekst = new TextRange(rtbList[index].Document.ContentStart, rtbList[index].Document.ContentEnd).Text;
                    var ceoTekstBezNevKar = ceoTekst.Substring(0, ceoTekst.Length - 2);

                    selekcija.Text = DateTime.Now.ToString();

                    if (selekcija.Text == ceoTekst)
                    {
                        rtbList[index].Document.Blocks.Clear();
                        rtbList[index].CaretPosition = rtbList[index].Document.ContentStart;

                        txtRange.Text = DateTime.Now.ToString();
                    }
                    else if (selekcija.Text == ceoTekstBezNevKar)
                    {
                        rtbList[index].Document.Blocks.Clear();
                        rtbList[index].CaretPosition = rtbList[index].Document.ContentStart;

                        txtRange.Text = DateTime.Now.ToString();
                    }
                    else if (selekcija.Text != ceoTekst)
                    {
                        selekcija.Text = "";
                        selekcija.Text = DateTime.Now.ToString();
                    }

                    if (rtbList[index].CaretPosition.GetPositionAtOffset(txtRange.Text.Length) != null)
                    {
                        rtbList[index].CaretPosition = rtbList[index].CaretPosition.GetPositionAtOffset(txtRange.Text.Length);
                    }

                    //rtbList[index].Selection.Text = DateTime.Now.ToString();
                    //rtbList[index].CaretPosition = rtbList[index].CaretPosition.GetPositionAtOffset(0);

                    activeRtbChanged[index] = true;                                                          // the selected text
                    rtbList[index].Focus();
                }
            };
            tabChild.TextChanged += (senderr, ee) => activeRtbChanged[index] = true;

            StackPanel sp = new StackPanel() { Orientation = Orientation.Horizontal };

            TextBlock tb = new TextBlock();
            tb.Text = "New";
            sp.Children.Add(tb);

            Button b = new Button();
            b.Content = "X";
            b.Background = new ImageBrush();
            b.BorderThickness = new Thickness(0, 0, 0, 0);
            b.Width = 17;
            b.Height = 17;
            b.Foreground = Brushes.Red;
            b.Click += CloseTab;
            sp.Children.Add(b);

            tab.Header = sp;

            tab.Content = tabChild;

            rtbList.Add(tabChild);

            activeRtbFilePath.Add("");                                                          // Initially open tab doesn't have a path yet

            activeRtbFormatAsString.Add("");                                                    // Initially open tab doesn't have a DataFormat yet

            activeRtbChanged.Add(false);                                                        // Initially open tab doesn't have any changes

            TabCntrl.Items.Add(tab);

            foreach (TabItem el in TabCntrl.Items)
            {
                if (el.Equals(tab))
                {
                    el.Focus();
                }
            }
        }

        private void RtbEditor_SelectionChanged(object sender, RoutedEventArgs e)
        {

            foreach (var el in rtbList)                                                         // Sender will be some RTB, and because we have many of them, we need to find the right one
            {                                                                                   // So we find his index and use it later
                if (el.Equals(sender as RichTextBox))
                {
                    index = rtbList.IndexOf(el);
                }
            }

            object temp = rtbList[index].Selection.GetPropertyValue(Inline.FontWeightProperty);      
            BtnBold.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontWeights.Bold));
            // Explanation of CLR and Dependency properties
            // CLR properties are all those properties that we have used until now (defined in classes), their downside is that they are stored in memory, whether their default value is changed or not
            // They just represent wrappers around a private variable. They use Get/Set to get or store values of those private variables.
            // Value of a CLR property depends only on the private property for which it was defined.
            // DependencyProperties are properties that are created only when they are used. For example, if a button has 50 properties, only those properties whose default value,
            // defined in metadata, was changed will be loaded into memory
            // Value of DependencyProperties depends on external sources (animations, data binding, styles...)
            // These properties are stored in a Dictionary of keys and values inside the base class called DependencyObject. The key is the name of the property, and the value is the value of the property
            // These properties have 2 methods: "GetValue" and "SetValue". Instead of getting and storing values from some field (like a CLR), they get and store values in that Dictionary.
            // What is interesting with these methods is that when, for example, "GetValue" is called, it looks for the values inside the Dictionary of the object from which is called (for example, some TextBox),
            // if it can't find the value, it calls the "GetValue" of the parent element to see if there might be a value in parents Dictionary, if it can't find the value, it continues along the tree (parent to parent)
            // until it finds the value (or not).
            // When it comes to "SetValue" method, if we, for example, set a value for some property on the level of a Window object, it won't update the values only in the Dictionary of the Window, instead, it will fire
            // "property-change" event where everything that depends on that event will know about it.
            // If something that heard that event, is also a DependencyProperty, it will also fire the same event.
            // So, if we for example, change the FontFamily property of the Window, it will change the font in all other controls.

            temp = rtbList[index].Selection.GetPropertyValue(Inline.FontStyleProperty);
            BtnItalic.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontStyles.Italic));

            temp = rtbList[index].Selection.GetPropertyValue(Inline.TextDecorationsProperty);
            BtnUnderline.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(TextDecorations.Underline));

            temp = rtbList[index].Selection.GetPropertyValue(Inline.FontFamilyProperty);
            CmbFontFamily.SelectedItem = temp;

            if (temp.ToString() == "{DependencyProperty.UnsetValue}")                                   // If the value is "{DependencyProperty.UnsetValue}" that means selected text has more FontFamily properties set
            {
                CmbFontFamily.Text = "";
                CmbFontFamily.SelectedItem = null;
            }
            else if (!rtbList[index].Selection.IsEmpty)
            {
                CmbFontFamily.SelectedItem = CmbFontFamily.SelectedItem;
                CmbFontFamily.Text = temp.ToString();
            }
            else if (rtbList[index].Selection.IsEmpty || CmbFontFamily.SelectedItem == null)
            {
                if (rtbList[index].CaretPosition.GetPositionAtOffset(1, LogicalDirection.Forward) != null)
                {
                    TextRange txtRange = new TextRange(rtbList[index].CaretPosition, rtbList[index].CaretPosition.GetPositionAtOffset(1, LogicalDirection.Forward));

                    CmbFontFamily.SelectedItem = txtRange.GetPropertyValue(Inline.FontFamilyProperty);
                    CmbFontFamily.Text = txtRange.GetPropertyValue(Inline.FontFamilyProperty).ToString();
                }
            }
            else if (rtbList[index].Selection.IsEmpty && CmbFontSize.SelectedItem != null)
            {
                TextRange txtRange = new TextRange(rtbList[index].Document.ContentStart,
                    rtbList[index].Document.ContentEnd);
                CmbFontFamily.SelectedItem = txtRange.GetPropertyValue(Inline.FontFamilyProperty);
                CmbFontFamily.Text = txtRange.GetPropertyValue(Inline.FontFamilyProperty).ToString();
            }

            temp = rtbList[index].Selection.GetPropertyValue(Inline.FontSizeProperty);              
            if (temp.ToString() == "{DependencyProperty.UnsetValue}")                                   // If the value is "{DependencyProperty.UnsetValue}" that means selected text has more FontSize properties set
            {
                CmbFontSize.Text = "";                                                                  
                CmbFontSize.SelectedItem = null;
            }
            else if (!rtbList[index].Selection.IsEmpty)
            {
                CmbFontSize.SelectedItem = CmbFontSize.SelectedItem;
                CmbFontSize.Text = temp.ToString();
            }
            else if (rtbList[index].Selection.IsEmpty || CmbFontSize.SelectedItem == null)
            {
                if (rtbList[index].CaretPosition.GetPositionAtOffset(1, LogicalDirection.Forward) != null)
                {
                    TextRange txtRange = new TextRange(rtbList[index].CaretPosition, rtbList[index].CaretPosition.GetPositionAtOffset(1, LogicalDirection.Forward));

                    CmbFontSize.SelectedItem = txtRange.GetPropertyValue(Inline.FontSizeProperty);
                    CmbFontSize.Text = txtRange.GetPropertyValue(Inline.FontSizeProperty).ToString();
                }
            }
            else if (rtbList[index].Selection.IsEmpty && CmbFontSize.SelectedItem != null)
            {
                TextRange txtRange = new TextRange(rtbList[index].Document.ContentStart,
                    rtbList[index].Document.ContentEnd);
                CmbFontSize.SelectedItem = txtRange.GetPropertyValue(Inline.FontSizeProperty);
                CmbFontSize.Text = txtRange.GetPropertyValue(Inline.FontSizeProperty).ToString();
            }

            Number_Of_Words_In_Rtb();                                                                   // Show number of words in a text

            temp = rtbList[index].Selection.GetPropertyValue(Inline.ForegroundProperty);            
            if (temp != null && temp.ToString() != "{DependencyProperty.UnsetValue}")
            {
                ClrPcker.SelectedColor = (Color?)ColorConverter.ConvertFromString(temp.ToString());     
            }
            else
            {
                ClrPcker.SelectedColor = null;
            }

            if (new TextRange(rtbList[index].Document.ContentStart, rtbList[index].Document.ContentEnd).Text == "")
            {
                activeRtbChanged[index] = false;
            }
        }

        private void CloseTab(object sender, RoutedEventArgs routedEventArgs)
        {
            Button x = (Button)sender;
            StackPanel sp = (StackPanel)x.Parent;
            TabItem t = (TabItem)sp.Parent;
            RichTextBox selectedRtb = (RichTextBox)t.Content;
            int selRtbIndex = 0;

            foreach (var rtb in rtbList)
            {
                if (rtb.Equals(selectedRtb))
                {
                    selRtbIndex = rtbList.IndexOf(rtb);
                    rtbList[selRtbIndex].Focus();
                }
            }

            TabCntrl.SelectedItem = t;

            if (activeRtbChanged[selRtbIndex])
            {
                MessageBoxResult result =
                    MessageBox.Show($"You have some unsaved changes, do you want to save your progress?",
                        "Exit", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.Cancel)
                {

                }
                else if (result == MessageBoxResult.No)
                {
                    if (TabCntrl.Items.Count > 0)
                    {
                        activeRtbChanged.RemoveAt(selRtbIndex);
                        activeRtbFilePath.RemoveAt(selRtbIndex);
                        activeRtbFormatAsString.RemoveAt(selRtbIndex);
                        rtbList.RemoveAt(selRtbIndex);
                        TabCntrl.Items.Remove(t);

                        if (TabCntrl.Items.Count == 0)
                        {
                            TabItem tab = new TabItem();

                            RichTextBox tabChild = new RichTextBox();

                            tabChild.SelectionChanged += RtbEditor_SelectionChanged;
                            tabChild.BorderThickness = new Thickness(1, 1, 1, 1);
                            tabChild.Margin = new Thickness(0, 0, 0, 0);
                            tabChild.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                            tabChild.AcceptsTab = true;
                            tabChild.Loaded += (senderr, ee) => tabChild.Focus();
                            tabChild.KeyUp += (senderr, ee) =>
                            {
                                if (ee.Key == Key.F5)
                                {
                                    var txtRange = new TextRange(rtbList[index].CaretPosition, rtbList[index].CaretPosition);
                                    var selekcija = rtbList[index].Selection;
                                    var ceoTekst = new TextRange(rtbList[index].Document.ContentStart, rtbList[index].Document.ContentEnd).Text;
                                    var ceoTekstBezNevKar = ceoTekst.Substring(0, ceoTekst.Length - 2);

                                    selekcija.Text = DateTime.Now.ToString();

                                    if (selekcija.Text == ceoTekst)
                                    {
                                        rtbList[index].Document.Blocks.Clear();
                                        rtbList[index].CaretPosition = rtbList[index].Document.ContentStart;

                                        txtRange.Text = DateTime.Now.ToString();
                                    }
                                    else if (selekcija.Text == ceoTekstBezNevKar)
                                    {
                                        rtbList[index].Document.Blocks.Clear();
                                        rtbList[index].CaretPosition = rtbList[index].Document.ContentStart;

                                        txtRange.Text = DateTime.Now.ToString();
                                    }
                                    else if (selekcija.Text != ceoTekst)
                                    {
                                        selekcija.Text = "";
                                        selekcija.Text = DateTime.Now.ToString();
                                    }

                                    if (rtbList[index].CaretPosition.GetPositionAtOffset(txtRange.Text.Length) != null)
                                    {
                                        rtbList[index].CaretPosition = rtbList[index].CaretPosition.GetPositionAtOffset(txtRange.Text.Length);
                                    }

                                    //rtbList[index].Selection.Text = DateTime.Now.ToString();
                                    //rtbList[index].CaretPosition = rtbList[index].CaretPosition.GetPositionAtOffset(0);

                                    activeRtbChanged[index] = true;                                                          // the selected text
                                    rtbList[index].Focus();
                                }
                            };
                            tabChild.TextChanged += (senderr, ee) => activeRtbChanged[index] = true;

                            StackPanel stackPanel = new StackPanel() { Orientation = Orientation.Horizontal };

                            TextBlock tb = new TextBlock();
                            tb.Text = "New";
                            stackPanel.Children.Add(tb);

                            Button b = new Button();
                            b.Content = "X";
                            b.Background = new ImageBrush();
                            b.BorderThickness = new Thickness(0, 0, 0, 0);
                            b.Width = 17;
                            b.Height = 17;
                            b.Foreground = Brushes.Red;
                            b.Click += CloseTab;
                            stackPanel.Children.Add(b);

                            tab.Header = stackPanel;

                            tab.Content = tabChild;

                            rtbList.Add(tabChild);

                            activeRtbFilePath.Add("");                          // New file doesn't have a path yet

                            activeRtbFormatAsString.Add("");                    // New file doesn't have a DataFormat yet

                            activeRtbChanged.Add(false);                        // New file doesn't have any changes yet

                            TabCntrl.Items.Add(tab);

                            tab.Focus();
                        }
                    }


                }
                else if (result == MessageBoxResult.Yes)
                {
                    if (TabCntrl.Items.Count > 0)
                    {
                        t.Focus();

                        if (((TextBlock) ((StackPanel) t.Header).Children[0]).Text == "New")
                        {
                            SaveFileDialog dialog = new SaveFileDialog();
                            dialog.Filter = "All files (*.*)|*.*| Rich Text Format (*.rtf)|*.rtf| Txt (*.txt)|*.txt";
                            if (dialog.ShowDialog() == true)
                            {
                                FileStream fileStream = new FileStream(dialog.FileName, FileMode.Create);
                                TextRange range = new TextRange(rtbList[index].Document.ContentStart,
                                    rtbList[index].Document.ContentEnd);
                                if (dialog.FilterIndex == 1 || dialog.FilterIndex == 3)
                                    range.Save(fileStream, DataFormats.Text);
                                else
                                    range.Save(fileStream, DataFormats.Rtf);
                                fileStream.Close();

                                string absoluteFileName = dialog.FileName;
                                int slashLastIndex = absoluteFileName.LastIndexOf('\\');
                                string relativeFileName = absoluteFileName.Substring(slashLastIndex + 1);

                                activeRtbChanged.RemoveAt(selRtbIndex);
                                activeRtbFilePath.RemoveAt(selRtbIndex);
                                activeRtbFormatAsString.RemoveAt(selRtbIndex);
                                rtbList.RemoveAt(selRtbIndex);
                                TabCntrl.Items.Remove(t);
                            }
                        }
                        else
                        {
                            if (activeRtbFormatAsString[selRtbIndex] != "")
                            {
                                DataFormat df = DataFormats.GetDataFormat(activeRtbFormatAsString[selRtbIndex]);
                                FileStream fileStream = new FileStream(activeRtbFilePath[selRtbIndex],
                                    FileMode.Create, FileAccess.Write);
                                TextRange range = new TextRange(rtbList[selRtbIndex].Document.ContentStart,
                                    rtbList[selRtbIndex].Document.ContentEnd);
                                range.Save(fileStream, df.Name);
                                fileStream.Close();

                                activeRtbChanged.RemoveAt(selRtbIndex);
                                activeRtbFilePath.RemoveAt(selRtbIndex);
                                activeRtbFormatAsString.RemoveAt(selRtbIndex);
                                rtbList.RemoveAt(selRtbIndex);
                                TabCntrl.Items.Remove(t);
                            }
                        }
                    }
                }
            }
            else
            {
                if (TabCntrl.Items.Count > 1)
                {
                    t.Focus();

                    activeRtbChanged.RemoveAt(selRtbIndex);
                    activeRtbFilePath.RemoveAt(selRtbIndex);
                    activeRtbFormatAsString.RemoveAt(selRtbIndex);
                    rtbList.RemoveAt(selRtbIndex);
                    TabCntrl.Items.Remove(t);
                }
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            bool atLeastOnechanged = false;
            foreach (RichTextBox rtb in rtbList)
            {
                if (activeRtbChanged[rtbList.IndexOf(rtb)])
                {
                    atLeastOnechanged = true;
                }
            }
            if (atLeastOnechanged)
            {
                int numberOfNotSavedTabs = 0;
                foreach (bool change in activeRtbChanged)
                {
                    if (change)
                    {
                        numberOfNotSavedTabs++;
                    }
                }
                MessageBoxResult result =
                    MessageBox.Show($"You have {numberOfNotSavedTabs} unsaved file(s), do you want to save your recent changes?",
                        "Exit", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
                else if (result == MessageBoxResult.No)
                {

                    e.Cancel = false;
                }
                else if (result == MessageBoxResult.Yes)
                {
                    foreach (TabItem tab in TabCntrl.Items)
                    {
                        tab.Focus();
                        if (((TextBlock)((StackPanel)tab.Header).Children[0]).Text == "New")
                        {
                            SaveFileDialog dialog = new SaveFileDialog();
                            dialog.Filter = "All files (*.*)|*.*| Rich Text Format (*.rtf)|*.rtf| Txt (*.txt)|*.txt";
                            if (dialog.ShowDialog() == true)
                            {
                                FileStream fileStream = new FileStream(dialog.FileName, FileMode.Create);
                                TextRange range = new TextRange(rtbList[index].Document.ContentStart,
                                    rtbList[index].Document.ContentEnd);
                                if (dialog.FilterIndex == 1 || dialog.FilterIndex == 3)
                                    range.Save(fileStream, DataFormats.Text);
                                else
                                    range.Save(fileStream, DataFormats.Rtf);

                                string absoluteFileName = dialog.FileName;
                                int slashLastIndex = absoluteFileName.LastIndexOf('\\');
                                string relativeFileName = absoluteFileName.Substring(slashLastIndex + 1);

                                e.Cancel = false;
                            }
                        }
                        else
                        {
                            TabItem activeTab = (TabItem)TabCntrl.SelectedItem;
                            RichTextBox activeRtb = (RichTextBox)activeTab.Content;
                            int activeRtbIndex = 0;

                            foreach (var rtb in rtbList)
                            {
                                if (rtb.Equals(activeRtb))
                                {
                                    activeRtbIndex = rtbList.IndexOf(rtb);
                                }
                            }

                            if (activeRtbFormatAsString[activeRtbIndex] != "")
                            {
                                DataFormat df = DataFormats.GetDataFormat(activeRtbFormatAsString[activeRtbIndex]);
                                FileStream fileStream = new FileStream(activeRtbFilePath[activeRtbIndex],
                                    FileMode.Create, FileAccess.Write);
                                TextRange range = new TextRange(rtbList[activeRtbIndex].Document.ContentStart,
                                    rtbList[activeRtbIndex].Document.ContentEnd);
                                range.Save(fileStream, df.Name);
                                fileStream.Close();
                                activeRtbChanged[activeRtbIndex] = false;
                                e.Cancel = false;
                            }
                        }
                    }
                }
            }
        }

        private void CmbFontFamily_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbFontFamily.SelectedItem != null)
            {
                if (CmbFontFamily.SelectedItem.ToString() == "{DependencyProperty.UnsetValue}")
                {

                }
                else if (!rtbList[index].Selection.GetPropertyValue(Inline.FontFamilyProperty).Equals(CmbFontFamily.SelectedItem))
                {
                    rtbList[index].Selection.ApplyPropertyValue(Inline.FontFamilyProperty, CmbFontFamily.SelectedItem);
                    activeRtbChanged[index] = true;
                    rtbList[index].Focus();
                }
            }
            rtbList[index].Focus();
        }

        private void ClrPcker_OnSelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            if (ClrPcker.SelectedColor != null)
            {
                SolidColorBrush boja = new SolidColorBrush((Color)ClrPcker.SelectedColor);
                if (rtbList[index].Selection.GetPropertyValue(Inline.ForegroundProperty).ToString() != boja.ToString())
                {
                    rtbList[index].Selection.ApplyPropertyValue(Inline.ForegroundProperty, boja);
                    activeRtbChanged[index] = true;
                }             
                ClrPcker.Focusable = false;
            }
            ClrPcker.IsOpen = false;
            rtbList[index].Focus();
        }

        private void CmbFontSize_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (CmbFontSize.Text != "")
            {
                if (!HasPunctuations(CmbFontSize.Text))
                {
                    if (HasOnlyNumbers(CmbFontSize.Text))
                    {
                        if (rtbList[index].Selection.GetPropertyValue(Inline.FontSizeProperty).ToString() !=
                            CmbFontSize.Text)
                        {
                            rtbList[index].Selection.ApplyPropertyValue(Inline.FontSizeProperty, CmbFontSize.Text);
                            activeRtbChanged[index] = true;
                        }
                    }
                    else
                    {
                        MessageBoxResult result =
                            MessageBox.Show("This is not a valid number.", "Hedit", MessageBoxButton.OK);
                        var font = rtbList[index].Selection.GetPropertyValue(Inline.FontSizeProperty);
                        if (font.ToString() != "{DependencyProperty.UnsetValue}")
                        {
                            CmbFontSize.SelectedItem = font;
                            CmbFontSize.Text = font.ToString();
                        }
                        CmbFontSize.SelectedItem = null;
                        CmbFontSize.Text = "";
                    }
                }
                else
                {
                    MessageBoxResult result = MessageBox.Show("This is not a valid number.", "Hedit", MessageBoxButton.OK);
                    var font = rtbList[index].Selection.GetPropertyValue(Inline.FontSizeProperty);
                    if (font.ToString() != "{DependencyProperty.UnsetValue}")
                    {
                        CmbFontSize.SelectedItem = font;
                        CmbFontSize.Text = font.ToString();
                    }
                    CmbFontSize.SelectedItem = null;
                    CmbFontSize.Text = "";
                }
            }
            else
            {
                if (rtbList[index].CaretPosition != null)
                {
                    TextRange txtRange = new TextRange(rtbList[index].CaretPosition, rtbList[index].CaretPosition);

                    CmbFontSize.SelectedItem = txtRange.GetPropertyValue(Inline.FontSizeProperty);
                    CmbFontSize.Text = txtRange.GetPropertyValue(Inline.FontSizeProperty).ToString();

                    rtbList[index].Focus();
                    activeRtbChanged[index] = true;
                }

            }
            rtbList[index].Focus();             
        }

        private void DateTimeButton_OnClick(object sender, RoutedEventArgs e)
        {
            var txtRange = new TextRange(rtbList[index].CaretPosition, rtbList[index].CaretPosition);
            var selekcija = rtbList[index].Selection;
            var ceoTekst = new TextRange(rtbList[index].Document.ContentStart, rtbList[index].Document.ContentEnd).Text;
            var ceoTekstBezNevKar = ceoTekst.Substring(0, ceoTekst.Length - 2);

            selekcija.Text = DateTime.Now.ToString();

            if (selekcija.Text == ceoTekst)
            {
                rtbList[index].Document.Blocks.Clear();
                rtbList[index].CaretPosition = rtbList[index].Document.ContentStart;

                txtRange.Text = DateTime.Now.ToString();
            }
            else if (selekcija.Text == ceoTekstBezNevKar)
            {
                rtbList[index].Document.Blocks.Clear();
                rtbList[index].CaretPosition = rtbList[index].Document.ContentStart;

                txtRange.Text = DateTime.Now.ToString();
            }
            else if (selekcija.Text != ceoTekst)
            {
                selekcija.Text = "";
                selekcija.Text = DateTime.Now.ToString();
            }

            if (rtbList[index].CaretPosition.GetPositionAtOffset(txtRange.Text.Length) != null)
            {
                rtbList[index].CaretPosition = rtbList[index].CaretPosition.GetPositionAtOffset(txtRange.Text.Length);
            }

            //rtbList[index].Selection.Text = DateTime.Now.ToString();
            //rtbList[index].CaretPosition = rtbList[index].CaretPosition.GetPositionAtOffset(0);

            activeRtbChanged[index] = true;                                                          // the selected text
            rtbList[index].Focus();
        }

        private bool HasPunctuations(string input)
        {
            bool result = input.IndexOfAny("[](){}*,:=;...#".ToCharArray()) != -1;          // Returns true if there is a punctuation in an array

            return result;
        }

        private bool HasOnlyNumbers(string input)
        {
            foreach (char c in input)
            {
                if (c < '0' || c > '9')
                {
                    return false;
                }
            }

            return true;
        }

        private void New_File_Command_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            TabItem tab = new TabItem();

            RichTextBox tabChild = new RichTextBox();

            tabChild.SelectionChanged += RtbEditor_SelectionChanged;
            tabChild.BorderThickness = new Thickness(1, 1, 1, 1);
            tabChild.Margin = new Thickness(0, 0, 0, 0);
            tabChild.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            tabChild.AcceptsTab = true;
            tabChild.Loaded += (senderr, ee) => tabChild.Focus();
            tabChild.KeyUp += (senderr, ee) =>
            {
                if (ee.Key == Key.F5)
                {
                    var txtRange = new TextRange(rtbList[index].CaretPosition, rtbList[index].CaretPosition);
                    var selekcija = rtbList[index].Selection;
                    var ceoTekst = new TextRange(rtbList[index].Document.ContentStart, rtbList[index].Document.ContentEnd).Text;
                    var ceoTekstBezNevKar = ceoTekst.Substring(0, ceoTekst.Length - 2);

                    selekcija.Text = DateTime.Now.ToString();

                    if (selekcija.Text == ceoTekst)
                    {
                        rtbList[index].Document.Blocks.Clear();
                        rtbList[index].CaretPosition = rtbList[index].Document.ContentStart;

                        txtRange.Text = DateTime.Now.ToString();
                    }
                    else if (selekcija.Text == ceoTekstBezNevKar)
                    {
                        rtbList[index].Document.Blocks.Clear();
                        rtbList[index].CaretPosition = rtbList[index].Document.ContentStart;

                        txtRange.Text = DateTime.Now.ToString();
                    }
                    else if (selekcija.Text != ceoTekst)
                    {
                        selekcija.Text = "";
                        selekcija.Text = DateTime.Now.ToString();
                    }

                    if (rtbList[index].CaretPosition.GetPositionAtOffset(txtRange.Text.Length) != null)
                    {
                        rtbList[index].CaretPosition = rtbList[index].CaretPosition.GetPositionAtOffset(txtRange.Text.Length);
                    }

                    //rtbList[index].Selection.Text = DateTime.Now.ToString();
                    //rtbList[index].CaretPosition = rtbList[index].CaretPosition.GetPositionAtOffset(0);

                    activeRtbChanged[index] = true;                                                          // the selected text
                    rtbList[index].Focus();
                }
            };
            tabChild.TextChanged += (senderr, ee) => activeRtbChanged[index] = true;

            StackPanel sp = new StackPanel() { Orientation = Orientation.Horizontal };

            TextBlock tb = new TextBlock();
            tb.Text = "New";
            sp.Children.Add(tb);

            Button b = new Button();
            b.Content = "X";
            b.Background = new ImageBrush();
            b.BorderThickness = new Thickness(0, 0, 0, 0);
            b.Width = 17;
            b.Height = 17;
            b.Foreground = Brushes.Red;
            b.Click += CloseTab;
            sp.Children.Add(b);

            tab.Header = sp;

            tab.Content = tabChild;

            rtbList.Add(tabChild);

            activeRtbFilePath.Add("");                          // New file doesn't have a path yet

            activeRtbFormatAsString.Add("");                    // New file doesn't have a DataFormat yet

            activeRtbChanged.Add(false);                        // New file doesn't have any changes yet

            TabCntrl.Items.Add(tab);

            tab.Focus();
        }

        private void Open_File_Command_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();                                                            
            dialog.Filter = "All files (*.*)|*.*| Rich Text Format (*.rtf)|*.rtf| Txt (*.txt)|*.txt";                
            if (dialog.ShowDialog() == true)                                                                         
            {
                string absoluteFileName = dialog.FileName;                                                           
                int slashLastIndex = absoluteFileName.LastIndexOf('\\');                                             
                string relativeFileName = absoluteFileName.Substring(slashLastIndex + 1);                            
                int dotLastIndex = absoluteFileName.LastIndexOf('.');
                string fileExtension = absoluteFileName.Substring(dotLastIndex);

                TabItem tab = new TabItem();                                                                         

                RichTextBox tabChild = new RichTextBox();                                                            

                tabChild.SelectionChanged += RtbEditor_SelectionChanged;                                             
                tabChild.BorderThickness = new Thickness(1, 1, 1, 1);                                                
                tabChild.Margin = new Thickness(0, 0, 0, 0);                                                         
                tabChild.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;                                     
                tabChild.AcceptsTab = true;                                                                          
                tabChild.Loaded += (senderr, ee) => tabChild.Focus();                                                
                tabChild.KeyUp += (senderr, ee) =>                                                                   
                {
                    if (ee.Key == Key.F5)
                    {
                        var txtRange = new TextRange(rtbList[index].CaretPosition, rtbList[index].CaretPosition);
                        var selekcija = rtbList[index].Selection;
                        var ceoTekst = new TextRange(rtbList[index].Document.ContentStart, rtbList[index].Document.ContentEnd).Text;
                        var ceoTekstBezNevKar = ceoTekst.Substring(0, ceoTekst.Length - 2);

                        selekcija.Text = DateTime.Now.ToString();

                        if (selekcija.Text == ceoTekst)
                        {
                            rtbList[index].Document.Blocks.Clear();
                            rtbList[index].CaretPosition = rtbList[index].Document.ContentStart;

                            txtRange.Text = DateTime.Now.ToString();
                        }
                        else if (selekcija.Text == ceoTekstBezNevKar)
                        {
                            rtbList[index].Document.Blocks.Clear();
                            rtbList[index].CaretPosition = rtbList[index].Document.ContentStart;

                            txtRange.Text = DateTime.Now.ToString();
                        }
                        else if (selekcija.Text != ceoTekst)
                        {
                            selekcija.Text = "";
                            selekcija.Text = DateTime.Now.ToString();
                        }

                        if (rtbList[index].CaretPosition.GetPositionAtOffset(txtRange.Text.Length) != null)
                        {
                            rtbList[index].CaretPosition = rtbList[index].CaretPosition.GetPositionAtOffset(txtRange.Text.Length);
                        }

                        //rtbList[index].Selection.Text = DateTime.Now.ToString();
                        //rtbList[index].CaretPosition = rtbList[index].CaretPosition.GetPositionAtOffset(0);

                        activeRtbChanged[index] = true;                                                          // the selected text
                        rtbList[index].Focus();
                    }
                };
                tabChild.TextChanged += (senderr, ee) => activeRtbChanged[index] = true;

                FileStream fileStream = new FileStream(dialog.FileName, FileMode.Open);                              
                TextRange range = new TextRange(tabChild.Document.ContentStart, tabChild.Document.ContentEnd);       
                if (fileExtension == ".rtf")                                                                         
                {
                    range.Load(fileStream, DataFormats.Rtf);
                    activeRtbFormatAsString.Add(DataFormats.Rtf.ToString());                                         
                }
                else
                {
                    range.Load(fileStream, DataFormats.Text);                                                        // Does not support docx files
                    activeRtbFormatAsString.Add(DataFormats.Text.ToString());                                        // Save DataFormat of loaded file
                }                                                                                                 
                fileStream.Close();

                activeRtbFilePath.Add(absoluteFileName);

                StackPanel sp = new StackPanel() {Orientation = Orientation.Horizontal};

                TextBlock tb = new TextBlock();
                tb.Text = relativeFileName;
                sp.Children.Add(tb);

                Button b = new Button();
                b.Content = "X";
                b.Background = new ImageBrush();
                b.BorderThickness = new Thickness(0,0,0,0);
                b.Width = 17;
                b.Height = 17;
                b.Foreground = Brushes.Red;
                b.Click += CloseTab;
                sp.Children.Add(b);

                tab.Header = sp;                                                                                     

                tab.Content = tabChild;                                                                              // Content of our Tab will be a RTB

                tabChild.CaretPosition = tabChild.CaretPosition.DocumentEnd;                                        

                rtbList.Add(tabChild);                                                                               // Add RTB to list of RTBs

                activeRtbChanged.Add(false);                                                                         // Just opened tab doesn't have any changes

                TabCntrl.Items.Add(tab);                                                                             // Add newly created Tab to TabControl

                tab.Focus();

                activeRtbChanged[index] = false;
            }
        }

        private void Save_File_Command_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            index = TabCntrl.SelectedIndex;
            if (activeRtbChanged[index])
            {
                TabItem tab = (TabItem)rtbList[index].Parent;
                if (((TextBlock)((StackPanel)tab.Header).Children[0]).Text == "New")            // Access the TextBlock that is inside the StackPanel which is stored in the header of a Tab
                {
                    SaveFileDialog dialog = new SaveFileDialog();
                    dialog.Filter = "All files (*.*)|*.*| Rich Text Format (*.rtf)|*.rtf| Txt (*.txt)|*.txt";
                    if (dialog.ShowDialog() == true)
                    {
                        FileStream fileStream = new FileStream(dialog.FileName, FileMode.Create);
                        TextRange range = new TextRange(rtbList[index].Document.ContentStart,
                            rtbList[index].Document.ContentEnd);
                        if (dialog.FilterIndex == 1 || dialog.FilterIndex == 3)
                        {
                            range.Save(fileStream, DataFormats.Text);
                            activeRtbFormatAsString[index] = (DataFormats.Text.ToString());
                        }
                        else
                        {
                            range.Save(fileStream, DataFormats.Rtf);
                            activeRtbFormatAsString[index] = (DataFormats.Rtf.ToString());
                        }
                        fileStream.Close();

                        string absoluteFileName = dialog.FileName;                                
                        int slashLastIndex = absoluteFileName.LastIndexOf('\\');                  
                        string relativeFileName = absoluteFileName.Substring(slashLastIndex + 1); 
                        int dotLastIndex = absoluteFileName.LastIndexOf('.');
                        string fileExtension = absoluteFileName.Substring(dotLastIndex);

                        StackPanel sp = new StackPanel() { Orientation = Orientation.Horizontal };

                        TextBlock tb = new TextBlock();
                        tb.Text = relativeFileName;
                        sp.Children.Add(tb);

                        Button b = new Button();
                        b.Content = "X";
                        b.Background = new ImageBrush();
                        b.BorderThickness = new Thickness(0, 0, 0, 0);
                        b.Width = 17;
                        b.Height = 17;
                        b.Foreground = Brushes.Red;
                        b.Click += CloseTab;
                        sp.Children.Add(b);

                        tab.Header = sp;

                        activeRtbFilePath[index] = absoluteFileName;
                    }
                    activeRtbChanged[index] = false;
                }
            }
            
            else
            {
                TabItem activeTab = (TabItem) TabCntrl.SelectedItem;
                RichTextBox activeRtb = (RichTextBox)activeTab.Content;
                int activeRtbIndex = 0;

                foreach (var rtb in rtbList)
                {
                    if (rtb.Equals(activeRtb))
                    {
                        activeRtbIndex = rtbList.IndexOf(rtb);
                    }                    
                }

                if (activeRtbFormatAsString[activeRtbIndex] != "")
                {
                    DataFormat df = DataFormats.GetDataFormat(activeRtbFormatAsString[activeRtbIndex]);
                    FileStream fileStream = new FileStream(activeRtbFilePath[activeRtbIndex], FileMode.Create, FileAccess.Write);
                    TextRange range = new TextRange(rtbList[activeRtbIndex].Document.ContentStart, rtbList[activeRtbIndex].Document.ContentEnd);
                    range.Save(fileStream, df.Name);
                    fileStream.Close();
                    activeRtbChanged[index] = false;
                }
            }
        }

        private void SaveAs_File_Command_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            index = TabCntrl.SelectedIndex;
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "All files (*.*)|*.*| Rich Text Format (*.rtf)|*.rtf| Txt (*.txt)|*.txt";
            if (dialog.ShowDialog() == true)
            {
                FileStream fileStream = new FileStream(dialog.FileName, FileMode.Create);
                TextRange range = new TextRange(rtbList[index].Document.ContentStart, rtbList[index].Document.ContentEnd);
                if (dialog.FilterIndex == 1 || dialog.FilterIndex == 3)
                {
                    range.Save(fileStream, DataFormats.Text);
                    activeRtbFormatAsString[index] = (DataFormats.Text.ToString());
                }
                else
                {
                    range.Save(fileStream, DataFormats.Rtf);
                    activeRtbFormatAsString[index] = (DataFormats.Rtf.ToString());
                }
                fileStream.Close();

                string absoluteFileName = dialog.FileName;                                                           
                int slashLastIndex = absoluteFileName.LastIndexOf('\\');                                             
                string relativeFileName = absoluteFileName.Substring(slashLastIndex + 1);                            

                TabItem tab = (TabItem)rtbList[index].Parent;

                StackPanel sp = new StackPanel() { Orientation = Orientation.Horizontal };

                TextBlock tb = new TextBlock();
                tb.Text = relativeFileName;
                sp.Children.Add(tb);

                Button b = new Button();
                b.Content = "X";
                b.Background = new ImageBrush();
                b.BorderThickness = new Thickness(0, 0, 0, 0);
                b.Width = 17;
                b.Height = 17;
                b.Foreground = Brushes.Red;
                b.Click += CloseTab;
                sp.Children.Add(b);

                tab.Header = sp;

                activeRtbFilePath[index] = absoluteFileName;
            }
            activeRtbChanged[index] = false;
        }

        private void Number_Of_Words_In_Rtb()
        {
            TextRange textRange = new TextRange(rtbList[index].Document.ContentStart, rtbList[index].Document.ContentEnd);
            int counter = Count_Number_Of_Words(textRange.Text);
            StatusBarTextBlock.Text = "Words: " + counter;
        }

        private void TabPanel_OnSizeChanged(object sender, SizeChangedEventArgs e)              // I DON NOT KNOW WHY IT WORKS THIS WAY! DO NOT TOUCH!
        {
            TabPanel tp = e.Source as TabPanel;
            TabCntrl.Height = e.NewSize.Height;
            TabCntrl.Width = e.NewSize.Width;

            rtbList[index].Height = tp.Height;
            rtbList[index].Width = tp.Width;
        }

        private void FindAndReplace_OnClick(object sender, RoutedEventArgs e)
        {
            FnRWindow newWindow = new FnRWindow();
            newWindow.DataContext = this;
            newWindow.ShowDialog();
        }

        private void CmbFontSize_OnDropDownOpened(object sender, EventArgs e)
        {
            CmbFontSize.SelectedItem = null;
        }

        private int Count_Number_Of_Words(string s)
        {
            int counter = 0;
            char[] delimiters = new char[] { ' ', '\r', '\t', '\n' };
            if (String.IsNullOrWhiteSpace(s))
            {
                counter = 0;
            }
            else
            {
                counter = s.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).Length;
                activeRtbChanged[index] = true;
            }

            return counter;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using NetworkService.ViewModel;

namespace NetworkService.Views
{
    /// <summary>
    /// Interaction logic for LittleWindow.xaml
    /// </summary>
    public partial class LittleCanvasView : UserControl
    {
        public static readonly DependencyProperty NestoProperty = DependencyProperty.Register("nestoViewProp", typeof(object), typeof(LittleCanvasView));

        public LittleCanvasView()
        {
            InitializeComponent();
            this.DataContext = new LittleCanvasViewModel();
        }

        public object nestoViewProp
        {
            get { return (GetValue(NestoProperty)); }
            set { SetValue(NestoProperty, value);}
        }
    }
}

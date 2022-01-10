using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace NetworkService.VML
{
    public static class ViewModelLocator
    {
        // When we want to create our own DependencyProperty property we first need to register it
        // [There are two types of registration: via Register() and via RegisterAttached
        // RegisterAttached() allows the value to be assigned to any DependencyObject (to use it for any of the Controls)
        // Register() only allows the value to be attached to the class passed as the "ownerType" parameter
        // NOTE: This applies only to the XAML based use. Apparently, in code there is not such limitation.]
        // In the register method we pass the: 
        //                                      "name of the property" ["AutoHookedUpViewModel"] 
        //                                      "data type of that property" ["typeof(bool)"]
        //                                      "type of parent which holds the property" ["ViewModelLocator"]
        //                                      "metadata" [may contain different things, like, default value,
        //                                                  callback method to raise change notification, etc.]
        // We may think that we can implement change notification using INotifyPropertyChanged interface on our custom DependencyObject
        // and implement OnPropertyChanged and raise event when the "setter" is called for "AutoHookedUpViewModel" property.
        // That's right and that will work if our "setter" is called when the "AutoHookedUpViewModel" property value changed.
        // But the problem is, our property setter will not be called any time the value is changed. 
        // It will only be called based on the way we change the value. 
        // For example, if we change the value from our "code behind", then our setter method of our property will be called.
        // But, if we use our dependency property on our design page and if we change the value through the style operations in the XAML,
        // then our setter will not be called. WPF has the mechanism to change the value directly inside the dependency property.
        // So, we may not get the notification. That's why we need to use callback method of the Metadata
        public static readonly DependencyProperty AutoHookedUpViewModelProperty = 
            DependencyProperty.RegisterAttached("AutoHookedUpViewModel",
                typeof(bool), 
                typeof(ViewModelLocator),
                new PropertyMetadata(false, AutoHookedUpViewModelChanged));

        public static bool AUtoHookedUpViewModel(DependencyObject obj)
        {
            return (bool) obj.GetValue(AutoHookedUpViewModelProperty);
        }

        public static void SetAutoHookedUpViewModel(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoHookedUpViewModelProperty, value);
        }

        // DependencyObject class is a baseclass for all WPF classes (Buttons, Textboxes, Paragraphs, Span...)
        // It is used for WPF property system
        // When we create a property as a DependencyProperty, then we automatically get the following features implemented for us
        // (Change Notification, Validation, Call Back, Inheritance, DataBinding, Styles, Default Values etc.)
        // Implementing these features by ourselves on each property we want to would result in a big process, so these are all coming out of the box for us
        // Basically, DependecyObject class contains a dictionary. So, when ever a value for some our dependency property is set or retrieved, it will change the value or read
        // from that dictionary. So, it is a simple key value pair.
        private static void AutoHookedUpViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (DesignerProperties.GetIsInDesignMode(d))
            {
                return;
            }

            var viewType = d.GetType();                                 // Here we will get the View type (important properties are Name and FullName)
            string str = viewType.FullName;                             // "str" will contain the FullName of the View (namespace + filename)
            str = str.Replace(".Views", ".ViewModel");                  // We are replacing ".Views" with ".ViewModel" to position to the ViewModel folder
            var viewTypeName = str;                                     // Temp variable to hold our updated name

            var viewModelTypeName = viewTypeName + "Model";             // Here we will store for example "NetworkService.ViewModel.NetworkDataViewModel"
            var viewModelType = System.Type.GetType(viewModelTypeName); // Next, we will get the type of the passed viewModelTypeName (name above)
            var viewModel = Activator.CreateInstance(viewModelType);    // viewModel will contain the actual instance of the ViewModel

            ((FrameworkElement) d).DataContext = viewModel;             // Here we set the gotten ViewModel as a DataContext for the passed View ("d")
        }
    }
}

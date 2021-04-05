using System.Collections.ObjectModel;
using System.Net.Mime;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using CefSharp.Wpf;

namespace Browser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    
    public partial class MainWindow
    {

        public MainWindow()
        {
            InitializeComponent();
            int lastIndex = products.Items.Count - 1;
            var tab_Item = Create_tab("google.com");
            var test = products.Items[lastIndex];
            products.Items[lastIndex] = tab_Item;
            products.Items.Add(test);
            products.SelectedIndex = lastIndex;
            
            // WebBrowsers = new ObservableCollection<Phone>
            // {
            //     new Phone {Browser = "new ChromiumWebBrowser()"},
            //     new Phone {Browser = "new ChromiumWButtonBase_OnClick          //     new Phone {Browser = "new ChromiumWebBrowser()2"},
            // };
            // products.ItemsSource = WebBrowsers;
        }

        private void AddTab_OnMouseDown(object sender, SelectionChangedEventArgs  e)
        {
            if (e.AddedItems.Contains(addTab))
            {
                int lastIndex = products.Items.Count - 1;
                var tab_Item = Create_tab("google.com");
                var test = products.Items[lastIndex];
                products.Items[lastIndex] = tab_Item;
                products.Items.Add(test);
                products.SelectedIndex = lastIndex;
            }
            
        }

        private TabItem Create_tab(string url)
        {
            TabItem tab_Item = new TabItem();
            tab_Item.MouseEnter += Tab_OnFocusableChanged;
            tab_Item.MouseLeave += Tab_OnMouseDown;
            StackPanel stack_Panel = new StackPanel();
            stack_Panel.Orientation = Orientation.Horizontal;
            StackPanel stack_PanelContent = new StackPanel();
            
            ChromiumWebBrowser WebBrowser = new ChromiumWebBrowser(){Address = url, Title = "Загрузка...", Height = 1000};
            TextBox text_BlockAddress = new TextBox();
            
            TextBlock text_Block = new TextBlock();
            text_Block.Margin = new Thickness(5);
            text_Block.HorizontalAlignment = HorizontalAlignment.Left;
            Button ButtonClose = new Button(){FontSize=10, Content="✖", Height=20, Width=20};
            ButtonClose.BorderThickness = new Thickness(0);
            ButtonClose.Background = Brushes.Transparent;
            ButtonClose.Visibility = Visibility.Hidden;
            ButtonClose.Click += Tab_ClickClose;

            Binding bind = new Binding();
            bind.Path = new PropertyPath("WebBrowser.Title");
            bind.Source = WebBrowser;
            text_Block.SetBinding(TextBlock.TextProperty, bind);
            Binding bindAddress = new Binding();
            bindAddress.Path = new PropertyPath("WebBrowser.Address");
            bindAddress.Source = WebBrowser;
            text_BlockAddress.SetBinding(TextBox.TextProperty, bindAddress);
            
            stack_Panel.Children.Add(text_Block);
            stack_Panel.Children.Add(ButtonClose);

            stack_PanelContent.Children.Add(text_BlockAddress);
            stack_PanelContent.Children.Add(WebBrowser);
                
            tab_Item.Header = stack_Panel;
            tab_Item.Content = stack_PanelContent;

            return tab_Item;
        }

        private void Tab_OnFocusableChanged(object sender, RoutedEventArgs routedEventArgs)
        {
            ((StackPanel) ((TabItem)sender).Header).Children[1].Visibility = Visibility.Visible;
        }

        private void Tab_OnMouseDown(object sender, MouseEventArgs mouseEventArgs)
        {
            ((StackPanel) ((TabItem)sender).Header).Children[1].Visibility = Visibility.Hidden;
        }

        private void Tab_ClickClose(object sender, RoutedEventArgs e)
        {
            products.Items.Remove((TabItem) ((StackPanel)((Button) sender).Parent).Parent);
        }

        private void Address_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                var g = (StackPanel) ((TextBox) sender).Parent;
                var y = (ChromiumWebBrowser)g.Children[1];
                ChromiumWebBrowser t;
                t = (ChromiumWebBrowser)((StackPanel)((TextBox) sender).Parent).Children[1];
                t.Address = ((TextBox) sender).Text;
            }
        }
    }
}
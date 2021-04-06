using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Browser.ViewModels;
using CefSharp;
using CefSharp.Wpf;

namespace Browser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private MainViewModel _vm;

        public MainWindow()
        {
            InitializeComponent();
            _vm = new MainViewModel();
            this.DataContext = _vm;
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

        private void Browser_OnFrameLoadStart(object sender, FrameLoadStartEventArgs e)
        {
            _vm.AppTitle = "Загрузка...";
        }

        private void Browser_OnFrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            _vm.AppTitle = "Браузер";
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

         private TabItem Create_tab(string url)
        {
            // --- Контент --- //
            ChromiumWebBrowser WebBrowser = new ChromiumWebBrowser(){Address = url, Title = "Загрузка...", Height = 1000};
            WebBrowser.FrameLoadStart += Browser_OnFrameLoadStart;
            WebBrowser.FrameLoadEnd += Browser_OnFrameLoadEnd;

            StackPanel stack_PanelContentButton = new StackPanel{Orientation = Orientation.Horizontal};
            stack_PanelContentButton.Children.Add(new Button{Content = "Back",Width = 50, Command = WebBrowser.BackCommand});
            stack_PanelContentButton.Children.Add(new Button{Content = "Forward",Width = 50, Command = WebBrowser.ForwardCommand});
            stack_PanelContentButton.Children.Add(new Button{Content = "Reload",Width = 50, Command = WebBrowser.ReloadCommand});
            TextBox text_BlockAddress = new TextBox();
            text_BlockAddress.SetBinding(TextBox.TextProperty, new Binding{Path = new PropertyPath("WebBrowser.Address"), Source = WebBrowser});
            stack_PanelContentButton.Children.Add(text_BlockAddress);
            
            StackPanel stack_PanelContent = new StackPanel();
            stack_PanelContent.Children.Add(stack_PanelContentButton);
            stack_PanelContent.Children.Add(WebBrowser);
            
            // --- Заголовок --- //
            
            StackPanel stack_PanelHeader = new StackPanel{Orientation = Orientation.Horizontal};
            TextBlock text_Block = new TextBlock
            {
                Margin = new Thickness(5),
                HorizontalAlignment = HorizontalAlignment.Left,
            };
            text_Block.SetBinding(TextBlock.TextProperty, new Binding{Path = new PropertyPath("WebBrowser.Title"), Source = WebBrowser});
            stack_PanelHeader.Children.Add(text_Block);
            Button CloseButton = new Button
            {
                FontSize = 10, Content = "✖", Height = 20, Width = 20,
                BorderThickness = new Thickness(0),
                Background = Brushes.Transparent,
                Visibility = Visibility.Hidden,

            };
            CloseButton.Click += Tab_ClickClose;
            stack_PanelHeader.Children.Add(CloseButton);

            // --- Формирование TabItem --- //
            TabItem tab_Item = new TabItem();
            tab_Item.MouseEnter += Tab_OnFocusableChanged;
            tab_Item.MouseLeave += Tab_OnMouseDown;
            tab_Item.Header = stack_PanelHeader;
            tab_Item.Content = stack_PanelContent;
            _vm.AppTitle = "Загрузка...";
            return tab_Item;
        }
    }
}
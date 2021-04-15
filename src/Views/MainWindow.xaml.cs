﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Browser.ViewModels;
using CefSharp;
using CefSharp.Wpf;
using Notifications.Wpf;

namespace Browser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private MainViewModel _vm;
        ObservableCollection<Favorite> favorits;
        List<History> historys;
        private NotificationManager notificationManager = new NotificationManager();

        public MainWindow()
        {
            InitializeComponent();
            _vm = new MainViewModel();
            this.DataContext = _vm;
            favorits = ManagementSave.loadFavoriteJSON();
            historys = ManagementSave.loadHistoryJSON();
            MenuFavorits.ItemsSource = favorits;
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
                _vm.CountForm += 1;
            }
        }

        private void Browser_OnFrameLoadStart(object sender, FrameLoadStartEventArgs e)
        {
            
        }

        private void Browser_OnFrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            historys.Add(new History{address = e.Url, date = DateTime.Now});
            ManagementSave.saveHistoryJSON(historys);
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
            var index = products.Items.IndexOf((TabItem) ((StackPanel) ((Button) sender).Parent).Parent);
            products.SelectedIndex = index==products.SelectedIndex?products.SelectedIndex-1:products.SelectedIndex;
            products.Items.RemoveAt(index);
            _vm.CountForm -= 1;
        }
        
        private void Address_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                var browser = (ChromiumWebBrowser)((StackPanel)((StackPanel)((TextBox) sender).Parent).Parent).Children[1];
                browser.Load(((TextBox) sender).Text);
            }
        }

         private TabItem Create_tab(string url)
        {
            // --- Контент --- //
            ChromiumWebBrowser WebBrowser = new ChromiumWebBrowser(){Address = url, Title = "Загрузка...", Height = 1000};
            WebBrowser.FrameLoadStart += Browser_OnFrameLoadStart;
            WebBrowser.FrameLoadEnd += Browser_OnFrameLoadEnd;

            StackPanel stack_PanelContentButton = new StackPanel{Orientation = Orientation.Horizontal};
            var BackButton = new Button
            {
                Content = "🢀", Command = WebBrowser.BackCommand,
                BorderThickness = new Thickness(1, 0, 0, 2), BorderBrush = Brushes.Aqua,
                FontSize = 15, Background = Brushes.Transparent
            };
            var ForwardButton = new Button
            {
                Content = "🢂", Command = WebBrowser.ForwardCommand,
                BorderThickness = new Thickness(1,0,0,2), BorderBrush = Brushes.Aqua,
                FontSize = 15, Background = Brushes.Transparent
            };
            var ReloadButton = new Button
            {
                Content = "⭮", Command = WebBrowser.ReloadCommand,
                BorderThickness = new Thickness(1,0,0,2), BorderBrush = Brushes.Aqua,
                FontSize = 15, Background = Brushes.Transparent
            };
            var FavoritButton = new Button
            {
                Content = "★", 
                BorderThickness = new Thickness(1,0,0,2), BorderBrush = Brushes.Aqua,
                FontSize = 15, Background = Brushes.Transparent
            };
            FavoritButton.Click += AddDelFavorite_OnClick;
            BackButton.SetBinding(Button.WidthProperty, new Binding{Path = new PropertyPath("WidthBackButton")});
            ForwardButton.SetBinding(Button.WidthProperty, new Binding{Path = new PropertyPath("WidthForwardButton")});
            ReloadButton.SetBinding(Button.WidthProperty, new Binding{Path = new PropertyPath("WidthReloadButton")});
            FavoritButton.SetBinding(Button.WidthProperty, new Binding{Path = new PropertyPath("WidthFavoritButton")});

            TextBox text_BlockAddress = new TextBox{BorderThickness = new Thickness(1,0,0,2), BorderBrush = Brushes.Aqua};
            text_BlockAddress.SetBinding(TextBox.TextProperty, new Binding{Path = new PropertyPath("WebBrowser.Address"), Source = WebBrowser});
            text_BlockAddress.SetBinding(TextBox.WidthProperty, new Binding{Path = new PropertyPath("WidthTextBoxAddress")});
            stack_PanelContentButton.Children.Add(BackButton);
            stack_PanelContentButton.Children.Add(ForwardButton);
            stack_PanelContentButton.Children.Add(ReloadButton);
            stack_PanelContentButton.Children.Add(text_BlockAddress);
            stack_PanelContentButton.Children.Add(FavoritButton);
            
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
            text_Block.SetBinding(TextBlock.MaxWidthProperty, new Binding{Path = new PropertyPath("MaxWidthTextBlock")});
            stack_PanelHeader.Children.Add(text_Block);
            Button CloseButton = new Button
            {
                FontSize = 10, Content = "✖", Height = 20, Width = 20,
                BorderThickness = new Thickness(0),
                Background = Brushes.Transparent,
                Visibility = Visibility.Hidden,

            };
            CloseButton.SetBinding(Button.WidthProperty, new Binding{Path = new PropertyPath("WidthButtonClose")});
            CloseButton.Click += Tab_ClickClose;
            stack_PanelHeader.Children.Add(CloseButton);

            // --- Формирование TabItem --- //
            TabItem tab_Item = new TabItem();
            tab_Item.SetBinding(TabItem.MaxWidthProperty, new Binding{Path = new PropertyPath("MaxWidthItem")});
            tab_Item.MouseEnter += Tab_OnFocusableChanged;
            tab_Item.MouseLeave += Tab_OnMouseDown;
            tab_Item.Header = stack_PanelHeader;
            tab_Item.Content = stack_PanelContent;
            return tab_Item;
        }

         private void Window_ClickClose(object sender, RoutedEventArgs e)
         {
             this.Close();
         }
         private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
         {
             DragMove();
         }

         private void Window_ClickWrap(object sender, RoutedEventArgs e)
         {
             this.WindowState = WindowState.Minimized;
         }

         private bool IsMaximized;
         private void Window_ClickWindowMode(object sender, RoutedEventArgs e)
         {
             this.WindowState = IsMaximized?WindowState.Normal:WindowState.Maximized;
             IsMaximized = !IsMaximized;
             ((Button) sender).Content = IsMaximized ?"❐":"□";
         }

         private void OpenTabFavorite_OnClick(object sender, RoutedEventArgs e)
         {
             int lastIndex = products.Items.Count - 1;
             var tab_Item = Create_tab(((MenuItem)sender).Uid);
             var test = products.Items[lastIndex];
             products.Items[lastIndex] = tab_Item;
             products.Items.Add(test);
             products.SelectedIndex = lastIndex;
             _vm.CountForm += 1;
         }

         private void AddDelFavorite_OnClick(object sender, RoutedEventArgs e)
         {
             var browser = ((ChromiumWebBrowser)((StackPanel)((StackPanel)((Button)sender).Parent).Parent).Children[1]);
             try
             {
                 favorits.Remove(favorits.Single(favorit => favorit.address == browser.Address));
                 notificationManager.Show(new NotificationContent
                 {
                     Title = "Закладка удалена",
                     Message = browser.Title,
                     Type = NotificationType.Error
                 });
             }
             catch (Exception exception)
             {
                 favorits.Add(new Favorite{title = browser.Title, address = browser.Address});
                 notificationManager.Show(new NotificationContent
                 {
                     Title = "Закладка добавлена",
                     Message = browser.Title,
                     Type = NotificationType.Success
                 });
             }
             ManagementSave.saveFavoriteJSON(favorits);
         }

         private int historyIndex = 0;
         private void MenuHistoryVisible_OnClick(object sender, RoutedEventArgs e)
         {
             GridHistory.Visibility = Visibility.Visible;
             GridHistory.IsEnabled = true;
             
             GridMain.Visibility = Visibility.Hidden;
             GridMain.IsEnabled = false;
             for (int i = historyIndex; i < historys.Count-1; i++)
             {
                 ListBoxHistory.Items.Add(historys[i].date+": "+historys[i].address);
             }

             historyIndex = historys.Count() - 1;
         }

         private void GridHistoryClose_OnClick(object sender, RoutedEventArgs e)
         {
             GridHistory.Visibility = Visibility.Hidden;
             GridHistory.IsEnabled = false;
             
             GridMain.Visibility = Visibility.Visible;
             GridMain.IsEnabled = true;
         }

         private void ListBoxHistory_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
         {
             string text = ((String) ListBoxHistory.SelectedItem).Split(' ')[2];
             
             Clipboard.SetText(text);

             notificationManager.Show(new NotificationContent
             {
                 Title = "Успешно скопиравано",
                 Message = text,
                 Type = NotificationType.Information
             });
         }
    }
}
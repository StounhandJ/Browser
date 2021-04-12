using System.ComponentModel;


namespace Browser.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {
        private int _maxWidthItem = 200;
        private int _WidthButtonClose = 20;
        private int _countForm = 1;
        private int _WidthWindow = 100;
        public int MaxWidthItem 
        {
            get
            {
                if (_countForm*_maxWidthItem>_WidthWindow-100)
                {
                    return (_WidthWindow - 100) / _countForm;
                }
                else
                {
                    return _maxWidthItem;
                }
            }
            set 
            {
                if (_maxWidthItem == value) return;

                _maxWidthItem = value;
                this.OnPropertyChanged("MaxWidthItem");
                this.OnPropertyChanged("MaxWidthTextBlock");
            }
        }
        public int MaxWidthTextBlock 
        {
            get
            {
                return MaxWidthItem - _WidthButtonClose-20;
            }
            set 
            {
                
            }
        }
        
        public int WidthButtonClose 
        {
            get
            {
                return _WidthButtonClose;
            }
            set 
            {
                
            }
        }
        public int CountForm 
        {
            get
            {
                return _countForm;
            }
            set 
            {
                if (_countForm == value) return;

                _countForm = value;
                this.OnPropertyChanged("MaxWidthItem");
                this.OnPropertyChanged("MaxWidthTextBlock");
            }
        }
        
        public int WidthWindow 
        {
            get
            {
                return _WidthWindow;
            }
            set 
            {
                if (_WidthWindow == value) return;

                _WidthWindow = value;
                this.OnPropertyChanged("MaxWidthItem");
                this.OnPropertyChanged("MaxWidthTextBlock");
                this.OnPropertyChanged("WidthWindow");
                update();
            }
        }
        
        private int _widthButtonLast = 50;
        private int _widthButtonNext = 50;
        private int _widthButtonReload = 50;
        private int _favoritCommand = 50;

        public int WidthBackButton 
        {
            get
            {
                return _widthButtonLast;
            }
            set 
            {
                
            }
        }

        public int WidthForwardButton 
        {
            get
            {
                return _widthButtonNext;
            }
            set 
            {
                
            }
        }
        
        public int WidthReloadButton 
        {
            get
            {
                return _widthButtonReload;
            }
            set 
            {
                
            }
        }
        
        public int WidthTextBoxAddress 
        {
            get
            {
                return _WidthWindow-_widthButtonLast-_widthButtonNext-_widthButtonReload-_favoritCommand;
            }
            set 
            {
                
            }
        }
        
        public int WidthFavoritButton 
        {
            get
            {
                return _favoritCommand;
            }
            set 
            {
                
            }
        }
        public void update()
        {
            this.OnPropertyChanged("WidthBackButton");
            this.OnPropertyChanged("WidthForwardButton");
            this.OnPropertyChanged("WidthReloadButton");
            this.OnPropertyChanged("WidthTextBoxAddress");
            this.OnPropertyChanged("WidthFavoritButton");
        }
		
        public event PropertyChangedEventHandler PropertyChanged;
		
        protected virtual void OnPropertyChanged(string propertyName)
        {			
            if (this.PropertyChanged!= null)			
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }	
    }
}
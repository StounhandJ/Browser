using System.ComponentModel;
using System.Diagnostics;


namespace Browser.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {
        private int _maxWidthItem = 200;
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
                this.OnPropertyChanged("WidthWindow");
            }
        }
		
        public event PropertyChangedEventHandler PropertyChanged;
		
        protected virtual void OnPropertyChanged(string propertyName)
        {			
            if (this.PropertyChanged!= null)			
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }	
    }
}
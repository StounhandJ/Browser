using System.ComponentModel;
using System.Diagnostics;


namespace Browser.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {
        private int _maxWidthItem = 60;
        private int _countForm = 1;
        public int MaxWidthItem 
        {
            get
            {
                return _maxWidthItem-(_countForm*10);
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
		
        public event PropertyChangedEventHandler PropertyChanged;
		
        protected virtual void OnPropertyChanged(string propertyName)
        {			
            if (this.PropertyChanged!= null)			
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }	
    }
}
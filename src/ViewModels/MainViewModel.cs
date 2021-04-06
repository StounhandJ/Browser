using System.ComponentModel;


namespace Browser.ViewModels
{
    class MainViewModel : INotifyPropertyChanged
    {
        private string _appTitle = "Браузер";

        public string AppTitle 
        {
            get { return _appTitle; }
            set 
            {
                if (_appTitle == value) return;

                _appTitle = value;
                this.OnPropertyChanged("AppTitle");
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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;


namespace PersonRest
{
    public class Person:INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        
        public int Id { get; set; }
        private string _name;

        

        public string Name
        {
            get { return _name; }
            set { _name=value;
                OnPropertyChanged();
            }
        }
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Domain
{
    public class Post : INotifyPropertyChanged
    {
        private string number = "149/4/";
        private DateTime dateStamp = DateTime.Now.Date;
        private string numberIncome = "";
        private DateTime dateIncome = DateTime.Now.Date;

        public string Number
        { 
            get => number;
            set
            {
                number = value;
                OnPropertyChanged(nameof(Number));
            }
        }
        public DateTime DateStamp 
        {
            get => dateStamp;
            set
            {
                dateStamp = value;
                OnPropertyChanged(nameof(DateStamp));
            }
        }
        public string NumberIncome 
        { 
            get => numberIncome;
            set
            {
                numberIncome = value;
                OnPropertyChanged(nameof(NumberIncome));
            }
        }
        public DateTime DateIncome 
        {
            get => dateIncome;
            set
            {
                dateIncome = value;
                OnPropertyChanged(nameof(DateIncome));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}

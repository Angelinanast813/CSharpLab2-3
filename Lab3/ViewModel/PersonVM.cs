using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Lab3
{
    internal class PersonVM : INotifyPropertyChanged
    {
        private string _name = "";
        private string _surname = "";
        private string _eadress = "";
        private DateTime _date = DateTime.Today;
        private string _birthday;
        private string _isadult;
        private string _sunsign;
        private string _chinsign;


        private bool _canExecute;
        private RelayCommand _calculateAgeCommand;
        private readonly Action _closeAction;
        
        internal PersonVM(Action closeAction)
        {       
            _closeAction = closeAction;       
            CanExecute = false;
        }

        #region Properties
        public DateTime Date
        {
            get { return _date; }
            set
            {
                _date = value;
                CanExecute = IsNotEmpty();
                OnPropertyChanged();
            }
        }

        public string DateString
        {
            get { return _date.ToShortDateString(); }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                CanExecute = IsNotEmpty();
                OnPropertyChanged();
            }
        }

        public string Surname
        {
            get { return _surname; }
            set
            {
                _surname = value;
                CanExecute = IsNotEmpty();
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get { return _eadress; }
            set
            {
                _eadress = value;
                CanExecute = IsNotEmpty();
                OnPropertyChanged();
            }
        }

        public string Birthday
        {
            get { return _birthday; }
            set
            {
                _birthday = value;
                OnPropertyChanged();
            }
        }

        public string Adult
        {
            get { return _isadult; }
            set
            {
                _isadult = value;

                OnPropertyChanged();
            }
        }

        public string WesternZodiac
        {
            get { return _sunsign; }
            private set
            {
                _sunsign = value;
                OnPropertyChanged();
            }
        }

        public string ChineseZodiac
        {
            get { return _chinsign; }
            private set
            {
                _chinsign = value;
                OnPropertyChanged();
            }
        }

        public bool CanExecute
        {
            get { return _canExecute; }
            private set
            {
                _canExecute = value;
                OnPropertyChanged();
            }
        }    
        #endregion

        public RelayCommand CalculateAgeCommand
        {
            get { return _calculateAgeCommand ?? (_calculateAgeCommand = new RelayCommand(DataPersCalcul)); }
        }
      
        internal static Person CreateUser(string firstName, string lastName, string email, DateTime date)
        {
            return new Person(firstName, lastName, email, date);
        }

        private async void DataPersCalcul(object o)
        {
           
            try
            {
                await Task.Run(() =>
                {
                    CreatingNewPerson.CreatedPerson = CreateUser(_name, _surname, _eadress, _date);
                    Thread.Sleep(500);
                   

                    if (DateTime.Today.DayOfYear == _date.DayOfYear)
                        MessageBox.Show($"Happy Birthday, dear {Name}!");

                    if (CreatingNewPerson.CreatedPerson.IsBirthday == true)
                    {
                        Birthday = $"Today is {_name}'s birthday";
                    }
                    else
                    {
                        Birthday = $"Today is not {_name}'s birthday";
                    }


                    if (CreatingNewPerson.CreatedPerson.IsAdult == true)
                    {
                        Adult = "Yes";
                    }
                    else
                    {
                        Adult = "No";
                    }

                    WesternZodiac = CreatingNewPerson.CreatedPerson.SunSign;
                    ChineseZodiac = CreatingNewPerson.CreatedPerson.ChineseSign;
                });
                _closeAction.Invoke();
                CanExecute = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                ClearInputValues();
            }
        
        }
       
        private void ClearInputValues()
        {
         
            Email = "";
        }

        private void ClearValues()
        {
            CanExecute = false;
            Birthday = "";
            Adult = "";
            WesternZodiac = "";
            ChineseZodiac = "";
        }

        private bool IsNotEmpty()
        {
            return _name != "" && _surname != "" && _eadress != "";
        }

        #region Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
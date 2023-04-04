using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HospitalWPF.User_Controls
{
    class SearchViewModel : INotifyPropertyChanged
    {
        public delegate void RunThis(string box);
        private string searchBox;
        private RunThis thing;
        public SearchViewModel(RunThis thing)
        {
            this.thing = thing;
        }

        public string SearchBox
        {
            get { return searchBox; }
            set
            {
                searchBox = value;
                OnPropertyChanged();
                thing(SearchBox);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}

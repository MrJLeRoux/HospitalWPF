using HospitalWPF.User_Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows;

namespace HospitalWPF
{
    class ViewModel : INotifyPropertyChanged
    {
        private string connectString = "Server = DESKTOP-PNLJN82\\SQLEXPRESS; Database=HospitalAdmin; Integrated Security=true;";
        private SearchViewModel customVM;
        private List<string> filteredList = new List<string>();
        private List<string> days = new List<string>();
        private List<string> months = new List<string>();
        private List<string> years = new List<string>();
        private List<string> fullList = new List<string>();
        private List<string> occupationsList = new List<string>() { "", "Doctor", "Nurse", "Custodian" };
        private string identity;
        private string firstName;
        private string lastName;
        private string selectedDay;
        private string selectedMonth;
        private string selectedYear;
        private string occupation;
        private string specLevel;
        private string selected;
        private AddButton adder;
        private RemoveButton removeButton;
        private ClearButton clearButton;
        public event PropertyChangedEventHandler? PropertyChanged;

        public ViewModel()
        {
            Adder = new AddButton(this);
            RemoveButton = new RemoveButton(this);
            ClearButton = new ClearButton(this);
            SearchViewModel.RunThis handler = SearchFilter;
            CustomVM = new SearchViewModel(handler);
            PopulateList(fullList);
            FilteredList = new List<string>(fullList);
            PopulateDateList(1, 31, Days);
            PopulateDateList(1, 12, Months);
            PopulateDateList(1900, 2023, Years);
        }

        public SearchViewModel CustomVM
        {
            get => customVM;
            set
            {
                customVM = value;
                OnPropertyChanged();
            }
        }

        public AddButton Adder
        {
            get { return adder; }
            set
            {
                adder = value;
                OnPropertyChanged();
            }
        }

        public RemoveButton RemoveButton
        {
            get { return removeButton; }
            set
            {
                removeButton = value;
                OnPropertyChanged();
            }
        }

        public ClearButton ClearButton
        {
            get { return clearButton; }
            set
            {
                clearButton = value;
                OnPropertyChanged();
            }
        }

        public List<string> FilteredList
        {
            get { return filteredList; }
            set
            {
                filteredList = value;
                OnPropertyChanged();
            }
        }

        public List<string> Days
        {
            get { return days; }
            set
            {
                days = value;
                OnPropertyChanged();
            }
        }

        public List<string> Months
        {
            get { return months; }
            set
            {
                months = value;
                OnPropertyChanged();
            }
        }

        public List<string> Years
        {
            get { return years; }
            set
            {
                years = value;
                OnPropertyChanged();
            }
        }

        public List<string> OccupationsList
        {
            get { return occupationsList; }
            set
            {
                occupationsList = value;
                OnPropertyChanged();
            }
        }

        public string Identity
        {
            get { return identity; }
            set
            {
                identity = value;
                OnPropertyChanged();
            }
        }

        public string Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                OnPropertyChanged();
                Display();
            }
        }

        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                OnPropertyChanged();
            }
        }

        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                OnPropertyChanged();
            }
        }

        public string SelectedDay
        {
            get { return selectedDay; }
            set
            {
                selectedDay = value;
                OnPropertyChanged();
            }
        }

        public string SelectedMonth
        {
            get { return selectedMonth; }
            set
            {
                selectedMonth = value;
                OnPropertyChanged();
            }
        }

        public string SelectedYear
        {
            get { return selectedYear; }
            set
            {
                selectedYear = value;
                OnPropertyChanged();
            }
        }

        public string Occupation
        {
            get { return occupation; }
            set
            {
                occupation = value;
                OnPropertyChanged();
            }
        }

        public string SpecLevel
        {
            get { return specLevel; }
            set
            {
                specLevel = value;
                OnPropertyChanged();
            }
        }


        public void OnPropertyChanged([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private void SearchFilter(string box)
        {
            PopulateList(fullList);
            FilteredList.Clear();
            foreach (string f in fullList)
            {
                if (f.Contains(box, StringComparison.OrdinalIgnoreCase) || box.Length == 0)
                {
                    FilteredList.Add(f);
                }
            }
            OnPropertyChanged("FilteredList");
        }

        private void PopulateList(List<string> list)
        {
            list.Clear();
            string query = "Select * FROM Doctors;";
            using SqlConnection Connection = new SqlConnection(connectString);
            Connection.Open();
            SqlCommand command = new SqlCommand(query, Connection);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                list.Add("Dr. " + (string)reader[2]);
            }
            reader.Close();
            query = "Select * FROM Nurses;";
            command = new SqlCommand(query, Connection);
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                list.Add("Nurse " + (string)reader[2]);
            }
            reader.Close();
            query = "Select * FROM Custodians;";
            command = new SqlCommand(query, Connection);
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                list.Add("Custodian " + (string)reader[2]);
            }
            Connection.Close();
        }

        public void AddToDatabase()
        {
            if (IsDateInvalid())
            {
                InputError("Invalid date. Please try again.");
            }
            else
            {
                decimal id = 0;
                string bday = $"{SelectedYear}-{SelectedMonth}-{SelectedDay}";
                Occupation = Occupation.Remove(0, Occupation.IndexOf(' ') + 1);
                string query = $"INSERT INTO Employees (firstname, lastname, birthdate, occupation) VALUES ('{FirstName}', '{LastName}', '{bday}', '{Occupation}') SELECT SCOPE_IDENTITY();";
                using SqlConnection connection = new SqlConnection(connectString);
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    id = (decimal)reader[0];
                }
                reader.Close();
                connection.Close();
                switch (Occupation)
                {
                    case "Doctor":

                        query = $"INSERT INTO Doctors (EmployeeID, firstname, lastname, birthdate, specialization) VALUES ({id}, '{FirstName}', '{LastName}', '{bday}', '{SpecLevel}')";
                        connection.Open();
                        command = new SqlCommand(query, connection);
                        command.ExecuteNonQuery();
                        connection.Close();
                        break;

                    case "Nurse":

                        query = $"INSERT INTO Nurses (EmployeeID, firstname, lastname, birthdate, nurselevel) VALUES ({id}, '{FirstName}', '{LastName}', '{bday}', '{SpecLevel}')";
                        connection.Open();
                        command = new SqlCommand(query, connection);
                        command.ExecuteNonQuery();
                        connection.Close();
                        break;

                    case "Custodian":

                        query = $"INSERT INTO Custodians (EmployeeID, firstname, lastname, birthdate) VALUES ({id}, '{FirstName}', '{LastName}', '{bday}')";
                        connection.Open();
                        command = new SqlCommand(query, connection);
                        command.ExecuteNonQuery();
                        connection.Close();
                        break;

                    default:

                        query = "DELETE FROM Employees WHERE id=(SELECT MAX(id) FROM Employees)";
                        connection.Open();
                        command = new SqlCommand(query, connection);
                        command.ExecuteNonQuery();
                        connection.Close();
                        MessageBox.Show("Wrong occupation selected. Please try again.", "Oops!", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                }
                SearchFilter("");
            }

        }

        private void InputError(string message)
        {
            Clear();
            MessageBox.Show(message, "Uh-Oh!", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void PopulateDateList(int startnum, int endnum, List<string> usedList)
        {
            usedList.Add("");
            for (int i = startnum; i <= endnum; i++)
            {
                if (i < 10)
                {
                    usedList.Add($"0{i}");
                }
                else
                {
                    usedList.Add($"{i}");
                }
            }
        }

        private bool IsDateInvalid() => Int32.Parse(SelectedDay) > DateTime.DaysInMonth(Int32.Parse(SelectedYear), Int32.Parse(SelectedMonth));

        public void Display()
        {
            if (!String.IsNullOrEmpty(Selected))
            {
                string date = "";
                LastName = Selected.Remove(0, Selected.IndexOf(" ") + 1);
                string commandString = $"SELECT * FROM Employees WHERE lastname = '{LastName}';";
                using SqlConnection connection = new SqlConnection(connectString);
                connection.Open();
                SqlCommand command = new SqlCommand(commandString, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Identity = reader[0].ToString();
                    FirstName = (string)reader[1];
                    date = reader[3].ToString();
                    Occupation = reader[4].ToString();
                }
                reader.Close();
                SelectedYear = date.Substring(0, 4);
                SelectedMonth = date.Substring(5, 2);
                SelectedDay = date.Substring(8, 2);
                switch (Occupation)
                {
                    case "Doctor":

                        commandString = $"SELECT specialization FROM Doctors WHERE EmployeeID = {Identity};";
                        command = new SqlCommand(commandString, connection);
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            SpecLevel = (string)reader[0];
                        }
                        reader.Close();
                        break;

                    case "Nurse":

                        commandString = $"SELECT nurselevel FROM Nurses WHERE EmployeeID = {Identity};";
                        command = new SqlCommand(commandString, connection);
                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            SpecLevel = (string)reader[0];
                        }
                        reader.Close();
                        break;

                    case "Custodian":

                        SpecLevel = "n/a";
                        break;

                    default:
                        break;
                }
                connection.Close();
            }
        }

        public void Remove()
        {
            try
            {
                bool done = false;
                string query = "";
                SqlCommand command;

                using SqlConnection connection = new SqlConnection(connectString);
                connection.Open();

                switch (Occupation)
                {
                    case "Doctor":

                        query = $"DELETE FROM Doctors WHERE EmployeeID = {Identity}";
                        command = new SqlCommand(query, connection);
                        command.ExecuteNonQuery();
                        done = true;
                        break;

                    case "Nurse":

                        query = $"DELETE FROM Nurses WHERE EmployeeID = {Identity}";
                        command = new SqlCommand(query, connection);
                        command.ExecuteNonQuery();
                        done = true;
                        break;

                    case "Custodian":

                        query = $"DELETE FROM Custodians WHERE EmployeeID = {Identity}";
                        command = new SqlCommand(query, connection);
                        command.ExecuteNonQuery();
                        done = true;
                        break;

                    default:

                        MessageBox.Show("Please select an employee to use the 'Remove' function.");
                        done = false;
                        break;
                }
                if (done == true)
                {
                    query = $"DELETE FROM Employees WHERE id = {Identity}";
                    command = new SqlCommand(query, connection);
                    command.ExecuteNonQuery();
                    FilteredList.Remove(Selected);
                    OnPropertyChanged("FilteredList");
                }
            }
            catch
            {
                MessageBox.Show("An error has occurred. Please contact your administrator.");
            }
        }

        public void Clear()
        {
            Identity = "";
            FirstName = "";
            LastName = "";
            SelectedDay = "";
            SelectedMonth = "";
            SelectedYear = "";
            Occupation = "";
            OnPropertyChanged("Occupation");
            SpecLevel = "";
        }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PresentationWPF.Models
{
    // NOTE: THIS CLASS IS ONLY FOR DEMONSTRATING DATA BINDING
    // This class implements INotifyPropertyChanged
    // to support one-way and two-way bindings
    // such that the UI element updates dynamically,
    // when the source has been changed.
    internal class Person : INotifyPropertyChanged
    {
        // Declare the event
        public event PropertyChangedEventHandler PropertyChanged;

        private String name;
        private int age;
        private int weight;
        private int score;
        private bool accepted;

        public Person(string name, int age, int weight, int score, bool accecpted)
        {
            this.name = name;
            this.age = age;
            this.weight = weight;
            this.score = score;
            this.accepted = accecpted;
        }

        public override string ToString()
        {
            String txt = "";
            txt += base.ToString() + "\n";
            txt += "Name: " + this.name + "\n";
            txt += "Age:" + this.age + "\n";
            txt += "Weight: " + this.weight + "\n";
            txt += "Score: " + this.score + "\n";
            txt += "Accepted: " + this.accepted + "\n";
            return txt;
        }

        public String Name
        {
            get { return this.name; }
            set
            {
                this.name = value;
                // Call OnPropertyChanged whenever the property is updated
                OnPropertyChanged();
            }
        }
        public int Age
        {
            get { return this.age; }
            set
            {
                this.age = value;
                OnPropertyChanged();
            }
        }
        public int Weight
        {
            get { return this.weight; }
            set
            {
                this.weight = value;
                OnPropertyChanged();
            }
        }
        public int Score
        {
            get { return this.score; }
            set
            {
                this.score = value;
                OnPropertyChanged();
            }
        }
        public Boolean Accepted
        {
            get { return this.accepted; }
            set
            {
                this.accepted = value;
                OnPropertyChanged();
            }
        }

        // Create the OnPropertyChanged method to raise the event
        // The calling member's name will be used as the parameter.
        // NOTE: [CallerMemberName] requires default value.
        protected void OnPropertyChanged([CallerMemberName] string methodName = null)
        {
            if (methodName != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(methodName));
                Trace.WriteLine("CallerMemberName: " + methodName);
            }
        }
    }
}

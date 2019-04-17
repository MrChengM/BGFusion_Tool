using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace BGFusionTools.Helper
{
    public class DataString :INotifyPropertyChanged
    {
        private string mystring;
        private bool? mybool;
        public DataString()
        {

        }
        public DataString(string sString,bool? bBool)
        {
            this.mystring=sString;
            this.mybool = bBool;
        }
        public string MyString
        {
            get { return mystring; }
            set
            {
                mystring = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("MyString"));
                }
            }

        }
        public bool? Mybool
        {
            get { return mybool; }
            set
            {
                mybool = value;
                if (PropertyChanged != null)
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Mybool"));
                }
            }

        }

        public event PropertyChangedEventHandler PropertyChanged; 

    }
}

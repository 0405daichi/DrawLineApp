using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using System.ComponentModel;
using System.Windows.Input;

namespace WpfApp3
{
    public class MyLine : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private bool _isSelected;
        
        public MyLine()
        {
            Start = new LinePoint(50, 50, this);
            End = new LinePoint(100, 50, this);
            _isSelected = false;
        }

        public LinePoint Start { get; set; }

        public LinePoint End { get; set; }

        public string LineName { get; set; }
        
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                if (_isSelected == value)
                    return;
                _isSelected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsSelected"));
            }
        }
        
    }
}

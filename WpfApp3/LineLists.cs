using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Media;
namespace WpfApp3
{
    public class LineLists
    {
        private double _index = 1;
        public ObservableCollection<MyLine> List { get; }
        public LineLists()
        {
            List = new ObservableCollection<MyLine>();
        }
        public ICommand Command => new RelayCommand(AddList);
        private void AddList(object param)
        {
            List.Add(new MyLine() { LineName = $"Line{_index}"});
            _index++;
        }
    }
}

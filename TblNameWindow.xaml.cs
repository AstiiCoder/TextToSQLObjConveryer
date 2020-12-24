using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfAppTest5
    {
    /// <summary>
    /// Логика взаимодействия для TblNameWindow.xaml
    /// </summary>
    public partial class TblNameWindow : Window
        {
        public TblNameWindow()
            {
            InitializeComponent();
            }

        private void Accept_Click(object sender, RoutedEventArgs e)
            {
            this.DialogResult = true;
            }

        public string TblName
            {
            get { return TblNameBox.Text; }
            }

        }
    }

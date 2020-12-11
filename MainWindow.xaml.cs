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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfAppTest5
    {
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
        {
        public MainWindow()
            {
            InitializeComponent();
            }

        private void Convert()
            {
            TextBoxSQL.Text = String.Empty;
            string SQL_str = "";
            string s = string.Empty;
            for (int i = 0; i < TextBoxGraph.LineCount; i++)
                {
                for (int j = 0; j < TextBoxGraph.GetLineText(i).Length; j++)
                    {
                    s = TextBoxGraph.GetLineText(i);
                    if ((s[j] == '|'))
                        {
                        s.Substring(j, s.Length - j).Trim();
                        break;
                        }
                    else
                        s = string.Empty;
                    
                    }
                if (s != string.Empty) TextBoxSQL.Text += s + "\n"; 
                }
            }

        private void TextBlockGraph_MouseEnter(object sender, MouseEventArgs e)
            {
            TextBoxGraph.Text = Clipboard.GetText();
            Convert();
            }
        }
    }

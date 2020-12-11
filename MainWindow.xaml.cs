using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
            TextBoxSQL.Text = string.Empty;
            string SQL_str = string.Empty;
            string s = string.Empty;
            bool TypesDetected = false; 
            for (int i = 0; i < TextBoxGraph.LineCount; i++)
                {
                for (int j = 0; j < TextBoxGraph.GetLineText(i).Length; j++)
                    {
                    s = TextBoxGraph.GetLineText(i);
                    if ((s[j] == '|'))
                        {
                        s.Substring(j, s.Length - j).Trim().Replace("  ", " ");
                        break;
                        }
                    else
                        s = string.Empty;                   
                    }

                if (s != string.Empty)
                    {
                    if (SQL_str == string.Empty)
                        {
                        if (TypesDetected == false)
                            {
                            //получим список полей
                            s = Regex.Replace(s, @"\s+", " ").Replace(" ", "");
                            string[] strs = s.Substring(1, s.Length - 2).Split('|');
                            //массив типов полей
                            string[] types = new string[strs.Length];
                            //если у таблицы есть хотя-бы одна строка, пробуем определить тип
                            if (TextBoxGraph.LineCount > 5)
                                {                               
                                string LineTwo = Regex.Replace(TextBoxGraph.GetLineText(i+2), @"\s+", " ").Replace(" ", "");
                                LineTwo = LineTwo.Substring(1, LineTwo.Length - 2).Trim().Replace("  ", " ");
                                MessageBox.Show(LineTwo);
                                string[] s1 = LineTwo.Split('|');
                                for (int t = 0; t < strs.Length; t++)
                                    {
                                    types[t] = "int";
                                    int num;
                                    try
                                        { 
                                        bool isInt = int.TryParse(s1[t], out num);
                                        if (isInt == false) types[t] = "varchar(50)"; 
                                        }
                                    catch
                                        { 
                                        }
                                    //if (s1[t].Contains(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator)) types[t] = "float";
                                    }
                                }
                            //синтаксис создания таблицы 
                            SQL_str = "create table ... (";
                            for (int k = 0; k < strs.Length; k++)
                                {
                                SQL_str = SQL_str + strs[k].Replace("|", ", ") + " " + types[k];
                                if (k != strs.Length - 1) SQL_str += ", ";
                                }
                            SQL_str += ")\n";
                            TypesDetected = true;
                            }
                        
                        }
                    else
                        {
                        //вставка значений по полям
                        s = Regex.Replace(s, @"\s+", " ").Replace(" ", "");
                        s = s.Substring(1, s.Length - 2);
                        if (SQL_str.Contains("select ")) SQL_str += "union all "; 
                        SQL_str += "select (" + s.Replace("|", ",") + ")\n"; 
                        }
                        
                    } 
                
                }
            TextBoxSQL.Text = SQL_str;
            }

        private void TextBlockGraph_MouseEnter(object sender, MouseEventArgs e)
            {
            if (TextBoxGraph.Text != Clipboard.GetText())
                { 
                TextBoxGraph.Text = Clipboard.GetText();
                Convert();               
                }         
            }
        }
    }

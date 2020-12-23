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
        public static string[] types;

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
            string MayBeName = string.Empty;

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
                            types = new string[strs.Length];
                            //если у таблицы есть хотя-бы одна строка, пробуем определить тип
                            if ((TextBoxGraph.LineCount > 5) && (TextBoxGraph.GetLineText(i + 2).Length>3))
                                {
                                string LineTwo = Regex.Replace(TextBoxGraph.GetLineText(i+2), @"\s+", " ").Replace(" ", "");
                                LineTwo = LineTwo.Substring(1, LineTwo.Length - 2).Trim().Replace("  ", " ");
                                string[] s1 = LineTwo.Split('|');                             
                                for (int t = 0; t < strs.Length; t++)
                                    {
                                    types[t] = "int";
                                    int num;
                                    try
                                        { 
                                        bool isInt = int.TryParse(s1[t], out num);
                                        if (isInt == false)
                                            {
                                            if (MayBeName == string.Empty) MayBeName = strs[t] + "s";
                                            types[t] = "varchar(50)"; 
                                            } 
                                        }
                                    catch
                                        { 
                                        }
                                    if (s1[t].Contains(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator))
                                        {
                                        float number;
                                        if (Single.TryParse(s1[t], out number)) types[t] = "float";
                                        } 
                                    }
                                }
                            //синтаксис создания таблицы 
                            if (MayBeName == string.Empty) MayBeName = "...";
                            SQL_str = "create table " + MayBeName + " (";
                            for (int k = 0; k < strs.Length; k++)
                                {                               
                                SQL_str = SQL_str + strs[k].Replace("|", ", ") + " " + types[k];
                                if (k != strs.Length - 1) SQL_str += ", ";
                                }
                            SQL_str += ")\n";
                            SQL_str += "insert into " + MayBeName + Environment.NewLine;
                            TypesDetected = true;
                            }                       
                        }
                    else
                        {
                        //вставка значений по полям
                        string Line = Regex.Replace(s, @"\s+", " ").Replace(" ", "");
                        Line = Line.Substring(1, Line.Length - 2).Trim().Replace("  ", " ");
                        string[] s1 = Line.Split('|');
                        string sline="";
                        for (int t = 0; t < s1.Length; t++)
                            {
                            if (types[t] == "varchar(50)")
                                {
                                sline += "'" + s1[t] + "'";
                                }
                            else
                                {
                                sline += s1[t];
                                }
                            if (t != s1.Length-1) sline += ", ";
                            }
                        if (SQL_str.Contains("select ")) SQL_str += "union all select " + sline + "\n";
                        else SQL_str += "select " + sline + "\n";
                        }
                        
                    } 
                
                }
            TextBoxSQL.Text = SQL_str;
            }

        private void InsertFromClibboard()
            {
            if ((Clipboard.GetText()!=String.Empty) && (TextBoxGraph.Text != Clipboard.GetText()) )
                {
                TextBoxGraph.Text = Clipboard.GetText();
                Convert();
                }
            }

        private void TextBlockGraph_MouseEnter(object sender, MouseEventArgs e)
            {
            InsertFromClibboard();
            }

        private void TextBoxGraph_DragOver(object sender, DragEventArgs e)
            {
            //InsertFromClibboard();
            }
        }
    }

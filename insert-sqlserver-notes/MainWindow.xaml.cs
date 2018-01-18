using Microsoft.Win32;
using SharpYaml.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace insert_sqlserver_notes
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Msg("请选择或拖入配置文件");
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {

        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void ImpFileBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "配置文件(*.yml)|*.yml";
            dialog.Multiselect = false;
            if (dialog.ShowDialog() == true)
            {
                FileStream fs = new FileStream(dialog.FileName, FileMode.Open, FileAccess.Read);

                var serializer = new Serializer();
                ymldoc ymldoc = new insert_sqlserver_notes.ymldoc();
                try
                {
                    ymldoc = serializer.Deserialize<ymldoc>(fs);
                }
                catch (Exception ex)
                {
                    Msg("解析配置文件异常：" + ex.Message);
                    return;
                }
                SqlShow ss = new insert_sqlserver_notes.SqlShow();
                ss.messagelog.Text = "";
                foreach (var table in ymldoc.table)
                {
                    if (string.IsNullOrEmpty(table.name))
                    {
                        break;
                    }
                    ss.messagelog.AppendText($"IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 'SCHEMA', N'dbo', 'TABLE', N'{table.name}', null, null)) > 0) execute sp_updateextendedproperty N'MS_Description',N'{table.note}',N'Schema',N'dbo',N'table',N'{table.name}',null,null ELSE execute sp_addextendedproperty N'MS_Description',N'{table.note}',N'Schema',N'dbo',N'table',N'{table.name}',null,null;\r");
                    foreach (var column in table.column)
                    {
                        if (string.IsNullOrEmpty(column.name))
                        {
                            break;
                        }
                        ss.messagelog.AppendText($"IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 'SCHEMA', N'dbo', 'TABLE', N'{table.name}', 'COLUMN', N'{column.name}')) > 0) execute sp_updateextendedproperty N'MS_Description',N'{column.note}',N'Schema',N'dbo',N'table',N'{table.name}',N'column',N'{column.name}' ELSE execute sp_addextendedproperty N'MS_Description',N'{column.note}',N'Schema',N'dbo',N'table',N'{table.name}',N'column',N'{column.name}';\r");
                    }
                }
                ss.Show();
            }
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            Msg("程序退出");
            this.Close();
        }

        private void TextBox_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
            e.Handled = true;
        }

        private void Msg(string msg)
        {
            messagelog.AppendText(msg + "\n");
            msg += "  [" + DateTime.Now.ToString("yyyyMMddHHmmss") + "]";
            string logPath = AppDomain.CurrentDomain.BaseDirectory + "Log/";
            if (!System.IO.Directory.Exists(logPath))
                System.IO.Directory.CreateDirectory(logPath);
            string logFile = logPath + "Log-" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
            System.IO.File.AppendAllLines(logFile, new string[] { msg });
        }
    }
}

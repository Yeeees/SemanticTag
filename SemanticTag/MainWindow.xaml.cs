using System;
using System.Collections.Generic;
using System.Data;
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

namespace SemanticTag
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static BasicBase bb = new BasicBase();


        IOTxt txtManager = new IOTxt();
        private string txtPath = "";
        private string excelPath = "";
        IOExcel excelManager = new IOExcel();
        static List<string> columnCollection = new List<string>();//储存整列所有词 (Store the all words in one column)
        static string headerName = "";//当前行表头名称 (Current sheet's name)

       
        

        private int lineNum = 0;//待标词所在行 (Next unmarked words line number)

        public MainWindow()
        {



            InitializeComponent();//初始化主界面UI (Initialise UI)
            //lock UI elements
            startBtn.IsEnabled = false;
            setColumnBtn.IsEnabled = false;
            wordsLabel.Visibility = Visibility.Collapsed;
            confirmBtn.IsEnabled = false;
            addItemToTagBtn.IsEnabled = false;

            this.Closing += F;//关闭主界面 (Close this app)

        }

        private void F(object o, System.ComponentModel.CancelEventArgs e)//询问是否确认关闭主界面 (Asking whether the user is going to close this app)
        {
            if (MessageBox.Show("是否确认关闭", "", MessageBoxButton.YesNo) == MessageBoxResult.No)
                e.Cancel = true;
        }

        private void sheetList_SelectionChanged(object sender, SelectionChangedEventArgs e)//如果sheet下拉菜单所选项发生改变，则生成新的表格界面 (If the content of sheet dropbox has changed, the following table would also changed)
        {

            try
            {
                string sheetTemp = sheetComboBox.SelectedValue.ToString();
                if (bb.getList().Contains(sheetTemp))
                {
                    DataTable DT = excelManager.genDT(sheetComboBox.SelectedValue.ToString(), excelPath);
                    showTable.DataContext = DT.DefaultView;
                    //lock UI
                    addItemToTagBtn.IsEnabled = true;
                    startBtn.IsEnabled = true;
                    setColumnBtn.IsEnabled = false;
                    confirmBtn.IsEnabled = false;
                }
                wordsLabel.Visibility = Visibility.Collapsed;
            }
            catch (Exception ee)
            {
                Console.WriteLine("{0} Exception caught.", ee);

            }

        }



        private void addItemToTagBtn_Click(object sender, RoutedEventArgs e)//在语义角色下拉菜单中添加新的标签 (Add new tag in semantic tag dropbox)
        {
            if (tagComboBox.Text != "" && tagComboBox.Text != null)
            {
                string newItem = tagComboBox.Text.Trim();
                bool flag = false;
                for (int i = 0; i < tagComboBox.Items.Count; i++)
                {
                    if (string.Compare(newItem, tagComboBox.Items[i].ToString()) == 0)
                    {
                        flag = true;

                        MessageBox.Show("已经有相同项，不能再添加");//If there has already existed same tag, show alert.
                        break;
                    }
                }
                if (flag == false)
                {
                    tagComboBox.Items.Add(newItem);
                    bb.getTagList().Add(newItem);
                    tagComboBox.SelectedValue = newItem;


                }
            }
            else
            {
                MessageBox.Show("无法添加空值");
            }
        }

        private int findNextWord()//寻找下一个待处理词 (Find next unmarked words)
        {
            bool flag = false;//找到下一个词时跳出loop (Stop the loop when the words has found)
            bool flag2 = false;//sheet是否还有未标注 (Check whether all words in sheet have been finished)
            lineNum = -1;//待处理词所在行 (Line number of the next unmarked words)
            columnCollection.Clear();
           

            foreach (System.Data.DataRowView dr in showTable.ItemsSource)
            {

                lineNum += 1;
                for (int i = 0; i < dr.Row.ItemArray.Length; i++)
                {
                    if (dr[i].ToString().Trim().Length == 0 || dr[i] == null)
                        continue;
                    else
                    {
                        string[] columnTxt = dr[i].ToString().Trim().Split(' ');
                        foreach (string words in columnTxt)
                        {

                            if (words == null || words.Trim().Length == 0)
                                continue;
                            if (bb.getWordsToSheet().ContainsKey(words) && bb.getWordsToSheet()[words].Contains(sheetComboBox.SelectedValue))
                            {
                                DataRowView drv = showTable.Items[lineNum] as DataRowView;
                                DataGridRow row = (DataGridRow)this.showTable.ItemContainerGenerator.ContainerFromIndex(lineNum);
                                row.Background = new SolidColorBrush(Colors.GreenYellow);
                                continue;
                            }
                            else
                            {

                                wordsLabel.Text = words;
                                flag = true;
                                flag2 = true;

                                double percent = (double)lineNum / dr.DataView.Count;
                                string percentText = percent.ToString("0.0%");//完成进程提示 (The process of current sheet)
                                processLabel.Content = "已完成 " + percentText;
                                DataRowView drv = showTable.Items[lineNum] as DataRowView;
                                DataGridRow row = (DataGridRow)this.showTable.ItemContainerGenerator.ContainerFromIndex(lineNum);
                                row.Background = new SolidColorBrush(Colors.LightBlue);

                                showTable.ScrollIntoView(lineNum);


                                break;
                            }
                        }
                        if (flag)
                            break;
                    }
                }
                if (flag)
                    break;
               
            }

            if (!flag2)
                MessageBox.Show("该sheet页已经全部标记完成"); //Alert when current sheet has been finished.
            setColumnBtn.IsEnabled = false;
            wordsLabel.Visibility = Visibility.Visible;
            confirmBtn.IsEnabled = true;

            return lineNum;



        }




        private void confirmBtn_Click(object sender, RoutedEventArgs e)//“确定”按钮，提交标注 (Confirm btn)
        {
            try
            {
                if (wordsLabel.Text.Equals("") || tagComboBox.SelectedValue.Equals(""))
                {
                    MessageBox.Show("无法添加空值");//Alert for adding null value
                    findNextWord();
                }
                else
                {
                    string[] newTag = new string[3];//格式为：sheet表名\t待标词\t标签 
                    newTag[0] = sheetComboBox.SelectedValue.ToString().Trim();
                    newTag[1] = wordsLabel.Text.ToString().Trim();
                    newTag[2] = tagComboBox.SelectedValue.ToString().Trim();
                    if (bb.getWordsToSheet().ContainsKey(newTag[1]) && bb.getWordsToSheet()[newTag[1]].Contains(newTag[0]))//如果待标词已经标注过 (If the words has been marked)
                    {
                        if (MessageBox.Show("在 " + newTag[0] + " 中重新添加： " + newTag[1] + " 为 " + newTag[2], "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            foreach (string[] item in bb.getTagDB())
                            {
                                if (item[0].Equals(newTag[0]) && item[1].Equals(newTag[1]))
                                    item[2] = newTag[2];
                            }
                        }
                            
                    }
                    else
                    {
                        bb.getTagDB().Add(newTag);
                        if (bb.getWordsToSheet().ContainsKey(newTag[1]))
                        {
                            List<string> inSheets = bb.getWordsToSheet()[newTag[1]];
                            inSheets.Add(newTag[0]);
                            bb.getWordsToSheet()[newTag[1]] = inSheets;
                        }
                        else
                        {
                            List<string> inSheets = new List<string>();
                            inSheets.Add(newTag[0]);
                            bb.getWordsToSheet().Add(newTag[1], inSheets);
                        }
                    }
                    this.txtManager.txtWriter(txtPath, bb);
                    int line = findNextWord();
                    
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine("{0} Exception caught.", ee);
            }
        }

        
        private void openExcelBtn_Click(object sender, RoutedEventArgs e)//打开Excel文件按钮 (Read Excel file btn)
        {
            Microsoft.Win32.OpenFileDialog dialog =
               new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "xlsx文件 |*.xlsx|xls文件|*.xls";
            if (dialog.ShowDialog() == true)
            {
                this.excelPath = dialog.FileName;
            }
        }

        private void openTxtBtn_Click(object sender, RoutedEventArgs e)//打开txt文件按钮 (Read TXT file btn)
        {
            Microsoft.Win32.OpenFileDialog dialog =
               new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "txt文件 |*.txt";
            if (dialog.ShowDialog() == true)
            {
                this.txtPath = dialog.FileName;
            }
        }

        private void readBtn_Click(object sender, RoutedEventArgs e)//读取按钮 (Loading btn)
        {
            if (excelPath == null || excelPath.Trim().Length == 0)
                MessageBox.Show("请导入excel文件");
            else if (txtPath == null || txtPath.Trim().Length == 0)
                MessageBox.Show("请导入txt文件");
            else
            {


                tagComboBox.Items.Clear();
                sheetComboBox.Items.Clear();
                excelManager.excelRead(excelPath, bb);
                lineNum = 0;
                txtManager.txtReader(txtPath, bb);
                List<string> sheetList = bb.getList();
                foreach (string sh in sheetList)
                {
                    sheetComboBox.Items.Add(sh);
                }

                sheetComboBox.SelectedIndex = 0;
                tagComboBox.Items.Clear();
                foreach (string item in bb.getTagList())
                {
                    tagComboBox.Items.Add(item);
                }
                tagComboBox.SelectedIndex = 0;
               
                

            }

        }

       

        private void startBtn_Click(object sender, RoutedEventArgs e)//查找下一个词按钮 (Find next unmarked words btn)
        {
            wordsLabel.Visibility = Visibility.Visible;
            int lineNum = findNextWord();//出发查找下一个词方法
            showTable.UpdateLayout();
            showTable.ScrollIntoView(showTable.Items[lineNum]);
            columnCollection.Clear();
            //unlock and lock UI elements
            setColumnBtn.IsEnabled = false;
            
            confirmBtn.IsEnabled = true;
        }



        public string searchWords(string words,string sheetName)//查找一个词在当前sheet中是否被标注过，返回被标注值 (Check whether the current words has been marked or not, then return the answer)
        {
            string type = "";
            if (bb.getWordsToSheet().ContainsKey(words) && bb.getWordsToSheet()[words].Contains(sheetName))
            {

                foreach (string[] item in bb.getTagDB())
                {
                    if (item[0].Equals(sheetName) && item[1].Equals(words))
                        type = item[2]; 
                }
                
            }
            return type;
        }


        private void showTable_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)//点击表格界面中的一个单元格 (Click one table cell)
        {

            try
            {
                DataGrid dg = sender as DataGrid;
                var cell = dg.CurrentCell;
                DataRowView item = cell.Item as DataRowView;
                if (item != null)
                {
                    this.wordsLabel.Text = item[cell.Column.DisplayIndex].ToString().Trim();
                    string type = searchWords(this.wordsLabel.Text, sheetComboBox.SelectedValue.ToString());
                    this.tagComboBox.Text = type;

                }
                columnCollection.Clear();
                DataTable DT = excelManager.genDT(sheetComboBox.SelectedValue.ToString(), excelPath);
                for (int i = 0; i < showTable.Items.Count; i++)//将整列所有词保存到columnCollection中，若点击标记整行则调用这个collection (Save all words in one column, mark all these words at once)
                {
                    string temp = "";

 
                    int num = cell.Column.DisplayIndex;
                    temp = DT.Rows[i][num].ToString();


                    headerName = showTable.Columns[cell.Column.DisplayIndex].Header.ToString();
                    if (temp != null && temp.Trim().Length != 0)
                        columnCollection.Add(temp);
    
                }
                confirmBtn.IsEnabled = true;
                setColumnBtn.IsEnabled = true;
                wordsLabel.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} Exception caught.", ex);
            }

        }

        private void setColumnBtn_Click(object sender, RoutedEventArgs e)//标记整行按钮 (Mark whole column btn)
        {
            if (columnCollection.Count == 0)
            {
                MessageBox.Show("当前列为空");
                setColumnBtn.IsEnabled = false;
            }
            else
            {
                string sheetName = sheetComboBox.SelectedValue.ToString();
                ColumnSet cs = new ColumnSet(headerName, columnCollection, sheetName, bb, this.txtPath);
                cs.ShowDialog();
                setColumnBtn.IsEnabled = false;
            }
        
        }





        








    }
}

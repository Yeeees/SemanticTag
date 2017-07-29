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

namespace SemanticTag
{
    /// <summary>
    /// Interaction logic for ColumnSet.xaml
    /// </summary>
    public partial class ColumnSet : Window
    {
        public List<string> columnList = new List<string>();
        public string sheetTitle = "";
        public BasicBase tempbb;
        public bool flag = false;
        private string txtPath="";
        
        
        public ColumnSet(string headerName,List<string> columnCollection,string sheetName,BasicBase bb,string path)
        {
            InitializeComponent();
            tempbb = bb;
            sheetTitle = sheetName;
            columnList = columnCollection;
            txtPath = path;
            txtLabel.Content = "将 "+headerName+" 中所有词添加为： ";

            foreach (string tag in tempbb.getTagList())
            {
                tagCombo.Items.Add(tag);
            }
            
            
        }

        private void yesBtn_Click(object sender, RoutedEventArgs e)
        {
            if (tagCombo.SelectedValue != null && tagCombo.SelectedValue.ToString().Trim().Length != 0)
            {
                IOTxt txtManager = new IOTxt();
                foreach (string row in columnList)
                {
                    if (row.Trim().Length == 0)
                        continue;
                    string[] wordsList = row.Trim().Split(' ');
                    foreach (string wordElement in wordsList)
                    {
                        string[] newTag = new string[3];
                        newTag[0] = sheetTitle;
                        newTag[1] = wordElement.ToString().Trim();
                        newTag[2] = tagCombo.SelectedValue.ToString().Trim();
                        if (tempbb.getWordsToSheet().ContainsKey(newTag[1]) && tempbb.getWordsToSheet()[newTag[1]].Contains(newTag[0]))
                        {
                           
                            foreach (string[] item in tempbb.getTagDB())
                            {
                                if (item[0].Equals(newTag[0]) && item[1].Equals(newTag[1]))
                                    item[2] = newTag[2];
                            }
                            


                        }
                        else
                        {
                            
                            tempbb.getTagDB().Add(newTag);
                            if (tempbb.getWordsToSheet().ContainsKey(newTag[1]))
                            {
                                List<string> inSheets = tempbb.getWordsToSheet()[newTag[1]];
                                inSheets.Add(newTag[0]);
                                tempbb.getWordsToSheet()[newTag[1]] = inSheets;
                            }
                            else
                            {
                                List<string> inSheets = new List<string>();
                                inSheets.Add(newTag[0]);
                                tempbb.getWordsToSheet().Add(newTag[1], inSheets);
                            }

                        }
                    }
                }
                flag = true;
                txtManager.txtWriter(txtPath, tempbb);
                
                this.Close();
            }
            else
                MessageBox.Show("未选择语义角色");
            

        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void addTagBtn_Click(object sender, RoutedEventArgs e)//添加新的角色词
        {
            if (tagCombo.Text != "")
            {
                string newItem = tagCombo.Text.Trim();
                bool flag = false;
                for (int i = 0; i < tagCombo.Items.Count; i++)
                {
                    if (string.Compare(newItem, tagCombo.Items[i].ToString()) == 0)
                    {
                        flag = true;

                        MessageBox.Show("已经有相同项，不能再添加");
                    }
                }
                if (flag == false)
                {
                    tagCombo.Items.Add(newItem);
                    tempbb.getTagList().Add(newItem);
                    tagCombo.SelectedValue = newItem;


                }
            }
        }


    }
}

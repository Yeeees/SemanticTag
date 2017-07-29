using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace SemanticTag
{
    public class BasicBase
    {
        private List<string> sheetTitle = new List<string>();//表名列表
        private Dictionary<string, DataTable> sheetsDB = new Dictionary<string, DataTable>();//根据sheet名查找对应DataTable
        private Dictionary<string, List<string>> wordsToSheet = new Dictionary<string, List<string>>();//词对应所在sheet列表
        private List<string[]> tagDB = new List<string[]>();//标记库，string[]为 sheet名，词，标记角色
        public List<string> tagList = new List<string>();//标签列表，根据读入txt生成
        public BasicBase()
        { 
        
        }

        public List<string> getList() 
        {
            return this.sheetTitle;
        }

        public Dictionary<string, DataTable> getSheetsDB()
        {
            return this.sheetsDB;
        }

        public Dictionary<string, List<string>> getWordsToSheet()
        {
            return this.wordsToSheet;
        }

        public List<string[]> getTagDB()
        {
            return tagDB;
        }

        public List<string> getTagList()
        {
            return tagList;
        }

    }
}

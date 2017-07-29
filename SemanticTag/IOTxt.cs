using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace SemanticTag
{
    //Input / Output functions for TXT
    class IOTxt
    {
        public IOTxt()
        {
            
        }
        
        public void txtReader(string txtPath,BasicBase bb)
        {
            using (StreamReader sr = File.OpenText(txtPath))
            {
                string s = String.Empty;
                if (bb.getWordsToSheet().Count != 0)
                    bb.getWordsToSheet().Clear();
                if (bb.getTagDB().Count != 0)
                    bb.getTagDB().Clear();
                bb.getTagList().Clear();
                while ((s = sr.ReadLine()) != null)
                {
                    string[] lineArr = s.Split('\t');
                    if (bb.getWordsToSheet().ContainsKey(lineArr[1]))
                    {
                        List<string> inSheets = bb.getWordsToSheet()[lineArr[1]];
                        inSheets.Add(lineArr[0]);
                        bb.getWordsToSheet()[lineArr[1]] = inSheets;
                    }
                    else
                    {
                        List<string> inSheets = new List<string>();
                        inSheets.Add(lineArr[0]);
                        bb.getWordsToSheet().Add(lineArr[1], inSheets);
                    }
                       
                    bb.getTagDB().Add(lineArr);

                    if (!bb.getTagList().Contains(lineArr[2]))
                        bb.getTagList().Add(lineArr[2]);
                }
                sr.Close();
            }
            
        }

        public void txtWriter(string pathout ,BasicBase bb)
        {
            

            StreamWriter sw = new StreamWriter(pathout, false);
            foreach (string[] listContent in bb.getTagDB())
            {
                
                for (int i = 0; i < listContent.Length; i++)
                {
                    if(i==listContent.Length-1)
                        sw.Write(listContent[i].ToString().Trim());
                    else
                        sw.Write(listContent[i].ToString().Trim()+"\t");

                }

                sw.WriteLine();
            }
            sw.Close();
            sw.Dispose();
        }
    }
}

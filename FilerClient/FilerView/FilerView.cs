using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using FilerView;

namespace FilerClient
{
    public partial class FilerView : Form
    {
        public event Action<string, string, string, string, string, string, string, string, string, string, string> SaveEvent;
        public event Action<string, string, string, string, string, string> SearchEvent;
        public event Action<string, string, string, string, string> DeleteEvent;
        public event Action<string, string, string, string> GetFullFileEvent;
        public SearchData[] foundInSearch;
        public FilerView()
        {
            InitializeComponent();
            ResultListBox.MouseDoubleClick += new MouseEventHandler(ResultListBox_MouseDoubleClick);
            foundInSearch = null;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            string fileName = null;
            string text = null;

            using (OpenFileDialog openFileDialog1 = new OpenFileDialog())
            {
                openFileDialog1.InitialDirectory = "c:\\";
                openFileDialog1.RestoreDirectory = true;

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    fileName = openFileDialog1.FileName;
                }
            }

            if (fileName != null)
            {
                text = System.IO.File.ReadAllText(fileName);
            }


            string File = text;
            string Date = DateTextBox.Text;
            string FileName = NameTextBox.Text;
            string Class = ClassComboBox.Text;
            string Unit = UnitComboBox.Text;
            string Section = SectionComboBox.Text;
            string Type = TypeComboBox.Text;
            string isLink = "false";
            string Override = "false";
            SaveEvent?.Invoke(File, null, Date, FileName, null, Class, Unit, Section, Type, isLink, Override);
        }

        private void SaveLinkButton_Click(object sender, EventArgs e)
        {
            AddLinkPopUp linkPopUp = new AddLinkPopUp();
            var result = linkPopUp.ShowDialog();
            if(result == DialogResult.OK)
            {
                
                string Link = linkPopUp.ReturnValue;
                string Date = DateTextBox.Text;
                string LinkName = NameTextBox.Text;
                string Class = ClassComboBox.Text;
                string Unit = UnitComboBox.Text;
                string Section = SectionComboBox.Text;
                string Type = TypeComboBox.Text;
                string isLink = "true";
                string Override = "false";
                SaveEvent?.Invoke("placeholder", Link, Date, "placeholder", LinkName, Class, Unit, Section, Type, isLink, Override);
                return;
            }
            if(result == DialogResult.Cancel)
            {
                return;
            }
            throw new Exception("The User Didn't choose ok or cancel in the AddLinkPopUp box. Not sure how.");
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            //example:  ResultListBox.Items.Add("hello");
            string Class = ClassComboBox.Text;
            string Unit = UnitComboBox.Text;
            string Section = SectionComboBox.Text;
            string Name = NameTextBox.Text;
            string Date = DateTextBox.Text;
            string Type = TypeComboBox.Text;
            SearchEvent?.Invoke(Class, Unit, Section, Name, Date, Type);
        }

        public void GenerateInfoBox(string v)
        {
            InfoBox box = new InfoBox(v);
            box.ShowDialog();
        }

        private void ResultListBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.ResultListBox.IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                string contents = foundInSearch[index].FileName;
                if(contents == null)
                {
                    contents = foundInSearch[index].Link;
                }
                SearchResultOptionPopUp popUp = new SearchResultOptionPopUp(contents); //We pass it the name of the file that was clicked.
                popUp.ShowDialog();
                if(popUp.DialogResult == DialogResult.OK) //Runs if they want to open the file.
                {
                    if (foundInSearch[index].isLink.Equals("false"))
                    {
                        GetFullFileEvent?.Invoke(foundInSearch[index].Class, foundInSearch[index].Unit, foundInSearch[index].Section, foundInSearch[index].FileName);
                    }
                }
                else if(popUp.DialogResult == DialogResult.No) //Runs if they want to Delete the file/link.
                {

                }
                else if(popUp.DialogResult == DialogResult.Cancel)
                {
                    //Do nothing.
                    
                }
                else
                {
                    throw new Exception("Someone found a way to avoid opening, deleting, or canceling. How did they do it?");
                }
            }
        }
        public bool LaunchOverridePopUp()
        {
            Overridebox box = new Overridebox();
            var result = box.ShowDialog();
            if(result == DialogResult.OK)
            {
                return true;
            }
            return false;
                
        }

        public class SearchData
        {
            public string FileName;
            public string LinkName;
            public string Date;
            public string Class;
            public string Unit;
            public string Section;
            public string Type;
            public string Link;
            public string isLink;
            public SearchData()
            {

            }
        }
    }
}

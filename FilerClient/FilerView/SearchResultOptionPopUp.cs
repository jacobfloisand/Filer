using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FilerView
{
    public partial class SearchResultOptionPopUp : Form
    {
        public SearchResultOptionPopUp(string v)
        {
            InitializeComponent();
            NameLabel.Text = v;
        }

        private void OpenButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No; //lol this means delete from now on.
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}

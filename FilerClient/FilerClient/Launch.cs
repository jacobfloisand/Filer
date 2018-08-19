using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FilerClient
{
    class Launch
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            FilerView GUI = new FilerView();
            FilerController controller = new FilerController(GUI);
            Application.Run(GUI);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DbSelect.Models.ComponentModels
{
    public class CmCreateFile
    {
        public TextBox FileStringName {  get; set; }
        public TextBox FileStringValues { get; set; }
        public Label FileStringFileName {  get; set; }
        public Label FileStringAction {  get; set; }

        public TextBox FileImgName {  get; set; }
        public PictureBox FileImgValue { get; set; }
        public Label FileImgFileName { get; set; }

        public TextBox FileGroupName { get; set; }
        public PictureBox[] FileGroupValues { get; set; }
        public Button FileGroupButtonNext { get; set; }
        public Button FileGroupButtonPrev { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DbSelect.SystemModels.ComponentModels
{
    public class CmStart
    {
        public Panel GlPanel {  get; set; }
        public TabControl Tab { get; set; }
        public ListBox List { get; set; }
        public TextBox Text { get; set; }


        public CheckBox LoadConfCheck {  get; set; }
        public Panel LoadConfPanel {  get; set; }
        public Label LoadConfLabel {  get; set; }
        public CheckBox SaveConfCheck {  get; set; }
        public Panel SaveConfPanel {  get; set; }
        public Label SaveConfLabel { get; set; }
        public CheckBox CreateTableCheck {  get; set; }
        public CheckBox FillingCheck { get; set; }
        public Label ConnectionLabel { get; set; }
        public TextBox ConnectionText {  get; set; }
        public Button ConnectionBut {  get; set; }

        public CheckBox QueryText { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DbSelect.Models.ComponentModels
{
    public class CmSystem
    {
        public TreeView Menu { get; set; }
        public TabControl Tab {  get; set; }
        public ContextMenuStrip MenuTables {  get; set; }
        public ContextMenuStrip MenuTable { get; set; }
        public ContextMenuStrip MenuFiles { get; set; }
        public ContextMenuStrip MenuFile {  get; set; }
        public ToolStripMenuItem ErrorList {  get; set; }
        public ToolStripMenuItem SaveConfig {  get; set; }
        public ToolStripMenuItem AdminPanel { get; set; }
        public DataGridView ErrorListData { get; set; }
        public TextBox ErrorListText {  get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DbSelect.Models.ComponentModels
{
    public class CmFilling
    {
        public ComboBox Tables {  get; set; }
        public ListBox Columns {  get; set; }
        public TabControl Tab {  get; set; }
        public Label Type {  get; set; }
        public Button bUp { get; set; }
        public Button bDown { get; set; }

        public TextBox ConditionText { get; set; }
        public ListBox ConditionList {  get; set; }
        public TextBox CountText { get; set; }
        public CmFillingComponent.IntConst IntConst { get; set; }
        public CmFillingComponent.IntRand IntRand { get; set; }
        public CmFillingComponent.IntIdConst IntIdConst { get; set; }
        public CmFillingComponent.IntIdRand IntIdRand { get; set; }
        public CmFillingComponent.StringConst StringConst { get; set; }
        public CmFillingComponent.StringFile StringFile { get; set; }
        public CmFillingComponent.FloatConst FloatConst { get; set; }
        public CmFillingComponent.FloatRand FloatRand { get; set; }
        public CmFillingComponent.DateConst DateConst { get; set; }
        public CmFillingComponent.DateRand DateRand { get; set; }
        public CmFillingComponent.ImgConst ImgConst { get; set; }
        public CmFillingComponent.ImgFile ImgFile { get; set; }
        public CmFillingComponent.BoolConst BoolConst { get; set; }
        
    }
}

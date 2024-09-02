using DbSelect.Models.ComponentModels;
using DbSelect.SystemModels.ComponentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DbSelect.Models.StaticModels
{
    public static class Cm
    {
        public static CmSystem System { get; set; }
        public static CmCreateTable CreateTable { get; set; }
        public static CmFilling Filling {  get; set; }
        public static CmCreateFile CreateFile { get; set; }
        public static CmStart StartPage {  get; set; }
    }
}

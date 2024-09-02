using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DbSelect.Models.ComponentModels
{
    public static class CmFillingComponent
    {
        public class IntConst
        {
            public NumericUpDown Value {  get; set; }
        }

        public class IntRand
        {
            public NumericUpDown ValueFrom {  get; set; }
            public NumericUpDown ValueTo { get; set;}
            public RadioButton R1 { get; set; }
			public RadioButton R2 { get; set; }
			public RadioButton R3 { get; set; }
		}

        public class IntIdConst
        {
            public ComboBox Table {  get; set; }
            public ComboBox Column { get; set; }
        }

        public class IntIdRand
        {
            public ComboBox Table { get; set; }
            public ComboBox Column { get; set; }
        }

        public class StringConst
        {
            public TextBox Value { get; set;}
        }

        public class StringFile
        {
            public ComboBox File { get; set; }
			public RadioButton R1 { get; set; }
			public RadioButton R2 { get; set; }
			public RadioButton R3 { get; set; }
		}

        public class FloatConst
        {
            public NumericUpDown Value { get; set;}
        }

        public class FloatRand
        {
            public NumericUpDown ValueFrom { get; set;}
            public NumericUpDown ValueTo { get; set;}
			public RadioButton R1 { get; set; }
			public RadioButton R2 { get; set; }
			public RadioButton R3 { get; set; }
		}

        public class DateConst
        {
            public DateTimePicker Value { get; set; }
        }

        public class DateRand
        {
            public DateTimePicker ValueFrom {  get; set; }
            public DateTimePicker ValueTo { get; set;}
			public RadioButton R1 { get; set; }
			public RadioButton R2 { get; set; }
			public RadioButton R3 { get; set; }
		}

        public class ImgConst
        {
            public ComboBox File { get; set;}
        }

        public class ImgFile
        {
           public ComboBox File { get; set;}
        }

        public class BoolConst
        {
            public CheckBox Value {  get; set; }
        }
    }
}

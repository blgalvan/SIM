using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace SIM
{
    public partial class FormDatos1 : Form
    {
        public string title;
        public Hashtable fields;
        public Hashtable extParameters;
        public Hashtable parameters = new Hashtable { };        
        private Hashtable _widgets = new Hashtable { };
        private List<TextBox> _textboxList;
        private List<Label> _labelList;

        
        public FormDatos1()
        {
            InitializeComponent();
            //Aseguramos que utiliza la configuración española para numeros decimales
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-ES");

            _textboxList = new List<TextBox> { 
                textBox1,
                textBox2,
                textBox3,
                textBox4,
                textBox5,
                textBox6
            };

            _labelList = new List<Label> {
                label1,
                label2,
                label3,
                label4,
                label5,
                labelTitle
            };
        }

        private void FormDatos1_Load(object sender, EventArgs e)
        {
            if (title != "")
            {
                labelTitle.Visible = true;
                labelTitle.Text = title;
            }

            foreach (string field in fields.Keys)
            {
                TextBox textbox = _textboxList[_widgets.Keys.Count];
                Label label = _labelList[_widgets.Keys.Count];
                _widgets.Add(field, textbox);
                textbox.Visible = true;
                textbox.Text = extParameters.Contains(field) ? (string)extParameters[field] : "";
                label.Visible = true;
                label.Text = (string)fields[field];
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (DictionaryEntry widget in _widgets)
            {
                TextBox textbox = (TextBox)widget.Value;
                string value = textbox.Text == "" ? "0" : textbox.Text;
                parameters.Add((string)widget.Key, Convert.ToDouble(value));
            }
            
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

    }
}

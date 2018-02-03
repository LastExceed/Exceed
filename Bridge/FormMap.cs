using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bridge {
    public partial class FormMap : Form {
        FormMain main;
        public FormMap(FormMain main) {
            InitializeComponent();
            this.main = main;
        }

        private void FormMap_FormClosed(object sender, FormClosedEventArgs e) {
            main.map = new FormMap(main);
        }
    }
}

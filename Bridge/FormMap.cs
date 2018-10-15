using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace Bridge {
    public partial class FormMap : Form {
        public FormMap() {
            InitializeComponent();
        }
        List<Label> playercollection = new List<Label>();
        private void Refreshtimer_Tick(object sender, EventArgs e) {
            Controls.Clear();
            playercollection.Clear();
            foreach (var entity in BridgeCore.dynamicEntities.Values.ToList()) {
                if (entity.hostility == Resources.Hostility.Player) {
                    Label playerlabel = new Label {
                        Left = (int)entity.position.x / 0x10000,
                        Top = 512 - (int)entity.position.y / 0x10000,
                        Text = entity.name,
                        AutoSize = true,
                    };
                    Controls.Add(playerlabel);
                    playercollection.Add(playerlabel);
                }
            }
        }
    }
}

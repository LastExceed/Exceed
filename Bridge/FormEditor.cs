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
    public partial class FormEditor : Form {
        bool isReady = true;
        byte buffer, memorize;
        FormMain main;

        public FormEditor(FormMain main) {
            InitializeComponent();
            this.main = main;
        }

        private void FormEditor_Shown(object sender, EventArgs e) {
            comboBoxClass.SelectedIndex = CwRam.memory.ReadByte(CwRam.EntityStart + 0x0140);
            radioButtonSubclass1.Checked = (CwRam.memory.ReadByte(CwRam.EntityStart + 0x0141) == 0);
            radioButtonSubclass2.Checked = (CwRam.memory.ReadByte(CwRam.EntityStart + 0x0141) == 1);

            numericUpDownCharacterLevel.Value = CwRam.memory.ReadInt(CwRam.EntityStart + 0x0190);
            numericUpDownXP.Value = CwRam.memory.ReadInt(CwRam.EntityStart + 0x0194);
            numericUpDownMoney.Value = CwRam.memory.ReadInt(CwRam.EntityStart + 0x1304);
            numericUpDownPlatinum.Value = CwRam.memory.ReadInt(CwRam.EntityStart + 0x1308);
        }

        //itemeditor
        private void ListBoxItem_SelectedIndexChanged(object sender, EventArgs e) {
            numericUpDownType.Value = CwRam.memory.ReadByte(CwRam.ItemStart + 0); ;
            numericUpDownSubType.Value = CwRam.memory.ReadByte(CwRam.ItemStart + 1);
            numericUpDownModifier.Value = CwRam.memory.ReadInt(CwRam.ItemStart + 4);
            numericUpDownUnknown.Value = CwRam.memory.ReadInt(CwRam.ItemStart + 8);
            numericUpDownRarity.Value = CwRam.memory.ReadByte(CwRam.ItemStart + 12);
            numericUpDownMaterial.Value = CwRam.memory.ReadByte(CwRam.ItemStart + 13);
            numericUpDownLevel.Value = CwRam.memory.ReadUShort(CwRam.ItemStart + 16);
            checkBoxAdapted.Checked = (CwRam.memory.ReadByte(CwRam.ItemStart + 14) == 1);
            numericUpDownVisible.Value = CwRam.memory.ReadUInt(CwRam.ItemStart + 276);

            tabControlItemEditor.Enabled = true;
            listBoxSlots.SelectedIndex = -1;
        }

        private void NumericUpDownType_ValueChanged(object sender, EventArgs e) {
            buffer = (byte)numericUpDownType.Value;
            CwRam.memory.WriteByte(CwRam.ItemStart + 0, buffer);

            isReady = false;
            if (buffer < 26) {
                comboBoxType.SelectedIndex = buffer;
            }
            else {
                comboBoxType.SelectedIndex = -1;
            }
            isReady = true;
        }
        private void NumericUpDownSubType_ValueChanged(object sender, EventArgs e) {
            buffer = (byte)numericUpDownSubType.Value;
            CwRam.memory.WriteByte(CwRam.ItemStart + 1, buffer);

            isReady = false;
            //if (buffer < ?)
            //{
            //    comboBoxSubType.SelectedIndex = buffer;
            //}
            //else
            //{
            //    comboBoxSubType.SelectedIndex = -1;
            //}
            isReady = true;
        }
        private void NumericUpDownModifier_ValueChanged(object sender, EventArgs e) {
            CwRam.memory.WriteInt(CwRam.ItemStart + 4, (int)numericUpDownModifier.Value);
        }
        private void NumericUpDownUnknown_ValueChanged(object sender, EventArgs e) {
            CwRam.memory.WriteInt(CwRam.ItemStart + 8, (int)numericUpDownUnknown.Value);
        }
        private void NumericUpDownRarity_ValueChanged(object sender, EventArgs e) {
            buffer = (byte)numericUpDownRarity.Value;
            CwRam.memory.WriteByte(CwRam.ItemStart + 12, buffer);

            isReady = false;
            if (buffer < 6) {
                comboBoxRarity.SelectedIndex = buffer;
            }
            else {
                comboBoxRarity.SelectedIndex = -1;
            }
            isReady = true;
        }
        private void NumericUpDownMaterial_ValueChanged(object sender, EventArgs e) {
            buffer = (byte)numericUpDownMaterial.Value;
            CwRam.memory.WriteByte(CwRam.ItemStart + 13, buffer);

            isReady = false;
            if (buffer < 29) {
                comboBoxMaterial.SelectedIndex = buffer;
            }
            else if (buffer > 127 && buffer < 132) {
                comboBoxMaterial.SelectedIndex = buffer - 99;
            }
            else {
                comboBoxMaterial.SelectedIndex = -1;
            }
            isReady = true;
        }
        private void NumericUpDownLevel_ValueChanged(object sender, EventArgs e) {
            CwRam.memory.WriteShort(CwRam.ItemStart + 16, (short)numericUpDownLevel.Value);
        }
        //checkbox
        private void CheckBoxAdapted_CheckedChanged(object sender, EventArgs e) {
            CwRam.memory.WriteByte(CwRam.ItemStart + 14, Convert.ToByte(checkBoxAdapted.Checked));
        }
        //combobox
        private void ComboBoxType_SelectedIndexChanged(object sender, EventArgs e) {
            if (isReady) {
                buffer = (byte)comboBoxType.SelectedIndex;
                CwRam.memory.WriteByte(CwRam.ItemStart + 0, buffer);
                numericUpDownType.Value = buffer;
            }
        }
        private void ComboBoxSubType_SelectedIndexChanged(object sender, EventArgs e) {
            if (isReady) {
                buffer = (byte)comboBoxSubType.SelectedIndex;
                CwRam.memory.WriteByte(CwRam.ItemStart + 1, buffer);
                numericUpDownSubType.Value = buffer;
            }
        }
        private void ComboBoxRarity_SelectedIndexChanged(object sender, EventArgs e) {
            if (isReady) {
                buffer = (byte)comboBoxRarity.SelectedIndex;
                CwRam.memory.WriteByte(CwRam.ItemStart + 12, buffer);
                numericUpDownRarity.Value = buffer;
            }
        }
        private void ComboBoxMaterial_SelectedIndexChanged(object sender, EventArgs e) {
            if (isReady) {
                buffer = (byte)comboBoxMaterial.SelectedIndex;
                CwRam.memory.WriteByte(CwRam.ItemStart + 13, buffer);
                numericUpDownMaterial.Value = buffer;
            }
        }
        //cubes/spirits
        private void ListBoxSlots_SelectedIndexChanged(object sender, EventArgs e) {
            isReady = false;
            numericUpDownX.Value = CwRam.memory.ReadSByte(CwRam.SlotStart + 0);
            numericUpDownY.Value = CwRam.memory.ReadSByte(CwRam.SlotStart + 1);
            numericUpDownZ.Value = CwRam.memory.ReadSByte(CwRam.SlotStart + 2);

            buffer = CwRam.memory.ReadByte(CwRam.SlotStart + 3);
            numericUpDownTypeC.Value = buffer;
            numericUpDownLevelC.Value = CwRam.memory.ReadShort(CwRam.SlotStart + 4);

            bool x = (listBoxSlots.SelectedIndex != -1);
            numericUpDownX.Enabled = x;
            numericUpDownY.Enabled = x;
            numericUpDownZ.Enabled = x;
            numericUpDownLevelC.Enabled = x;
            numericUpDownTypeC.Enabled = x;
            comboBoxTypeC.Enabled = x;
            checkBoxHighlight.Enabled = x;

            isReady = true;
        }

        private void NumericUpDownX_ValueChanged(object sender, EventArgs e) {
            CwRam.memory.WriteSByte(CwRam.SlotStart + 0, (sbyte)numericUpDownX.Value);
        }
        private void NumericUpDownY_ValueChanged(object sender, EventArgs e) {
            CwRam.memory.WriteSByte(CwRam.SlotStart + 1, (sbyte)numericUpDownY.Value);
        }
        private void NumericUpDownZ_ValueChanged(object sender, EventArgs e) {
            CwRam.memory.WriteSByte(CwRam.SlotStart + 2, (sbyte)numericUpDownZ.Value);
        }
        private void NumericUpDownTypeC_ValueChanged(object sender, EventArgs e) {
            buffer = (byte)numericUpDownTypeC.Value;
            CwRam.memory.WriteByte(CwRam.SlotStart + 3, buffer);
            isReady = false;
            if (buffer < 29) {
                comboBoxTypeC.SelectedIndex = buffer;
            }
            else if (buffer > 127 && buffer < 132) {
                comboBoxTypeC.SelectedIndex = buffer - 99;
            }
            else {
                comboBoxTypeC.SelectedIndex = -1;
            }
            isReady = true;
        }
        private void ComboBoxTypeC_SelectedIndexChanged(object sender, EventArgs e) {
            if (isReady) {
                buffer = (byte)comboBoxTypeC.SelectedIndex;
                CwRam.memory.WriteByte(CwRam.SlotStart + 3, buffer);
                numericUpDownTypeC.Value = buffer;
            }
        }
        private void NumericUpDownLevelC_ValueChanged(object sender, EventArgs e) {
            CwRam.memory.WriteShort(CwRam.SlotStart + 4, (short)numericUpDownLevelC.Value);
        }
        private void NumericUpDownVisible_ValueChanged(object sender, EventArgs e) {
            CwRam.memory.WriteUShort(CwRam.ItemStart + 276, (ushort)numericUpDownVisible.Value);
        }

        private void TimerHighlight_Tick(object sender, EventArgs e) {
            if (CwRam.memory.ReadByte(CwRam.SlotStart + 3) == 5) {
                CwRam.memory.WriteByte(CwRam.SlotStart + 3, 131);
            }
            else {
                CwRam.memory.WriteByte(CwRam.SlotStart + 3, 5);
            }
        }
        private void CheckBoxHighlight_CheckedChanged(object sender, EventArgs e) {
            if (checkBoxHighlight.Checked) {
                listBoxItem.Enabled = false;
                listBoxSlots.Enabled = false;
                numericUpDownTypeC.Enabled = false;
                comboBoxTypeC.Enabled = false;
                memorize = CwRam.memory.ReadByte(CwRam.SlotStart + 3);
                TimerHighlight.Enabled = true;

            }
            else {
                TimerHighlight.Enabled = false;
                CwRam.memory.WriteByte(CwRam.SlotStart + 3, memorize);
                numericUpDownTypeC.Enabled = true;
                comboBoxTypeC.Enabled = true;
                listBoxSlots.Enabled = true;
                listBoxItem.Enabled = true;
            }
        }

        //character editor
        private void NumericUpDownCharacterLevel_ValueChanged(object sender, EventArgs e) {
            CwRam.memory.WriteInt(CwRam.EntityStart + 0x0190, (int)numericUpDownCharacterLevel.Value);
        }
        private void NumericUpDownXP_ValueChanged(object sender, EventArgs e) {
            CwRam.memory.WriteInt(CwRam.EntityStart + 0x0194, (int)numericUpDownXP.Value);
        }
        private void NumericUpDownMoney_ValueChanged(object sender, EventArgs e) {
            CwRam.memory.WriteInt(CwRam.EntityStart + 0x1304, (int)numericUpDownMoney.Value);
        }
        private void NumericUpDownPlatinum_ValueChanged(object sender, EventArgs e) {
            CwRam.memory.WriteInt(CwRam.EntityStart + 0x1308, (int)numericUpDownPlatinum.Value);
        }
        private void ComboBoxClass_SelectedIndexChanged(object sender, EventArgs e) {
            CwRam.memory.WriteByte(CwRam.EntityStart + 0x0140, (byte)comboBoxClass.SelectedIndex);
        }
        private void RadioButtonSubclass1_CheckedChanged(object sender, EventArgs e) {
            if (radioButtonSubclass1.Checked) {
                CwRam.memory.WriteByte(CwRam.EntityStart + 0x0141, 0);
            }
            else {
                CwRam.memory.WriteByte(CwRam.EntityStart + 0x0141, 1);
            }

        }

        private void FormEditor_FormClosed(object sender, FormClosedEventArgs e) {
            main.editor = new FormEditor(main);
        }
    }
}

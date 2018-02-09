namespace Bridge {
    partial class FormEditor {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.groupBoxClass = new System.Windows.Forms.GroupBox();
            this.comboBoxClass = new System.Windows.Forms.ComboBox();
            this.radioButtonSubclass1 = new System.Windows.Forms.RadioButton();
            this.radioButtonSubclass2 = new System.Windows.Forms.RadioButton();
            this.numericUpDownMoney = new System.Windows.Forms.NumericUpDown();
            this.tabControlItemEditor = new System.Windows.Forms.TabControl();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.labelLevel = new System.Windows.Forms.Label();
            this.labelMaterial = new System.Windows.Forms.Label();
            this.labelRarity = new System.Windows.Forms.Label();
            this.labelRecipe = new System.Windows.Forms.Label();
            this.labelModifier = new System.Windows.Forms.Label();
            this.labelSubType = new System.Windows.Forms.Label();
            this.labelType = new System.Windows.Forms.Label();
            this.comboBoxMaterial = new System.Windows.Forms.ComboBox();
            this.comboBoxRarity = new System.Windows.Forms.ComboBox();
            this.comboBoxSubType = new System.Windows.Forms.ComboBox();
            this.comboBoxType = new System.Windows.Forms.ComboBox();
            this.checkBoxAdapted = new System.Windows.Forms.CheckBox();
            this.numericUpDownLevel = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownMaterial = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownRarity = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownRecipe = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownModifier = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownSubType = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownType = new System.Windows.Forms.NumericUpDown();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.buttonNone = new System.Windows.Forms.Button();
            this.buttonAll = new System.Windows.Forms.Button();
            this.checkBoxHighlight = new System.Windows.Forms.CheckBox();
            this.numericUpDownVisible = new System.Windows.Forms.NumericUpDown();
            this.labelVisible = new System.Windows.Forms.Label();
            this.comboBoxTypeC = new System.Windows.Forms.ComboBox();
            this.labelTypeC = new System.Windows.Forms.Label();
            this.numericUpDownTypeC = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownLevelC = new System.Windows.Forms.NumericUpDown();
            this.labelLevelC = new System.Windows.Forms.Label();
            this.labelPosZ = new System.Windows.Forms.Label();
            this.labelPosY = new System.Windows.Forms.Label();
            this.labelPosX = new System.Windows.Forms.Label();
            this.numericUpDownZ = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownY = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownX = new System.Windows.Forms.NumericUpDown();
            this.listBoxSlots = new System.Windows.Forms.ListBox();
            this.numericUpDownPlatinum = new System.Windows.Forms.NumericUpDown();
            this.listBoxItem = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDownXP = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownCharacterLevel = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.TimerHighlight = new System.Windows.Forms.Timer(this.components);
            this.timerRefresh = new System.Windows.Forms.Timer(this.components);
            this.groupBoxClass.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMoney)).BeginInit();
            this.tabControlItemEditor.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaterial)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRarity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRecipe)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownModifier)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSubType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownType)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownVisible)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTypeC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLevelC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPlatinum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownXP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCharacterLevel)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxClass
            // 
            this.groupBoxClass.Controls.Add(this.comboBoxClass);
            this.groupBoxClass.Controls.Add(this.radioButtonSubclass1);
            this.groupBoxClass.Controls.Add(this.radioButtonSubclass2);
            this.groupBoxClass.Location = new System.Drawing.Point(248, 0);
            this.groupBoxClass.Name = "groupBoxClass";
            this.groupBoxClass.Size = new System.Drawing.Size(80, 73);
            this.groupBoxClass.TabIndex = 25;
            this.groupBoxClass.TabStop = false;
            this.groupBoxClass.Text = "class";
            // 
            // comboBoxClass
            // 
            this.comboBoxClass.FormattingEnabled = true;
            this.comboBoxClass.Items.AddRange(new object[] {
            "None",
            "Warrior",
            "Ranger",
            "Mage",
            "Rogue"});
            this.comboBoxClass.Location = new System.Drawing.Point(3, 14);
            this.comboBoxClass.Name = "comboBoxClass";
            this.comboBoxClass.Size = new System.Drawing.Size(73, 21);
            this.comboBoxClass.TabIndex = 2;
            this.comboBoxClass.SelectedIndexChanged += new System.EventHandler(this.ComboBoxClass_SelectedIndexChanged);
            // 
            // radioButtonSubclass1
            // 
            this.radioButtonSubclass1.AutoSize = true;
            this.radioButtonSubclass1.Location = new System.Drawing.Point(4, 34);
            this.radioButtonSubclass1.Name = "radioButtonSubclass1";
            this.radioButtonSubclass1.Size = new System.Drawing.Size(72, 17);
            this.radioButtonSubclass1.TabIndex = 3;
            this.radioButtonSubclass1.TabStop = true;
            this.radioButtonSubclass1.Text = "subclass1";
            this.radioButtonSubclass1.UseVisualStyleBackColor = true;
            this.radioButtonSubclass1.CheckedChanged += new System.EventHandler(this.RadioButtonSubclass1_CheckedChanged);
            // 
            // radioButtonSubclass2
            // 
            this.radioButtonSubclass2.AutoSize = true;
            this.radioButtonSubclass2.Location = new System.Drawing.Point(4, 49);
            this.radioButtonSubclass2.Name = "radioButtonSubclass2";
            this.radioButtonSubclass2.Size = new System.Drawing.Size(72, 17);
            this.radioButtonSubclass2.TabIndex = 4;
            this.radioButtonSubclass2.TabStop = true;
            this.radioButtonSubclass2.Text = "subclass2";
            this.radioButtonSubclass2.UseVisualStyleBackColor = true;
            // 
            // numericUpDownMoney
            // 
            this.numericUpDownMoney.Location = new System.Drawing.Point(273, 145);
            this.numericUpDownMoney.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numericUpDownMoney.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.numericUpDownMoney.Name = "numericUpDownMoney";
            this.numericUpDownMoney.Size = new System.Drawing.Size(57, 20);
            this.numericUpDownMoney.TabIndex = 5;
            this.numericUpDownMoney.ValueChanged += new System.EventHandler(this.NumericUpDownMoney_ValueChanged);
            // 
            // tabControlItemEditor
            // 
            this.tabControlItemEditor.Controls.Add(this.tabPage4);
            this.tabControlItemEditor.Controls.Add(this.tabPage3);
            this.tabControlItemEditor.Enabled = false;
            this.tabControlItemEditor.Location = new System.Drawing.Point(53, 0);
            this.tabControlItemEditor.Name = "tabControlItemEditor";
            this.tabControlItemEditor.SelectedIndex = 0;
            this.tabControlItemEditor.Size = new System.Drawing.Size(175, 187);
            this.tabControlItemEditor.TabIndex = 14;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.labelLevel);
            this.tabPage4.Controls.Add(this.labelMaterial);
            this.tabPage4.Controls.Add(this.labelRarity);
            this.tabPage4.Controls.Add(this.labelRecipe);
            this.tabPage4.Controls.Add(this.labelModifier);
            this.tabPage4.Controls.Add(this.labelSubType);
            this.tabPage4.Controls.Add(this.labelType);
            this.tabPage4.Controls.Add(this.comboBoxMaterial);
            this.tabPage4.Controls.Add(this.comboBoxRarity);
            this.tabPage4.Controls.Add(this.comboBoxSubType);
            this.tabPage4.Controls.Add(this.comboBoxType);
            this.tabPage4.Controls.Add(this.checkBoxAdapted);
            this.tabPage4.Controls.Add(this.numericUpDownLevel);
            this.tabPage4.Controls.Add(this.numericUpDownMaterial);
            this.tabPage4.Controls.Add(this.numericUpDownRarity);
            this.tabPage4.Controls.Add(this.numericUpDownRecipe);
            this.tabPage4.Controls.Add(this.numericUpDownModifier);
            this.tabPage4.Controls.Add(this.numericUpDownSubType);
            this.tabPage4.Controls.Add(this.numericUpDownType);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(167, 161);
            this.tabPage4.TabIndex = 0;
            this.tabPage4.Text = "Item properties";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // labelLevel
            // 
            this.labelLevel.AutoSize = true;
            this.labelLevel.Location = new System.Drawing.Point(-3, 130);
            this.labelLevel.Name = "labelLevel";
            this.labelLevel.Size = new System.Drawing.Size(33, 13);
            this.labelLevel.TabIndex = 18;
            this.labelLevel.Text = "Level";
            // 
            // labelMaterial
            // 
            this.labelMaterial.AutoSize = true;
            this.labelMaterial.Location = new System.Drawing.Point(-3, 109);
            this.labelMaterial.Name = "labelMaterial";
            this.labelMaterial.Size = new System.Drawing.Size(44, 13);
            this.labelMaterial.TabIndex = 17;
            this.labelMaterial.Text = "Material";
            // 
            // labelRarity
            // 
            this.labelRarity.AutoSize = true;
            this.labelRarity.Location = new System.Drawing.Point(-3, 88);
            this.labelRarity.Name = "labelRarity";
            this.labelRarity.Size = new System.Drawing.Size(34, 13);
            this.labelRarity.TabIndex = 16;
            this.labelRarity.Text = "Rarity";
            // 
            // labelRecipe
            // 
            this.labelRecipe.AutoSize = true;
            this.labelRecipe.Location = new System.Drawing.Point(-3, 67);
            this.labelRecipe.Name = "labelRecipe";
            this.labelRecipe.Size = new System.Drawing.Size(41, 13);
            this.labelRecipe.TabIndex = 15;
            this.labelRecipe.Text = "Recipe";
            // 
            // labelModifier
            // 
            this.labelModifier.AutoSize = true;
            this.labelModifier.Location = new System.Drawing.Point(-3, 46);
            this.labelModifier.Name = "labelModifier";
            this.labelModifier.Size = new System.Drawing.Size(44, 13);
            this.labelModifier.TabIndex = 14;
            this.labelModifier.Text = "Modifier";
            // 
            // labelSubType
            // 
            this.labelSubType.AutoSize = true;
            this.labelSubType.Location = new System.Drawing.Point(-3, 25);
            this.labelSubType.Name = "labelSubType";
            this.labelSubType.Size = new System.Drawing.Size(50, 13);
            this.labelSubType.TabIndex = 13;
            this.labelSubType.Text = "SubType";
            // 
            // labelType
            // 
            this.labelType.AutoSize = true;
            this.labelType.Location = new System.Drawing.Point(-3, 4);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(51, 13);
            this.labelType.TabIndex = 12;
            this.labelType.Text = "ItemType";
            // 
            // comboBoxMaterial
            // 
            this.comboBoxMaterial.FormattingEnabled = true;
            this.comboBoxMaterial.Items.AddRange(new object[] {
            "None",
            "Iron",
            "Wood",
            "unknown",
            "unknown",
            "Obsidian",
            "unknown",
            "Bone",
            "unknown",
            "unknown",
            "unknown",
            "Gold",
            "Silver",
            "Emerald",
            "Sapphire",
            "Ruby",
            "Diamond",
            "Sandstone",
            "Saurian",
            "Parrot",
            "Mammoth",
            "Plant",
            "Ice",
            "Licht",
            "Glass",
            "Silk",
            "Linen",
            "Cotton",
            "",
            "Fire",
            "Unholy",
            "Ice",
            "Wind"});
            this.comboBoxMaterial.Location = new System.Drawing.Point(91, 105);
            this.comboBoxMaterial.Name = "comboBoxMaterial";
            this.comboBoxMaterial.Size = new System.Drawing.Size(75, 21);
            this.comboBoxMaterial.TabIndex = 11;
            this.comboBoxMaterial.SelectedIndexChanged += new System.EventHandler(this.ComboBoxMaterial_SelectedIndexChanged);
            // 
            // comboBoxRarity
            // 
            this.comboBoxRarity.FormattingEnabled = true;
            this.comboBoxRarity.Items.AddRange(new object[] {
            "Normal",
            "Uncommon",
            "Rare",
            "Epic",
            "Legendary",
            "Mythic"});
            this.comboBoxRarity.Location = new System.Drawing.Point(91, 84);
            this.comboBoxRarity.Name = "comboBoxRarity";
            this.comboBoxRarity.Size = new System.Drawing.Size(75, 21);
            this.comboBoxRarity.TabIndex = 10;
            this.comboBoxRarity.SelectedIndexChanged += new System.EventHandler(this.ComboBoxRarity_SelectedIndexChanged);
            // 
            // comboBoxSubType
            // 
            this.comboBoxSubType.FormattingEnabled = true;
            this.comboBoxSubType.Location = new System.Drawing.Point(91, 21);
            this.comboBoxSubType.Name = "comboBoxSubType";
            this.comboBoxSubType.Size = new System.Drawing.Size(75, 21);
            this.comboBoxSubType.TabIndex = 9;
            this.comboBoxSubType.SelectedIndexChanged += new System.EventHandler(this.ComboBoxSubType_SelectedIndexChanged);
            // 
            // comboBoxType
            // 
            this.comboBoxType.FormattingEnabled = true;
            this.comboBoxType.Items.AddRange(new object[] {
            "None",
            "Food",
            "Formula",
            "Weapon",
            "Chest Armor",
            "Gloves",
            "Boots",
            "Shoulder Armor",
            "Amulet",
            "Ring",
            "Block",
            "Resource",
            "Coin",
            "Platinum",
            "Leftover",
            "Beak",
            "Painting",
            "Vase/Pot",
            "Candle",
            "Pet",
            "Pet Food",
            "Quest Item",
            "Unknown",
            "Special",
            "Lamp",
            "Mana Cube"});
            this.comboBoxType.Location = new System.Drawing.Point(91, 0);
            this.comboBoxType.Name = "comboBoxType";
            this.comboBoxType.Size = new System.Drawing.Size(75, 21);
            this.comboBoxType.TabIndex = 8;
            this.comboBoxType.SelectedIndexChanged += new System.EventHandler(this.ComboBoxType_SelectedIndexChanged);
            // 
            // checkBoxAdapted
            // 
            this.checkBoxAdapted.AutoSize = true;
            this.checkBoxAdapted.Location = new System.Drawing.Point(-5, 147);
            this.checkBoxAdapted.Name = "checkBoxAdapted";
            this.checkBoxAdapted.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxAdapted.Size = new System.Drawing.Size(69, 17);
            this.checkBoxAdapted.TabIndex = 7;
            this.checkBoxAdapted.Text = "Adapted ";
            this.checkBoxAdapted.UseVisualStyleBackColor = true;
            this.checkBoxAdapted.CheckedChanged += new System.EventHandler(this.CheckBoxAdapted_CheckedChanged);
            // 
            // numericUpDownLevel
            // 
            this.numericUpDownLevel.Location = new System.Drawing.Point(50, 126);
            this.numericUpDownLevel.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.numericUpDownLevel.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.numericUpDownLevel.Name = "numericUpDownLevel";
            this.numericUpDownLevel.Size = new System.Drawing.Size(116, 20);
            this.numericUpDownLevel.TabIndex = 6;
            this.numericUpDownLevel.ValueChanged += new System.EventHandler(this.NumericUpDownLevel_ValueChanged);
            // 
            // numericUpDownMaterial
            // 
            this.numericUpDownMaterial.Location = new System.Drawing.Point(50, 105);
            this.numericUpDownMaterial.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownMaterial.Name = "numericUpDownMaterial";
            this.numericUpDownMaterial.Size = new System.Drawing.Size(40, 20);
            this.numericUpDownMaterial.TabIndex = 5;
            this.numericUpDownMaterial.ValueChanged += new System.EventHandler(this.NumericUpDownMaterial_ValueChanged);
            // 
            // numericUpDownRarity
            // 
            this.numericUpDownRarity.Location = new System.Drawing.Point(50, 84);
            this.numericUpDownRarity.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownRarity.Name = "numericUpDownRarity";
            this.numericUpDownRarity.Size = new System.Drawing.Size(40, 20);
            this.numericUpDownRarity.TabIndex = 4;
            this.numericUpDownRarity.ValueChanged += new System.EventHandler(this.NumericUpDownRarity_ValueChanged);
            // 
            // numericUpDownRecipe
            // 
            this.numericUpDownRecipe.Location = new System.Drawing.Point(50, 63);
            this.numericUpDownRecipe.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numericUpDownRecipe.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.numericUpDownRecipe.Name = "numericUpDownRecipe";
            this.numericUpDownRecipe.Size = new System.Drawing.Size(116, 20);
            this.numericUpDownRecipe.TabIndex = 3;
            this.numericUpDownRecipe.ValueChanged += new System.EventHandler(this.NumericUpDownRecipe_ValueChanged);
            // 
            // numericUpDownModifier
            // 
            this.numericUpDownModifier.Location = new System.Drawing.Point(50, 42);
            this.numericUpDownModifier.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numericUpDownModifier.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.numericUpDownModifier.Name = "numericUpDownModifier";
            this.numericUpDownModifier.Size = new System.Drawing.Size(116, 20);
            this.numericUpDownModifier.TabIndex = 2;
            this.numericUpDownModifier.ValueChanged += new System.EventHandler(this.NumericUpDownModifier_ValueChanged);
            // 
            // numericUpDownSubType
            // 
            this.numericUpDownSubType.Location = new System.Drawing.Point(50, 21);
            this.numericUpDownSubType.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownSubType.Name = "numericUpDownSubType";
            this.numericUpDownSubType.Size = new System.Drawing.Size(40, 20);
            this.numericUpDownSubType.TabIndex = 1;
            this.numericUpDownSubType.ValueChanged += new System.EventHandler(this.NumericUpDownSubType_ValueChanged);
            // 
            // numericUpDownType
            // 
            this.numericUpDownType.Location = new System.Drawing.Point(50, 0);
            this.numericUpDownType.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownType.Name = "numericUpDownType";
            this.numericUpDownType.Size = new System.Drawing.Size(40, 20);
            this.numericUpDownType.TabIndex = 0;
            this.numericUpDownType.ValueChanged += new System.EventHandler(this.NumericUpDownType_ValueChanged);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.buttonNone);
            this.tabPage3.Controls.Add(this.buttonAll);
            this.tabPage3.Controls.Add(this.checkBoxHighlight);
            this.tabPage3.Controls.Add(this.numericUpDownVisible);
            this.tabPage3.Controls.Add(this.labelVisible);
            this.tabPage3.Controls.Add(this.comboBoxTypeC);
            this.tabPage3.Controls.Add(this.labelTypeC);
            this.tabPage3.Controls.Add(this.numericUpDownTypeC);
            this.tabPage3.Controls.Add(this.numericUpDownLevelC);
            this.tabPage3.Controls.Add(this.labelLevelC);
            this.tabPage3.Controls.Add(this.labelPosZ);
            this.tabPage3.Controls.Add(this.labelPosY);
            this.tabPage3.Controls.Add(this.labelPosX);
            this.tabPage3.Controls.Add(this.numericUpDownZ);
            this.tabPage3.Controls.Add(this.numericUpDownY);
            this.tabPage3.Controls.Add(this.numericUpDownX);
            this.tabPage3.Controls.Add(this.listBoxSlots);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(167, 161);
            this.tabPage3.TabIndex = 1;
            this.tabPage3.Text = "cubes/spirits";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // buttonNone
            // 
            this.buttonNone.Enabled = false;
            this.buttonNone.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonNone.Location = new System.Drawing.Point(35, 106);
            this.buttonNone.Name = "buttonNone";
            this.buttonNone.Size = new System.Drawing.Size(40, 15);
            this.buttonNone.TabIndex = 4;
            this.buttonNone.Text = "NONE";
            this.buttonNone.UseVisualStyleBackColor = true;
            // 
            // buttonAll
            // 
            this.buttonAll.Enabled = false;
            this.buttonAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAll.Location = new System.Drawing.Point(-1, 106);
            this.buttonAll.Name = "buttonAll";
            this.buttonAll.Size = new System.Drawing.Size(36, 15);
            this.buttonAll.TabIndex = 4;
            this.buttonAll.Text = "ALL";
            this.buttonAll.UseVisualStyleBackColor = true;
            // 
            // checkBoxHighlight
            // 
            this.checkBoxHighlight.AutoSize = true;
            this.checkBoxHighlight.Enabled = false;
            this.checkBoxHighlight.Location = new System.Drawing.Point(78, 83);
            this.checkBoxHighlight.Name = "checkBoxHighlight";
            this.checkBoxHighlight.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxHighlight.Size = new System.Drawing.Size(68, 30);
            this.checkBoxHighlight.TabIndex = 4;
            this.checkBoxHighlight.Text = "Highlight\r\nSelected";
            this.checkBoxHighlight.UseVisualStyleBackColor = true;
            this.checkBoxHighlight.CheckedChanged += new System.EventHandler(this.CheckBoxHighlight_CheckedChanged);
            // 
            // numericUpDownVisible
            // 
            this.numericUpDownVisible.Location = new System.Drawing.Point(95, 143);
            this.numericUpDownVisible.Maximum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.numericUpDownVisible.Name = "numericUpDownVisible";
            this.numericUpDownVisible.Size = new System.Drawing.Size(35, 20);
            this.numericUpDownVisible.TabIndex = 13;
            this.numericUpDownVisible.ValueChanged += new System.EventHandler(this.NumericUpDownVisible_ValueChanged);
            // 
            // labelVisible
            // 
            this.labelVisible.AutoSize = true;
            this.labelVisible.Location = new System.Drawing.Point(16, 145);
            this.labelVisible.Name = "labelVisible";
            this.labelVisible.Size = new System.Drawing.Size(81, 13);
            this.labelVisible.TabIndex = 12;
            this.labelVisible.Text = "visible slots:  0 -";
            // 
            // comboBoxTypeC
            // 
            this.comboBoxTypeC.Enabled = false;
            this.comboBoxTypeC.FormattingEnabled = true;
            this.comboBoxTypeC.Items.AddRange(new object[] {
            "None",
            "Iron",
            "Wood",
            " ",
            " ",
            "Obsidian",
            " ",
            "Bone",
            " ",
            " ",
            " ",
            "Gold",
            "Silver",
            "Emerald",
            "Sapphire",
            "Ruby",
            "Diamond",
            "Sandstone",
            "Saurian",
            "Parrot",
            "Mammoth",
            "Plant",
            "Ice",
            "Licht",
            "Glass",
            "Silk",
            "Linen",
            "Cotton",
            " ",
            "Fire",
            "Unholy",
            "Ice",
            "Wind"});
            this.comboBoxTypeC.Location = new System.Drawing.Point(81, 122);
            this.comboBoxTypeC.Name = "comboBoxTypeC";
            this.comboBoxTypeC.Size = new System.Drawing.Size(85, 21);
            this.comboBoxTypeC.TabIndex = 11;
            this.comboBoxTypeC.SelectedIndexChanged += new System.EventHandler(this.ComboBoxTypeC_SelectedIndexChanged);
            // 
            // labelTypeC
            // 
            this.labelTypeC.AutoSize = true;
            this.labelTypeC.Location = new System.Drawing.Point(-1, 126);
            this.labelTypeC.Name = "labelTypeC";
            this.labelTypeC.Size = new System.Drawing.Size(31, 13);
            this.labelTypeC.TabIndex = 10;
            this.labelTypeC.Text = "Type";
            // 
            // numericUpDownTypeC
            // 
            this.numericUpDownTypeC.Enabled = false;
            this.numericUpDownTypeC.Location = new System.Drawing.Point(30, 123);
            this.numericUpDownTypeC.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownTypeC.Name = "numericUpDownTypeC";
            this.numericUpDownTypeC.Size = new System.Drawing.Size(50, 20);
            this.numericUpDownTypeC.TabIndex = 9;
            this.numericUpDownTypeC.ValueChanged += new System.EventHandler(this.NumericUpDownTypeC_ValueChanged);
            // 
            // numericUpDownLevelC
            // 
            this.numericUpDownLevelC.Enabled = false;
            this.numericUpDownLevelC.Location = new System.Drawing.Point(126, 63);
            this.numericUpDownLevelC.Maximum = new decimal(new int[] {
            32767,
            0,
            0,
            0});
            this.numericUpDownLevelC.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.numericUpDownLevelC.Name = "numericUpDownLevelC";
            this.numericUpDownLevelC.Size = new System.Drawing.Size(40, 20);
            this.numericUpDownLevelC.TabIndex = 8;
            this.numericUpDownLevelC.ValueChanged += new System.EventHandler(this.NumericUpDownLevelC_ValueChanged);
            // 
            // labelLevelC
            // 
            this.labelLevelC.AutoSize = true;
            this.labelLevelC.Location = new System.Drawing.Point(80, 66);
            this.labelLevelC.Name = "labelLevelC";
            this.labelLevelC.Size = new System.Drawing.Size(33, 13);
            this.labelLevelC.TabIndex = 7;
            this.labelLevelC.Text = "Level";
            // 
            // labelPosZ
            // 
            this.labelPosZ.AutoSize = true;
            this.labelPosZ.Location = new System.Drawing.Point(80, 46);
            this.labelPosZ.Name = "labelPosZ";
            this.labelPosZ.Size = new System.Drawing.Size(32, 13);
            this.labelPosZ.TabIndex = 6;
            this.labelPosZ.Text = "PosZ";
            // 
            // labelPosY
            // 
            this.labelPosY.AutoSize = true;
            this.labelPosY.Location = new System.Drawing.Point(80, 25);
            this.labelPosY.Name = "labelPosY";
            this.labelPosY.Size = new System.Drawing.Size(32, 13);
            this.labelPosY.TabIndex = 5;
            this.labelPosY.Text = "PosY";
            // 
            // labelPosX
            // 
            this.labelPosX.AutoSize = true;
            this.labelPosX.Location = new System.Drawing.Point(80, 4);
            this.labelPosX.Name = "labelPosX";
            this.labelPosX.Size = new System.Drawing.Size(32, 13);
            this.labelPosX.TabIndex = 4;
            this.labelPosX.Text = "PosX";
            // 
            // numericUpDownZ
            // 
            this.numericUpDownZ.Enabled = false;
            this.numericUpDownZ.Location = new System.Drawing.Point(126, 42);
            this.numericUpDownZ.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.numericUpDownZ.Minimum = new decimal(new int[] {
            128,
            0,
            0,
            -2147483648});
            this.numericUpDownZ.Name = "numericUpDownZ";
            this.numericUpDownZ.Size = new System.Drawing.Size(40, 20);
            this.numericUpDownZ.TabIndex = 3;
            this.numericUpDownZ.ValueChanged += new System.EventHandler(this.NumericUpDownZ_ValueChanged);
            // 
            // numericUpDownY
            // 
            this.numericUpDownY.Enabled = false;
            this.numericUpDownY.Location = new System.Drawing.Point(126, 21);
            this.numericUpDownY.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.numericUpDownY.Minimum = new decimal(new int[] {
            128,
            0,
            0,
            -2147483648});
            this.numericUpDownY.Name = "numericUpDownY";
            this.numericUpDownY.Size = new System.Drawing.Size(40, 20);
            this.numericUpDownY.TabIndex = 2;
            this.numericUpDownY.ValueChanged += new System.EventHandler(this.NumericUpDownY_ValueChanged);
            // 
            // numericUpDownX
            // 
            this.numericUpDownX.Enabled = false;
            this.numericUpDownX.Location = new System.Drawing.Point(126, 0);
            this.numericUpDownX.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.numericUpDownX.Minimum = new decimal(new int[] {
            128,
            0,
            0,
            -2147483648});
            this.numericUpDownX.Name = "numericUpDownX";
            this.numericUpDownX.Size = new System.Drawing.Size(40, 20);
            this.numericUpDownX.TabIndex = 1;
            this.numericUpDownX.ValueChanged += new System.EventHandler(this.NumericUpDownX_ValueChanged);
            // 
            // listBoxSlots
            // 
            this.listBoxSlots.ColumnWidth = 19;
            this.listBoxSlots.FormattingEnabled = true;
            this.listBoxSlots.Items.AddRange(new object[] {
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31",
            "32"});
            this.listBoxSlots.Location = new System.Drawing.Point(-2, -2);
            this.listBoxSlots.MultiColumn = true;
            this.listBoxSlots.Name = "listBoxSlots";
            this.listBoxSlots.Size = new System.Drawing.Size(80, 108);
            this.listBoxSlots.TabIndex = 0;
            this.listBoxSlots.SelectedIndexChanged += new System.EventHandler(this.ListBoxSlots_SelectedIndexChanged);
            // 
            // numericUpDownPlatinum
            // 
            this.numericUpDownPlatinum.Location = new System.Drawing.Point(273, 165);
            this.numericUpDownPlatinum.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numericUpDownPlatinum.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.numericUpDownPlatinum.Name = "numericUpDownPlatinum";
            this.numericUpDownPlatinum.Size = new System.Drawing.Size(57, 20);
            this.numericUpDownPlatinum.TabIndex = 6;
            this.numericUpDownPlatinum.ValueChanged += new System.EventHandler(this.NumericUpDownPlatinum_ValueChanged);
            // 
            // listBoxItem
            // 
            this.listBoxItem.FormattingEnabled = true;
            this.listBoxItem.Items.AddRange(new object[] {
            "using",
            "unknown",
            "neck",
            "chest",
            "feet",
            "hands",
            "shoulder",
            "Lweapon",
            "Rweapon",
            "leftRing",
            "rightRing",
            "lamp",
            "special",
            "pet"});
            this.listBoxItem.Location = new System.Drawing.Point(0, 0);
            this.listBoxItem.Name = "listBoxItem";
            this.listBoxItem.Size = new System.Drawing.Size(53, 186);
            this.listBoxItem.TabIndex = 13;
            this.listBoxItem.SelectedIndexChanged += new System.EventHandler(this.ListBoxItem_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(245, 109);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "level";
            // 
            // numericUpDownXP
            // 
            this.numericUpDownXP.Location = new System.Drawing.Point(273, 125);
            this.numericUpDownXP.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numericUpDownXP.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.numericUpDownXP.Name = "numericUpDownXP";
            this.numericUpDownXP.Size = new System.Drawing.Size(57, 20);
            this.numericUpDownXP.TabIndex = 1;
            this.numericUpDownXP.ValueChanged += new System.EventHandler(this.NumericUpDownXP_ValueChanged);
            // 
            // numericUpDownCharacterLevel
            // 
            this.numericUpDownCharacterLevel.Location = new System.Drawing.Point(273, 105);
            this.numericUpDownCharacterLevel.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numericUpDownCharacterLevel.Minimum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            -2147483648});
            this.numericUpDownCharacterLevel.Name = "numericUpDownCharacterLevel";
            this.numericUpDownCharacterLevel.Size = new System.Drawing.Size(57, 20);
            this.numericUpDownCharacterLevel.TabIndex = 0;
            this.numericUpDownCharacterLevel.ValueChanged += new System.EventHandler(this.NumericUpDownCharacterLevel_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(229, 167);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 13);
            this.label6.TabIndex = 21;
            this.label6.Text = "platinum";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(256, 127);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(18, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "xp";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(236, 147);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 20;
            this.label5.Text = "money";
            // 
            // TimerHighlight
            // 
            this.TimerHighlight.Interval = 250;
            this.TimerHighlight.Tick += new System.EventHandler(this.TimerHighlight_Tick);
            // 
            // timerRefresh
            // 
            this.timerRefresh.Interval = 150;
            this.timerRefresh.Tick += new System.EventHandler(this.TimerRefresh_Tick);
            // 
            // FormEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(331, 186);
            this.Controls.Add(this.groupBoxClass);
            this.Controls.Add(this.numericUpDownMoney);
            this.Controls.Add(this.numericUpDownPlatinum);
            this.Controls.Add(this.listBoxItem);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tabControlItemEditor);
            this.Controls.Add(this.numericUpDownXP);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.numericUpDownCharacterLevel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label6);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormEditor";
            this.Text = "Character editor";
            this.TopMost = true;
            this.Shown += new System.EventHandler(this.FormEditor_Shown);
            this.groupBoxClass.ResumeLayout(false);
            this.groupBoxClass.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMoney)).EndInit();
            this.tabControlItemEditor.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownMaterial)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRarity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownRecipe)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownModifier)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSubType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownType)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownVisible)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTypeC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLevelC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPlatinum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownXP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCharacterLevel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.GroupBox groupBoxClass;
        public System.Windows.Forms.ComboBox comboBoxClass;
        public System.Windows.Forms.RadioButton radioButtonSubclass1;
        public System.Windows.Forms.RadioButton radioButtonSubclass2;
        public System.Windows.Forms.NumericUpDown numericUpDownMoney;
        public System.Windows.Forms.TabControl tabControlItemEditor;
        public System.Windows.Forms.TabPage tabPage4;
        public System.Windows.Forms.Label labelLevel;
        public System.Windows.Forms.Label labelMaterial;
        public System.Windows.Forms.Label labelRarity;
        public System.Windows.Forms.Label labelRecipe;
        public System.Windows.Forms.Label labelModifier;
        public System.Windows.Forms.Label labelSubType;
        public System.Windows.Forms.Label labelType;
        public System.Windows.Forms.ComboBox comboBoxMaterial;
        public System.Windows.Forms.ComboBox comboBoxRarity;
        public System.Windows.Forms.ComboBox comboBoxSubType;
        public System.Windows.Forms.ComboBox comboBoxType;
        public System.Windows.Forms.CheckBox checkBoxAdapted;
        public System.Windows.Forms.NumericUpDown numericUpDownLevel;
        public System.Windows.Forms.NumericUpDown numericUpDownMaterial;
        public System.Windows.Forms.NumericUpDown numericUpDownRarity;
        public System.Windows.Forms.NumericUpDown numericUpDownRecipe;
        public System.Windows.Forms.NumericUpDown numericUpDownModifier;
        public System.Windows.Forms.NumericUpDown numericUpDownSubType;
        public System.Windows.Forms.NumericUpDown numericUpDownType;
        public System.Windows.Forms.TabPage tabPage3;
        public System.Windows.Forms.Button buttonNone;
        public System.Windows.Forms.Button buttonAll;
        public System.Windows.Forms.CheckBox checkBoxHighlight;
        public System.Windows.Forms.NumericUpDown numericUpDownVisible;
        public System.Windows.Forms.Label labelVisible;
        public System.Windows.Forms.ComboBox comboBoxTypeC;
        public System.Windows.Forms.Label labelTypeC;
        public System.Windows.Forms.NumericUpDown numericUpDownTypeC;
        public System.Windows.Forms.NumericUpDown numericUpDownLevelC;
        public System.Windows.Forms.Label labelLevelC;
        public System.Windows.Forms.Label labelPosZ;
        public System.Windows.Forms.Label labelPosY;
        public System.Windows.Forms.Label labelPosX;
        public System.Windows.Forms.NumericUpDown numericUpDownZ;
        public System.Windows.Forms.NumericUpDown numericUpDownY;
        public System.Windows.Forms.NumericUpDown numericUpDownX;
        public System.Windows.Forms.ListBox listBoxSlots;
        public System.Windows.Forms.NumericUpDown numericUpDownPlatinum;
        public System.Windows.Forms.ListBox listBoxItem;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.NumericUpDown numericUpDownXP;
        public System.Windows.Forms.NumericUpDown numericUpDownCharacterLevel;
        public System.Windows.Forms.Label label6;
        public System.Windows.Forms.Label label2;
        public System.Windows.Forms.Label label5;
        public System.Windows.Forms.Timer TimerHighlight;
        private System.Windows.Forms.Timer timerRefresh;
    }
}
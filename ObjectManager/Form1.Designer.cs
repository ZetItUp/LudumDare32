namespace ObjectManager
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.cboBaseItems = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtWidth = new System.Windows.Forms.TextBox();
            this.txtHeight = new System.Windows.Forms.TextBox();
            this.trkDepth = new System.Windows.Forms.TrackBar();
            this.lblDepth = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtScale = new System.Windows.Forms.TextBox();
            this.chkDrawAlways = new System.Windows.Forms.CheckBox();
            this.chkAllowToDrawDebug = new System.Windows.Forms.CheckBox();
            this.chkAnimate = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtColorR = new System.Windows.Forms.TextBox();
            this.txtColorG = new System.Windows.Forms.TextBox();
            this.txtColorB = new System.Windows.Forms.TextBox();
            this.txtColorA = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.trkDepth)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Base Object Type";
            // 
            // cboBaseItems
            // 
            this.cboBaseItems.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBaseItems.FormattingEnabled = true;
            this.cboBaseItems.Items.AddRange(new object[] {
            "BoxDebris",
            "ExitPortal",
            "ExtraLife",
            "FlameBowl",
            "ItemCrate",
            "PowerUp",
            "Star"});
            this.cboBaseItems.Location = new System.Drawing.Point(15, 37);
            this.cboBaseItems.Name = "cboBaseItems";
            this.cboBaseItems.Size = new System.Drawing.Size(141, 21);
            this.cboBaseItems.Sorted = true;
            this.cboBaseItems.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Name";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(15, 89);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(165, 20);
            this.txtName.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Width";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(112, 116);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Height";
            // 
            // txtWidth
            // 
            this.txtWidth.Location = new System.Drawing.Point(15, 132);
            this.txtWidth.Name = "txtWidth";
            this.txtWidth.Size = new System.Drawing.Size(66, 20);
            this.txtWidth.TabIndex = 6;
            // 
            // txtHeight
            // 
            this.txtHeight.Location = new System.Drawing.Point(115, 132);
            this.txtHeight.Name = "txtHeight";
            this.txtHeight.Size = new System.Drawing.Size(65, 20);
            this.txtHeight.TabIndex = 7;
            // 
            // trkDepth
            // 
            this.trkDepth.Location = new System.Drawing.Point(15, 182);
            this.trkDepth.Name = "trkDepth";
            this.trkDepth.Size = new System.Drawing.Size(165, 45);
            this.trkDepth.TabIndex = 8;
            this.trkDepth.ValueChanged += new System.EventHandler(this.trkDepth_ValueChanged);
            // 
            // lblDepth
            // 
            this.lblDepth.AutoSize = true;
            this.lblDepth.Location = new System.Drawing.Point(15, 166);
            this.lblDepth.Name = "lblDepth";
            this.lblDepth.Size = new System.Drawing.Size(36, 13);
            this.lblDepth.TabIndex = 9;
            this.lblDepth.Text = "Depth";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 230);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(34, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Scale";
            // 
            // txtScale
            // 
            this.txtScale.Location = new System.Drawing.Point(80, 227);
            this.txtScale.Name = "txtScale";
            this.txtScale.Size = new System.Drawing.Size(100, 20);
            this.txtScale.TabIndex = 11;
            this.txtScale.Text = "1";
            // 
            // chkDrawAlways
            // 
            this.chkDrawAlways.AutoSize = true;
            this.chkDrawAlways.Location = new System.Drawing.Point(18, 253);
            this.chkDrawAlways.Name = "chkDrawAlways";
            this.chkDrawAlways.Size = new System.Drawing.Size(86, 17);
            this.chkDrawAlways.TabIndex = 12;
            this.chkDrawAlways.Text = "Draw always";
            this.chkDrawAlways.UseVisualStyleBackColor = true;
            // 
            // chkAllowToDrawDebug
            // 
            this.chkAllowToDrawDebug.AutoSize = true;
            this.chkAllowToDrawDebug.Location = new System.Drawing.Point(18, 276);
            this.chkAllowToDrawDebug.Name = "chkAllowToDrawDebug";
            this.chkAllowToDrawDebug.Size = new System.Drawing.Size(167, 17);
            this.chkAllowToDrawDebug.TabIndex = 13;
            this.chkAllowToDrawDebug.Text = "Allowed to draw debug border";
            this.chkAllowToDrawDebug.UseVisualStyleBackColor = true;
            // 
            // chkAnimate
            // 
            this.chkAnimate.AutoSize = true;
            this.chkAnimate.Location = new System.Drawing.Point(18, 299);
            this.chkAnimate.Name = "chkAnimate";
            this.chkAnimate.Size = new System.Drawing.Size(64, 17);
            this.chkAnimate.TabIndex = 14;
            this.chkAnimate.Text = "Animate";
            this.chkAnimate.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 328);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(116, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Draw Color (R, G, B, A)";
            // 
            // txtColorR
            // 
            this.txtColorR.Location = new System.Drawing.Point(19, 344);
            this.txtColorR.Name = "txtColorR";
            this.txtColorR.Size = new System.Drawing.Size(32, 20);
            this.txtColorR.TabIndex = 16;
            this.txtColorR.Text = "255";
            // 
            // txtColorG
            // 
            this.txtColorG.Location = new System.Drawing.Point(57, 344);
            this.txtColorG.Name = "txtColorG";
            this.txtColorG.Size = new System.Drawing.Size(32, 20);
            this.txtColorG.TabIndex = 17;
            this.txtColorG.Text = "255";
            // 
            // txtColorB
            // 
            this.txtColorB.Location = new System.Drawing.Point(95, 344);
            this.txtColorB.Name = "txtColorB";
            this.txtColorB.Size = new System.Drawing.Size(32, 20);
            this.txtColorB.TabIndex = 18;
            this.txtColorB.Text = "255";
            // 
            // txtColorA
            // 
            this.txtColorA.Location = new System.Drawing.Point(133, 344);
            this.txtColorA.Name = "txtColorA";
            this.txtColorA.Size = new System.Drawing.Size(32, 20);
            this.txtColorA.TabIndex = 19;
            this.txtColorA.Text = "255";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 497);
            this.Controls.Add(this.txtColorA);
            this.Controls.Add(this.txtColorB);
            this.Controls.Add(this.txtColorG);
            this.Controls.Add(this.txtColorR);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.chkAnimate);
            this.Controls.Add(this.chkAllowToDrawDebug);
            this.Controls.Add(this.chkDrawAlways);
            this.Controls.Add(this.txtScale);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblDepth);
            this.Controls.Add(this.trkDepth);
            this.Controls.Add(this.txtHeight);
            this.Controls.Add(this.txtWidth);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboBaseItems);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Object Manager";
            ((System.ComponentModel.ISupportInitialize)(this.trkDepth)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboBaseItems;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtWidth;
        private System.Windows.Forms.TextBox txtHeight;
        private System.Windows.Forms.TrackBar trkDepth;
        private System.Windows.Forms.Label lblDepth;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtScale;
        private System.Windows.Forms.CheckBox chkDrawAlways;
        private System.Windows.Forms.CheckBox chkAllowToDrawDebug;
        private System.Windows.Forms.CheckBox chkAnimate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtColorR;
        private System.Windows.Forms.TextBox txtColorG;
        private System.Windows.Forms.TextBox txtColorB;
        private System.Windows.Forms.TextBox txtColorA;
    }
}


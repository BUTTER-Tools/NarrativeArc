namespace NarrativeArc
{
    partial class SettingsForm_NarrativeArc
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm_NarrativeArc));
            this.OKButton = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.SegmentsUpDown = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.ScalingMethodDropDown = new System.Windows.Forms.ComboBox();
            this.includeDataPointsCheckbox = new System.Windows.Forms.CheckBox();
            this.AllowOverlapsCheckbox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.SegmentsUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // OKButton
            // 
            this.OKButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OKButton.Location = new System.Drawing.Point(112, 237);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(118, 40);
            this.OKButton.TabIndex = 6;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(11, 45);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(178, 20);
            this.label10.TabIndex = 12;
            this.label10.Text = "Number of Segments";
            // 
            // SegmentsUpDown
            // 
            this.SegmentsUpDown.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SegmentsUpDown.Location = new System.Drawing.Point(205, 44);
            this.SegmentsUpDown.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.SegmentsUpDown.Name = "SegmentsUpDown";
            this.SegmentsUpDown.Size = new System.Drawing.Size(120, 26);
            this.SegmentsUpDown.TabIndex = 13;
            this.SegmentsUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.SegmentsUpDown.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(11, 93);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 20);
            this.label1.TabIndex = 14;
            this.label1.Text = "Scaling Method";
            // 
            // ScalingMethodDropDown
            // 
            this.ScalingMethodDropDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ScalingMethodDropDown.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ScalingMethodDropDown.FormattingEnabled = true;
            this.ScalingMethodDropDown.Location = new System.Drawing.Point(204, 91);
            this.ScalingMethodDropDown.Name = "ScalingMethodDropDown";
            this.ScalingMethodDropDown.Size = new System.Drawing.Size(121, 26);
            this.ScalingMethodDropDown.TabIndex = 15;
            // 
            // includeDataPointsCheckbox
            // 
            this.includeDataPointsCheckbox.AutoSize = true;
            this.includeDataPointsCheckbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.includeDataPointsCheckbox.Location = new System.Drawing.Point(15, 141);
            this.includeDataPointsCheckbox.Name = "includeDataPointsCheckbox";
            this.includeDataPointsCheckbox.Size = new System.Drawing.Size(219, 20);
            this.includeDataPointsCheckbox.TabIndex = 16;
            this.includeDataPointsCheckbox.Text = "Include Arc Data Points in Output";
            this.includeDataPointsCheckbox.UseVisualStyleBackColor = true;
            // 
            // AllowOverlapsCheckbox
            // 
            this.AllowOverlapsCheckbox.AutoSize = true;
            this.AllowOverlapsCheckbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AllowOverlapsCheckbox.Location = new System.Drawing.Point(15, 183);
            this.AllowOverlapsCheckbox.Name = "AllowOverlapsCheckbox";
            this.AllowOverlapsCheckbox.Size = new System.Drawing.Size(276, 20);
            this.AllowOverlapsCheckbox.TabIndex = 17;
            this.AllowOverlapsCheckbox.Text = "Allow Narrative Dimension Dependencies";
            this.AllowOverlapsCheckbox.UseVisualStyleBackColor = true;
            // 
            // SettingsForm_NarrativeArc
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(342, 289);
            this.Controls.Add(this.AllowOverlapsCheckbox);
            this.Controls.Add(this.includeDataPointsCheckbox);
            this.Controls.Add(this.ScalingMethodDropDown);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SegmentsUpDown);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.OKButton);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SettingsForm_NarrativeArc";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Plugin Name";
            ((System.ComponentModel.ISupportInitialize)(this.SegmentsUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown SegmentsUpDown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox ScalingMethodDropDown;
        private System.Windows.Forms.CheckBox includeDataPointsCheckbox;
        private System.Windows.Forms.CheckBox AllowOverlapsCheckbox;
    }
}
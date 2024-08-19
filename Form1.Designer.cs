namespace VimPig
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox commandTextBox;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            commandTextBox = new TextBox();
            panel1 = new Panel();
            textBox1 = new TextBox();
            panel2 = new Panel();
            panel3 = new Panel();
            label1 = new Label();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // commandTextBox
            // 
            commandTextBox.BackColor = Color.FromArgb(30, 30, 30);
            commandTextBox.BorderStyle = BorderStyle.None;
            commandTextBox.ForeColor = SystemColors.Window;
            commandTextBox.Location = new Point(34, 9);
            commandTextBox.Margin = new Padding(4, 3, 4, 3);
            commandTextBox.Name = "commandTextBox";
            commandTextBox.Size = new Size(260, 16);
            commandTextBox.TabIndex = 1;
            commandTextBox.Visible = false;
            commandTextBox.KeyDown += Scintilla_KeyDown;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(45, 45, 45);
            panel1.Controls.Add(textBox1);
            panel1.Location = new Point(12, 54);
            panel1.Name = "panel1";
            panel1.Size = new Size(909, 414);
            panel1.TabIndex = 4;
            // 
            // textBox1
            // 
            textBox1.Anchor = AnchorStyles.Left;
            textBox1.BackColor = Color.FromArgb(64, 64, 64);
            textBox1.BorderStyle = BorderStyle.None;
            textBox1.ForeColor = SystemColors.Control;
            textBox1.Location = new Point(16, 15);
            textBox1.Margin = new Padding(4, 3, 4, 3);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(879, 385);
            textBox1.TabIndex = 0;
            textBox1.KeyDown += Scintilla_KeyDown;
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(45, 45, 45);
            panel2.Controls.Add(commandTextBox);
            panel2.Location = new Point(291, 474);
            panel2.Name = "panel2";
            panel2.Size = new Size(329, 33);
            panel2.TabIndex = 5;
            // 
            // panel3
            // 
            panel3.BackColor = Color.FromArgb(45, 45, 45);
            panel3.Controls.Add(label1);
            panel3.Location = new Point(291, 10);
            panel3.Name = "panel3";
            panel3.Size = new Size(329, 38);
            panel3.TabIndex = 5;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top;
            label1.Font = new Font("Segoe UI", 12F);
            label1.ForeColor = SystemColors.Control;
            label1.Location = new Point(3, 9);
            label1.Name = "label1";
            label1.Size = new Size(323, 21);
            label1.TabIndex = 1;
            label1.Text = "Words: 0 | Lines: 0";
            label1.TextAlign = ContentAlignment.TopCenter;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(30, 30, 30);
            ClientSize = new Size(933, 519);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 3, 4, 3);
            Name = "Form1";
            Text = "VimPig";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            ResumeLayout(false);
        }

        private Panel panel1;
        private TextBox textBox1;
        private Panel panel2;
        private Panel panel3;
        private Label label1;
    }
}
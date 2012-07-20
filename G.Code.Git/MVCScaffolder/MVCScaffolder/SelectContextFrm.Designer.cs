namespace MVCScaffolder
{
    partial class SelectContextFrm
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
            this.chkBeDllList = new System.Windows.Forms.CheckedListBox();
            this.btnload = new System.Windows.Forms.Button();
            this.chkBeList = new System.Windows.Forms.CheckedListBox();
            this.btnok = new System.Windows.Forms.Button();
            this.btnselect = new System.Windows.Forms.Button();
            this.btnunselect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // chkBeDllList
            // 
            this.chkBeDllList.FormattingEnabled = true;
            this.chkBeDllList.Location = new System.Drawing.Point(13, 13);
            this.chkBeDllList.Name = "chkBeDllList";
            this.chkBeDllList.Size = new System.Drawing.Size(183, 148);
            this.chkBeDllList.TabIndex = 0;
            this.chkBeDllList.SelectedIndexChanged += new System.EventHandler(this.chkBeDllList_SelectedIndexChanged);
            // 
            // btnload
            // 
            this.btnload.Location = new System.Drawing.Point(202, 13);
            this.btnload.Name = "btnload";
            this.btnload.Size = new System.Drawing.Size(75, 23);
            this.btnload.TabIndex = 1;
            this.btnload.Text = "加载类";
            this.btnload.UseVisualStyleBackColor = true;
            this.btnload.Click += new System.EventHandler(this.btnload_Click);
            // 
            // chkBeList
            // 
            this.chkBeList.FormattingEnabled = true;
            this.chkBeList.Location = new System.Drawing.Point(367, 13);
            this.chkBeList.Name = "chkBeList";
            this.chkBeList.Size = new System.Drawing.Size(260, 340);
            this.chkBeList.TabIndex = 2;
            // 
            // btnok
            // 
            this.btnok.Location = new System.Drawing.Point(251, 332);
            this.btnok.Name = "btnok";
            this.btnok.Size = new System.Drawing.Size(75, 23);
            this.btnok.TabIndex = 3;
            this.btnok.Text = "确定";
            this.btnok.UseVisualStyleBackColor = true;
            this.btnok.Click += new System.EventHandler(this.btnok_Click);
            // 
            // btnselect
            // 
            this.btnselect.Location = new System.Drawing.Point(286, 13);
            this.btnselect.Name = "btnselect";
            this.btnselect.Size = new System.Drawing.Size(75, 23);
            this.btnselect.TabIndex = 4;
            this.btnselect.Text = "全选";
            this.btnselect.UseVisualStyleBackColor = true;
            this.btnselect.Click += new System.EventHandler(this.btnselect_Click);
            // 
            // btnunselect
            // 
            this.btnunselect.Location = new System.Drawing.Point(286, 43);
            this.btnunselect.Name = "btnunselect";
            this.btnunselect.Size = new System.Drawing.Size(75, 23);
            this.btnunselect.TabIndex = 5;
            this.btnunselect.Text = "反选";
            this.btnunselect.UseVisualStyleBackColor = true;
            this.btnunselect.Click += new System.EventHandler(this.btnunselect_Click);
            // 
            // SelectContextFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(648, 367);
            this.Controls.Add(this.btnunselect);
            this.Controls.Add(this.btnselect);
            this.Controls.Add(this.btnok);
            this.Controls.Add(this.chkBeList);
            this.Controls.Add(this.btnload);
            this.Controls.Add(this.chkBeDllList);
            this.Name = "SelectContextFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "选择Context";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SelectContextFrm_FormClosed);
            this.Load += new System.EventHandler(this.SelectContextFrm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox chkBeDllList;
        private System.Windows.Forms.Button btnload;
        private System.Windows.Forms.CheckedListBox chkBeList;
        private System.Windows.Forms.Button btnok;
        private System.Windows.Forms.Button btnselect;
        private System.Windows.Forms.Button btnunselect;
    }
}
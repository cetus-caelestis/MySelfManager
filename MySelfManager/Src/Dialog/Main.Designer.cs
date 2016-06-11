namespace MySelfManager
{
    partial class MySelfManager
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.taskTreeView_ = new System.Windows.Forms.TreeView();
            this.workProgressVal_ = new System.Windows.Forms.Label();
            this.workProgressHandle_ = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.finishBotton_ = new System.Windows.Forms.Button();
            this.transitionBotton_ = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.workstatus_ = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.workEnd_ = new System.Windows.Forms.Label();
            this.workname_ = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.taskContextMenu_ = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addChild_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.この上に作業を挿入ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.この下に作業を挿入ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.RemoveTask_ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.workProgressHandle_)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.taskContextMenu_.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Location = new System.Drawing.Point(322, 32);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(139, 33);
            this.button1.TabIndex = 0;
            this.button1.Text = "タスクを追加";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBox1.Location = new System.Drawing.Point(9, 36);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(296, 25);
            this.textBox1.TabIndex = 1;
            this.textBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // taskTreeView_
            // 
            this.taskTreeView_.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.taskTreeView_.Location = new System.Drawing.Point(12, 12);
            this.taskTreeView_.Name = "taskTreeView_";
            this.taskTreeView_.ShowPlusMinus = false;
            this.taskTreeView_.Size = new System.Drawing.Size(467, 177);
            this.taskTreeView_.TabIndex = 2;
            this.taskTreeView_.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.taskTreeView_.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.taskTreeView__NodeMouseClick);
            // 
            // workProgressVal_
            // 
            this.workProgressVal_.Location = new System.Drawing.Point(390, 158);
            this.workProgressVal_.Name = "workProgressVal_";
            this.workProgressVal_.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.workProgressVal_.Size = new System.Drawing.Size(60, 18);
            this.workProgressVal_.TabIndex = 5;
            this.workProgressVal_.Text = "  0％";
            this.workProgressVal_.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.workProgressVal_.DoubleClick += new System.EventHandler(this.workProgressVal__DoubleClick);
            // 
            // workProgressHandle_
            // 
            this.workProgressHandle_.Enabled = false;
            this.workProgressHandle_.Location = new System.Drawing.Point(6, 149);
            this.workProgressHandle_.Maximum = 20;
            this.workProgressHandle_.Name = "workProgressHandle_";
            this.workProgressHandle_.Size = new System.Drawing.Size(389, 69);
            this.workProgressHandle_.TabIndex = 6;
            this.workProgressHandle_.ValueChanged += new System.EventHandler(this.workProgressHandle__ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 18);
            this.label2.TabIndex = 7;
            this.label2.Text = "期間：";
            // 
            // finishBotton_
            // 
            this.finishBotton_.Enabled = false;
            this.finishBotton_.Location = new System.Drawing.Point(322, 202);
            this.finishBotton_.Name = "finishBotton_";
            this.finishBotton_.Size = new System.Drawing.Size(139, 33);
            this.finishBotton_.TabIndex = 8;
            this.finishBotton_.Text = "完了";
            this.finishBotton_.UseVisualStyleBackColor = true;
            this.finishBotton_.Click += new System.EventHandler(this.finishBotton__Click);
            // 
            // transitionBotton_
            // 
            this.transitionBotton_.Enabled = false;
            this.transitionBotton_.Location = new System.Drawing.Point(177, 202);
            this.transitionBotton_.Name = "transitionBotton_";
            this.transitionBotton_.Size = new System.Drawing.Size(139, 33);
            this.transitionBotton_.TabIndex = 9;
            this.transitionBotton_.Text = "開始";
            this.transitionBotton_.UseVisualStyleBackColor = true;
            this.transitionBotton_.Click += new System.EventHandler(this.activeBotton__Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 111);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 18);
            this.label3.TabIndex = 10;
            this.label3.Text = "状態：";
            // 
            // workstatus_
            // 
            this.workstatus_.AutoSize = true;
            this.workstatus_.ForeColor = System.Drawing.SystemColors.Highlight;
            this.workstatus_.Location = new System.Drawing.Point(70, 111);
            this.workstatus_.Name = "workstatus_";
            this.workstatus_.Size = new System.Drawing.Size(20, 18);
            this.workstatus_.TabIndex = 11;
            this.workstatus_.Text = "　";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.workEnd_);
            this.groupBox1.Controls.Add(this.workname_);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.workProgressVal_);
            this.groupBox1.Controls.Add(this.workstatus_);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.transitionBotton_);
            this.groupBox1.Controls.Add(this.finishBotton_);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.workProgressHandle_);
            this.groupBox1.Location = new System.Drawing.Point(12, 206);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(467, 249);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "作業状況：";
            // 
            // workEnd_
            // 
            this.workEnd_.AutoSize = true;
            this.workEnd_.Location = new System.Drawing.Point(70, 75);
            this.workEnd_.Name = "workEnd_";
            this.workEnd_.Size = new System.Drawing.Size(20, 18);
            this.workEnd_.TabIndex = 14;
            this.workEnd_.Text = "　";
            // 
            // workname_
            // 
            this.workname_.AutoSize = true;
            this.workname_.Location = new System.Drawing.Point(70, 41);
            this.workname_.Name = "workname_";
            this.workname_.Size = new System.Drawing.Size(20, 18);
            this.workname_.TabIndex = 13;
            this.workname_.Text = "　";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 41);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 18);
            this.label5.TabIndex = 12;
            this.label5.Text = "名前：";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.textBox1);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Location = new System.Drawing.Point(12, 461);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(467, 82);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "新規タスク";
            // 
            // taskContextMenu_
            // 
            this.taskContextMenu_.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.taskContextMenu_.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addChild_ToolStripMenuItem,
            this.toolStripSeparator1,
            this.この上に作業を挿入ToolStripMenuItem,
            this.この下に作業を挿入ToolStripMenuItem,
            this.toolStripSeparator2,
            this.RemoveTask_ToolStripMenuItem});
            this.taskContextMenu_.Name = "contextMenuStrip1";
            this.taskContextMenu_.Size = new System.Drawing.Size(243, 136);
            // 
            // addChild_ToolStripMenuItem
            // 
            this.addChild_ToolStripMenuItem.Name = "addChild_ToolStripMenuItem";
            this.addChild_ToolStripMenuItem.Size = new System.Drawing.Size(242, 30);
            this.addChild_ToolStripMenuItem.Text = "子作業を追加";
            this.addChild_ToolStripMenuItem.Click += new System.EventHandler(this.addChild_ToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(239, 6);
            // 
            // この上に作業を挿入ToolStripMenuItem
            // 
            this.この上に作業を挿入ToolStripMenuItem.Name = "この上に作業を挿入ToolStripMenuItem";
            this.この上に作業を挿入ToolStripMenuItem.Size = new System.Drawing.Size(242, 30);
            this.この上に作業を挿入ToolStripMenuItem.Text = "この上に作業を挿入";
            // 
            // この下に作業を挿入ToolStripMenuItem
            // 
            this.この下に作業を挿入ToolStripMenuItem.Name = "この下に作業を挿入ToolStripMenuItem";
            this.この下に作業を挿入ToolStripMenuItem.Size = new System.Drawing.Size(242, 30);
            this.この下に作業を挿入ToolStripMenuItem.Text = "この下に作業を挿入";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(239, 6);
            // 
            // RemoveTask_ToolStripMenuItem
            // 
            this.RemoveTask_ToolStripMenuItem.Name = "RemoveTask_ToolStripMenuItem";
            this.RemoveTask_ToolStripMenuItem.Size = new System.Drawing.Size(242, 30);
            this.RemoveTask_ToolStripMenuItem.Text = "削除";
            this.RemoveTask_ToolStripMenuItem.Click += new System.EventHandler(this.EraseTask_ToolStripMenuItem_Click);
            // 
            // MySelfManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(488, 553);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.taskTreeView_);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximumSize = new System.Drawing.Size(510, 2000);
            this.MinimumSize = new System.Drawing.Size(510, 480);
            this.Name = "MySelfManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "MySelfManager";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MySelfManager_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.workProgressHandle_)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.taskContextMenu_.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TreeView taskTreeView_;
        private System.Windows.Forms.Label workProgressVal_;
        private System.Windows.Forms.TrackBar workProgressHandle_;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button finishBotton_;
        private System.Windows.Forms.Button transitionBotton_;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label workstatus_;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label workname_;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label workEnd_;
        private System.Windows.Forms.ContextMenuStrip taskContextMenu_;
        private System.Windows.Forms.ToolStripMenuItem addChild_ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem この上に作業を挿入ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem この下に作業を挿入ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem RemoveTask_ToolStripMenuItem;
    }
}


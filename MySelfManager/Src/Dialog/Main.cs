using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyUtility;

namespace MySelfManager
{

    public partial class MySelfManager : Form
    {
        public MySelfManager()
        {
            InitializeComponent();
            taskTreeView_.PathSeparator = "/";

            // 追加のウィンドウ設定
            Size = Properties.Settings.Default.windowSize;

            // 前回のロード
            LoadTaskHistory();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddTask();
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                AddTask();
                e.Handled = true;
            }
        }
        private void AddTask()
        {
            string taskname = textBox1.Text;
            if (taskname.Count() == 0)
            {
                MessageBox.Show("タスク名が入力されていません", "注意", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!Utility.Xml.IsValidXmlName(taskname))
            {
                MessageBox.Show("無効な文字が含まれています：\n", "注意");
                return;
            }
            textBox1.Text = "";

            // 登録とビューの更新
            TaskInfoManager.Entry(taskname, taskname);
            TreeViewUpdate();

            // ビューの選択を変える
            TreeNode[] nodes = taskTreeView_.Nodes.Find(taskname + taskTreeView_.PathSeparator, false);

            if (nodes.Count() > 0)
                taskTreeView_.SelectedNode = nodes[0];
        }

        private void FormAnimationEvent(double val)
        {
            // 自身のウィンドウサイズの再計算
            Invoke((Action)(()=> 
            {
                Height = (int)val;
            }));
        }

        // パーセント表示がダブルクリックされる
        private void workProgressVal__DoubleClick(object sender, EventArgs e)
        {
            if (workProgressHandle_.Enabled)
            {
                workProgressHandle_.Value = workProgressHandle_.Maximum;
            }
        }

        // 値が変わったとき
        private void workProgressHandle__ValueChanged(object sender, EventArgs e)
        {
            ProgressUpdate();
        }

        // タスク/作業が選択されたとき
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            InfomationWindowUpdate();
        }

        public static string GetTransitionBottonText(TaskStatus stat)
        {
            switch (stat)
            {
                case TaskStatus.still: return "開始";
                case TaskStatus.active: return "サスペンド";
                case TaskStatus.suspend: return "再開";
                case TaskStatus.completion: return "サスペンド";
            }
            return "";
        }
        private void FullUpdate()
        {
            TreeViewUpdate();
            InfomationWindowUpdate();
        }

        private void InfomationWindowUpdate()
        {
            // タスク情報の取得
            var taskinfo = FindSelectTaskInfo();
            if (taskinfo == null) return;

            workname_.Text = taskinfo.Name;
            workstatus_.Text = taskinfo.StatusText;
            workstatus_.ForeColor = taskinfo.StatusColor;

            // ボタンの変化
            transitionBotton_.Text = GetTransitionBottonText(taskinfo.State);
            transitionBotton_.Enabled = taskinfo.State != TaskStatus.completion;

            // スライダの変更
            workProgressHandle_.Value = taskinfo.Percent * workProgressHandle_.Maximum / 100;
            workProgressHandle_.Enabled = taskinfo.State == TaskStatus.active;
            ProgressUpdate();
        }

        private void ProgressUpdate()
        {
            var progress = 100 * workProgressHandle_.Value / workProgressHandle_.Maximum;

            // テキストの変更
            workProgressVal_.Text = progress + "％";

            // タスク情報の更新
            var taskinfo = FindSelectTaskInfo();
            if (taskinfo == null) return;

            taskinfo.Percent = progress;

            // 100％で「完了」ボタンの有効化
            finishBotton_.Enabled = (taskinfo.State == TaskStatus.active) &&(progress == 100);
        }

        // ツリービューの更新
        private void TreeViewUpdate()
        {
            var selectedPath = taskTreeView_.SelectedNode?.FullPath;
            TreeNodeCollection backup = taskTreeView_.Nodes;
            taskTreeView_.Nodes.Clear();

            TaskInfoManager.ForEach(
            (path, info) =>
            {
                // ノードの追加
                var node = Utility.Forms.PopulateTreeView(taskTreeView_, path);
                if (node == null) return;

                // テキストなどの状態変更
                node.ForeColor = info.StatusColor;
            });

            // 状態の維持 (ツリーが完成してからもう一度ループ)
            TaskInfoManager.ForEach(
           (path, info) =>
           {
               var nodes = taskTreeView_.Nodes.Find(path + taskTreeView_.PathSeparator, true);
               if (nodes.Count() == 0) return;

               // 要素の展開を制御
               if (info.IsExpanded)
                   nodes[0].Expand();
               else
                   nodes[0].Collapse();
           });

            // ビュー更新前の選択を維持する
            if (selectedPath != null)
            {
                TreeNode[] nodes = taskTreeView_.Nodes.Find(selectedPath + taskTreeView_.PathSeparator, true);
                taskTreeView_.SelectedNode = (nodes.Count() > 0) ? nodes[0] : null;
            }
        }
        // ツリービューの装飾のみ更新
        private void TreeViewDecorationUpdate(TreeNodeCollection parentnodes)
        {
            if (parentnodes == null) return;

            foreach (TreeNode node in parentnodes)
            {
                TreeViewDecorationUpdate(node.Nodes);

                // 一致する要素を取得
                var info = TaskInfoManager.Find(node.FullPath);
                if (info != null)
                {
                    node.ForeColor = info.StatusColor;
                }
            }
        }

        private TaskInfo FindSelectTaskInfo()
        {
            if (taskTreeView_.SelectedNode == null)
            {
                return null;
            }
            return TaskInfoManager.Find(taskTreeView_.SelectedNode.FullPath);
        }

        // ステータス変化ボタンクリック
        private void activeBotton__Click(object sender, EventArgs e)
        {
            var taskinfo = FindSelectTaskInfo();
            if (taskinfo == null) return;

            // 状態を遷移させる
            switch (taskinfo.State)
            {
                case TaskStatus.still: taskinfo.State = TaskStatus.active;   break;
                case TaskStatus.active: taskinfo.State = TaskStatus.suspend; break;
                case TaskStatus.suspend: taskinfo.State = TaskStatus.active; break;
            }
            FullUpdate();
        }

        private void finishBotton__Click(object sender, EventArgs e)
        {
            var taskinfo = FindSelectTaskInfo();
            if (taskinfo == null) return;

            // 状態を遷移させる
            taskinfo.State = TaskStatus.completion;
            FullUpdate();

            // 一つ下の要素があれば進行中にする
            // todo 別関数化
            var fullpath = taskTreeView_.SelectedNode.FullPath;
            var nodegroup = taskTreeView_.Nodes.Find(fullpath + taskTreeView_.PathSeparator, true);
            if (nodegroup.Count() > 0 && nodegroup[0].NextNode != null && nodegroup[0].Parent != null)
            {
                var nextNode = nodegroup[0].NextNode;
                var nextInfo = TaskInfoManager.Find(nextNode.FullPath);

                if(nextInfo.State == TaskStatus.still)
                    nextInfo.State = TaskStatus.active;

                TreeViewDecorationUpdate(nextNode.Parent.Nodes);
                taskTreeView_.SelectedNode = nextNode;
            }
        }

        private void taskTreeView__NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // 右クリックされたとき
            if (e.Button == MouseButtons.Right)
            {
                taskTreeView_.SelectedNode = e.Node;
                taskContextMenu_.Show(taskTreeView_.PointToScreen(new Point(e.X, e.Y)));
                return;
            }
        }
        // 子を追加
        private void addChild_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UniDialog<WorkDescriber>.ShowDialog(this, 
            (result)=> 
            {
                if (result != DialogResult.OK) return;

                // アクティブになっている要素を取得
                var node = taskTreeView_.SelectedNode;
                if (node == null) return;

                // アクティブ中のタスクの配下に追加
                TaskInfoManager.EntryRange(node.FullPath, WorkDescriber.ResultStrings);

                // ツリービュー更新
                TreeViewUpdate();
            });
        }

        // ロード
        private void LoadTaskHistory()
        {
            TaskInfoManager.Load(Properties.Settings.Default.serializedTasks);

            if (Properties.Settings.Default.lastDay.Day != DateTime.Today.Day)
            {
                TaskInfoManager.OutputHistory(DateTime.Today.AddSeconds(-1));
                Properties.Settings.Default.lastDay = DateTime.Today;
                Properties.Settings.Default.Save();
            }
 
           FullUpdate();
        }
        // セーブ
        private void SaveTaskHistory()
        {
            Properties.Settings.Default.serializedTasks = TaskInfoManager.Serialize();
            Properties.Settings.Default.windowSize = Size;
            Properties.Settings.Default.Save();
        }

        private void MySelfManager_FormClosed(object sender, FormClosedEventArgs e)
        {
            SaveTaskHistory();

            // fixme: なんか例外がでるので対策。原因不明
            Deactivate -= MySelfManager_Deactivate;
        }

        private void EraseTask_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TaskInfoManager.Remove(taskTreeView_.SelectedNode?.FullPath);
            FullUpdate();
        }

        // 自身がアクティブになったとき
        private void MySelfManager_Activated(object sender, EventArgs e)
        {
            Opacity = 1.0;
        }
        // 自身が非アクティブになったとき
        private void MySelfManager_Deactivate(object sender, EventArgs e)
        {
            // todo 後々ツールの設定から編集できるようにする
            const double DeactivateOpacity = 0.35;
            Opacity = DeactivateOpacity;
        }

        // 名前の変更を開始する
        // https://msdn.microsoft.com/ja-jp/library/system.windows.forms.treenode.beginedit(v=vs.110).aspx
        private void Rename_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (taskTreeView_.SelectedNode == null) return;

            // 編集可能状態に設定
            taskTreeView_.LabelEdit = true;
            if (!taskTreeView_.SelectedNode.IsEditing)
            {
                taskTreeView_.SelectedNode.BeginEdit();
            }
        }
        // 名前の変更を終了する
        private void taskTreeView__AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            // hack: 助長な部分が多い、もう少し短くできるはず..
            if (e.Label == null)
            {
                taskTreeView_.LabelEdit = false;
                return;
            }
            if(e.Label.Length == 0)
            {
                e.CancelEdit = true;
                taskTreeView_.LabelEdit = false;
                return;
            }
            if (!Utility.Xml.IsValidXmlName(e.Label))
            {
                e.CancelEdit = true;
                MessageBox.Show("無効な文字が含まれています：\n ", "注意");
                e.Node.BeginEdit();
                return;

            }

            // 名前の変更を反映
            var info = TaskInfoManager.Find(e.Node.FullPath);
            if(info != null) info.Name = e.Label;

            // 正しく終了
            e.Node.EndEdit(false);
            taskTreeView_.LabelEdit = false;
        }

        private void taskTreeView__AfterCollapse(object sender, TreeViewEventArgs e)
        {
            var info = TaskInfoManager.Find(e.Node?.FullPath);
            if(info != null) info.IsExpanded = false;          
        }

        private void taskTreeView__AfterExpand(object sender, TreeViewEventArgs e)
        {
            var info = TaskInfoManager.Find(e.Node?.FullPath);
            if (info != null) info.IsExpanded = true;
        }

        // アイテムのドラッグを開始
        // http://dobon.net/vb/dotnet/control/tvdraganddrop.html
        private void taskTreeView__ItemDrag(object sender, ItemDragEventArgs e)
        {
            TreeView tv = (TreeView)sender;
            tv.SelectedNode = (TreeNode)e.Item;
            tv.Focus();
            //ノードのドラッグを開始する
            DragDropEffects dde =
                tv.DoDragDrop(e.Item, DragDropEffects.All);
            //移動した時は、ドラッグしたノードを削除する
            if ((dde & DragDropEffects.Move) == DragDropEffects.Move)
                tv.Nodes.Remove((TreeNode)e.Item);
        }

        private void taskTreeView__DragOver(object sender, DragEventArgs e)
        {
            //ドラッグされているデータがTreeNodeか調べる
            if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                if ((e.KeyState & 8) == 8 &&
                    (e.AllowedEffect & DragDropEffects.Copy) ==
                    DragDropEffects.Copy)
                    //Ctrlキーが押されていればCopy
                    //"8"はCtrlキーを表す
                    e.Effect = DragDropEffects.Copy;
                else if ((e.AllowedEffect & DragDropEffects.Move) ==
                    DragDropEffects.Move)
                    //何も押されていなければMove
                    e.Effect = DragDropEffects.Move;
                else
                    e.Effect = DragDropEffects.None;
            }
            else
                //TreeNodeでなければ受け入れない
                e.Effect = DragDropEffects.None;

            //マウス下のNodeを選択する
            if (e.Effect != DragDropEffects.None)
            {
                TreeView tv = (TreeView)sender;
                //マウスのあるNodeを取得する
                TreeNode target =
                    tv.GetNodeAt(tv.PointToClient(new Point(e.X, e.Y)));
                //ドラッグされているNodeを取得する
                TreeNode source =
                    (TreeNode)e.Data.GetData(typeof(TreeNode));
                //マウス下のNodeがドロップ先として適切か調べる
                if (target != null && target != source &&
                        !Utility.Forms.IsChildNode(source, target))
                {
                    //Nodeを選択する
                    if (target.IsSelected == false)
                        tv.SelectedNode = target;
                }
                else
                    e.Effect = DragDropEffects.None;
            }
        }
        private void taskTreeView__DragDrop(object sender, DragEventArgs e)
        {
            //ドロップされたデータがTreeNodeか調べる
            if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                TreeView tv = (TreeView)sender;
                //ドロップされたデータ(TreeNode)を取得
                TreeNode source =
                    (TreeNode)e.Data.GetData(typeof(TreeNode));
                //ドロップ先のTreeNodeを取得する
                TreeNode target =
                    tv.GetNodeAt(tv.PointToClient(new Point(e.X, e.Y)));
                //マウス下のNodeがドロップ先として適切か調べる
                if (target != null && target != source &&
                    !Utility.Forms.IsChildNode(source, target))
                {
                    //ドロップされたNodeのコピーを作成
                    TreeNode cln = (TreeNode)source.Clone();
                    //Nodeを追加
                    target.Nodes.Add(cln);
                    //ドロップ先のNodeを展開
                    target.Expand();
                    //追加されたNodeを選択
                    tv.SelectedNode = cln;

                    // 要素の移動
                    TaskInfoManager.Move(source.FullPath, cln.FullPath);

                }
                else
                    e.Effect = DragDropEffects.None;
            }
            else
                e.Effect = DragDropEffects.None;
        }
    }
}

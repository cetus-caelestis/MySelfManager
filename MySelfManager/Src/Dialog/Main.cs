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
        private EaseAnimator m_formAnimator = new EaseAnimator(Ease.InOut2);

        public MySelfManager()
        {
            InitializeComponent();
            m_formAnimator.TimerEvent += FormAnimationEvent;
            taskTreeView_.PathSeparator = "/";
            this.TopMost = true;

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
            taskTreeView_.Nodes.Clear();

            TaskInfoManager.ForEach(
            (path, info) =>
            {
                // ノードの追加
                var node = PopulateTreeView(taskTreeView_, path);

                // テキストなどの状態変更
                if (node != null)
                {
                    node.ForeColor = info.StatusColor;
                }
            });

            // ビュー更新前の選択を維持する
            if (selectedPath != null)
            {
                TreeNode[] nodes = taskTreeView_.Nodes.Find(selectedPath + taskTreeView_.PathSeparator, true);
                taskTreeView_.SelectedNode = (nodes.Count() > 0) ? nodes[0] : null;
            }
            // 折りたたまれた要素の全展開
            taskTreeView_.ExpandAll();
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
                string[] names = WorkDescriber.ResultString.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                TaskInfoManager.EntryRange(node.FullPath, names);

                // ツリービュー更新
                TreeViewUpdate();
            });
        }

        //http://stackoverflow.com/questions/1155977/populate-treeview-from-a-list-of-path
        private static TreeNode PopulateTreeView(TreeView treeView, string path)
        {
            TreeNode lastNode = null;
            string subPathAgg = string.Empty;

            foreach (string subPath in path.Split(treeView.PathSeparator.ToCharArray()))
            {
                subPathAgg += subPath + treeView.PathSeparator;
                TreeNode[] nodes = treeView.Nodes.Find(subPathAgg, true);
                if (nodes.Length == 0)
                {
                    if (lastNode == null)
                        lastNode = treeView.Nodes.Add(subPathAgg, subPath);
                    else
                        lastNode = lastNode.Nodes.Add(subPathAgg, subPath);
                }
                else
                {
                    lastNode = nodes[0];
                }
            }
            return lastNode;
        }

        // ロード
        private void LoadTaskHistory()
        {
            TaskInfoManager.Load(Settings.Default.serializedTasks);
            FullUpdate();
        }
        // セーブ
        private void SaveTaskHistory()
        {
            Settings.Default.serializedTasks = TaskInfoManager.Serialize();
            Settings.Default.Save();
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
            const double DeactivateOpacity = 0.20;
            Opacity = DeactivateOpacity;
        }
    }
}

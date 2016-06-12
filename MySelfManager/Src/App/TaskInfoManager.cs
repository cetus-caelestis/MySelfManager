using System;
using System.Collections.Generic;
using System.Linq;
using MyUtility;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Text;

namespace MySelfManager
{
    public class TaskInfoManager : Singleton<TaskInfoManager>
    {
        private XElement m_taskTree = new XElement("root");

        // todo impl関数の実装をstatic関数側に統合して排除
        static public void Load(string serializedStr) { Get().LoadImpl(serializedStr); }

        static public string Serialize() { return Get().SerializeImpl(); }

        static public void Entry(string path, string name) { Get().EntryImpl(path, name); }

        static public void EntryRange(string parentPathkey, string[] names) { Get().EntryRangeImpl(parentPathkey, names); }

        static public void Remove(string path) { Get().RemoveImpl(path); }

        static public void ForEach(Action<string, TaskInfo> func) { Get().ForEachImpl(func); }

        static public TaskInfo Find(string key) { return Get().FindImpl(key); }

        // 要素の移動
        static public void Move(string beforePath, string aftorPath)
        {
            Utility.Xml.Move(Get().m_taskTree, ToXPath(beforePath), ToXPath(aftorPath));
        }
        // ロード
        public bool LoadImpl(string serializedStr)
        {
            if (serializedStr == "") return true;

            // フォーマットの一致判定(最低限)
            var loadElem = XElement.Parse(serializedStr);
            if (loadElem != null && loadElem.Name.LocalName == "root")
            {
                m_taskTree = loadElem;
            }
            return true;
        }

        // 古い履歴の書き出しと削除
        // hack 助長な気がする
        static public void OutputHistory(DateTime until)
        {
            // 終了済みのタスクを抜き出す
            var cloneTree = XElement.Parse(Get().m_taskTree.ToString());
            Utility.Xml.ForEach(Get().m_taskTree.Elements(), (path, elem) =>
            {
                // 同じノードをクローンから探す
                var cloneElem = cloneTree.XPathSelectElement(path);
                if (cloneElem == null) return;

                // まだ終わっていないタスクは排除する
                var notcompletion = Utility.Xml.AllOf(cloneElem, (p, e) =>
                {
                    var info = new TaskInfo(e);

                    // 排除条件
                    if (info.End > until) return true;
                    if (info.State != TaskStatus.completion) return true;

                    return false;
                });
                if (notcompletion)
                    cloneElem.Remove();
            });
            // 残った終了済み要素を書き出す
            TaskInfoWriter.Write("History" + until.ToString("yyyyMMdd") + ".txt", cloneTree.Elements(), until);

            // 終了済みのタスクを削除する
            Utility.Xml.ForEach(cloneTree.Elements(), (clonePath, cloneElem) =>
            {
                // 同じノードをオリジナルから探す
                var elem = Get().m_taskTree.XPathSelectElement(clonePath);
                if (elem == null) return;

                var iscompletion = Utility.Xml.AllOf(cloneElem, (p, e) =>
                {
                    var info = new TaskInfo(e);
                    if (info.End > until) return false;
                    if (info.State != TaskStatus.completion) return false;

                    return true;
                });
                if (iscompletion) elem.Remove();
            });
        }

        // https://msdn.microsoft.com/ja-jp/library/system.xml.linq.xelement.writeto(v=vs.110).aspx
        public string SerializeImpl()
        {
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings xws = new XmlWriterSettings();

            using (XmlWriter xw = XmlWriter.Create(sb, xws))
            {
                m_taskTree.Save(xw);
            }
            return sb.ToString();
        }

        // 要素の追加
        private void EntryImpl(string path, string name)
        {
            var elem = m_taskTree.XPathSelectElement(ToXPath(path));

            if (elem == null)
                elem = Utility.Xml.InsertNode(m_taskTree, ToXPath(path));

            var info = TaskInfo.Apply(ref elem);
            info.Name = name;
        }

        // 要素の範囲追加
        private void EntryRangeImpl(string parentPathkey, string[] names)
        {
            foreach (var n in names)
            {
                if (n.Count() > 0)
                    EntryImpl(parentPathkey + "/" + n, n);
            }
        }

        // 要素の削除
        private void RemoveImpl(string path)
        {
            var node = m_taskTree.XPathSelectElement(ToXPath(path));
            node?.Remove();
        }

        // 要素を探索
        private TaskInfo FindImpl(string path)
        {
            if (path.Length == 0) { return null; }
            return new TaskInfo(m_taskTree.XPathSelectElement(ToXPath(path)));
        }

        // 列挙
        private void ForEachImpl(Action<string, TaskInfo> func)
        {
            Utility.Xml.ForEach(m_taskTree.Elements(), (path, elem) =>
             {
                 func(path, new TaskInfo(elem));
             });

        }

        static private string ToXPath(string str)
        {
            str.Replace(@"\", @"/");
            return str;
        }
    }
}

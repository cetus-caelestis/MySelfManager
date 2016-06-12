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
            var oldElem = Get().m_taskTree.XPathSelectElement(ToXPath(beforePath));
            if (oldElem == null) return;

            oldElem.Remove();
            Get().InsertNode(Get().m_taskTree, ToXPath(aftorPath), oldElem);
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
                elem = InsertNode(m_taskTree, ToXPath(path));

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

        // 事前にXPathSelectElementでチェックを行うこと
        private XElement InsertNode(XElement parent, string xPath, XElement newElem = null)
        {
            // パース
            string[] paths = xPath.Split("/".ToCharArray(), 2);

            // 要素を追加
            var elem = parent.Element(paths[0]);
            if (elem == null)
            {
                if (newElem != null && paths.Count() == 1 && paths[0] == newElem.Name.LocalName)
                    parent.Add(XElement.Parse(newElem.ToString()));
                else
                    parent.Add(new XElement(paths[0]) );

                elem = parent.Element(paths[0]);
            }
            // ここが最後
            if (paths.Count() == 1)
            {
                return elem;
            }
            // 再起
            return InsertNode(elem, paths[1], newElem);
        }
        // 列挙
        // todo: 言語使用のForEachと利用方法を差し替えるか考え中..
        private void ForEachImpl(Action<string, TaskInfo> func)
        {
            ForEachImpl(m_taskTree.Elements(), "", func);
        }
        private void ForEachImpl(IEnumerable<XElement> elements, string path, Action<string, TaskInfo> func)
        {
            foreach (var elem in elements)
            {
                string currentPath = path + elem.Name;
                func(currentPath, new TaskInfo(elem));

                // 子に対して再起
                ForEachImpl(elem.Elements(), currentPath + "/", func);
            }
        }

        static private string ToXPath(string str)
        {
            str.Replace(@"\", @"/");
            return str;
        }
    }
}

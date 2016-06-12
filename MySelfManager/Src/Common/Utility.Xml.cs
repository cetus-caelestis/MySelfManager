using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace MyUtility
{
    public partial class Utility
    {
        public class Xml
        {
            static public bool IsValidXmlName(string text)
            {
                try
                {
                    XmlConvert.VerifyName(text);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            static public void ForEach(IEnumerable<XElement> elements, Action<string, XElement> func)
            {
                ForEach(elements, "", "/", func);
            }

            static public void ForEach(IEnumerable<XElement> elements, string path, string separator, Action<string, XElement> func)
            {
                foreach (var elem in elements)
                {
                    string currentPath = path + elem.Name;
                    func(currentPath, elem);

                    // 子に対して再起
                    ForEach(elem.Elements(), currentPath + separator, separator, func);
                }
            }
            static public bool AllOf(XElement element, Func<string, XElement, bool> func)
            {
                if (func("", element) == false) return false;
                return AllOf(element.Elements(), func);
            }
            static public bool AllOf(IEnumerable<XElement> elements, Func<string, XElement, bool> func)
            {
                bool rslt = true;
                ForEach(elements, (path, elem) =>
                {
                    if (func(path, elem) == false) rslt = false;
                });
                return rslt;
            }

            static public XElement InsertNode(XElement parent, string xPath, XElement newElem = null)
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
                        parent.Add(new XElement(paths[0]));

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

            static public void Move(XElement beforetree, string beforePath, string aftorPath)
            {
                Move(beforetree, beforePath, beforetree, aftorPath);
            }
            static public void Move(XElement beforetree, string beforePath, XElement aftertree, string aftorPath)
            {
                var oldElem = beforetree.XPathSelectElement(beforePath);
                if (oldElem == null) return;

                oldElem.Remove();
                InsertNode(aftertree, aftorPath, oldElem);
            }


        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MyUtility;

namespace MySelfManager
{
    class TaskInfoWriter
    {
        static private string titleformarts =
            "■□----------------------------------------\r\n□■ 進捗状況 {0}\r\n■□----------------------------------------\r\n\r\n";


        static private string[] formarts =
        {
            "\r\n■□ {0}\r\n□■----------------------------------------\r\n",
            "\r\n■{0,-30}{1}\r\n",
            "　・{0,-28}{1}\r\n",
            "　　・{0,-26}{1}\r\n",
        };

        static public void Write(string filepath, IEnumerable<XElement> elements, DateTime recodetime)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendFormat(titleformarts, recodetime.ToShortDateString());

            Utility.Xml.ForEach(elements, (path, node) =>
            {
                if (node == null) return;
                var info = new TaskInfo(node);

                // 時間表示 (下に子供がいない場合のみ)
                string timestr = "";
                if(info.Start != DateTime.MinValue && !node.HasElements)
                {
                    timestr =  info.Start.ToString("MM/dd H:mm");
                    timestr += " - ";

                    if (info.End != DateTime.MinValue)
                    {
                        if (info.Start.Day == info.End.Day)
                            timestr += info.End.ToString("H:mm");
                        else
                            timestr += info.End.ToString("MM/dd H:mm");
                    }
                }

                // 階層によって装飾を変える
                int nest = path.Length - path.Replace("/", "").Length;
                builder.AppendFormat(formarts[nest], info.Name, timestr);
            });

            // 出力
            Encoding sjisEnc = Encoding.GetEncoding("Shift_JIS");
            System.IO.StreamWriter writer =
              new System.IO.StreamWriter(filepath, true, sjisEnc);
            writer.Write(builder);
            writer.Close();
        }
    }
}

using System;
using System.Drawing;
using System.Xml.Linq;

namespace MySelfManager
{
    public enum TaskStatus
    {
        still,
        active,
        suspend,
        completion,
    }

    // todo: XMLのシリアライズが利用できるはず...
    // todo: 理解に時間がかかるため今回は保留とする
    public class TaskInfo
    {
        // 名前
        private string   m_name;

        // 進捗
        private int      m_percent = 0;

        // 開始と終了
        private DateTime m_start = new DateTime();
        private DateTime m_end = new DateTime();

        // 状態
        private TaskStatus m_state = TaskStatus.still;

        // 展開されているか (ツールで必要な情報)
        private bool m_isExpanded = true;

        // アタッチされているelem
        private XElement m_elem = null;

        public string Name
        {
            get { return m_name; }
            set
            {
                m_name = value;
                m_elem?.SetAttributeValue("name", m_name);
            }
        }
        public int Percent
        {
            get { return m_percent; }
            set
            {
                m_percent = value;
                m_elem?.SetAttributeValue("percent", m_percent);
            }
        }
        public DateTime Start
        {
            get { return m_start; }
            private set
            {
                m_start = value;
                m_elem?.SetAttributeValue("start", m_start);
            }
        }
        public DateTime End
        {
            get { return m_end; }
            private set
            {
                m_end = value;
                m_elem?.SetAttributeValue("end", m_end);
            }
        }

        public TaskStatus State
        {
            get { return m_state; }
            set
            {
                m_state = value;

                // 状態の変化によって時間を設定
                if (value == TaskStatus.active)
                    this.Start = DateTime.Now;

                if (value == TaskStatus.completion)
                    this.End = DateTime.Now;

                m_elem?.SetAttributeValue("state", (int)m_state);
            }
        }
        public bool IsExpanded
        {
            get { return m_isExpanded; }
            set
            {
                m_isExpanded = value;
                m_elem?.SetAttributeValue("isExpanded", m_isExpanded);
            }
        }

        public string StatusText
        {
            get { return GetStatusText(m_state); }
        }
        public Color StatusColor
        {
            get { return GetStatusColor(m_state); }
        }

        public TaskInfo(string name)
        {
            m_name = name;
        }
        public TaskInfo(XElement elem)
        {
            m_elem = elem;
            if (m_elem != null)
            {
                m_name = elem.Attribute("name").Value;
                m_percent = int.Parse(elem.Attribute("percent").Value);
                m_state = (TaskStatus)int.Parse(elem.Attribute("state").Value);

                // これ以降は必須ではないパラメータ
                string valueTmp;

                valueTmp = elem.Attribute("start")?.Value;
                if (valueTmp != null) m_start = DateTime.Parse(valueTmp);

                valueTmp = elem.Attribute("end")?.Value;
                if (valueTmp != null) m_end = DateTime.Parse(valueTmp);

                valueTmp = elem.Attribute("isExpanded")?.Value;
                if(valueTmp != null) m_isExpanded = bool.Parse(valueTmp);
            }
        }
        public static string GetStatusText(TaskStatus stat)
        {
            switch (stat)
            {
                case TaskStatus.still:      return "未着手";
                case TaskStatus.active:     return "進行中";
                case TaskStatus.suspend:    return "サスペンド";
                case TaskStatus.completion: return "完了";
            }
            return "";
        }
        public static Color GetStatusColor(TaskStatus stat)
        {
            switch (stat)
            {
                case TaskStatus.still: return Color.DarkSlateGray;
                case TaskStatus.active: return Color.CornflowerBlue;
                case TaskStatus.suspend: return Color.IndianRed;
                case TaskStatus.completion: return Color.DarkGray;
            }
            return Color.Black;
        }

        public static TaskInfo Apply( ref XElement elem)
        {
            if (elem == null) return null;

            // デフォルト設定
            {
                var info = new TaskInfo("");
                elem.SetAttributeValue("name", info.m_name);
                elem.SetAttributeValue("percent", info.m_percent);
                elem.SetAttributeValue("start", info.m_start);
                elem.SetAttributeValue("end", info.m_end);
                elem.SetAttributeValue("state", (int)info.m_state);
                elem.SetAttributeValue("isExpanded", info.m_isExpanded);
            }
            return new TaskInfo(elem);
        }
    }
}

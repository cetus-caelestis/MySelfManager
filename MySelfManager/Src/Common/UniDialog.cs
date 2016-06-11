using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyUtility
{
    public class UniDialog<T>
        where T : Form, new()
    {
        private static T m_dialog;

        // モーダルダイアログ
        static public void ShowDialog(IWin32Window owner, Action<DialogResult> rsltCallback = null)
        {
            try
            {
                // すでに稼働中
                if (m_dialog != null)
                {
                    m_dialog.Invoke(new ToActiveDelegate(ActiveDialog), new object[] { m_dialog });
                    return;
                }

                m_dialog = new T();

                var rslt = m_dialog.ShowDialog(owner);

                if (rsltCallback != null)
                    rsltCallback(rslt);
            }
            finally
            {
                m_dialog = null;
            }
        }
        // モードレスダイアログ
        static public void Show(IWin32Window owner, Action<DialogResult> rsltCallback = null)
        {
            // すでに稼働中
            if (m_dialog != null)
            {
                m_dialog.Invoke(new ToActiveDelegate(ActiveDialog), new object[] { m_dialog });
                return;
            }

            // 新しくダイアログを作成
            m_dialog = new T();
            m_dialog.FormClosed += (obj, e) =>
            {
                if (rsltCallback != null)
                    rsltCallback(m_dialog.DialogResult);

                m_dialog = null;
            };
            m_dialog.Show(owner);
        }

        // ダイアログ取得
        static T Get()
        {
            return m_dialog;
        }

        delegate void ToActiveDelegate(Form form);
        static void ActiveDialog(Form form)
        {
            form.WindowState = FormWindowState.Normal;
            form.Activate();

            for (int i = 0; i < form.OwnedForms.Length; ++i)
            {
                form.OwnedForms[i].Invoke(new ToActiveDelegate(ActiveDialog), new object[] { form.OwnedForms[i] });
            }
        }

    }
}

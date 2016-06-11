using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUtility
{
    class Ease
    {
        public static double InOut2(double f)
        {
            Debug.Assert(f >= 0 && f <= 1.0);

            // 0 ～ 2にスケール
            f *= 2;

            if (f <= 1.0f)
            {
                // f値(0 ～ 1) で 0.5までイーズイン 
                return 0.5f * f * f * f;
            }

            // 0 ～ 1範囲に直す(元 0.5 ～ 1.0)
            f -= 1;
            f = 1 - f;

            // 0.5 ～ 1 までのイーズアウト
            return 1 - (0.5f * ((f * f * f)));
        }
    }
    public class EaseAnimator
    {

        private Func<double, double> m_easefunc;
        private System.Timers.Timer m_timer = new System.Timers.Timer();
        private double m_totalmsec;
        private double m_aftormsec;
        private double m_before;
        private double m_aftor;

        public event Action<double> TimerEvent;

        // コンストラクタ
        public EaseAnimator(Func<double, double> easefunc)
        {
            m_easefunc = easefunc;

            m_timer.Enabled = true;
            m_timer.AutoReset = true;
            m_timer.Elapsed += OnTimerEvent;
            m_timer.Stop();
        }

        // アニメーション開始
        public void Animation(double before, double aftor, double totalmsec, double intervalmsec = 10)
        {
            m_totalmsec = totalmsec;
            m_aftormsec = 0;
            m_timer.Interval = intervalmsec;
            m_before = before;
            m_aftor = aftor;

            // 「変化前の値」を通知
            TimerEvent(m_before);
            m_timer.Start();
        }

        // タイマーイベント
        public void OnTimerEvent(object source, EventArgs e)
        {
            m_aftormsec += m_timer.Interval;
            if (m_aftormsec >= m_totalmsec)
            {
                m_aftormsec = m_totalmsec;
                m_timer.Stop();     // ついでにタイマーも止めておく
            }

            double ease = m_easefunc(m_aftormsec / m_totalmsec);
            TimerEvent(m_before + (m_aftor - m_before)*ease);
        }
    }

}

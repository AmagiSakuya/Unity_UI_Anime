using Sakuya.UnityUIAnime.Define;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sakuya.UnityUIAnime
{
    public class CanvasGroupAlpha_UIAnime : CompUIAnime<CanvasGroup, CanvasGroupAlpha_UIAnime_Define>
    {
        Coroutine m_alphaAnime;
        float m_orignAlpha;
        protected virtual void DoFade(Action Callback = null)
        {
            if (animeDefine == null || m_alphaAnime != null || animeDefine.alphaAnimeQueue.Length == 0) return;
            m_orignAlpha = target.alpha;
            m_alphaAnime = StartCoroutine(IFloatAnime(animeDefine.alphaAnimeQueue, animeDefine.alphaAnimeLoop,
            (FloatAnimeSettings setting, float time) =>
            {
                target.alpha = CalcFloatValueByTime(setting, time);
            },
            () =>
            {
                m_alphaAnime = null;
                Callback?.Invoke();
            }));
        }

        protected virtual void KillFade()
        {
            if (m_alphaAnime == null) return;
            StopCoroutine(m_alphaAnime);
            m_alphaAnime = null;
            target.alpha = m_orignAlpha;
        }

        #region 动画
        public override void DoAnime()
        {
            DoFade();
        }
        public override void KillAnime()
        {
            KillFade();
        }
        #endregion

        #region 共通方法
        IEnumerator IFloatAnime(FloatAnimeSettings[] queue, bool isLoop, Action<FloatAnimeSettings, float> OnAnimateTimeChange, Action Callback = null)
        {
            float time;
            for (int i = 0; i < queue.Length; i++)
            {
                yield return new WaitForSeconds(queue[i].delay);
                time = 0;
                while (time < queue[i].duration)
                {
                    time += Time.deltaTime;
                    OnAnimateTimeChange(queue[i], time);
                    yield return null;
                }

                if (isLoop && i == queue.Length - 1)
                {
                    i = -1;
                }
            }
            Callback?.Invoke();
        }

        float CalcFloatValueByTime(FloatAnimeSettings setting, float time)
        {
            return Mathf.Lerp(setting.from, setting.to, setting.curve.Evaluate(time / setting.duration));
        }
        #endregion
    }
}

using Sakuya.UnityUIAnime.Define;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sakuya.UnityUIAnime
{
    public class RectTransform_UIAnime : CompUIAnime<RectTransform, RectTransform_UIAnime_Define>
    {
        #region 位移动画用变量
        Coroutine m_posAnime;
        Vector3 anchoredPositionOrigin;
        #endregion

        #region 旋转动画用变量
        Coroutine m_rotAnime;
        Vector3 anchoredRotOrigin;
        #endregion

        #region 缩放动画用变量
        Coroutine m_scaleAnime;
        Vector3 anchoredScaleOrigin;
        #endregion

        #region 位移动画
        protected virtual void DoMove(Action Callback = null)
        {
            if (animeDefine == null || m_posAnime != null || animeDefine.positionAnimeQueue.Length == 0) return;
            anchoredPositionOrigin = target.anchoredPosition;
            m_posAnime = StartCoroutine(IVector3Anime(animeDefine.positionAnimeQueue, animeDefine.posAnimeLoop,
            (Vector3AnimeSettings setting, float time) =>
            {
                target.anchoredPosition = CalcVactorValueByTime(setting, anchoredPositionOrigin, animeDefine.relativePos, time);
            },
            () =>
            {
                m_posAnime = null;
                Callback?.Invoke();
            }));
        }

        protected virtual void KillMove()
        {
            if (m_posAnime == null) return;
            StopCoroutine(m_posAnime);
            m_posAnime = null;
            target.anchoredPosition = anchoredPositionOrigin;
        }
        #endregion

        #region 旋转动画
        protected virtual void DoRotate(Action Callback = null)
        {
            if (animeDefine == null || m_rotAnime != null || animeDefine.rotationAnimeQueue.Length == 0) return;
            anchoredRotOrigin = target.localEulerAngles;
            m_rotAnime = StartCoroutine(IVector3Anime(animeDefine.rotationAnimeQueue, animeDefine.rotationAnimeLoop,
            (Vector3AnimeSettings setting, float time) =>
            {
                target.localEulerAngles = CalcVactorValueByTime(setting, anchoredRotOrigin, animeDefine.relativeRotation, time);
            },
            () =>
            {
                m_rotAnime = null;
                Callback?.Invoke();
            }));
        }

        protected virtual void KillRotate()
        {
            if (m_rotAnime == null) return;
            StopCoroutine(m_rotAnime);
            m_rotAnime = null;
            target.localEulerAngles = anchoredRotOrigin;
        }
        #endregion

        #region 缩放动画
        protected virtual void DoScale(Action Callback = null)
        {
            if (animeDefine == null || m_scaleAnime != null || animeDefine.scaleAnimeQueue.Length == 0) return;
            anchoredScaleOrigin = target.localScale;
            m_scaleAnime = StartCoroutine(IVector3Anime(animeDefine.scaleAnimeQueue, animeDefine.scaleAnimeLoop,
            (Vector3AnimeSettings setting, float time) =>
            {
                target.localScale = CalcVactorValueByTime(setting, anchoredScaleOrigin, animeDefine.relativeScale, time);
            },
            () =>
            {
                m_scaleAnime = null;
                Callback?.Invoke();
            }));
        }

        protected virtual void KillScale()
        {
            if (m_scaleAnime == null) return;
            StopCoroutine(m_scaleAnime);
            m_scaleAnime = null;
            target.localScale = anchoredScaleOrigin;
        }
        #endregion

        #region 动画
        public override void DoAnime()
        {
            DoMove();
            DoRotate();
            DoScale();
        }
        public override void KillAnime()
        {
            KillMove();
            KillRotate();
            KillScale();
        }
        #endregion

        #region 共通方法
        IEnumerator IVector3Anime(Vector3AnimeSettings[] queue, bool isLoop, Action<Vector3AnimeSettings, float> OnAnimateTimeChange, Action Callback = null)
        {
            float time;
            for (int i = 0; i < queue.Length; i++)
            {
                yield return new WaitForSeconds(queue[i].delay);
                time = 0;
                while (time < queue[i].duration)
                {
                    time += Time.deltaTime;
                    if(queue[i].timeDecimalPoint >= 0)
                    {
                        time = (float)Math.Round(time, queue[i].timeDecimalPoint);
                    }
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

        Vector3 CalcVactorValueByTime(Vector3AnimeSettings setting, Vector3 orgin, bool relative, float time)
        {
            Vector3 m_from = relative ? orgin + setting.from : setting.from;
            Vector3 m_to = relative ? orgin + setting.to : setting.to;
            Vector3 pos = Vector3.Lerp(m_from, m_to, setting.curve.Evaluate(time / setting.duration));
            return pos;
        }
        #endregion
    }
}
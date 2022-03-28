using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sakuya.UnityUIAnime
{
    public abstract class CompUIAnime<TComp, TSettings> : UIAnimeBase where TComp : Component where TSettings : ScriptableObject
    {
        [SerializeField] TComp m_target;
        [SerializeField] bool m_playOnEnable;
        [SerializeField] TSettings m_animeDefine;

        private void Awake()
        {
            KillAnime();
        }

        private void OnEnable()
        {
            if (!m_playOnEnable) return;
            DoAnime();
        }

        private void OnDisable()
        {
            KillAnime();
        }

        protected TComp target
        {
            get
            {
                if (m_target == null)
                {
                    m_target = GetComponent<TComp>();
                    if (m_target == null) { m_target = gameObject.AddComponent<TComp>(); }
                }
                return m_target;
            }
            set
            {
                m_target = value;
            }
        }

        protected TSettings animeDefine
        {
            get
            {
                return m_animeDefine;
            }
            set
            {
                m_animeDefine = value;
            }
        }
    }
}


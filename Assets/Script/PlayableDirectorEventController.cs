using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.Playables;
using UnityEngine.Events;

namespace Bm
{
	

	public class PlayableDirectorEventController : MonoBehaviour
	{
		[HideInInspector]
		public List<PlayableDirectorEventNode> events = new List<PlayableDirectorEventNode>();

		[EnumName("动画控制器:")]
		public PlayableDirector director;

		[EnumName("进度:")]
		public float percent;

		[EnumName("运行时间:")]
		public float time;

		private int status = 0;
		private void Awake()
		{
			time = 0;

		}

        private void OnEnable()
        {
			if (!director.playOnAwake)
			{
				director.played += OnPlay;
				director.paused += OnPause;
				director.stopped += OnEnd;
			}
			else
            {
				Clear();
				status = 1;
				director.stopped += OnEnd;
			}
		}

        private void OnDisable()
        {
			if (!director.playOnAwake)
			{
				director.played -= OnPlay;
				director.paused -= OnPause;
				director.stopped -= OnEnd;
			}
			else
			{
				status = 0;
				director.stopped -= OnEnd;
			}
		}

        void OnPlay(PlayableDirector director)
        {
			status = 1;
			Clear();
		}

		void OnPause(PlayableDirector director)
		{
			status = 0;
		}

		void OnEnd(PlayableDirector director)
		{
			status = 0;
			if (Application.isPlaying) ExecEvent(1);
		}

		private void Update()
        {
            switch(status)
            {
				case 1:
					_Update();
					break;
            }
        }

		public void _Update()
		{
			percent = (float)(director.time / director.duration);

			if (percent >= 1)
			{
				percent = 1;
				status = 0;
			}
			if(Application.isPlaying)ExecEvent(percent);
		}

		protected void ExecEvent(float _percent)
		{
			for (int i = 0; i < events.Count; i++)
			{
				if (_percent >= events[i].progress && !events[i].isDo)
				{
					events[i].isDo = true;
					events[i].mEvent.Invoke();
				}
			}
		}

		private void Clear()
        {
			for (int i = 0; i < events.Count; i++)
			{
				events[i].isDo = false;
			}
		}

		public void Reset()
        {
			director = GetComponent<PlayableDirector>();
        }


		public void AddEventByTime(string _name, float _time, UnityAction _event = null)
        {
			AddEventByPercent(_name, _time / (float)director.duration, _event);
		}

		public void AddEventByPercent(string _name, float _progress, UnityAction _event=null)
		{
			var t = new PlayableDirectorEventNode();
			t.name = _name;
			t.progress = _progress;
			if(_event!=null)
            {
				t.mEvent = new UnityEvent();
				t.mEvent.AddListener(_event);
			}
			events.Add(t);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Bm
{
	[System.Serializable]
	public class PlayableDirectorEventNode
	{
		[EnumName("名称")]
		public string name = "";

		[Range(0, 1)]
		public float progress = 0;

		[HideInInspector]
		public bool isDo = false;

		public UnityEvent mEvent;
	}
}

using UnityEngine;
using System.Collections.Generic;
using System;

namespace UnityEngine.Rendering.PostProcessing
{
	public class PostProcessProfile : ScriptableObject
	{
		public List<PostProcessEffectSettings> settings;

        internal T GetSetting<T>()
        {
            throw new NotImplementedException();
        }
    }
}

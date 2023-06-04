using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
	internal class SaveLoad
	{
		private const string TouchSensitivityKey = "t";

		public static float TouchSensitivity { get => PlayerPrefs.GetFloat(TouchSensitivityKey);
			set => PlayerPrefs.SetFloat(TouchSensitivityKey, value); }

	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
	public class EnemyBossData
	{
		public const int Damage = 20;
		public int Health = 100;
		public const float Speed = 1;
		public const float ReloadTime = 3f;
		public float TimeOfLastShot = 0;
		public const int PowerPerKill = 50;

	}
}
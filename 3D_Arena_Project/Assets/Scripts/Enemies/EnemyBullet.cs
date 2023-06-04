using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;

namespace Enemies
{
	class EnemyBullet : MonoBehaviour
	{
		[SerializeField]
		private Rigidbody _rb;
		private Transform _huntTarget;

		public Transform HuntTarget { get => _huntTarget; set => _huntTarget = value; }

		private void Update()
		{
			_rb.velocity =
				EnemyBulletData.Speed * (HuntTarget.position - transform.position).normalized;
		}
		private void OnCollisionEnter(Collision collision)
		{
			if(collision.gameObject.layer == Layers.Default)
			{
				Destroy(gameObject);
			}
			else
			{
				if(collision.gameObject.TryGetComponent(out Player player))
				{
					player.GetHit(EnemyBulletData.Damage);
				}
			}
			var possiblePlayer = Physics.OverlapSphere(transform.position, 1, Layers.Player);

		}
	}
}

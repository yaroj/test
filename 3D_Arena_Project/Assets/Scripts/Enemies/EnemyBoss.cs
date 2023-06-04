using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

namespace Enemies
{
	public class EnemyBoss : MonoBehaviour, IEnemy
	{
		//public Player player;
		private EnemyBossData _data;
		[SerializeField] private EnemyBullet _bullet;
		[SerializeField] private Rigidbody _rb;
		public Player Player { get; set; }

		public int PowerPerKill => EnemyBossData.PowerPerKill;

		private void Awake()
		{
			_data = new EnemyBossData();
		}


		private void Update()
		{
			if (Time.time - _data.TimeOfLastShot > EnemyBossData.ReloadTime)
			{
				_data.TimeOfLastShot = Time.time;
				Shoot();
			}
			_rb.velocity =
					EnemyBossData.Speed * (Player.transform.position - transform.position).normalized;
		}

		public void Shoot()
		{
			EnemyBullet b = Instantiate(_bullet, transform.position, Quaternion.identity);
			b.HuntTarget = Player.CurrentLure;
			Destroy(b.gameObject, 2);
		}

		public bool GetHit(int damage)
		{
			_data.Health -= damage;
			if (_data.Health <= 0)
			{
				Destroy(gameObject);
				gameObject.SetActive(false);
				return true;
			}
			return false;
		}

		void OnCollisionEnter(Collision collision)
		{
			if (collision.gameObject.layer == Layers.Player)
			{
				if (collision.gameObject.TryGetComponent(out Player player))
				{
					player.GetHit(EnemyData.Damage);
					Destroy(gameObject);
				}
			}
		}

	}
}
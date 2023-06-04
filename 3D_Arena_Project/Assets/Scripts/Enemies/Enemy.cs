using Assets.Scripts;
using System.Collections;
using UnityEngine;

namespace Enemies
{
	public class Enemy : MonoBehaviour,  IEnemy
	{
		[SerializeField]
		private Rigidbody _rb;

		private EnemyData _data;

		private bool hunting = false;

		public Player Player { get; set; }

		public int PowerPerKill => EnemyData.PowerPerKill;

		void Start()
		{
			_data = new EnemyData();
			StartCoroutine(Jump());
		}



		IEnumerator Jump()
		{
			_rb.useGravity = false;
			_rb.velocity = Vector3.up * EnemyData.JumpHeight;
			yield return new WaitForSeconds(1);
			StartCoroutine(WaitInAir());
		}

		IEnumerator WaitInAir()
		{
			_rb.velocity = Vector3.zero;
			yield return new WaitForSeconds(EnemyData.TimeInAir);
			hunting = true;
		}


		// Update is called once per frame
		void Update()
		{
			if (hunting)
				_rb.velocity =
					EnemyData.Speed * (Player.transform.position - transform.position).normalized;
		}

		void OnCollisionEnter(Collision collision)
		{
			if(collision.gameObject.layer == Layers.Player)
			{
				if (collision.gameObject.TryGetComponent(out Player player))
				{
					player.GetHit(EnemyData.Damage);
					Destroy(gameObject);
				}
			}
		}

		public bool GetHit(int damage)
		{
			_data.Health -= damage;
			print($"got hit {damage}  current health {_data.Health}");
			if (_data.Health <= 0)
			{
				Destroy(gameObject);
				gameObject.SetActive(false);
				return true;
			}
			return false;
		}

	}
}
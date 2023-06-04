using Assets.Scripts;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CatchCo;
using System;
using Enemies;
using TMPro;

public class Player : MonoBehaviour
{
	public PlayerData data;
	public Transform LurePrefab;
	public Transform CurrentLure = null;

	[SerializeField] private Rigidbody _rb;
	[SerializeField] private Bullet _bullet;
	[SerializeField] private Slider _healthBar;
	[SerializeField] private Slider _powerBar;
	[SerializeField] private Button _ultaButton;
	[SerializeField] private TMP_Text _score;
	
	private static Player _instance;
	
	public static Player Instance { get => _instance; set => _instance = value; }

	void Awake()
	{
		Instance = this;
		data = new PlayerData();
		CreateLure();
		UpdateUI();
		transform.rotation = Quaternion.identity;
	}

	public void OnEnemyKill(int powerBonus = 10)
	{
		data.Score++;
		data.Power += powerBonus;
		data.Power = Math.Min(data.Power, PlayerData.MaxPower);
		UpdateUI();
	}

	private void UpdateUI()
	{
		_healthBar.value = data.Health;
		_powerBar.value = data.Power;
		_ultaButton.interactable = data.Power == PlayerData.MaxPower;
		_score.text = $"enemies killed {data.Score}";
	}


	public void TryShoot()
	{
		TryShoot(transform.forward);
	}

	public void TryShoot(Vector3 direction)
	{
		print(data.TimeOfLastShot);

		if (Time.time - data.TimeOfLastShot < PlayerData.ReloadTime)
		{
			return;
		}
		data.TimeOfLastShot = Time.time;
		var b = Instantiate(_bullet, transform.position, Quaternion.identity);
		b.Rb.velocity = (direction.normalized) * BulletData.Speed;
		b.Player = this;
		Destroy(b.gameObject, 2);
	}

	public void UltimateSkill()
	{
		data.Power = 0;
		UpdateUI();
		EnemySpawner.Instance.KillAllEnemies();
	}

	public void Move(Vector3 direction)
	{
		var velocity = transform.TransformDirection(direction);
		velocity.y = -0.02f;
		velocity.Normalize();
		velocity *= PlayerData.MovementSpeed;
		_rb.velocity = velocity;
	}


	public void Rotate(Vector2 direction)
	{
		transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(direction.y, direction.x, 0));
	}

	void CreateLure()
	{
		if ((CurrentLure != null))
			Destroy(CurrentLure.gameObject, 10);
		CurrentLure = Instantiate(LurePrefab, transform);
		CurrentLure.localPosition = Vector3.zero;
	}

	public void PrepareToTeleport()
	{
		CreateLure();
		_rb.velocity = Vector3.zero;
	}

	public void GetHit(int damage, int powerstolen = 5)
	{
		data.Health -= damage;
		data.Power -= powerstolen;
		print(data.Health + "  " + data.Power);
		UpdateUI();
		if(data.Health <= 0)
		{
			GameManager.Inst.OnDeath();
		}
	}
}

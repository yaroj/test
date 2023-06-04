using Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	[SerializeField] private GameObject _deathMenu;
	[SerializeField] private Button _restartButton;
	[SerializeField] private Button _pauseButton;
	[SerializeField] private Sprite pauseSprite;
	[SerializeField] private Sprite playSprite;
	[SerializeField] private GameObject pauseMenu;


	private static GameManager _inst;
	private bool paused = false;


	public static GameManager Inst { get => _inst; set => _inst = value; }


	private void Start()
	{
		Inst = this;
		_restartButton.onClick.AddListener(Restart);
		_pauseButton.onClick.AddListener(TogglePause);
		TogglePause(false);
	}

	public void OnDeath()
	{
		_deathMenu.SetActive(true);
		EnemySpawner.Instance.gameObject.SetActive(false);
		TogglePause(true);
		_pauseButton.gameObject.SetActive(false);
	}

	public void Restart()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);

	}

	public void TogglePause() 
	{
		TogglePause(!paused);
	}

	public void TogglePause(bool newValue)
	{
		paused = newValue;
		if(paused)
		{
			Time.timeScale = 0;
			_pauseButton.image.sprite = playSprite;
			pauseMenu.SetActive(true);
		}
		else
		{
			Time.timeScale = 1;
			_pauseButton.image.sprite = pauseSprite;
			pauseMenu.SetActive(false);
		}
	}
}

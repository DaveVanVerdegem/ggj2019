using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCanvas : MonoBehaviour
{
	#region Inspector Fields
	public GameObject WinScreen = null;

	public GameObject LoseScreen = null;
	#endregion

	#region Properties
	public static GameCanvas Instance = null;
	#endregion

	#region Life Cycle
	// Start is called before the first frame update
	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);
	}

	// Update is called once per frame
	private void Update()
	{
	}
	#endregion

	#region Scene Management
	public void ReloadScene()
	{
		Debug.LogWarning("Reloading scene.", this);

		HideScreens();

		Time.timeScale = 1f;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
	#endregion

	public void ShowWin()
	{
		WinScreen.SetActive(true);
		Time.timeScale = 0f;
	}

	public void ShowLose()
	{
		LoseScreen.SetActive(true);
		Time.timeScale = 0f;
	}

	public void HideScreens()
	{
		WinScreen.SetActive(false);
		LoseScreen.SetActive(false);
	}
}
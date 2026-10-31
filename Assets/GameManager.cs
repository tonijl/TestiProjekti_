using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
	public GameObject gameOverPanel;
	
	public void GameOver()
	{
		gameOverPanel.SetActive(true);
		Time.timeScale = 0f;
	}
	
	public void RestartGame()
	{
		Time.timeScale = 1f;
		
		SceneManager.LoadScene(
			SceneManager.GetActiveScene().buildIndex
			);
	}
	
}

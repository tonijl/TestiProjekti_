using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
	public static ScoreManager instance;
	public TMP_Text scoreText;
	public TMP_Text highScoreText;
	private int score = 0;
	private int highScore;
	public EnemySpawner enemySpawner;
	
    private void Awake()
    {
        instance = this;
    }
	
	
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
		//load highscore
		highScore = PlayerPrefs.GetInt("HighScore",0);
		
        UpdateScoreUI();
    }
	
	public void AddScore(int amount)
	{
		score += amount;
		
		//Save new highscore if necessary
		
		Debug.Log("current highScore:"+ highScore.ToString());
		
		if (score > highScore)
		{
			Debug.Log("setting new highScore!");
			
			highScore = score;
			
			PlayerPrefs.SetInt("HighScore", highScore);
			PlayerPrefs.Save();
			
		}
		
		
		UpdateScoreUI();
		
		enemySpawner.UpdateScore(score);
		//enemySpawner.CheckScore(score);
	}
	
	void UpdateScoreUI()
	{
		
		if(scoreText != null){
			scoreText.text = "Score: " + score;
		}
		
		if(highScoreText != null){
			highScoreText.text = "High Score: "+highScore;
		}
		
	}


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController uIController;

    public Text scoreText;
    public int maxScore;
    public int currentScore;
    public Slider scoreSlider;
    public LevelComplete levelComplete;
    public LevelFailed levelFailed;

    private void Awake()
    {
        uIController = this;
    }

    private void Start()
    {
        currentScore = 0;
        scoreSlider.maxValue = maxScore;
    }
    public static void UPdateScore(int score)
    {
        uIController.currentScore += score;
        uIController.scoreText.text = uIController.currentScore.ToString() + "/" + uIController.maxScore;
        uIController.scoreSlider.value = uIController.currentScore;

        if (uIController.currentScore >= uIController.maxScore)
        {
            uIController.levelComplete.gameObject.SetActive(true);
            print("Level Complete");
        }
    }
}

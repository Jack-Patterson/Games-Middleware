using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoalUiElement : MonoBehaviour
{
    public static GoalUiElement Instance;
    private int score = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void IncrementScore()
    {
        score++;
        GetComponent<TextMeshPro>().text = $"Score<br> {score}";
    }
}

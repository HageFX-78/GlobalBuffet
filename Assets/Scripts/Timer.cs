using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timer;
    [SerializeField] int timerTillGameOver;

    private void Start()
    {
        TextUpdate();
        StartCoroutine(StartTimer());
    }

    void TextUpdate()
    {
        timer.text = timerTillGameOver > 60 ? $"{timerTillGameOver / 60}:{(timerTillGameOver % 60).ToString("D2")}" : $"{timerTillGameOver}";
    }

    IEnumerator StartTimer()
    {
        while (timerTillGameOver > 0)
        {
            TextUpdate();
            yield return new WaitForSeconds(1.0f);
            timerTillGameOver--;
        }
        GameUIManager.guiInstance.GameOver(false);
    }
}

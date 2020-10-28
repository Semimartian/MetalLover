using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreFeedbackUI : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI text;
    public void Spawn(int score)
    {
        text.text ="+"+ score.ToString();


    }

    private void Die()
    {
        GameManager.Recycle(this);
    }
}

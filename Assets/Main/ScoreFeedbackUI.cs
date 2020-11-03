using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreFeedbackUI : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI text;
    [SerializeField] private Animator animator;
    public void Spawn(Vector2 position, int score)
    {
        transform.position = position;
        text.text ="+"+ score.ToString();
        animator.SetTrigger("Spawn");
        Invoke("Die", 1f);
    }

    private void Die()
    {
        GameManager.Recycle(this);
    }
}

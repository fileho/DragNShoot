using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (Physics2D.simulationMode == SimulationMode2D.Script)
            return;

        UIManager.Instance.DrawWin();
        FindObjectOfType<Ball>().gameObject.SetActive(false);
    }
}

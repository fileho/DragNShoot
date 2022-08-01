using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Physics2D.simulationMode == SimulationMode2D.Script)
            return;

        SoundManager.instance.PlayBoost();
    }
}

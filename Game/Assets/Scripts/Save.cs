using UnityEngine;

public class Save : MonoBehaviour
{
    public Data data;

    public static Save Instance;

    private void Awake()
    {
        Instance = this;
    }

    public bool StickToBall()
    {
        return data.stickToBall;
    }

    public bool FinishLevel(int index, int score)
    {
        int val = data.levels[index];
        if (val != -1 && score >= val) return false;
        
        data.levels[index] = score;
        return true;
    }

    public int GetHighScore(int index)
    {
        return data.levels[index];
    }
}

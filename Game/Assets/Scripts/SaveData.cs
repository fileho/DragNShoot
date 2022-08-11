using UnityEditorInternal;

[System.Serializable]
public class SaveData
{
    public SaveData(int[] levels, float sfxVolume, float soundtrackVolume, bool stickToBall)
    {
        this.levels = levels;
        this.sfxVolume = sfxVolume;
        this.soundtrackVolume = soundtrackVolume;
        this.stickToBall = stickToBall;
    }

    public int[] levels;
    public float sfxVolume;
    public float soundtrackVolume;
    public bool stickToBall;

    public static SaveData DefaultData()
    {
        var saveData = new SaveData(new[] {-1, -1, -1}, 0.5f, 0.5f, true);
        return saveData;
    }

    public int GetMaxLevelComplete()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            if (levels[i] == -1)
                return i;
        }

        return levels.Length - 1;
    }
}

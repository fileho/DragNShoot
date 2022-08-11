using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Save : MonoBehaviour
{
    public static Save Instance;

    private SaveData saveData;

    private void Awake()
    {
        LoadProgress();
        Instance = this;
    }

    public bool StickToBall()
    {
        return saveData.stickToBall;
    }

    public bool FinishLevel(int index, int score)
    {
        int val = saveData.levels[index];
        if (val != -1 && score >= val) return false;
        
        saveData.levels[index] = score;
        SaveProgress();
        return true;
    }

    public int GetHighScore(int index)
    {
        return saveData.levels[index];
    }

    public void SetSfxVolume(float value)
    {
        saveData.sfxVolume = value;
        SaveProgress();
    }

    public void SetSoundTrackVolume(float value)
    {
        saveData.soundtrackVolume = value;
        SaveProgress();
    }

    public void SetBallMode(bool mode)
    {
        saveData.stickToBall = mode;
        SaveProgress();
    }

    public void SwapBallMode()
    {
        saveData.stickToBall = !saveData.stickToBall;
        SaveProgress();
    }

    public float SfxVolume()
    {
        return saveData.sfxVolume;
    } 

    public float SoundtrackVolume()
    {
        return saveData.soundtrackVolume;
    }

    public int GetMaxLevelComplete()
    {
        return saveData.GetMaxLevelComplete();
    }

    public void ResetSave()
    {
        string path = Application.persistentDataPath + "/save.bin";

        if (File.Exists(path))
        {
            File.Delete(path);
        }

        saveData = SaveData.DefaultData();
    }


    private void SaveProgress()
    {
        var formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/save.bin";
        FileStream stream = new FileStream(path, FileMode.Create) {Position = 0};

        formatter.Serialize(stream, saveData);
        stream.Close();
    }



    private void LoadProgress()
    {
        string path = Application.persistentDataPath + "/save.bin";

        if (!File.Exists(path))
        {
            saveData = SaveData.DefaultData();
            return;
        }

        var formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Open) {Position = 0};

        saveData = formatter.Deserialize(stream) as SaveData;
        stream.Close();
    }
}

using UnityEngine;

public class AchievementSystem
{
    public static void Unlock(string key)
    {
        PlayerPrefs.SetInt(key, 1);
    }

    public static bool IsUnlocked(string key)
    {
        return PlayerPrefs.GetInt(key, 0) == 1;
    }
}
using UnityEngine;

[System.Serializable]
public class Quest
{
    public string questName;
    public string description;

    public int requiredAmount;
    public int currentAmount;

    public bool isCompleted;

    public void AddProgress(int amount)
    {
        currentAmount += amount;

        if (currentAmount >= requiredAmount)
        {
            currentAmount = requiredAmount;
            isCompleted = true;
        }
    }
}

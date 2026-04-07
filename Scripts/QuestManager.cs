using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    public Quest currentQuest;

    public QuestUIController questUI;
    public QuestPopup questPopup;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        StartQuest("Collect Wood", "Collect 5 pieces of wood", 5);
    }

    public void StartQuest(string name, string desc, int amount)
    {
        currentQuest = new Quest
        {
            questName = name,
            description = desc,
            requiredAmount = amount,
            currentAmount = 0,
            isCompleted = false
        };

        questUI.ShowQuest(name, desc + " (0/" + amount + ")");
    }

    public void AddProgress(int amount)
    {
        if (currentQuest == null || currentQuest.isCompleted) return;

        currentQuest.AddProgress(amount);

        questUI.UpdateObjective(
            currentQuest.description + " (" +
            currentQuest.currentAmount + "/" +
            currentQuest.requiredAmount + ")"
        );

        if (currentQuest.isCompleted)
        {
            CompleteQuest();
        }
    }

    void CompleteQuest()
    {
        questUI.HideQuest();
        questPopup.ShowPopup("Quest Completed: " + currentQuest.questName);
    }
}

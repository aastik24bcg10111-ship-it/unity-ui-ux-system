using UnityEngine;
using TMPro;

public class QuestUIController : MonoBehaviour
{
    public GameObject questPanel;
    public TextMeshProUGUI questTitle;
    public TextMeshProUGUI questObjective;

    public void ShowQuest(string title, string objective)
    {
        questPanel.SetActive(true);
        questTitle.text = title;
        questObjective.text = objective;
    }

    public void UpdateObjective(string objective)
    {
        questObjective.text = objective;
    }

    public void HideQuest()
    {
        questPanel.SetActive(false);
    }
}

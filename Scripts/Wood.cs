using UnityEngine;

public class Wood : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            QuestManager.instance.AddProgress(1);
            Destroy(gameObject);
        }
    }
}
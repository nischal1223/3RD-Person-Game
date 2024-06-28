using UnityEngine;

public class CourseCompleteTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Entered");
        if (other.CompareTag("Player"))
        {
            Debug.Log("Course completed!");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    [SerializeField] private WaypointPath path; // the path we are following

    private Transform sourceWP; // the waypoint transform we are travelling from
    private Transform targetWP; // the waypoint transform we are travelling to
    private int targetWPIndex = 0; // the waypoint index we are travelling to

    private float timeToWP; // the total time to get from sourceWP to targetWP
    private float elapsedTimeToWP = 0; // the elapsed time (sourceWP to targetWP)
    private float speed = 3.0f; // movement speed

    private bool isPaused = false; // to track if the platform is paused

    void Start()
    {
        TargetNextWaypoint();
    }

    void FixedUpdate()
    {
        if (!isPaused)
        {
            MoveTowardsWaypoint();
        }
    }

    // Determine what waypoint we are going to next & set associated variables
    void TargetNextWaypoint()
    {
        // reset the elapsed time
        elapsedTimeToWP = 0;

        // determine the source
        sourceWP = path.GetWaypoint(targetWPIndex);

        // determine the target
        targetWPIndex++;
        // if we exhausted our waypoints, go to the first
        if (targetWPIndex >= path.GetWaypointCount())
        {
            targetWPIndex = 0;
        }
        targetWP = path.GetWaypoint(targetWPIndex);

        // calculate distance to waypoint
        float distanceToWP = Vector3.Distance(sourceWP.position, targetWP.position);
        // calculate time to waypoint
        timeToWP = distanceToWP / speed;

        // Start the pause coroutine
        StartCoroutine(PauseBeforeNextWaypoint());
    }

    // Travel towards the target waypoint (call this from Update or FixedUpdate())
    private void MoveTowardsWaypoint()
    {
        // calculate the elapsed time spent on the way to this waypoint
        elapsedTimeToWP += Time.deltaTime;
        // calculate percent complete
        float elapsedTimePercentage = elapsedTimeToWP / timeToWP;
        // move
        transform.position = Vector3.Lerp(sourceWP.position, targetWP.position, elapsedTimePercentage);
        // rotate
        transform.rotation = Quaternion.Lerp(sourceWP.rotation, targetWP.rotation, elapsedTimePercentage);
        // check if we've reached our waypoint (based on time)
        if (elapsedTimePercentage >= 1)
        {
            TargetNextWaypoint();
        }
    }

    // Coroutine to pause the platform for 1 second
    private IEnumerator PauseBeforeNextWaypoint()
    {
        isPaused = true;
        Debug.Log("Platform paused at waypoint: " + targetWPIndex);
        yield return new WaitForSeconds(1);
        isPaused = false;
        Debug.Log("Platform resumed from waypoint: " + targetWPIndex);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("enter");
            other.transform.parent = this.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("exit");
            other.transform.parent = null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointPath : MonoBehaviour
{
    // Get the transform of a specific waypoint (position, rotation, scale)
    public Transform GetWaypoint(int index)
    {
        return transform.GetChild(index).transform;
    }

    // Return the number of waypoints in the path
    public int GetWaypointCount()
    {
        return transform.childCount;
    }
}


using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    [SerializeField] Waypoint _waypointPrefab;

    public List<Waypoint> Waypoints { get; private set; } = new();

    void OnEnable()
    {
        Player.OnWaypointSet += Player_OnWaypointSet;
    }

    void OnDisable()
    {
        Player.OnWaypointSet -= Player_OnWaypointSet;
    }

    void Player_OnWaypointSet(Vector2 playerPosition)
    {
        Waypoint newWayponit = Instantiate(_waypointPrefab, playerPosition, Quaternion.identity, transform);
        Waypoints.Add(newWayponit);
        newWayponit.name = $"Waypoint ({Waypoints.Count})";
    }
}

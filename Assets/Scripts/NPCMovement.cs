using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    private Rigidbody2D myRigidBody;
    private Animator anim;

    private Vector2[] waypoints; // Stores the waypoints for the chosen path
    private int waypointIndex = 0;
    private bool isWaiting = false;

    // Lists for Path1
    [SerializeField] private List<int> skipListPath1; // Waypoints to skip in Path1
    [SerializeField] private List<int> lookDownListPath1; // Waypoints to look down in Path1

    // Lists for Path2
    [SerializeField] private List<int> skipListPath2; // Waypoints to skip in Path2
    [SerializeField] private List<int> lookDownListPath2; // Waypoints to look down in Path2

    private List<int> currentSkipList; // Skip list for the chosen path
    private List<int> currentLookDownList; // Look down list for the chosen path

    private float waypointThreshold = 0.2f; // Stopping threshold

    [SerializeField] private Transform npcSpawner; // Reference to the NPC spawner

    // Path selection options
    public enum PathSelection { Path1, Path2, Random }
    [SerializeField] private PathSelection pathSelection = PathSelection.Random; // Default to Random

    void Start()
    {
        anim = GetComponent<Animator>();
        myRigidBody = GetComponent<Rigidbody2D>();

        // Choose the path based on the selected option
        GameObject chosenPath = ChoosePath();

        if (chosenPath == null)
        {
            Debug.LogError("Path not found! Make sure Path1 and Path2 exist in the scene.");
            return;
        }

        // Set the current skip and look down lists based on the chosen path
        if (chosenPath.name == "Path1")
        {
            currentSkipList = skipListPath1;
            currentLookDownList = lookDownListPath1;
        }
        else
        {
            currentSkipList = skipListPath2;
            currentLookDownList = lookDownListPath2;
        }

        // Store waypoint positions as Vector2
        List<Vector2> waypointList = new List<Vector2>();
        foreach (Transform child in chosenPath.transform)
        {
            waypointList.Add(child.position);
        }
        waypoints = waypointList.ToArray();

        if (waypoints.Length == 0)
        {
            Debug.LogError("No waypoints found in the chosen path!");
            return;
        }

        // Set NPC to start at the spawner's position
        if (npcSpawner != null)
        {
            transform.position = npcSpawner.position;
        }
        else
        {
            Debug.LogWarning("NPC Spawner not assigned! Using default position.");
        }
    }

    void FixedUpdate()
    {
        if (!isWaiting)
        {
            Vector2 target = waypoints[waypointIndex];
            Vector2 direction = (target - (Vector2)transform.position).normalized;

            // Move the NPC
            myRigidBody.linearVelocity = direction * speed;

            anim.SetBool("isWalking", true);
            anim.SetFloat("MoveX", direction.x);
            anim.SetFloat("MoveY", direction.y);

            // Check if NPC reached the waypoint
            if (Vector2.Distance(transform.position, target) < waypointThreshold)
            {
                Debug.Log($"Reached Waypoint {waypointIndex} in {(currentSkipList == skipListPath1 ? "Path1" : "Path2")}");

                // Check if the current waypoint is in the skip list
                if (currentSkipList.Contains(waypointIndex))
                {
                    // Skip this waypoint and move to the next one
                    waypointIndex = (waypointIndex + 1) % waypoints.Length;
                    Debug.Log($"Skipping Waypoint {waypointIndex}");
                }
                else
                {
                    // 50% chance to stop or skip
                    if (Random.Range(0, 2) == 0) // Random.Range(0, 2) returns 0 or 1
                    {
                        // Stop at this waypoint
                        StartCoroutine(WaitAtWaypoint());
                    }
                    else
                    {
                        // Skip this waypoint and move to the next one
                        waypointIndex = (waypointIndex + 1) % waypoints.Length;
                        Debug.Log($"Randomly Skipping Waypoint {waypointIndex}");
                    }
                }
            }
        }
    }

    private IEnumerator WaitAtWaypoint()
    {
        isWaiting = true;
        myRigidBody.linearVelocity = Vector2.zero; // Stop movement
        anim.SetBool("isWalking", false);

        // Check if the current waypoint is in the look down list
        if (currentLookDownList.Contains(waypointIndex))
        {
            Debug.Log($"Looking down at Waypoint {waypointIndex}");
            anim.SetFloat("MoveX", 0); // Stop horizontal movement
            anim.SetFloat("MoveY", -1); // Set to look down
        }
        else
        {
            Debug.Log($"Pausing at Waypoint {waypointIndex} without looking down");
            anim.SetFloat("MoveX", 0); // Stop horizontal movement
            anim.SetFloat("MoveY", 1); // Set to look up (or default idle)
        }

        yield return new WaitForSeconds(2f); // Pause at waypoint

        // Move to the next waypoint
        waypointIndex = (waypointIndex + 1) % waypoints.Length;
        Debug.Log($"Next Waypoint: {waypointIndex}");

        isWaiting = false;
    }

    private GameObject ChoosePath()
    {
        switch (pathSelection)
        {
            case PathSelection.Path1:
                return GameObject.Find("Path1");
            case PathSelection.Path2:
                return GameObject.Find("Path2");
            case PathSelection.Random:
                return Random.Range(0, 2) == 0 ? GameObject.Find("Path1") : GameObject.Find("Path2");
            default:
                Debug.LogWarning("Invalid path selection! Defaulting to Random.");
                return Random.Range(0, 2) == 0 ? GameObject.Find("Path1") : GameObject.Find("Path2");
        }
    }
}
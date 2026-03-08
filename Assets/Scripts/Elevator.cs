using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // For UI text display

public class Elevator : MonoBehaviour
{
    public enum ElevatorState { Idle, Moving } // state of the elevator
    public ElevatorState currentState = ElevatorState.Idle;
    
    // The current floor the elevator is on (0 = ground floor, 1 = first floor, etc.)
    public int currentFloor = 0; // Start at ground floor
    public float speed = 3f;
    public float yOffset = 0.75f; // The offset to make it sit ON the floor
    public TMP_Text floorDisplay;
    
    // Assuming the ground floor is at y = -4 and each floor is 2 units apart
    private float groundFloorY = -4f;
    private float floorGap = 2f;
    private Queue<int> requests = new Queue<int>();
    private Vector2 targetPos;

    void Start()
    {
        // Added yOffset
        transform.position = new Vector2(transform.position.x, groundFloorY + (currentFloor * floorGap) + yOffset);
        UpdateDisplay();
    }

    // Called by ElevatorManager when a floor is requested
    public void RequestFloor(int floor) 
    {
        if (!requests.Contains(floor))
        {
            requests.Enqueue(floor);
            if (currentState == ElevatorState.Idle)
            {
                ProcessNextRequest();
            }
        }
    }
    // Process the next floor request in the queue
    void ProcessNextRequest()
    {
        if (requests.Count > 0)
        {
            int nextFloor = requests.Dequeue();
            // Added yOffset here
            targetPos = new Vector2(transform.position.x, groundFloorY + (nextFloor * floorGap) + yOffset);
            currentState = ElevatorState.Moving;
            StartCoroutine(MoveToFloor(nextFloor));
        }
        else
        {
            // No more requests, go idle
            currentState = ElevatorState.Idle;
        }
    }

    IEnumerator MoveToFloor(int targetFloor)
    {
        // Move towards the target position until we are close enough
        while (Mathf.Abs(transform.position.y - targetPos.y) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            // Account for yOffset when calculating the display number
            currentFloor = Mathf.RoundToInt((transform.position.y - groundFloorY - yOffset) / floorGap);
            UpdateDisplay();
            yield return null;
        }
        // Snap to the exact position and update the floor number
        transform.position = targetPos;
        currentFloor = targetFloor;
        UpdateDisplay();

        yield return new WaitForSeconds(1f);
        ProcessNextRequest();
    }
    // Update the floor display text
    void UpdateDisplay()
    {
        if (floorDisplay != null) floorDisplay.text = currentFloor.ToString();
    }
}
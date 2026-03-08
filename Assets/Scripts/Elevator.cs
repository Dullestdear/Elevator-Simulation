using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Elevator : MonoBehaviour
{
    public enum ElevatorState { Idle, Moving }
    public ElevatorState currentState = ElevatorState.Idle;

    public int currentFloor = 0;
    public float speed = 3f;
    public float yOffset = 0.75f; // The offset to make it sit ON the floor
    public TMP_Text floorDisplay;

    private float groundFloorY = -4f;
    private float floorGap = 2f;
    private Queue<int> requests = new Queue<int>();
    private Vector2 targetPos;

    void Start()
    {
        // Added yOffset here
        transform.position = new Vector2(transform.position.x, groundFloorY + (currentFloor * floorGap) + yOffset);
        UpdateDisplay();
    }

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
            currentState = ElevatorState.Idle;
        }
    }

    IEnumerator MoveToFloor(int targetFloor)
    {
        while (Mathf.Abs(transform.position.y - targetPos.y) > 0.01f)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            // Account for yOffset when calculating the display number
            currentFloor = Mathf.RoundToInt((transform.position.y - groundFloorY - yOffset) / floorGap);
            UpdateDisplay();
            yield return null;
        }

        transform.position = targetPos;
        currentFloor = targetFloor;
        UpdateDisplay();

        yield return new WaitForSeconds(1f);
        ProcessNextRequest();
    }

    void UpdateDisplay()
    {
        if (floorDisplay != null) floorDisplay.text = currentFloor.ToString();
    }
}
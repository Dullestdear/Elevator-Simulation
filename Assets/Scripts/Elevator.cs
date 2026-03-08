using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // For the text display later

public class Elevator : MonoBehaviour
{
    public enum ElevatorState { Idle, Moving }
    public ElevatorState currentState = ElevatorState.Idle;

    public int currentFloor = 0;
    public float speed = 3f;
    public TMP_Text floorDisplay; // Drag your text here later

    private float groundFloorY = -4f;
    private float floorGap = 2f;
    private Queue<int> requests = new Queue<int>();
    private Vector2 targetPos;

    void Start()
    {
        // Set starting position based on current floor
        transform.position = new Vector2(transform.position.x, groundFloorY + (currentFloor * floorGap));
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
            targetPos = new Vector2(transform.position.x, groundFloorY + (nextFloor * floorGap));
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
            currentFloor = Mathf.RoundToInt((transform.position.y - groundFloorY) / floorGap);
            UpdateDisplay();
            yield return null;
        }

        transform.position = targetPos;
        currentFloor = targetFloor;
        UpdateDisplay();

        // Pause briefly to simulate doors opening/loading
        yield return new WaitForSeconds(1f);
        ProcessNextRequest();
    }

    void UpdateDisplay()
    {
        if (floorDisplay != null) floorDisplay.text = currentFloor.ToString();
    }
}
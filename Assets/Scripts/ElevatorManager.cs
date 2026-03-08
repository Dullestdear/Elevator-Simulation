using UnityEngine;

public class ElevatorManager : MonoBehaviour
{
    public Elevator[] elevators; // Assign these in the Unity Inspector

    // Called when a floor button is pressed to call an elevator
    public void CallElevatorToFloor(int requestedFloor)
    {
        Elevator bestLift = null;
        int minDistance = int.MaxValue;

        // Find the closest IDLE lift
        foreach (Elevator lift in elevators)
        {
            // Only consider idle elevators for immediate response
            if (lift.currentState == Elevator.ElevatorState.Idle)
            {
                int distance = Mathf.Abs(lift.currentFloor - requestedFloor);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    bestLift = lift;
                }
            }
        }

        // If all are moving, queue it on the one closest to the request anyway
        if (bestLift == null)
        {
            foreach (Elevator lift in elevators)
            {
                // Consider all elevators, even if they are moving, to find the closest one
                int distance = Mathf.Abs(lift.currentFloor - requestedFloor);
                if (distance < minDistance)
                {
                    // This will allow the request to be queued on the closest elevator, even if it's currently moving
                    minDistance = distance;
                    bestLift = lift;
                }
            }
        }
        // Request the best lift to go to the requested floor
        if (bestLift != null) bestLift.RequestFloor(requestedFloor);
    }
}
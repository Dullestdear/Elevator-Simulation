using UnityEngine;

public class ElevatorManager : MonoBehaviour
{
    public Elevator[] elevators;

    public void CallElevatorToFloor(int requestedFloor)
    {
        Elevator bestLift = null;
        int minDistance = int.MaxValue;

        // Find the closest IDLE lift
        foreach (Elevator lift in elevators)
        {
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
                int distance = Mathf.Abs(lift.currentFloor - requestedFloor);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    bestLift = lift;
                }
            }
        }

        if (bestLift != null) bestLift.RequestFloor(requestedFloor);
    }
}
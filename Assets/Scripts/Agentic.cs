using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine.InputSystem;
using Debug = UnityEngine.Debug;


public class Agentic : Agent
{
    Rigidbody Body;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Body = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        this.Body.linearVelocity = Vector3.zero;
        this.Body.angularVelocity = Vector3.zero;
        this.transform.localPosition = new Vector3(Random.value * 6-3,2,Random.value*6-3);

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(this.transform.localPosition);
        sensor.AddObservation(this.Body.linearVelocity.x);
        sensor.AddObservation(this.Body.linearVelocity.z);
        // sensor.AddObservation
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actionBuffers.ContinuousActions[0];
        controlSignal.z = actionBuffers.ContinuousActions[1];
        Body.AddForce(controlSignal*15);

        float speedOfBall = this.Body.linearVelocity.magnitude;
        float fallPenalty = -100;

        if (this.transform.localPosition.y < 0)
        {
            SetReward(fallPenalty);
            EndEpisode();
        }
        else
        {
            SetReward(speedOfBall);
        }

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;

        // Guard against any duplicate/unconfigured Agentic component in the scene
        if (continuousActionsOut.Length < 2)
        {
            Debug.LogWarning($"Disabling unconfigured Agentic component on GameObject: '{gameObject.name}' (ContinuousActions length was {continuousActionsOut.Length})", this.gameObject);
            this.enabled = false;
            return;
        }

        float moveX = 0f;
        float moveZ = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.dKey.isPressed) moveX += 1f;
            if (Keyboard.current.aKey.isPressed) moveX -= 1f;
            if (Keyboard.current.wKey.isPressed) moveZ += 1f;
            if (Keyboard.current.sKey.isPressed) moveZ -= 1f;
        }

        continuousActionsOut[0] = moveX;
        continuousActionsOut[1] = moveZ;
    }

}

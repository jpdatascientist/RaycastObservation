using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;

public class RaycastAgent : Agent
{
    public Transform target;
    Rigidbody rBody;

    public override void Initialize()
    {
        rBody = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        if (this.transform.localPosition.y < 0)
        {
            this.rBody.angularVelocity = Vector3.zero; 
            this.rBody.velocity = Vector3.zero; 
            this.transform.localPosition = new Vector3(0.0f, 0.5f, 0.0f);
        }
        target.localPosition = new Vector3(
            Random.value*8-4, 0.5f, Random.value*8-4);
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
    
        Vector3 dirToGo = Vector3.zero;
        Vector3 rotateDir = Vector3.zero;
        int action = actionBuffers.DiscreteActions[0];
        if (action == 1) dirToGo = transform.forward;
        if (action == 2) dirToGo = transform.forward * -1.0f;
        if (action == 3) rotateDir = transform.up * -1.0f;
        if (action == 4) rotateDir = transform.up; 
        this.transform.Rotate(rotateDir, Time.deltaTime * 200f); 
        this.rBody.AddForce(dirToGo * 0.4f, ForceMode.VelocityChange);

        float distanceToTarget = Vector3.Distance(
        this.transform.localPosition, target.localPosition); 
        if (distanceToTarget < 1.42f)
       {
            AddReward(1.0f);
            EndEpisode();
       }
        if (this.transform.localPosition.y < 0)
        {
            EndEpisode();
        }

    }   

    public override void Heuristic(in ActionBuffers actionBuffers)
    {
        var actionsOut = actionBuffers.DiscreteActions;
        actionsOut[0] = 0;
        if (Input.GetKey(KeyCode.UpArrow)) actionsOut[0] = 1;
        if (Input.GetKey(KeyCode.DownArrow)) actionsOut[0] = 2;
        if (Input.GetKey(KeyCode.LeftArrow)) actionsOut[0] = 3;
        if (Input.GetKey(KeyCode.RightArrow)) actionsOut[0] = 4;
    }
}
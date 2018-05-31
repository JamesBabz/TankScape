using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIDriver : MonoBehaviour
{


    private NavMeshAgent _agent;
    private bool _inRange;
    private GameObject _target;

    // Use this for initialization
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameObject.Find("Game Rules").GetComponent<GameRules>().LoseCondition.ToString().Equals("FailToProtect"))
        {
            _target = GameObject.Find("Game Rules").GetComponent<GameRules>().TargetToProtect;
        }
        else
        {
            _target = GameObject.Find("Player");
        }
        
        if (_target != null)
        {
            if (!_inRange)
            {
                _agent.destination = _target.transform.position;
            }
            InstantlyTurn(_agent.destination);
        }



    }

    void OnTriggerEnter(Collider gameObjectCollider)
    {
        if (gameObjectCollider.transform.root.gameObject == _target)
        {
            _agent.isStopped = true;
            _inRange = true;
        }
    }

    void OnTriggerExit(Collider gameObjectCollider)
    {
        if (gameObjectCollider.transform.root.gameObject == _target)
        {
            _agent.isStopped = false;
            _inRange = false;
        }
    }

    private void InstantlyTurn(Vector3 destination)
    {
        //When on target -> dont rotate!
        if ((destination - transform.position).magnitude < 0.1f) return;

        Vector3 direction = (destination - transform.position).normalized;
        Quaternion qDir = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, qDir, Time.deltaTime * 10f);
    }



}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class BulletBehaviorScript : MonoBehaviour
{

    public float Speed = 2;
    public int Amount = 1;
    public double Cooldown = 0.5;
    public double Damage;
    public float MaxTravelTime = 5;

    private GameObject _shooter;
    private Vector3 _prevPos;
    
    // Use this for initialization
    void Start()
    {
        _prevPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _prevPos = transform.position;

        transform.position += transform.forward * Time.deltaTime * Speed;

        CheckForHit();
       
        

    }

    private void CheckForHit()
    {
        RaycastHit[] hits = Physics.RaycastAll(new Ray(_prevPos, (transform.position - _prevPos).normalized), (transform.position - _prevPos).magnitude);

        for (int i = 0; i < hits.Length; i++)
        {
            Transform currHit = hits[i].collider.transform;
            bool target = currHit.root.tag.Contains("Target") || currHit.tag.Contains("Target");
            bool pass = currHit.tag.Contains("Pass");
            
            if(target && !pass)
            {
                String[] Tags = Regex.Split(currHit.tag + currHit.root.tag, @"(?<!^)(?=[A-Z])").Distinct().ToArray(); //Gets all tags split on upppercase
                foreach(String tag in Tags)
                {
                    switch (tag)
                    {
                        case "Tank":
                            TankStats tankStats = currHit.root.gameObject.GetComponent<TankStats>();
                            tankStats.triggerHit(gameObject);
                            break;
                        case "Object":
                            ObjectStats objectStats = currHit.gameObject.GetComponent<ObjectStats>();
                            objectStats.TriggerHit(gameObject);
                            break;
                        case "Npc":
                            NPCStats NPCStats = currHit.root.gameObject.GetComponent<NPCStats>();
                            NPCStats.TriggerHit(gameObject);
                            break;

                    }

                }
               
                Destroy(gameObject);
                return;
            } 
            }
        }
    

    public void SetShooter(GameObject shooter)
    {
        _shooter = shooter;
    }

    public GameObject GetShooter()
    {
        return _shooter;
    }
    
}

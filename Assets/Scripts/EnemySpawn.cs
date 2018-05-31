using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawn : MonoBehaviour
{

    public Transform enemy1;
    public Transform enemy2;
    public Transform enemy3;

    private int timer1;
    private int timer2;
    private int timer3;

    private Vector3 randomPos;
    private Vector3 randomPos1;
    private Vector3 randomPos2;
    private Vector3 randomPos3;

    private int currentRound;
    private int DiffCounter;
    private float timer;
    private float counter;

    private const float range = 5.0f;

    // Use this for initialization
    void Start ()
    {
       

        currentRound = 1;
    }

    // Update is called once per frame
    void Update ()
	{
	    randomPos = Random.insideUnitSphere * range;
	    randomPos1 = Random.insideUnitSphere * range;
	    randomPos2 = Random.insideUnitSphere * range;
	    randomPos3 = Random.insideUnitSphere * range;

        waves();
	}

    private void waves()
    {
        round1();
        round2();
        round3();
    }

    private void round1()
    {
        timer += Time.deltaTime;

        if (Math.Floor(timer) == 5 && counter == 0)
        {
            Wave1(enemy1);
            if (DiffCounter == 1)
            {
                Wave1(enemy2);
            }
            counter++;
        }
        else if (Math.Floor(timer) == 10 && counter == 1)
        {
            Wave2(enemy1);
            if (DiffCounter == 1)
            {
                Wave2(enemy2);
            }
            counter++;
        }
        else if (Math.Floor(timer) == 20 && counter == 2)
        {
            Wave3(enemy1);
            if (DiffCounter == 1)
            {
                Wave3(enemy2);
            }
            counter++;
        }
    }

    private void round2()
    {
        if (Math.Floor(timer) == 30 && counter == 3)
        {
            currentRound++;
            if (DiffCounter == 1)
            {
                Wave1(enemy3);
            }
            Wave1(enemy2);
            counter++;
        }
        else if (Math.Floor(timer) == 35 && counter == 4)
        {
            Wave2(enemy2);
            if (DiffCounter == 1)
            {
                Wave2(enemy3);
            }
            counter++;
        }
        else if (Math.Floor(timer) == 45 && counter == 5)
        {
            Wave3(enemy2);
            if (DiffCounter == 1)
            {
                Wave3(enemy3);
            }
            counter++;
        }
    }

    private void round3()
    {
        if (Math.Floor(timer) == 50 && counter == 6)
        {
            currentRound++;
            if (DiffCounter == 1)
            {
                Wave1(enemy1);
                Wave1(enemy2);
            }
            Wave1(enemy3);
            counter++;
        }
        else if (Math.Floor(timer) == 60 && counter == 7)
        {
            Wave2(enemy3);
            if (DiffCounter == 1)
            {
                Wave2(enemy1);
                Wave2(enemy2);
            }
            counter++;
        }
        else if (Math.Floor(timer) == 70 && counter == 8)
        {
            Wave3(enemy3);
            if (DiffCounter == 1)
            {
                Wave3(enemy1);
                Wave3(enemy2);
            }
            counter++;
            timer = 0;
            counter = 0;
            DiffCounter++;
        }
    }

    void Wave1(Transform enemy)
    {
        randomPos.y = 0;

        Instantiate(enemy, transform.position + randomPos, Quaternion.identity);
    }

    void Wave2(Transform enemy)
    {
        randomPos.y = 0;
        randomPos2.y = 0;

        Instantiate(enemy, transform.position + randomPos, Quaternion.identity);
        Instantiate(enemy, transform.position + randomPos2, Quaternion.identity);
    }

    void Wave3(Transform enemy)
    {
        randomPos.y = 0;
        randomPos1.y = 0;
        randomPos2.y = 0;
        randomPos3.y = 0;

        Instantiate(enemy, transform.position + randomPos, Quaternion.identity);
        Instantiate(enemy, transform.position + randomPos1, Quaternion.identity);
        Instantiate(enemy, transform.position + randomPos2, Quaternion.identity);
        Instantiate(enemy, transform.position + randomPos3, Quaternion.identity);
    }

    public int GetCurrRound()
    {
        return currentRound;
    }
}



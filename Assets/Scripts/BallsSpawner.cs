using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallsSpawner : MonoBehaviour
{
    public static BallsSpawner Instance;
    [SerializeField] private GameObject BallPrefab;
    [SerializeField] private int BallsNeeded;

    private List<GameObject> Balls = new List<GameObject>();
    private int currentBallNumber = -1;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        InitializeBalls(BallsNeeded);
    }

    private void InitializeBalls(int size)
    {
        for(int i=0; i< size; i++)
        {
            GameObject newBall = Instantiate(BallPrefab,transform);
            newBall.transform.position = transform.position;
            newBall.name = "Ball_" + i.ToString();
            Balls.Add(newBall);
        }
    }

    public GameObject GetBall()
    {
        currentBallNumber += 1;
        if(currentBallNumber >= BallsNeeded)
            currentBallNumber = 0;

       return Balls[currentBallNumber];
    }
}

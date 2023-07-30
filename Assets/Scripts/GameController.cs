using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    [SerializeField] private GameObject Hoop;
    [SerializeField] private float speed = 5f;
    private bool isActivated = false;
    private bool moveLeft = true;

    [SerializeField] private int Points;

    [SerializeField] private int seconds = 59;
    [SerializeField] private int minutes = 2;
    [SerializeField] private int total_game_minutes = 3;

    public bool isGameOver = false;
    private Vector3 Hoop_initialPosition;

    [SerializeField] private Text Point_Text;
    [SerializeField] private Text Timer_Text;
    [SerializeField] private Text TotalPoint_Text;
    [SerializeField] private GameObject GameUiItems;
    [SerializeField] private GameObject GameOverUiItems;



    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        Timer_Text.text = total_game_minutes.ToString() + ":00";
        minutes = total_game_minutes - 1;
        StartCoroutine(Timer());
        Hoop_initialPosition = Hoop.transform.position;
    }

    private void Update()
    {
        if(isActivated && !isGameOver)
        {
            if(moveLeft)
            {
                Hoop.transform.Translate(Vector3.forward * speed * Time.deltaTime);
                if (Hoop.transform.position.x < -1.80)
                    moveLeft = false;
            }
            else
            {
                Hoop.transform.Translate(Vector3.back * speed * Time.deltaTime);
                if(Hoop.transform.position.x > 1.9)
                    moveLeft = true;
            }
        }
    }

    public void IncreasePoint()
    {
        Points += 1;
        Point_Text.text = Points.ToString();
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(1f);
        seconds--;
        if (seconds < 0)
        {
            isActivated = true;
            speed += 1;
            seconds = 59;
            minutes--;
        }
        if (minutes == 0)
            isGameOver = true;

        if (!isGameOver)
        {
            if(seconds < 10)
                Timer_Text.text = minutes.ToString() + ":0" + seconds.ToString();
            else
                Timer_Text.text = minutes.ToString() + ":" + seconds.ToString();
            StartCoroutine(Timer());
        }
        else
        {
            GameUiItems.SetActive(false);
            TotalPoint_Text.text = Points.ToString();
            GameOverUiItems.SetActive(true);
        }
    }

    public void PlayAgain()
    {
        Hoop.transform.position = Hoop_initialPosition;
        speed = 5f;
        isActivated = false;
        moveLeft = true;
        Points = 0;
        seconds = 0;
        minutes = 0;
        isGameOver = false;
        GameOverUiItems.SetActive(false);
        GameUiItems.SetActive(true);
        Timer_Text.text = total_game_minutes.ToString() + ":00";
        StartCoroutine(Timer());
    }

    public void ExitApplication()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}

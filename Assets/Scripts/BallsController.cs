using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallsController : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 dragStartPos;
    private Vector3 dragEndPos;
    private float dragStartTime;
    private float dragEndTime;

    [SerializeField] private float maxPower = 5f;
    [SerializeField] private float minPower = 2f;
    [SerializeField] private float currentPower;
    [SerializeField] private float forwardForce = 3f;
    [SerializeField] private float upwardForce = 2f;

    [SerializeField] private GameObject CentrePointer;
    private GameObject currentBall;
    private Rigidbody ballRigidbody;

    [SerializeField] private Slider PowerSlider;
    [SerializeField] private GameObject SliderGroup;

    private bool isInput = false;
    private bool doOnce = false;
    private bool isDone = false;
    // Start is called before the first frame update
    void Start()
    {
        PowerSlider.minValue = minPower;
        PowerSlider.maxValue = maxPower;
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameController.instance.isGameOver)
        {
            MouseInput();
            TouchInput();
        }
        if (!isDone)
            SelectBall();
    }

    private void MouseInput()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            SliderGroup.SetActive(true);
            currentBall = BallsSpawner.Instance.GetBall();
            ballRigidbody = currentBall.GetComponent<Rigidbody>();
            ballRigidbody.velocity = Vector3.zero;
            ballRigidbody.angularVelocity = Vector3.zero;
            currentBall.transform.position = CentrePointer.transform.position;
            ballRigidbody.useGravity = false;
            isDragging = true;
            dragStartPos = Input.mousePosition;
            dragStartTime = Time.time;
            currentPower = minPower;
            isInput = true;
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isInput = false;
            isDragging = false;
            dragEndPos = Input.mousePosition;
            dragEndTime = Time.time;

            Vector3 swipeDirection = (dragEndPos - dragStartPos).normalized;
            currentPower = Mathf.Clamp((dragEndTime - dragStartTime) * 5f, minPower, maxPower);
            ballRigidbody.useGravity = true;
            
            ballRigidbody.AddForce(transform.forward * forwardForce, ForceMode.Impulse);
            ballRigidbody.AddForce(transform.up * upwardForce, ForceMode.Impulse);
            ballRigidbody.AddForce(swipeDirection * currentPower, ForceMode.Impulse);
        }

        if(isInput)
        {
            var time = Time.time;
            currentPower = Mathf.Clamp((time - dragStartTime) * 5f, minPower, maxPower);
            PowerSlider.value = currentPower;
        }
        else
        {
            StartCoroutine(DisappearSlider());
        }
    }

    private void TouchInput()
    {
        if (Input.touchCount > 0)
        {
            if(!doOnce)
            {
                SelectBall();
                currentBall.transform.position = CentrePointer.transform.position;
                ballRigidbody.useGravity = false;
                SliderGroup.SetActive(true);
                isDragging = true;
                currentPower = minPower;
                isInput = true;
            }
            Touch touch = Input.GetTouch(0);
            
            if (touch.phase == TouchPhase.Began)
            {
                dragStartPos = touch.position;
                dragStartTime = Time.time;
            }
            else if (touch.phase == TouchPhase.Ended && isDragging)
            {
                isInput = false;
                isDragging = false;
                dragEndPos = touch.position;
                dragEndTime = Time.time;

                Vector3 swipeDirection = (dragEndPos - dragStartPos).normalized;
                currentPower = Mathf.Clamp((dragEndTime - dragStartTime) * 5f, minPower, maxPower);

                ballRigidbody.useGravity = true;

                ballRigidbody.AddForce(transform.forward * forwardForce, ForceMode.Impulse);
                ballRigidbody.AddForce(transform.up * upwardForce, ForceMode.Impulse);
                ballRigidbody.AddForce(swipeDirection * currentPower, ForceMode.Impulse);
                doOnce = false;
            }
        }

        if (isInput)
        {
            var time = Time.time;
            currentPower = Mathf.Clamp((time - dragStartTime) * 5f, minPower, maxPower);
            PowerSlider.value = currentPower;
        }
        else
        {
            StartCoroutine(DisappearSlider());
        }
    }

    private IEnumerator DisappearSlider()
    {
        yield return new WaitForSeconds(1f);
        if(!isInput)
            SliderGroup.SetActive(false);
    }

    private void SelectBall()
    {
        doOnce = true;
        currentBall = BallsSpawner.Instance.GetBall();
        ballRigidbody = currentBall.GetComponent<Rigidbody>();
        ballRigidbody.velocity = Vector3.zero;
        ballRigidbody.angularVelocity = Vector3.zero;
        isDone = true;
    }
}

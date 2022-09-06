using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{


    [Header("Player References")]
    [SerializeField] GameObject baseSplit;
    [SerializeField] MeshRenderer playerMR;
    [SerializeField] Collider playerCollider;
    [SerializeField] GameObject playerSplit;//Outer split ball

    [Header("Player Settings")]
    [SerializeField] float moveSpeed;
    [SerializeField] float splitSpeed;
    [SerializeField] float splitMaxDistance;

    [Header("Control Settings")]
    [SerializeField] float dragMinLength;
    [SerializeField] float dragMaxLength;

    [SerializeField] float splitMinLength;
    [SerializeField] float splitMaxLength;

    [SerializeField] float splitCooldownTime;
    [SerializeField] float cooldownPartition;
    [SerializeField] RectTransform cooldownBar;

    //Local values of touch controls
    private Vector3 firstTouchPos, currentTouchPos;
    private bool isDragging;
    private Touch firstTouch;
    private int firstFingerID;

    private Vector2 secStartPos, secCurrentPos;
    private bool isDraggingSecond, secTouchIsActive;
    private Touch secTouch;//Second touch
    private int secondFingerID;

    //Snapping values
    [SerializeField] float snapTime;
    [SerializeField] float snapPartition;
    public bool isSnapping;
    private Vector3 snapDelta;


    [Header("Control References")]
    [SerializeField] GameObject joystickWhole;
    [SerializeField] GameObject joystickCircle;
    [SerializeField] GameObject joystickBottom;

    [SerializeField] GameObject splitStickWhole;
    [SerializeField] GameObject splitStickWholeCircle;
    [SerializeField] GameObject splitStickWholeBottom;

    public bool notEaten;
    public static PlayerMovement pmInstance;

    IEnumerator sp;
    private void Awake()
    {
        pmInstance = this;
    }
    private void Start()
    {
        isDragging = false;
        isDraggingSecond = false;
        secTouchIsActive = false;


        isSnapping = false;
        joystickWhole.SetActive(false);
        splitStickWhole.SetActive(false);

        notEaten = true;

        //sp = SnapBack();
    }
    private void Update()
    {
        if (Time.timeScale != 0 && notEaten)
        {
            if (Input.touchCount >= 1)
            {
                firstTouch = Input.GetTouch(0);

                //TOuchphase part
                if (firstTouch.phase == TouchPhase.Began && firstTouch.position.x < Screen.width / 2)
                {
                    firstTouchPos = firstTouch.position;
                    firstFingerID = firstTouch.fingerId;

                    joystickWhole.SetActive(true);
                    joystickWhole.transform.position = firstTouchPos;

                }
                else if (firstTouch.phase == TouchPhase.Moved && firstTouch.fingerId == firstFingerID)
                {
                    currentTouchPos = firstTouch.position;
                    isDragging = true;
                }
                else if (firstTouch.phase == TouchPhase.Ended && firstTouch.fingerId == firstFingerID)
                {
                    isDragging = false;
                    joystickWhole.SetActive(false);
                    if (secTouchIsActive)
                    {
                        splitStickWhole.SetActive(false);

                        StartCoroutine(SnapBack());
                    }

                }

            }
            if (Input.touchCount >= 2 && !isSnapping)
            {

                secTouch = Input.GetTouch(1);

                if (secTouch.phase == TouchPhase.Began && secTouch.position.x > Screen.width / 2)
                {

                    secStartPos = secTouch.position;
                    secTouchIsActive = true;
                    secondFingerID = secTouch.fingerId;

                    isDraggingSecond = true;


                    splitStickWhole.SetActive(true);
                    splitStickWhole.transform.position = secStartPos;

                    playerSplit.SetActive(true);
                    baseSplit.SetActive(true);
                    playerMR.enabled = false;
                    playerCollider.enabled = false;
                }
                else if (secTouch.phase == TouchPhase.Moved && secTouch.fingerId == secondFingerID)
                {
                    secCurrentPos = secTouch.position;
                }
                else if (secTouch.phase == TouchPhase.Ended && secTouch.fingerId == secondFingerID)
                {
                    isDraggingSecond = false;
                    secTouchIsActive = false;

                    splitStickWhole.SetActive(false);
                    StartCoroutine(SnapBack());

                }
            }

        }
    }
    private void FixedUpdate()
    {
        Vector2 defaultPos = joystickBottom.transform.position;//Position of wherever first tap is
        Vector2 defaultPos2 = splitStickWhole.transform.position;//Position of wherever second tap is
        if (isDragging)
        {
            Vector2 dragDelta = Vector2.ClampMagnitude(currentTouchPos - firstTouchPos, dragMaxLength);

            if (dragDelta.magnitude >= dragMinLength)
            {
                joystickCircle.transform.position = new Vector2(defaultPos.x + dragDelta.x, defaultPos.y + dragDelta.y);

                gameObject.transform.Translate(new Vector3(dragDelta.x, 0, dragDelta.y) * (moveSpeed * transform.localScale.x) * Time.deltaTime);
            }
        }
        else
        {
            joystickCircle.transform.position = defaultPos;
        }

        //Second tap/touch 
        if (isDraggingSecond)
        {
            Vector3 playerPos = transform.position;
            Vector2 secDragDelta = Vector2.ClampMagnitude(secCurrentPos - defaultPos2, splitMaxLength);
            Vector2 offsetDragDelta = Vector2.ClampMagnitude(secCurrentPos - defaultPos2, splitMaxDistance * transform.localScale.x);
            snapDelta = offsetDragDelta;

            if (secDragDelta.magnitude > splitMinLength)
            {
                splitStickWholeCircle.transform.position = new Vector2(defaultPos2.x + secDragDelta.x, defaultPos2.y + secDragDelta.y);
                playerSplit.transform.position = new Vector3(playerPos.x + offsetDragDelta.x, 0, playerPos.z + offsetDragDelta.y);
            }
        }
        else
        {
            splitStickWhole.transform.position = defaultPos2;
        }

    }

    IEnumerator SnapBack()
    {
        AudioManager.amInstance.PlaySF("split");
        isSnapping = true;
        isDragging = false;
        isDraggingSecond = false;
        playerSplit.gameObject.tag = "SnappingSplit";

        Vector3 targetPos;
        Vector2 splitSnapDistance = snapDelta / snapPartition;
        float snapSplitTimer = snapTime / snapPartition;
        int localCounter = (int)snapPartition;

        targetPos = baseSplit.transform.position;

        while (localCounter > 0)
        {
            playerSplit.transform.position += new Vector3(splitSnapDistance.x, 0, splitSnapDistance.y) * -1;
            localCounter--;
            yield return new WaitForSeconds(snapSplitTimer);
        }

        playerSplit.transform.position = targetPos;
        playerSplit.transform.localPosition = new Vector3(0, 0, 0);


        playerSplit.SetActive(false);
        baseSplit.SetActive(false);
        playerMR.enabled = true;
        playerCollider.enabled = true;

        StartCoroutine(SplitCooldownTimer());
    }

    IEnumerator SplitCooldownTimer()
    {
        Vector2 originalSize = cooldownBar.sizeDelta;
        cooldownBar.gameObject.SetActive(true);

        float localLoopCount = cooldownPartition;
        float localSplitTime = splitCooldownTime / cooldownPartition;
        float localSplitSize = originalSize.x / cooldownPartition;

        while (localLoopCount > 0)
        {
            cooldownBar.sizeDelta -= new Vector2(localSplitSize, 0);
            localLoopCount--;
            yield return new WaitForSeconds(localSplitTime);
        }
        cooldownBar.gameObject.SetActive(false);
        cooldownBar.sizeDelta = originalSize;


        //Enable movement
        isSnapping = false;
    }
    public void PlayerEaten()
    {
        notEaten = false;
        playerSplit.SetActive(false);
        baseSplit.SetActive(false);
        playerMR.enabled = false;
        playerCollider.enabled = false;

        //Disable movements
        isDragging = false;
        isDraggingSecond = false;

        Invoke("CallGameOver", 1.0f);

    }
    private void CallGameOver()
    {
        GameUIManager.guiInstance.GameOver(true);
    }
}

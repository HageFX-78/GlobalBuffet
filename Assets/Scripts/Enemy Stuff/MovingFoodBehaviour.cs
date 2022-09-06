using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingFoodBehaviour : MonoBehaviour
{
    private float timeBeforeNextMove;

    Vector3 dir;
    Vector3 displacement;
    float moveDistance;
    private bool initialSpawn = true;

    IEnumerator rm;
    private void Awake()
    {
        rm = RandomMovement();
    }
    private void OnEnable()
    {
        if (!initialSpawn)
        {
            StartCoroutine(rm);
        }

    }
    private void OnDisable()
    {
        if (initialSpawn)
        {
            initialSpawn = false;
        }

    }
    IEnumerator RandomMovement()
    {
        while (gameObject.activeSelf == true)
        {
            dir = Random.insideUnitCircle.normalized;
            moveDistance = Random.Range(5.0f, 10.0f) * transform.localScale.x;
            displacement = dir * moveDistance;

            timeBeforeNextMove = Random.Range(1.0f, 5.0f);
            StartCoroutine(IncrementMovement(displacement, timeBeforeNextMove));

            yield return new WaitForSeconds(timeBeforeNextMove);

        }
    }

    IEnumerator IncrementMovement(Vector3 distance, float dur)
    {
        float splitCount = 200;
        float splitTime = dur / splitCount;
        Vector3 splitDistance = distance / splitCount;

        while (splitCount > 0)
        {

            transform.position += new Vector3(splitDistance.x, 0, splitDistance.y);

            yield return new WaitForSeconds(splitTime);
            splitCount--;
        }
    }
}

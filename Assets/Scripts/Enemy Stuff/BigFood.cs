using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigFood : MonoBehaviour
{
    private void OnEnable()
    {
        //Random grow spurt chance
        if (Random.Range(0, 4) == 0)
        {
            transform.localScale *= 10;
        }
    }
}

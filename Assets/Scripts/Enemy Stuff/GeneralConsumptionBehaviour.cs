using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralConsumptionBehaviour : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("SplitType") && !other.CompareTag("SnappingSplit"))
        {
            float collidedSize = (float)double.Parse(other.gameObject.name);
            if (collidedSize < transform.localScale.x)
            {
                if (other.CompareTag("Player"))
                {
                    AudioManager.amInstance.PlaySF("eat");
                    PlayerMovement.pmInstance.PlayerEaten();
                }
                else
                {
                    other.gameObject.SetActive(false);
                }
                StartCoroutine(SizeUpdate(collidedSize));

            }
        }

    }

    IEnumerator SizeUpdate(float plusValue)
    {

        Vector3 oldScale, oldPos;
        float resizeVal = plusValue / 20;
        float resizeSplitTIme = 0.4f / 20;
        int localLoopCount = 20;

        while (localLoopCount > 0)
        {
            oldScale = transform.localScale;
            oldPos = transform.transform.position;
            transform.localScale = new Vector3(oldScale.x + resizeVal, oldScale.y + resizeVal, oldScale.z + resizeVal);

            localLoopCount--;
            yield return new WaitForSeconds(resizeSplitTIme);
        }
    }
}

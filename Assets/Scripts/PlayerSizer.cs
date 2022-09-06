using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerSizer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI size;

    [SerializeField] public static float baseSize;
    [SerializeField] float sizeOffset;
    [SerializeField] Transform playerTR;

    [SerializeField] float resizeTime;
    [SerializeField] int resizeSplit;
    AudioManager am;

    private void Awake()
    {
        am = AudioManager.amInstance;
    }
    private void Start()
    {
        baseSize = transform.localScale.x;
        gameObject.name = $"{baseSize}";
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SoftType"))
        {
            float collidedSize = (float)double.Parse(other.gameObject.name);
            if (collidedSize < baseSize)
            {
                am.PlaySF("eat");
                StartCoroutine(SizeUpdate(collidedSize));
                other.gameObject.SetActive(false);
                GameUIManager.guiInstance.UpdateScore(100 * Mathf.RoundToInt(baseSize));
            }

        }
        else if (other.CompareTag("HardType"))
        {
            float collidedSize = (float)double.Parse(other.gameObject.name);
            if (baseSize - collidedSize > 3 * baseSize)
            {
                am.PlaySF("eat");
                StartCoroutine(SizeUpdate(collidedSize));
                other.gameObject.SetActive(false);
                GameUIManager.guiInstance.UpdateScore(200 * Mathf.RoundToInt(baseSize));
            }
            else if (baseSize >= collidedSize)
            {
                am.PlaySF("hurt");
                StartCoroutine(SizeUpdate(-collidedSize));
                other.gameObject.SetActive(false);
                GameUIManager.guiInstance.UpdateScore(-100 * Mathf.RoundToInt(baseSize));
            }
        }
    }


    IEnumerator SizeUpdate(float plusValue)
    {
        plusValue = plusValue * sizeOffset;

        Vector3 oldScale;
        float resizeVal = plusValue / resizeSplit;
        float resizeSplitTIme = resizeTime / resizeSplit;
        int localLoopCount = resizeSplit;

        baseSize += plusValue;
        while (localLoopCount > 0)
        {
            oldScale = playerTR.localScale;
            playerTR.localScale = new Vector3(oldScale.x + resizeVal, oldScale.y + resizeVal, oldScale.z + resizeVal);

            gameObject.name = $"{baseSize}";
            size.text = $"{baseSize.ToString("F2")}kg";

            localLoopCount--;
            yield return new WaitForSeconds(resizeSplitTIme);
        }

    }
    public void ExternalCallSizeUpdate(float sizeVal)
    {
        StartCoroutine(SizeUpdate(sizeVal));
    }
}

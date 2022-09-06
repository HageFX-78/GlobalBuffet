using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapCollided : MonoBehaviour
{
    [SerializeField] PlayerMovement parentScriptRef;
    [SerializeField] PlayerSizer parentSizerRef;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SplitType"))
        {
            gameObject.tag = "SplitType";
        }
        else if (other.CompareTag("HardType") && gameObject.tag.Equals("SnappingSplit"))
        {
            parentSizerRef.ExternalCallSizeUpdate((float)double.Parse(other.gameObject.name) * 2);//Double size for snapped targets
            other.gameObject.SetActive(false);
            GameUIManager.guiInstance.UpdateScore(500 * Mathf.RoundToInt(PlayerSizer.baseSize));
        }
    }
}

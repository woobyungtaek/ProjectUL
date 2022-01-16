using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [Header("Popup")]
    [SerializeField]
    private BoxCollider mBoxCollider;

    protected void SetColliderSize()
    {
        mBoxCollider = GetComponent<BoxCollider>();
        if(mBoxCollider == null) { return; }

        RectTransform rectTransform = GetComponent<RectTransform>();
        if(rectTransform == null) { return; }

        mBoxCollider.size =new Vector3(rectTransform.rect.width, rectTransform.rect.height, 1);
    }

}

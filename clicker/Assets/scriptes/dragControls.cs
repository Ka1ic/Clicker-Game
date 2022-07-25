using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class dragControls : MonoBehaviour
{
    [SerializeField] private GameObject particlePref;

    [SerializeField] private float delay = .1f;

    [HideInInspector] public bool isDrag = false;

    private bool isInst = false;

    private void FixedUpdate()
    {
        /*if (Input.touchCount == 0)
        {
            isDrag = false;
            return;
        }

        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)      
        {
            return;
        }

        Debug.Log(EventSystem.current.IsPointerOverGameObject());

        if (EventSystem.current.IsPointerOverGameObject())
        {
            isDrag = false;
            return;
        }

        isDrag = true;

        if (!dinamicDataHolder.isCreateParticles)
            return;

        if (!isInst)
        {
            StartCoroutine(inst(touch));
        }*/
    }
    private void OnMouseDrag()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            isDrag = false;
            return;
        }

        isDrag = true;

        if (!dinamicDataHolder.isCreateParticles)
            return;

        if (!isInst) StartCoroutine(inst());
    }

    private void OnMouseUp()
    {
        isDrag = false;
    }

    private IEnumerator inst()
    {
        isInst = true;

        Instantiate(particlePref, new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0), Quaternion.identity);

        yield return new WaitForSeconds(delay);

        isInst = false;
    }
}

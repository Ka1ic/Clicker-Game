using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class box : MonoBehaviour
{
    [HideInInspector] public boxeType bt;

    private Animator anim;

    private dragControls dc;

    private bool isFall = false;

    private bool isActive = true;

    private bool onDestroy = false;

    public void Awake()
    {
        globalEventManager.sendCountBoxesChanged(1);
    }

    public void wake()
    {
        anim = GetComponent<Animator>();

        if(GameObject.Find("drag zone") != null)
        {
            dc = GameObject.Find("drag zone").GetComponent<dragControls>();
        }

        if (this.gameObject.activeInHierarchy)
            StartCoroutine(fall());
        else
            isActive = false;
    }

    //private void FixedUpdate()
    //{
    //    if (!Input.GetMouseButton(0))
    //        return;

    //    if (EventSystem.current.IsPointerOverGameObject() || dc == null || onDestroy)
    //        return;

    //    if (!dc.isDrag || isFall)
    //        return;

    //    //Debug.Log(Vector2.Distance(this.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition)));

    //    if (Vector2.Distance(this.transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition)) < 2f)
    //        OnMouseUpAsButton();
    //}

    private void OnDisable()
    {
        isActive = false;

        transform.localScale = new Vector3(1, 1, 1);

        if(isFall)
        {
            transform.parent.transform.position = new Vector3(Random.Range(-staticDataHolder.screenLimitationX, staticDataHolder.screenLimitationX), Random.Range(-staticDataHolder.screenLimitationY, staticDataHolder.screenLimitationY));
            
            isFall = false;
        }
    }

    private void OnEnable()
    {
        if (!isActive)
            StartCoroutine(idel());

        isActive = true;
    }

    IEnumerator fall()
    {
        isFall = true;

        Vector2 endPos = new Vector2(transform.position.x, Random.Range(-staticDataHolder.screenLimitationY, staticDataHolder.screenLimitationY));

        float t = 0;

        const float animationDuration = 1.5f;

        StartCoroutine(idel());

        while (t < 1)
        {
            transform.parent.transform.position = Vector2.Lerp(transform.position, endPos, t);

            t += Time.deltaTime / animationDuration;

            yield return null;
        }

        isFall = false;
    }

    private void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject() || dc == null || onDestroy)
            return;

        if (dc.isDrag && !isFall)
            OnMouseUpAsButton();
    }

    //private void OnMouseOver()
    //{
    //    if (!EventSystem.current.IsPointerOverGameObject() && dc != null && !onDestroy)
    //    {
    //        if (dc.isInst && !isFall)
    //        {
    //            OnMouseUpAsButton();
    //        }
    //    }
    //}

    IEnumerator idel()
    {
        anim.Play("idel_box");

        yield return new WaitForSeconds(Random.Range(1.5f, 2.5f));

        StartCoroutine(idel());
    }

    void OnMouseUpAsButton()
    {
        onDestroy = true;

        GameObject newUnit = Instantiate(staticDataHolder.unitPref, transform.position, Quaternion.identity);

        newUnit.GetComponent<mem>().lvl = staticDataHolder.memesLvls[0];

        globalEventManager.sendCountUnitsChanged(newUnit, true);

        globalEventManager.sendCountBoxesChanged(-1);

        globalEventManager.sendFillSBChanged(1);

        Destroy(this.gameObject);
    }
}

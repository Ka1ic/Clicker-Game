using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class mem : MonoBehaviour
{
    public memLvl lvl;

    [SerializeField] private Transform moneySpawnPoint;

    private GameObject moneyPref;

    private float screenLimitationX = staticDataHolder.screenLimitationX;

    private float screenLimitationY = staticDataHolder.screenLimitationY;

    private bool isFacingRight = true;

    private bool isStaying = false;

    private float moneyProduceDelay = 2;

    [HideInInspector] public Coroutine MEMmove;

    private Coroutine rtlz; //return to limit zone

    private float timeOfClick = 0;

    private Vector3 lastMousePosition = new Vector3(0, 0, 0);

    private bool isDrag = false;

    private bool isFirstItaration = true;

    private Vector2 offset;

    private int[] moneyBehavior;

    private int[] moneyBehaviorForTap;

    private int counting = 0;

    private int countingForTap = 0;

    private bool onDestroy = false;

    private bool isMP = false; // is money produce this moment

    private bool isSum = false;

    private bool isStartEnabeled = false;// for correct work on enable and disable

    private bool spaw = false; // for onDestroy in other map

    private bool isMove = false;

    public void wake()
    {
        moneyBehavior = lvl.getMoneyBehavior();

        if(lvl.lvl < 2)
            moneyBehaviorForTap = lvl.getMoneyBehavior();
        else
            moneyBehaviorForTap = staticDataHolder.memesLvls[lvl.lvl - 2].getMoneyBehavior();

        transform.localScale = new Vector3(lvl.sizeX, lvl.sizeY, 1);

        moneyPref = staticDataHolder.moneyPref;

        if (lvl.sprite != null)
        {
            GetComponent<SpriteRenderer>().sprite = lvl.sprite;

            if (transform.Find("left eye") != null && transform.Find("right eye") != null)
            {
                Destroy(transform.Find("left eye").gameObject);

                Destroy(transform.Find("right eye").gameObject);
            }
        }
        else
        {
            GetComponent<SpriteRenderer>().color += new Color(-lvl.lvl % 3 / 2f, -lvl.lvl % 4 / 3f, -lvl.lvl % 5 / 4f);
        }
    }

    private void Start()
    {
        isStartEnabeled = true;

        OnEnable();
    }

    private void startMoving()
    {
        if (!isMove)
        {
            if (MEMmove != null)
                StopCoroutine(MEMmove);

            MEMmove = StartCoroutine(move());

            isMove = true;
        }
    }

    private void OnDisable()
    {
        if (onDestroy)
        {
            if (spaw)
            {
                if(lvl.lvl + 1 == 6 || lvl.lvl + 1 == 12 || lvl.lvl + 1 == 18)
                {
                    GameObject anim = Instantiate(staticDataHolder.unitAnimPref, transform.position, Quaternion.identity);

                    anim.transform.SetParent(transform.parent);

                    anim.GetComponent<memChangeLocation>().wake(staticDataHolder.memesLvls[lvl.lvl + 1]);
                }

                GameObject newUnit = Instantiate(staticDataHolder.unitPref, transform.position, Quaternion.identity);

                newUnit.GetComponent<mem>().lvl = staticDataHolder.memesLvls[lvl.lvl + 1];

                globalEventManager.sendCountUnitsChanged(newUnit, true);

                //text profit
                if(dinamicDataHolder.isCreateTextMoney)
                {
                    GameObject text = Instantiate(staticDataHolder.textPref, transform.position, Quaternion.identity);

                    text.transform.SetParent(transform.parent);

                    text.GetComponent<textOnMoneyIncrease>().wake(newUnit.GetComponent<mem>().lvl.moneyPerSec() - lvl.moneyPerSec() * 2, false);
                }
            }

            globalEventManager.sendCountUnitsChanged(this.gameObject, false);

            Destroy(this.gameObject);
        }
        else
        {
            //isActiveOnCreate = false;

            if (transform.localScale.z > 0)
            {
                transform.localScale = new Vector3(lvl.sizeX, lvl.sizeY, 1);

                isFacingRight = true;
            }
            else
            {
                transform.localScale = new Vector3(lvl.sizeX, lvl.sizeY, -1);

                isFacingRight = false;
            }

            isMove = false;

            isMP = false;
        }
    }

    private void OnEnable()
    {
        if(isStartEnabeled)
        {
            StartCoroutine(enableOn());
        }
    }

    private IEnumerator enableOn()
    {
        yield return new WaitForSeconds(Random.Range(.1f, 1.5f));

        startMoving();

        StartCoroutine(Behavior(moneyProduceDelay));
    }

    private void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
            lastMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseDrag()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (!onDestroy)
            {
                timeOfClick += Time.deltaTime;

                if (isDrag || timeOfClick >= .3f || Mathf.Abs(lastMousePosition.x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x) > .1f || Mathf.Abs(lastMousePosition.y - Camera.main.ScreenToWorldPoint(Input.mousePosition).y) > .1f)
                {
                    if (isFirstItaration)
                    {
                        isDrag = true;

                        isFirstItaration = false;

                        offset.x = transform.position.x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x;

                        offset.y = transform.position.y - Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
                    }

                    if (MEMmove != null && isMove)
                    {
                        StopCoroutine(MEMmove);

                        isMove = false;
                    }

                    if (rtlz != null)
                    {
                        StopCoroutine(rtlz);
                    }

                    Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

                    transform.position = new Vector3(Camera.main.ScreenToWorldPoint(mousePos).x + offset.x, Camera.main.ScreenToWorldPoint(mousePos).y + offset.y, transform.position.z);

                    if (lastMousePosition.x < Camera.main.ScreenToWorldPoint(Input.mousePosition).x && !isFacingRight)
                        flip();

                    if (lastMousePosition.x > Camera.main.ScreenToWorldPoint(Input.mousePosition).x && isFacingRight)
                        flip();
                }

                lastMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }
    }

    private void OnMouseUp()
    {
        if (onDestroy)
            return;

        isSum = false;

        checkDistance();

        timeOfClick = 0;

        if (isDrag)
        {
            isFirstItaration = true;

            isDrag = false;

            if (Mathf.Abs(transform.position.x) > screenLimitationX || Mathf.Abs(transform.position.y) > screenLimitationY)
            {
                rtlz = StartCoroutine(returnToLimitZone());
            }
            else
            {
                if (!isSum)
                {
                    startMoving();
                }
            }
        }
        else
        {
            if (!isMP)
            {
                StartCoroutine(moneyProduce(true));
            }
        }
    }

    private void checkDistance()
    {
        if(lvl.lvl < 6)
        {
            if (dinamicDataHolder.allUnitsSmallMap.Count <= 1) return;

            int indexOfNearest = 0;

            if (dinamicDataHolder.allUnitsSmallMap[0] == this.gameObject)
            {
                indexOfNearest = 1;
            }

            for (int i = 0; i < dinamicDataHolder.allUnitsSmallMap.Count; i++)
            {
                if (dinamicDataHolder.allUnitsSmallMap[i] != this.gameObject && dinamicDataHolder.allUnitsSmallMap[i].GetComponent<mem>().lvl.lvl == lvl.lvl)
                {
                    if (Vector2.Distance(dinamicDataHolder.allUnitsSmallMap[i].transform.position, this.gameObject.transform.position) < Vector2.Distance(dinamicDataHolder.allUnitsSmallMap[indexOfNearest].transform.position, this.gameObject.transform.position))
                    {
                        indexOfNearest = i;
                    }
                }
            }

            if (Vector2.Distance(dinamicDataHolder.allUnitsSmallMap[indexOfNearest].transform.position, this.gameObject.transform.position) < 1.5f && dinamicDataHolder.allUnitsSmallMap[indexOfNearest].GetComponent<mem>().lvl.lvl == lvl.lvl)
            {
                if(lvl.lvl + 1 == 6 && dinamicDataHolder.allUnitsMiddleMap.Count >= 16)
                {
                    GameObject.Find("Canvas").transform.Find("menu limit exceeded").gameObject.SetActive(true);
                }
                else
                {
                    globalEventManager.itemsSum(new GameObject[] { this.gameObject, dinamicDataHolder.allUnitsSmallMap[indexOfNearest] });

                    isSum = true;
                }
            }
        }
        else if(lvl.lvl < 12)
        {
            if (dinamicDataHolder.allUnitsMiddleMap.Count <= 1) return;

            int indexOfNearest = 0;

            if (dinamicDataHolder.allUnitsMiddleMap[0] == this.gameObject)
            {
                indexOfNearest = 1;
            }

            for (int i = 0; i < dinamicDataHolder.allUnitsMiddleMap.Count; i++)
            {
                if (dinamicDataHolder.allUnitsMiddleMap[i] != this.gameObject && dinamicDataHolder.allUnitsMiddleMap[i].GetComponent<mem>().lvl.lvl == lvl.lvl)
                {
                    if (Vector2.Distance(dinamicDataHolder.allUnitsMiddleMap[i].transform.position, this.gameObject.transform.position) < Vector2.Distance(dinamicDataHolder.allUnitsMiddleMap[indexOfNearest].transform.position, this.gameObject.transform.position))
                    {
                        indexOfNearest = i;
                    }
                }
            }

            if (Vector2.Distance(dinamicDataHolder.allUnitsMiddleMap[indexOfNearest].transform.position, this.gameObject.transform.position) < 1.5f && dinamicDataHolder.allUnitsMiddleMap[indexOfNearest].GetComponent<mem>().lvl.lvl == lvl.lvl)
            {
                if (lvl.lvl + 1 == 12 && dinamicDataHolder.allUnitsBigMap.Count >= 16) 
                {
                    GameObject.Find("Canvas").transform.Find("menu limit exceeded").gameObject.SetActive(true);
                }
                else
                {
                    globalEventManager.itemsSum(new GameObject[] { this.gameObject, dinamicDataHolder.allUnitsMiddleMap[indexOfNearest] });

                    isSum = true;
                }
            }
        }
        else if(lvl.lvl < 18)
        {
            if (dinamicDataHolder.allUnitsBigMap.Count <= 1) return;

            int indexOfNearest = 0;

            if (dinamicDataHolder.allUnitsBigMap[0] == this.gameObject)
            {
                indexOfNearest = 1;
            }

            for (int i = 0; i < dinamicDataHolder.allUnitsBigMap.Count; i++)
            {
                if (dinamicDataHolder.allUnitsBigMap[i] != this.gameObject && dinamicDataHolder.allUnitsBigMap[i].GetComponent<mem>().lvl.lvl == lvl.lvl)
                {
                    if (Vector2.Distance(dinamicDataHolder.allUnitsBigMap[i].transform.position, this.gameObject.transform.position) < Vector2.Distance(dinamicDataHolder.allUnitsBigMap[indexOfNearest].transform.position, this.gameObject.transform.position))
                    {
                        indexOfNearest = i;
                    }
                }
            }

            if (Vector2.Distance(dinamicDataHolder.allUnitsBigMap[indexOfNearest].transform.position, this.gameObject.transform.position) < 1.5f && dinamicDataHolder.allUnitsBigMap[indexOfNearest].GetComponent<mem>().lvl.lvl == lvl.lvl)
            {
                if (lvl.lvl + 1 == 18)
                {
                    GameObject.Find("Canvas").transform.Find("menu limit exceeded").gameObject.SetActive(true);
                }
                else
                {
                    globalEventManager.itemsSum(new GameObject[] { this.gameObject, dinamicDataHolder.allUnitsBigMap[indexOfNearest] });

                    isSum = true;
                }
            }
        }
    }

    private IEnumerator returnToLimitZone()
    {
        if(!onDestroy)
        {
            Vector3 endPos = new Vector3(Random.Range(-screenLimitationX, screenLimitationX), Random.Range(-screenLimitationY, screenLimitationY), transform.position.z);

            float t = 0;

            const float animationDuration = 5f;

            if (endPos.x > transform.position.x && !isFacingRight)
                flip();

            if (endPos.x < transform.position.x && isFacingRight)
                flip();

            while (t < 1)
            {
                transform.position = Vector2.Lerp(transform.position, endPos, t);

                t += Time.deltaTime / animationDuration;

                yield return null;
            }

            startMoving();
        }
    }

    private IEnumerator move()
    {
        Vector2 startPos = transform.position;

        Vector2 endPos = new Vector2(transform.position.x + .25f * Random.Range(-1, 2), transform.position.y + .25f * Random.Range(-1, 2));

        if (endPos.x == transform.position.x && endPos.y == transform.position.y)
        {
            if (isStaying)
            {
                int[] offset = { -1, 1 };

                endPos = new Vector2(transform.position.x + .25f * offset[Random.Range(0, 2)], transform.position.y + .25f * offset[Random.Range(0, 2)]);

                isStaying = false;

                if (endPos.x > screenLimitationX)
                {
                    endPos.x -= .5f;
                }
                else if (endPos.x < -screenLimitationX)
                {
                    endPos.x += .5f;
                }
                else if (endPos.y > screenLimitationY)
                {
                    endPos.y -= .5f;
                }
                else if (endPos.y < -screenLimitationY)
                {
                    endPos.y += .5f;
                }
            }
            else
            {
                isStaying = true;
            }
        }
        else
        {
            isStaying = false;

            if (endPos.x > screenLimitationX)
            {
                endPos.x -= .5f;
            }
            else if (endPos.x < -screenLimitationX)
            {
                endPos.x += .5f;
            }
            else if (endPos.y > screenLimitationY)
            {
                endPos.y -= .5f;
            }
            else if (endPos.y < -screenLimitationY)
            {
                endPos.y += .5f;
            }
        }

        if ((isFacingRight && endPos.x < transform.position.x) || (!isFacingRight && endPos.x > transform.position.x))
        {
            flip();
        }

        float t = 0;

        const float animationDuration = 1.5f;

        while (t < 1)
        {
            transform.position = Vector2.Lerp(transform.position, endPos, t);

            t += Time.deltaTime / animationDuration;

            yield return null;
        }

        yield return new WaitForSeconds(2);

        MEMmove = StartCoroutine(move());
    }

    private void flip()
    {
        if(transform.localScale.x < 0)
        {
            isFacingRight = true;

            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
        else
        {
            isFacingRight = false;

            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
    }

    private IEnumerator Behavior(float delay)
    {
        yield return new WaitForSeconds(delay);

        if(!isMP)
        {
            StartCoroutine(moneyProduce());
        }

        StartCoroutine(Behavior(delay));
    }

    private IEnumerator moneyProduce(bool isTap = false)
    {
        isMP = true;

        int speed = 7;

        Vector2 scaleAtStart = new Vector2(transform.localScale.x, transform.localScale.y);

        for (int i = 0; i < speed; i++)
        {
            if ((transform.localScale.x > 0 && scaleAtStart.x < 0) || (transform.localScale.x < 0 && scaleAtStart.x > 0))
            {
                scaleAtStart.x *= -1;
            }

            transform.localScale = new Vector3(transform.localScale.x + scaleAtStart.x * .1f / speed, transform.localScale.y - scaleAtStart.y * .25f / speed);

            yield return new WaitForSeconds(.01f);
        }

        GameObject moneyItem = Instantiate(moneyPref, moneySpawnPoint.position, Quaternion.identity);

        moneyItem.transform.SetParent(transform.parent);

        if(!isTap)
        {
            counting++;

            if (counting >= moneyBehavior.Length)
            {
                counting = 0;
            }

            moneyItem.GetComponent<money>().on(staticDataHolder.coinsLvls[moneyBehavior[counting]]);
        }
        else
        {
            countingForTap++;

            if (countingForTap >= moneyBehaviorForTap.Length)
            {
                countingForTap = 0;
            }

            moneyItem.GetComponent<money>().on(staticDataHolder.coinsLvls[moneyBehaviorForTap[countingForTap]]);
        }

        for (int i = 0; i < speed; i++)
        {
            if ((transform.localScale.x > 0 && scaleAtStart.x < 0) || (transform.localScale.x < 0 && scaleAtStart.x > 0))
            {
                scaleAtStart.x *= -1;
            }

            transform.localScale = new Vector3(transform.localScale.x - scaleAtStart.x * .1f / speed, transform.localScale.y + scaleAtStart.y * .25f / speed);

            yield return new WaitForSeconds(.01f);
        }

        if ((transform.localScale.x > 0 && scaleAtStart.x < 0) || (transform.localScale.x < 0 && scaleAtStart.x > 0))
        {
            scaleAtStart.x *= -1;
        }

        if (transform.localScale.x != scaleAtStart.x || transform.localScale.y != scaleAtStart.y)
        {
            transform.localScale = new Vector3(scaleAtStart.x, scaleAtStart.y);
        }

        isMP = false;
    }

    public IEnumerator sum(Vector3 center, bool spawn)
    {
        onDestroy = true;

        spaw = spawn;

        for(float i = 0; i < 1; i += .05f)
        {
            transform.position = Vector2.Lerp(transform.position, center, i);

            yield return new WaitForSeconds(.02f);
        }

        //if (spawn)
        //{
        //    GameObject newUnit = Instantiate(staticDataHolder.unitPref, transform.position, Quaternion.identity);

        //    newUnit.GetComponent<mem>().lvl = staticDataHolder.memesLvls[lvl.lvl + 1];

        //    globalEventManager.sendCountUnitsChanged(newUnit, true);
        //}

        //globalEventManager.sendCountUnitsChanged(this.gameObject, false);

        this.gameObject.SetActive(false);
        //Destroy(this.gameObject);
    }
}


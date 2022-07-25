using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapsController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] plug;

    [SerializeField] private GameObject canvas;

    [SerializeField] private GameObject[] buttons;

    private Camera cam;

    private int indexCurrentMap = 0;

    private bool scale = false;

    private float screenRatio = (float)Screen.height / (float)Screen.width * 5.625f;

    private void Awake()
    {
        cam = Camera.main.transform.Find("cam").gameObject.GetComponent<Camera>();

        globalEventManager.onNewMemUnlocked.AddListener(newMemInvented);
    }

    private void Start()
    {
        if (dinamicDataHolder.indexLastUnlockedMem >= 6)
        {
            buttons[0].transform.parent.gameObject.SetActive(true);
        }
        if(dinamicDataHolder.indexLastUnlockedMem >= 12)
            buttons[2].SetActive(true);

        StartCoroutine(moneyControls());
    }

    public void changeMap(int indexOfMap)
    {
        if(indexOfMap > indexCurrentMap && !scale)
        {
            StartCoroutine(cameraScale(true, indexOfMap));

            indexCurrentMap = indexOfMap;
        }
        else if(indexOfMap < indexCurrentMap && !scale)
        {
            StartCoroutine(cameraScale(false, indexOfMap));

            indexCurrentMap = indexOfMap;
        }
    }

    private IEnumerator cameraScale(bool isBringCloser, int indexOfMap)
    {
        scale = true;

        if(isBringCloser)
        {
            Texture2D screenShot = makeScreenshot();

            plug[0].sprite = Sprite.Create(screenShot, new Rect(0, 0, screenShot.width, screenShot.height), new Vector2(.5f, .5f), 100);

            plug[0].gameObject.SetActive(true);

            Camera.main.orthographicSize = screenRatio / 100f;

            staticDataHolder.maps[indexCurrentMap].SetActive(false);

            staticDataHolder.maps[indexOfMap].SetActive(true);

            for (float i = screenRatio / 100f; i <= screenRatio; i += i * .1f)
            {
                Camera.main.orthographicSize = i;

                yield return new WaitForSeconds(.005f);
            }

            Camera.main.orthographicSize = screenRatio;

            plug[0].gameObject.SetActive(false);
        }
        else
        {
            Texture2D screenShot = makeScreenshot();

            plug[1].sprite = Sprite.Create(screenShot, new Rect(0, 0, screenShot.width, screenShot.height), new Vector2(.5f, .5f), 100);

            plug[1].gameObject.SetActive(true);

            Camera.main.orthographicSize = screenRatio * 100f;

            staticDataHolder.maps[indexCurrentMap].SetActive(false);

            staticDataHolder.maps[indexOfMap].SetActive(true);

            for (float i = screenRatio * 100f; i >= screenRatio; i -= i / 10f)
            {
                Camera.main.orthographicSize = i;

                yield return new WaitForSeconds(.01f);
            }

            Camera.main.orthographicSize = screenRatio;

            plug[1].gameObject.SetActive(false);
        }

        scale = false;
    }

    private Texture2D makeScreenshot()
    {
        RenderTexture currentRT = RenderTexture.active;
        // Создаем текстуру, в которую будем рендерить
        // Создаем текстуру, пропорциональную размеру террейна
        RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 24);

        // Устанавливаем созданную текстуру как целевую
        RenderTexture.active = rt;
        cam.targetTexture = rt;

        // Принудительно вызываем рендер камеры
        cam.Render();

        // Получаем обычную текстуру из RenderTexture'ы
        // Ее можно будет использовать в игре, или же
        // Сохранить, что мы и сделаем
        Texture2D screenShot = new Texture2D(rt.width, rt.height);
        screenShot.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        screenShot.Apply();

        RenderTexture.active = currentRT;
        cam.targetTexture = null;

        Destroy(rt);

        return screenShot;
    }

    private void newMemInvented(int indexOfInvented)
    {
        if (dinamicDataHolder.indexLastUnlockedMem >= 6)
        {
            buttons[0].transform.parent.gameObject.SetActive(true);
        }
        if (dinamicDataHolder.indexLastUnlockedMem >= 12)
            buttons[2].SetActive(true);
    }

    private IEnumerator moneyControls()
    {
        int profit = 0;

        for(int i = 0; i < staticDataHolder.maps.Length; i++)
        {
            if(i != indexCurrentMap)
            {
                for(int j = 0; j < staticDataHolder.maps[i].transform.childCount; j++)
                {
                    if(staticDataHolder.maps[i].transform.GetChild(j).CompareTag("memItem"))
                        profit += (int)(staticDataHolder.maps[i].transform.GetChild(j).GetComponent<mem>().lvl.moneyPerSec() * 2);
                }
            }
        }

        yield return new WaitForSeconds(2);

        globalEventManager.sendMoneyCountChanged(profit);

        StartCoroutine(moneyControls());
    }
}

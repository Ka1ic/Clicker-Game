using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class lvlLaoder : MonoBehaviour
{
    [SerializeField] private GameObject[] loadImages;

    void Start()
    {
        StartCoroutine(LoadWithProgres(1));
    }

    IEnumerator LoadWithProgres(int sceneIndex)
    {
        yield return new WaitForSeconds(.5f);

        for (int i = 0; i < 11; i++)
        {
            if (i >= 9)
            {
                loadImages[3].SetActive(true);
            }
            else if (i >= 6)
            {
                loadImages[2].SetActive(true);
            }
            else if (i >= 3)
            {
                loadImages[1].SetActive(true);
            }

            yield return new WaitForSeconds(.1f);
        }

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            yield return null;
        }
    }
}

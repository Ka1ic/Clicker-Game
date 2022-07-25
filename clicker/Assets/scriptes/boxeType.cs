using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "new boxeType", menuName = "boxeType")]
public class boxeType : ScriptableObject
{
    public Sprite sprite;

    public int indexOfType;

    [HideInInspector] public string nameOfType;

    private void Awake()
    {
        string[] types = new string[] { "standart box", "rare box" };

        nameOfType = types[indexOfType];
    }
}

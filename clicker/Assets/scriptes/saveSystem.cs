using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public static class saveSystem
{
    private static string path = Application.persistentDataPath + "/data.data";

    public static void saveDinamicDataHolder()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        FileStream stream = new FileStream(path, FileMode.Create);

        dinamicDataHolderData data = new dinamicDataHolderData(false);

        formatter.Serialize(stream, data);

        stream.Close();
    }

    public static dinamicDataHolderData loadDinamicDataHolder()
    {
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(path, FileMode.Open);

            dinamicDataHolderData data = formatter.Deserialize(stream) as dinamicDataHolderData;

            stream.Close();

            return data;
        }
        else
        {
            return null;
        }
    }

    public static void clearData()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        FileStream stream = new FileStream(path, FileMode.Create);

        dinamicDataHolderData data = new dinamicDataHolderData(true);

        formatter.Serialize(stream, data);

        stream.Close();
    }
}

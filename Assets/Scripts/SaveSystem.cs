/*using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public static class SaveSystem
{
    public static void SavePlayer(PlayerController playerController)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/player.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(playerController);

        formatter.Serialize(stream, data);
        stream.Close();

    }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        }

        else
        {
            Debug.LogError("Сохраненный файл не найден в " + path);
            return null;
        }
    }
}
*/
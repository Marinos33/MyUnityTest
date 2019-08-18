using UnityEngine;
using System.IO; //using pour acceder a des fichier supplementaire hors du projet et qui ne sont pas des script
using System.Runtime.Serialization.Formatters.Binary; // using pour lire des fichier en binaire
public static class SaveSystem
{
    public static void Save(someValues values)
    {
        BinaryFormatter formatter = new BinaryFormatter(); // createur/lecteur de fichier binaire
        string path = Application.persistentDataPath + "/saves.any"; //localisation du fichier de sauvegarde (peu importe le nom aprés le . c'est le nom de l'extension du fichier
        Debug.Log(Application.persistentDataPath + "/saves.any");
        FileStream stream = new FileStream(path, FileMode.Create); // read & write system (ici crée ou update le fichier de sauvegarde)

        Data data = new Data(values);

        formatter.Serialize(stream, data); // lit le script
        stream.Close(); // ferme le mode lecture
    }

    public static Data Load()
    {
        string path = Application.persistentDataPath + "/saves.any";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open); // read & write system (ici lit le fichier)

            Data data = formatter.Deserialize(stream) as Data ; //envoie des donnée du fichier dans un objet possédant les variables d'ou sont tirée les donnée
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("no save files in " + path);
            return null;
        }
    }
}

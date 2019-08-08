using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//https://www.youtube.com/watch?v=tdSmKaJvCoA&list=WL&index=7&t=0s

    /*le script a pour but que lorsque on veut faire spawner des objets on le reutiliser quand il n'est plus utile au lieu de supprimer et recréer a l'infini 
    (+ optimisé que instantiate dans une boucle)*/
public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    //classe pour stocker et definir les gameobjects du tableau poolDictionnary
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }
    //le singleton permet de grab les propriété (gameobjetc, fonction, etc) et les envoyer dans un autre script( pour plus d'info sur le singleton voir http://wiki.unity3d.com/index.php/Singleton)
    #region Singleton

    public static ObjectPooler Instance;
    private void Awake()
    {
        Instance = this;
    }

    #endregion

    public List<Pool> pools;
    //dictionary est un tableau speciale permmettant de recuperer facilement un objet a un emplacement precis du tableau, ici c'est le premier (queue) qui a comme type un gameobject
    public Dictionary<string, Queue<GameObject>> poolDictionnary;
    
    private void Start()
    {
        poolDictionnary = new Dictionary<string, Queue<GameObject>>();

        // met les element de la list pools dans le dictionary
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>(); //crée la queue de la liste

            for(int i=0;i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab); //place la prefab dans un gameobject
                obj.SetActive(false); // desactive l'objet
                objectPool.Enqueue(obj); //place l'objet dans la queue
            }

            poolDictionnary.Add(pool.tag, objectPool); //ajoute l'objet dans le dictionnaire (d'abord le tag de l'objet puis l'objet) l'objet et référencé par son tag un peu comme un tableau associatif)
        }
    }

    public GameObject SpawnFromPool (string tag, Vector3 position, Quaternion rotation)
    {
        //verifie si le tag rentrer dans le public string existe
        if (!poolDictionnary.ContainsKey(tag))
        {
            Debug.LogWarning("pool with tag " + tag + " doesn't exist");
            return null;
        }


        GameObject objectToSpawn  = poolDictionnary[tag].Dequeue(); // reference l'objet actuel du dictionnaire dans un gameobject

        objectToSpawn.SetActive(true); //active l'objet
        objectToSpawn.transform.position = position; //set la position
        objectToSpawn.transform.rotation = rotation; // set la rotation

        IPooledObject pooledObj =  objectToSpawn.GetComponent<IPooledObject>(); //recupere l'interface du gameobject

        //applique le script de l'interface si il y a un  objet
        if(pooledObj != null)
        {
            pooledObj.onObjectSpawn();
        }

        poolDictionnary[tag].Enqueue(objectToSpawn);//place l'objet dans la queue
        return objectToSpawn;
    }

}

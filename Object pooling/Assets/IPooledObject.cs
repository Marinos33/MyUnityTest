using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// permet a tous les objet derivée de cette interface de posseder les proprieté de l'interface
public interface IPooledObject
{
    void onObjectSpawn();
}

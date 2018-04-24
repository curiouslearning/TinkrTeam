using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerInitializer : MonoBehaviour {

    public GameManager gameManager;

    public bool makePersistent = true;

    void Awake()
    {
        if (!GameManager.Instance)
        {
            Debug.Log("NO GAME MANAGER INSTANCE FOUND - CREATING ONE!");
            GameManager instance = Instantiate(gameManager) as GameManager;

            if (makePersistent)
                DontDestroyOnLoad(instance.gameObject);
        }

        Destroy(this.gameObject);
    }
   
}

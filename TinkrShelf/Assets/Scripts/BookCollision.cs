using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class BookCollision : ShelfManager
{
    public static GameObject bookenter = null;
    public static GameObject bookexit = null;

    public static int count = 0;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update()
    {
       

    }
    void OnTriggerEnter(Collider collider)
    {


        if (collider.gameObject.name == "Entry")
        {
            Debug.Log("book enter is");
            bookenter = this.gameObject;
            count++;
            Debug.Log(count);
           
            

        }
        else if (collider.gameObject.name == "Exit")
        {
            Debug.Log("book exit is");
            bookexit = this.gameObject;
            count++;
            Debug.Log(count);

        }
        if (count == 2)
        {
            if (Navigation.arrowleft == false)
            {
                loadbookdata(bookenter, bookexit);
            }
            else
            {
                loadbookdata(bookexit, bookenter);
                Navigation.arrowleft = false;
            }
            count = 0;

        }


    }
    
}

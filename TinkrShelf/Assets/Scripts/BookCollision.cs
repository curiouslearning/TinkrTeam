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
        else if (collider.gameObject.name == "one")
        {
            this.gameObject.GetComponent<BookObject>().position = 1;
            
        }
        else if (collider.gameObject.name == "two")
        {
            this.gameObject.GetComponent<BookObject>().position = 2;
            
        }
        else if (collider.gameObject.name == "three")
        {
            this.gameObject.GetComponent<BookObject>().position = 3;
 
        }
        else if (collider.gameObject.name == "four")
        {
            this.gameObject.GetComponent<BookObject>().position = 4;

        }
        else if(collider.gameObject.name == "five")
        {
            this.gameObject.GetComponent<BookObject>().position = 5;

        }
        if (count == 2)
        {
            if (arrowright==true || arrowright60==true)  //right arrow
            {
                loadbookdata(bookenter, bookexit);
            }
            else if(arrowleft==true || arrowleft60==true)   //left arrow
            {
                loadbookdata(bookexit, bookenter);
               
            }
            count = 0;

        }


    }
    
}

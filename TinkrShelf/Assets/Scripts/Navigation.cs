using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Navigation : MonoBehaviour{
    bool check = false;
    int count = 0;
    string name = "";
    public static bool arrowleft=false;
    public GameObject bookwheel;
    public void Update()
    {
        if(check==true)
        {
            if(name=="left")
                {
                bookwheel.transform.Rotate(0, 0, 1);
            }
            else
            {
                bookwheel.transform.Rotate(0, 0, -1);
            }
            count++;
            if(count==30)
            {
                check = false;
            }
        }
    }


    public void left()
    {
        count = 0;
        check = true;
        name = "left";
        arrowleft = true;
    }
    public void right()
    {
        count = 0;
        check = true;
        name = "right";
    }
}

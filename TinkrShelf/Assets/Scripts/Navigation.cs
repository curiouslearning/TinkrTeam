using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Navigation : MonoBehaviour{
    bool check = false;
    int count = 0;
    string name = "";
    public void Update()
    {
        if(check==true)
        {
            if(name=="left")
                {
                transform.Rotate(0, 0, 1);
            }
            else
            {
                transform.Rotate(0, 0, -1);
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
    }
    public void right()
    {
        count = 0;
        check = true;
        name = "right";
    }
}

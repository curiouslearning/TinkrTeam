using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager18 : SManager
{
    public GameObject chewinggo;
    public GameObject burpgo;
    public GameObject chewing1;
    public GameObject chewing2;
    public GameObject chewing3;

    public GameObject burp;
    
    public override void Update()
        
    {
        base.Update();
        if (chewing1.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("removed"))
        {
            chewinggo.SetActive(false);
            burpgo.SetActive(true);
            StartCoroutine(waitForTime());
        }
    }

    IEnumerator waitForTime()
    {
        yield return new WaitForSeconds(4.33f);
        NextScene();
    }


}

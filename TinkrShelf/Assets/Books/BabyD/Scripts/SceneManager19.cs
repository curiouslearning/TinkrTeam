using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager19 : SManager {


	// Use this for initialization
	public override void Start () {
        base.Start();
        StartCoroutine(PlayNonLoopSound(0,0.03f));
    }
	


}

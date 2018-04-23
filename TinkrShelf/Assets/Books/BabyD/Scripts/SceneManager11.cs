using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager11 : SManager
{
    public GameObject frog;

    // Use this for initialization
	public override void Start() {
		base.Start ();
        frog.GetComponent<SpriteRenderer>().color = GameManager.red;
    }
    public override void OnPairedMouseDown(TinkerText tinkerText)
    {

        tinkerText.pairedGraphic.gameObject.GetComponent<SpriteRenderer>().color = GameManager.yellow;

    }
    public override void OnPairedMouseUp(TinkerText tinkerText)
    {
        frog.GetComponent<SpriteRenderer>().color = GameManager.red;    
    }
}
using System.Collections;
using UnityEngine;
public class SceneManager01 : SManager {
	public GameObject graphicEgg;
	public GameObject eggCrackLastClip;
	public GameObject eggCrack;
	private int noOfTaps;
    TinkerGraphic checkTap;

	public override void Start() {
		base.Start ();
		noOfTaps = 0;
	}
	public override void Update() {
        base.Update();
		if (eggCrack.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("removed"))
		{
			eggCrackLastClip.SetActive(true);
			StartCoroutine(waitForTime());
		}
	}
	//public GameObject eggShellBit1;
	//public GameObject eggShellBit2;

	//speed=no of frames/time
	IEnumerator waitForTime()
	{
		yield return new WaitForSeconds(2.33f);
        NextScene();
    }


	public override void OnMouseDown(GameObject go)
	{
        base.OnMouseDown(go);
        
		if (go.GetComponent<TinkerText>() != null)
		{
			stanzaManager.OnMouseDown (go.GetComponent<TinkerText> ());
			if (go.name == "tap") {
                StartCoroutine(PlayNonLoopSound(0));
				if (noOfTaps < 3) {
					noOfTaps++;
					graphicEgg.GetComponent<Animator> ().SetTrigger ("crack"+noOfTaps);
				} else {
					graphicEgg.GetComponent<Animator> ().SetTrigger ("crack3");
					noOfTaps = 0;
				}
			}
		}

	}

	public override void OnMouseDown(TinkerGraphic tinkerGraphic)
	{
       
		if (tinkerGraphic.name == "eggcrack")
		{
            Debug.Log("down");
            checkTap = tinkerGraphic;
            StartCoroutine(PlayNonLoopSound(0));
            if (noOfTaps < 2)
			{
				noOfTaps++;
				tinkerGraphic.pairedText1.clipPlay();
				graphicEgg.GetComponent<Animator>().SetTrigger("crack" + noOfTaps);

				//eggShellBit1.GetComponent<Animator>().SetTrigger("shellcrack");
				//eggShellBit2.GetComponent<Animator>().SetTrigger("shellcrack");
			}
			else
			{
				graphicEgg.GetComponent<Animator>().SetTrigger("crack3");
				tinkerGraphic.pairedText2.clipPlay();
				noOfTaps = 0;
			}
		}

	}

 

    public override void OnMouseUp(TinkerGraphic tinkerGraphic)
	{
     
        if (tinkerGraphic.name == "eggcrack" && checkTap!= null)
		{
            Debug.Log("up");
            checkTap = null;
			if (noOfTaps < 3)
			{
				if (noOfTaps == 0)
				{
                    tinkerGraphic.pairedText2.clipResume();
				}
				else
				    tinkerGraphic.pairedText1.clipResume();
			}
			else
            {
                tinkerGraphic.pairedText1.clipResume();
				noOfTaps = 0;
			}
		}

	}
}

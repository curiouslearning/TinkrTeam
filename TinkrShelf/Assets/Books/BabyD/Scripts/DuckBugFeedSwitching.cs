using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DuckBugFeedSwitching : MonoBehaviour {

	public GameObject duckIdle;
	public GameObject duckChew;
	public GameObject duckMouthOpen;
	public GameObject bugInMouth;
	public GameObject handHint;
	public static int bugCounter;
	public SManager sceneManager;


	// Use this for initialization
	void Start () {
		bugCounter = 1;
	}


	void OnTriggerEnter(Collider collider)
	{   
		//num.text = bugCounter.ToString ();
		duckIdle.SetActive(false);  
		duckChew.SetActive (false);
		duckMouthOpen.SetActive (true); //mouth open
		bugInMouth = collider.gameObject;
		Debug.Log (bugInMouth);
	}

	void OnTriggerExit(Collider collider)
	{
		bugInMouth = null;
		duckIdle.SetActive (false);
		duckMouthOpen.SetActive (true);
		duckChew.SetActive (false);
	}



	public void FeedDuckOnMouseUp()
	{   //num.text = "";
		
		if (bugInMouth) {
			Destroy (bugInMouth);
			handHint.SetActive (false);
			duckIdle.SetActive (false);
			duckMouthOpen.SetActive (false);
			duckChew.SetActive (true);
			StartCoroutine (SetChewFalse ());
            PlayAudioCount();
			bugCounter++;
			if (bugCounter == 9) {
				sceneManager.NextScene ();
			}
		} if(bugInMouth==null)
		  {
			duckIdle.SetActive (true);
			duckMouthOpen.SetActive (false);
			duckChew.SetActive (false);}
		    
	}

    public void PlayAudioCount()
    {

        AudioClip clip1 = (AudioClip)Resources.Load("Audio/VO/child_" + bugCounter);
        gameObject.GetComponent<AudioSource>().PlayOneShot(clip1);
    }


	 public IEnumerator SetChewFalse()
		{yield return new WaitForSeconds (2.0f);
		duckChew.SetActive (false);
		duckIdle.SetActive (true);
			
		}
	}


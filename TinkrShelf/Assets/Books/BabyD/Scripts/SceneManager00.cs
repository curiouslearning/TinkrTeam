using System.Collections;
using UnityEngine;

public class SceneManager00 : SManager {

    public override void Start()
    {
        base.Start();
        StartCoroutine(PlayLoopingSound(0,0.49f,0.5f));
    }
    public override void OnMouseDown(GameObject go)
    {
		if (go.name == "egg_anim")
		{
            StartCoroutine(PlayNonLoopSound(1,getAudioLength(0)));
            StartCoroutine(waitForTime());
		}
    }
   

    IEnumerator waitForTime()
    {
        //play egg crack and then go to next Scene
        yield return new WaitForSeconds(GetComponents<AudioSource>()[1].clip.length);
        NextScene();
    }

}

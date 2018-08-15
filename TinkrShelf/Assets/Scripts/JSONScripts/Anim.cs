using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Anim  {
    public int id;
    public string animName;
    public int numberOfImages;
	public bool onTouch;
	public bool onStart;
	public float[] secPerFrame;
	public Sequence[] sequences; 
}

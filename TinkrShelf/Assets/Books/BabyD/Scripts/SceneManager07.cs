using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager07 : SManager
{
    //waterdrops
    public GameObject fallingWater;
    public GameObject fallingRed;
    public GameObject fallingYellow;
    public GameObject fallingBlue;

    //leaves
    public GameObject waterLeaf;
    public GameObject redLeaf;
    public GameObject blueLeaf;
    public GameObject yellowLeaf;
    //babyd components
    public GameObject babyD;
    public GameObject water;
    public GameObject movingArms;
    //public GameObject[] bubbles;
    public GameObject bubble1;
    public GameObject bubble2;
    public GameObject bubble3;
    public GameObject bubble4;
    public GameObject bubble5;
    public GameObject bubble6;
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer spriteRendererWater;
    private SpriteRenderer spriteRendererMovingArms;
    private SpriteRenderer bubbleSprite1;
    private SpriteRenderer bubbleSprite2;
    private SpriteRenderer bubbleSprite3;
    private SpriteRenderer bubbleSprite4;
    private SpriteRenderer bubbleSprite5;
    private SpriteRenderer bubbleSprite6;
    // Use this for initialization
	public override void Start() {
       
		base.Start ();
        ChangeColor(GameManager.white);
        //Fetch the SpriteRenderer from the GameObject

        spriteRenderer = babyD.GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.white;
        spriteRendererWater = water.GetComponent<SpriteRenderer>();
  
        bubbleSprite1 = bubble1.GetComponent<SpriteRenderer>();
        bubbleSprite2 = bubble2.GetComponent<SpriteRenderer>();
        bubbleSprite3 = bubble3.GetComponent<SpriteRenderer>();
        bubbleSprite4 = bubble4.GetComponent<SpriteRenderer>();
        bubbleSprite5 = bubble5.GetComponent<SpriteRenderer>();
        bubbleSprite6 = bubble6.GetComponent<SpriteRenderer>();

        spriteRendererMovingArms = movingArms.GetComponent<SpriteRenderer>();
    }
    public override void OnMouseDown(GameObject go)
    {
        base.OnMouseDown(go);
        Color duckcolor = getDuckColor();
        if (duckcolor == Color.white)
            SingleColor(go);
        else
        {
            MixColor(go);
        }
        PlayAudioOfLeafs(go);
    }

    public override void OnMouseDown(TinkerGraphic tinkerGraphic)
    {
        if(getDuckColor()==Color.white||(tinkerGraphic.gameObject.name=="red_leaf"&&getDuckColor()==GameManager.red)|| (tinkerGraphic.gameObject.name == "yellow_leaf" && getDuckColor() == GameManager.yellow)|| (tinkerGraphic.gameObject.name == "blue_leaf" && getDuckColor() == GameManager.blue))
            base.OnMouseDown(tinkerGraphic);
        if (tinkerGraphic.gameObject.name == "face")
        {
           AudioClip clip1 = (AudioClip)Resources.Load("Audio/VO/child_" + getColorName());
           tinkerGraphic.gameObject.GetComponent<AudioSource>().PlayOneShot(clip1);
        }
    }
    public  string getColorName()
    {
        Color c = getDuckColor();
        if (c == GameManager.white) return "white";
        else if (c == GameManager.red) return "red";
        else if (c == GameManager.blue) return "blue";
        else if (c == GameManager.green) return "green";
        else if (c == GameManager.orange) return "orange";
        else if (c == GameManager.purple) return "purple";
        else if (c == GameManager.brown) return "brown";
        else if (c == GameManager.yellow) return "yellow";
        return null;
    }

    public void PlayAudioOfLeafs(GameObject go)
    {
        if (go.name=="water_leaf")
        {
            StartCoroutine(PlayNonLoopSound(0));
        }
        else if(go.name =="red_leaf" || go.name =="blue_leaf" || go.name =="yellow_leaf")
        {

            StartCoroutine(PlayNonLoopSound(1));
        }
    }


    public Color getDuckColor()
    {
        return spriteRenderer.color;
    }
    public void ChangeColor(Color color)
    {

        StartCoroutine(WaitTime(color));
    }
    IEnumerator WaitTime(Color color)
    {
        yield return new WaitForSeconds(0.5f);
        spriteRenderer.color = color;
        spriteRendererWater.color = color;
        spriteRendererMovingArms.color = color;
        bubbleSprite1.color = color;
        bubbleSprite2.color = color;
        bubbleSprite3.color = color;
        bubbleSprite4.color = color;
        bubbleSprite5.color = color;
        bubbleSprite6.color = color;
        GameManager.duckColor = color;

    }
    public void MixColor(GameObject go)
    {
        Debug.Log("mixColor");
        Color duckcolor = getDuckColor();
        if (go.name == "water_leaf")
        {
            waterLeaf.GetComponent<Animator>().SetTrigger("start");
            fallingWater.GetComponent<Animator>().SetTrigger("start");
            ChangeColor(GameManager.white);
            PlayAudioOfLeafs(go);
        }
        else if (duckcolor == GameManager.orange || duckcolor == GameManager.brown
            || duckcolor == GameManager.purple || duckcolor == GameManager.green)
        {
            Debug.Log("entered");
            if (go.name == "red_leaf")
            {
                redLeaf.GetComponent<Animator>().SetTrigger("start");
                fallingRed.GetComponent<Animator>().SetTrigger("start");

            }
            else if (go.name == "blue_leaf")
            {
                blueLeaf.GetComponent<Animator>().SetTrigger("start");
                fallingBlue.GetComponent<Animator>().SetTrigger("start");

            }
            else if (go.name == "yellow_leaf")
            {
                yellowLeaf.GetComponent<Animator>().SetTrigger("start");
                fallingYellow.GetComponent<Animator>().SetTrigger("start");


            }
            else if (go.name == "water_leaf")
            {
                waterLeaf.GetComponent<Animator>().SetTrigger("start");
                fallingWater.GetComponent<Animator>().SetTrigger("start");

            }
            ChangeColor(GameManager.brown);
            StartCoroutine(PlayNonLoopSound(3,getAudioLength(1)));
        }

        else if (duckcolor == GameManager.yellow)
        {
            if (go.name == "red_leaf")
            {
                redLeaf.GetComponent<Animator>().SetTrigger("start");
                fallingRed.GetComponent<Animator>().SetTrigger("start");

                ChangeColor(GameManager.orange);
                StartCoroutine(PlayNonLoopSound(4, getAudioLength(1)));

            }
            else if (go.name == "blue_leaf")
            {
                blueLeaf.GetComponent<Animator>().SetTrigger("start");
                fallingBlue.GetComponent<Animator>().SetTrigger("start");
                ChangeColor(GameManager.green);
                StartCoroutine(PlayNonLoopSound(5, getAudioLength(1)));
            }
            else
            {
                SingleColor(go);
            }

        }
        else if (duckcolor == GameManager.red)
        {
            if (go.name == "yellow_leaf")
            {
                yellowLeaf.GetComponent<Animator>().SetTrigger("start");
                fallingYellow.GetComponent<Animator>().SetTrigger("start");
                ChangeColor(GameManager.orange);
                StartCoroutine(PlayNonLoopSound(4,getAudioLength(1)));
            }
            else if (go.name == "blue_leaf")
            {
                blueLeaf.GetComponent<Animator>().SetTrigger("start");
                fallingBlue.GetComponent<Animator>().SetTrigger("start");
                ChangeColor(GameManager.purple);
                StartCoroutine(PlayNonLoopSound(6, getAudioLength(1)));
            }
            else
            {
                SingleColor(go);
            }

        }
        else if (duckcolor == GameManager.blue)
        {
            if (go.name == "yellow_leaf")
            {
                fallingYellow.GetComponent<Animator>().SetTrigger("start");
                yellowLeaf.GetComponent<Animator>().SetTrigger("start");
                ChangeColor(GameManager.green);
                StartCoroutine(PlayNonLoopSound(5, getAudioLength(1)));
            }
            else if (go.name == "red_leaf")
            {
                fallingRed.GetComponent<Animator>().SetTrigger("start");
                redLeaf.GetComponent<Animator>().SetTrigger("start");
                ChangeColor(GameManager.purple);
                StartCoroutine(PlayNonLoopSound(6, getAudioLength(1)));
            }
            else
            {
                SingleColor(go);
            }

        }



    }


    public void SingleColor(GameObject go)
    {
        Debug.Log("singleColor");
        if (go.name == "red_leaf")
        {
            redLeaf.GetComponent<Animator>().SetTrigger("start");
            fallingRed.GetComponent<Animator>().SetTrigger("start");

            ChangeColor(GameManager.red);
            StartCoroutine(PlayNonLoopSound(7, getAudioLength(1)));
        }
        if (go.name == "blue_leaf")
        {
            fallingBlue.GetComponent<Animator>().SetTrigger("start");
            blueLeaf.GetComponent<Animator>().SetTrigger("start");
            ChangeColor(GameManager.blue);
            StartCoroutine(PlayNonLoopSound(8, getAudioLength(1)));
        }
        if (go.name == "yellow_leaf")
        {
            fallingYellow.GetComponent<Animator>().SetTrigger("start");
            yellowLeaf.GetComponent<Animator>().SetTrigger("start");
            ChangeColor(GameManager.yellow);
            StartCoroutine(PlayNonLoopSound(9, getAudioLength(1)));

        }
        if (go.name == "water_leaf")
        {
            fallingWater.GetComponent<Animator>().SetTrigger("start");
            waterLeaf.GetComponent<Animator>().SetTrigger("start");
            ChangeColor(GameManager.white);
        }
    }


}

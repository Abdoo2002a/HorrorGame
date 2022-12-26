using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteControl : MonoBehaviour
{
    Animator anim;
    


    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (GameObject.Find("item holder").transform.childCount > 0)
            {
                RemoteControl r = GameObject.Find("item holder").transform.GetChild(0).gameObject.GetComponent<RemoteControl>();
                if (r != null)
                {
                    if (!OldTv.instance.TvOn)
                    {
                        anim.SetTrigger("click");
                        AudioManager.instance.playSound("remote sound turn on");
                        OldTv.instance.openTv();                        
                    }
                    else
                    {
                        anim.SetTrigger("click");
                        AudioManager.instance.playSound("remote sound turn on");
                        OldTv.instance.closeTv();
                    }
                }
            }
        }
    }
}

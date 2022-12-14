using System.Collections;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{ 
    public TextMeshProUGUI textComponent;
    
    [HideInInspector]
    string[] lines;
    public float textSpeed;
    public bool enableGlitchEffect;

    private int index;

    [HideInInspector]
    public bool dialogueOn;

    //to know if the code is typing the line and if it's true it typy the whole line once and not use the effect
    bool inTextLine;

    private void Start()
    {    
        dialogueOn = false;
        inTextLine = false;
        textComponent.transform.parent.gameObject.SetActive(false);
    }

    private void Update()
    {   
        if (dialogueOn && textComponent.transform.parent.gameObject.activeSelf && (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Return))){
            NextLine();
        }
    }




    public void startDialogue(string[] lines)
    {
        //for the mini game dialouge, before the dialogue start the screen glitchs
        if (enableGlitchEffect)
        {
            MiniGame.instance.glitchScreen(0.2f);
        }

        textComponent.transform.parent.gameObject.SetActive(true);
        textComponent.text = string.Empty;
        this.lines = lines;
        index = 0;
        StartCoroutine(typeLine());
        dialogueOn = true;           
    }
    IEnumerator typeLine()
    {
        
        foreach(char c in lines[index].ToCharArray())
        {       
            inTextLine = true;
            textComponent.text += c;          
            yield return new WaitForSeconds(textSpeed);
        }
        inTextLine = false;
    }

    void NextLine()
    {
        
        if (!inTextLine)
        {

            if (index < lines.Length - 1)
            {
                textComponent.text = string.Empty;
                index++;

                StartCoroutine(typeLine());
            }
            else
            {
                //for the mini game dialouge, when the dialogue ends the screen glitchs
                if (enableGlitchEffect)
                {
                    MiniGame.instance.glitchScreen(0.2f);
                }
                dialogueOn = false;
                textComponent.transform.parent.gameObject.SetActive(false);
            }        
        }
        else {

            StopAllCoroutines();
            inTextLine = false;
            textComponent.text = string.Empty;
            textComponent.text = lines[index];
  
        }
        
    }
}

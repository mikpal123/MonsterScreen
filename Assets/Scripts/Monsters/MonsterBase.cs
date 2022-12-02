using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// base class of all available monsters
/// every monster is created based on that
/// </summary>
public class MonsterBase : MonoBehaviour
{
    public List<Button> place = new List<Button>();             //a list that contains all the buttons occupied by the monster 
    protected string monsterType;                               //text that will be displayed after spawn
    public bool canBeSelected;                                  //true = we can click on button that contains our monster
    // Start is called before the first frame update
    protected virtual void Start()
    {
        foreach(Button btn in place)
        {
            FillWithMonster(btn);       
        }       
    }

    //set values for our monster
    protected void FillWithMonster(Button button)
    {   
        button.GetComponentInChildren<Text>().text = monsterType;
        if (button == place[0])
        {
            button.gameObject.GetComponent<MonsterBase>().canBeSelected = true;
        }
        else                                                                               //every other slot than first can't be selected
        {
            ButtonHandler handler = button.transform.parent.transform.parent.gameObject.GetComponent<Canvas>().gameObject.GetComponent<ButtonHandler>();
            button.gameObject.GetComponent<MonsterBase>().canBeSelected = false;
            button.gameObject.GetComponent<ColorChange>().ChangeButtonColor(button);

            if (handler.firstButtton == button)
            {
                handler.firstButtton = null;
            }
            if (handler.secondButtton == button)
            {
                handler.secondButtton = null;
            }
            handler.CanSwap();
        }
    }
    protected void OnDestroy()                                                              //reset values for our monster when it's destroyd
    {
        foreach(Button btn in place)
        {
            btn.GetComponentInChildren<Text>().text = "";
        }
    }
}

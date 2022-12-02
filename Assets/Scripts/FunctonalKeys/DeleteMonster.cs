using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Delete any MonsterBase type object
/// </summary>
public class DeleteMonster : MonoBehaviour
{
    //values ​​that must be set in Unity
    public Button delete;
    public Canvas canvas;

    //values that don't need to be set in Unity
    private ButtonHandler handler;

    void Start()
    {
        //setup some values
        Button btn = delete.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
        handler = canvas.GetComponent<ButtonHandler>();
    }

    void TaskOnClick()
    {
        MonsterDelete();
        handler.CanSwap();
    }
    //check if we can delete monster
    private void MonsterDelete()
    {
        _MonsterDelete(handler.firstButtton);        
        _MonsterDelete(handler.secondButtton);
        
    }

    //delete monster
    private void _MonsterDelete(Button btn)
    {   if (btn == null) return;

        if (btn.GetComponent<MonsterBase>() != null)
        {
            List<Button> tmp = btn.gameObject.GetComponent<MonsterBase>().place;

            foreach (Button button in tmp)
            {
                DestroyImmediate(button.gameObject.GetComponent<MonsterBase>());
            }
        }
    }
}

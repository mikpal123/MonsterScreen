using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Swap any MonsterBase type monster
/// </summary>
public class Swap : MonoBehaviour
{
	//values ​​that must be set in Unity
	public Button swap;
	public Canvas canvas;

	//values that don't need to be set in Unity
	private Button firstButtonRef;
	private Button secondButtonRef;	
	private ButtonHandler handler;
	private List<Button> buttonsToFill1 = new List<Button>();
	private List<MonsterBase> monstersTofill1 = new List<MonsterBase>();
	private List<Button> buttonsToFill2 = new List<Button>();
	private List<MonsterBase> monstersTofill2 = new List<MonsterBase>();
    private bool canSwap = true;

	void Start()
	{
		Button btn = swap.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
		handler = canvas.GetComponent<ButtonHandler>();
	}

    private void Update()
    {
        if (handler.canSwap)
        {
            ColorBlock cb = swap.colors;
            cb.selectedColor = Color.white;
            cb.highlightedColor = Color.white;
            cb.pressedColor = Color.white;
            cb.normalColor = Color.white;
            swap.colors = cb;
        }           
        else
        {
            ColorBlock cb = swap.colors;
            cb.selectedColor = Color.red;
            cb.highlightedColor = Color.red;
            cb.pressedColor = Color.red;
            cb.normalColor = Color.red;
            swap.colors = cb;
        }
    }
    void TaskOnClick()
    {
        if (!handler.canSwap) return;
        
        //setup this every click (simply for reference)
        firstButtonRef = handler.firstButtton;
        secondButtonRef = handler.secondButtton;

        SwapMonster();
        handler.CanSwap();      
    }

    private void SwapMonster()
    {
        if (handler.firstButtton == null && handler.secondButtton == null) return;
        
        if (firstButtonRef.gameObject.GetComponent<MonsterBase>() != null && secondButtonRef.gameObject.GetComponent<MonsterBase>() != null) //two monsters are selected
        {
            var monster1 = firstButtonRef.gameObject.GetComponent<MonsterBase>().place;
            var monster2 = secondButtonRef.gameObject.GetComponent<MonsterBase>().place;


            //same size monsters
            if (monster1.Count == monster2.Count)
            {
                SaveMonster(monster1,buttonsToFill1, monstersTofill1);
                SaveMonster(monster2,buttonsToFill2, monstersTofill2);


                FillWithSameSizeMonster(monstersTofill2, buttonsToFill1);
                FillWithSameSizeMonster(monstersTofill1, buttonsToFill2);               
            }
            // when second monster is bigger than first
            else if (monster1.Count < monster2.Count)
            {
                SwapBiggerMonster(secondButtonRef, firstButtonRef);
            }
                
            //when first monster is bigger than first
            else
            {
                SwapBiggerMonster(firstButtonRef, secondButtonRef);                   
            }
        }
        else
        {
            if(firstButtonRef.gameObject.GetComponent<MonsterBase>() != null)
            {
                SwapEmptyMonster(firstButtonRef, secondButtonRef);
            }
            else if(secondButtonRef.gameObject.GetComponent<MonsterBase>() != null)
            {
                SwapEmptyMonster(secondButtonRef, firstButtonRef);
            }
        }
        buttonsToFill1.Clear();
        buttonsToFill2.Clear();
        monstersTofill1.Clear();
        monstersTofill2.Clear();       
    }

    private void FillWithSameSizeMonster(List<MonsterBase> monstersToFill, List<Button> buttonsToFill)
    {
        int j = monstersToFill.Count - 1;
        for (int i = 0; i < buttonsToFill.Count; i++)                                              //fill monster 2 in monster 1 slots
        {
            buttonsToFill[i].gameObject.AddComponent(monstersToFill[j].GetType());
            foreach (Button button in buttonsToFill)
            {
                buttonsToFill[i].gameObject.GetComponent<MonsterBase>().place.Add(button);
            }
            j--;
        }
    }

    private void SaveMonster(List<Button> monsters, List<Button> buttonsToFill, List<MonsterBase> monstersToFill, bool destroyMonster = true)
    {
        foreach (Button monster in monsters)                                                      //save values of monster
        {
            buttonsToFill.Add(monster);
            monstersToFill.Add(monster.gameObject.GetComponent<MonsterBase>());
            if(destroyMonster)
            DestroyImmediate(monster.gameObject.GetComponent<MonsterBase>());
        }
    }

    private void SwapEmptyMonster(Button monsterButton, Button emptyButton)
    {
        MonsterBase monster = monsterButton.gameObject.GetComponent<MonsterBase>();                             //setup references
        canSwap = true;

        SaveMonster(monster.place, buttonsToFill1, monstersTofill1, false);                                            //save values of Bigger monster
        
        for (int i = 1; i < monster.place.Count; i++)                                                               //check if bigger monster can be created forward
        {
            if (handler.NextChild(emptyButton, i) == null)
            {
                canSwap = false;
                break;
            }         
            
            Button nextButton = handler.NextButton(emptyButton, i);
            if (nextButton.gameObject.GetComponent<MonsterBase>() != null && !buttonsToFill1.Contains(nextButton))
            {
                canSwap = false;
                break;
            }
        }
        AddEmptyMonster(emptyButton, monster, canSwap);
        
        if(!canSwap)                                                                                                   //check if bigger monster can be created backward
        {
            for (int i = 1; i < monster.place.Count; i++)
            {

                if (handler.NextChild(emptyButton, -i) == null)
                {
                    canSwap = false;
                    break;
                }

                Button nextButton = handler.NextButton(emptyButton, -i);

                if (nextButton.gameObject.GetComponent<MonsterBase>() != null && buttonsToFill1.Contains(nextButton))
                {
                    canSwap = true;
                }
                else if (nextButton.gameObject.GetComponent<MonsterBase>() == null)
                {
                    canSwap = true;
                }
                else
                {
                    canSwap = false;
                }
            }
            AddEmptyMonster(handler.NextButton(emptyButton, -(monster.place.Count - 1)), monster, canSwap);           
        }
    }

    private void AddEmptyMonster(Button btn, MonsterBase monster, bool canSwap)
    {
        if (!canSwap) return;

        foreach (Button button in monster.place)                                                       //destroy monsters
        {
            DestroyImmediate(button.gameObject.GetComponent<MonsterBase>());
        }
        AddBiggerMonster(btn, monstersTofill1);       //add monster backward
    }

    private void SwapBiggerMonster(Button biggerMonsterButton, Button smalerMonsterButton)
    {
        MonsterBase biggerMonster = biggerMonsterButton.gameObject.GetComponent<MonsterBase>();                //setup references
        MonsterBase smallerMonster = smalerMonsterButton.gameObject.GetComponent<MonsterBase>();
        int neededSize = biggerMonster.place.Count - smallerMonster.place.Count;
        canSwap = true;

        SaveMonster(biggerMonster.place, buttonsToFill2, monstersTofill2, false);
        

        for (int i = 1; i <= neededSize; i++)                                                                  //check if bigger monster can be created forward
        {
            if (handler.NextChild(smalerMonsterButton, i) == null)
            {
                canSwap = false;
                break;
            }

            Button nextButton = handler.NextButton(smalerMonsterButton, i);
            if (nextButton.gameObject.GetComponent<MonsterBase>() != null && !buttonsToFill2.Contains(nextButton))
            {
                canSwap = false;
                break;
            }
        }
        if (canSwap)
        {

            SaveMonster(smallerMonster.place, buttonsToFill1, monstersTofill1);            
                        
            foreach (Button button in biggerMonster.place)
            {
                DestroyImmediate(button.gameObject.GetComponent<MonsterBase>());
            }

            AddBiggerMonster(buttonsToFill1[0], monstersTofill2);                                                       //add monsters forward


            for (int i = 0; i < buttonsToFill2.Count; i++)
            {
                if (!handler.IsThereMonster(buttonsToFill2[i]))
                {
                    AddBiggerMonster(buttonsToFill2[i], monstersTofill1);
                    break;
                }
            }
        }
        else                                                                                                      //check if bigger monster can be created backward
        {
            for (int i = 1; i <= neededSize; i++)
            {

                if (handler.NextChild(smalerMonsterButton, -i) != null)
                {
                    Button nextButton = handler.NextButton(smalerMonsterButton, -i);

                    if (nextButton.gameObject.GetComponent<MonsterBase>() != null && buttonsToFill2.Contains(nextButton))
                    {
                        canSwap = true;
                    }
                    else if (nextButton.gameObject.GetComponent<MonsterBase>() == null)
                    {
                        canSwap = true;
                    }
                    else
                    {
                        canSwap = false;
                        break;
                    }
                }
                else
                {
                    canSwap = false;
                    break;
                }

            }
            if (canSwap)
            {
                SaveMonster(smallerMonster.place, buttonsToFill1, monstersTofill1);
                
                foreach (Button button in biggerMonster.place)
                {
                    DestroyImmediate(button.gameObject.GetComponent<MonsterBase>());
                }

                AddBiggerMonster(handler.NextButton(buttonsToFill1[0], -neededSize), monstersTofill2);                    //add monsters backwards
                for (int i = 0; i < buttonsToFill2.Count; i++)
                {
                    if (!handler.IsThereMonster(buttonsToFill2[i]))
                    {
                        AddBiggerMonster(buttonsToFill2[i], monstersTofill1);
                        break;
                    }
                }

            }
        }
    }

 
    private void AddBiggerMonster(Button start, List<MonsterBase> monstersToFill)
    {
		List<Button> tmpList = new List<Button>();

        for (int i = 0; i < monstersToFill.Count; i++)                      //create monster
		{
			handler.NextButton(start, i).gameObject.AddComponent(monstersToFill[i].GetType());
			tmpList.Add(handler.NextButton(start, i));
		}				
		
		foreach(Button button in tmpList)
        {
			for (int i = 0; i < tmpList.Count; i++)                         //save places of monster
            {
                tmpList[i].gameObject.GetComponent<MonsterBase>().place.Add(button);
			}			
		}
		tmpList.Clear();        
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleInput : MonoBehaviour
{

    private int playerIndex;
    private int currentValue;
    private int currentCategory;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            playerIndex = 0;
            currentValue = 0;
            currentCategory = 0;
        } else if (Input.GetKeyDown(KeyCode.Return))
        {
            BattleManager.Instance.StartBattle();
        }

        //Value Input
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentValue += 1;
        } else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentValue += 2;
        } else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentValue += 3;
        } else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentValue += 4;
        } else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            currentValue += 5;
        }

        //Category Input
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            currentCategory = 0;
        } else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            currentCategory = 1;
        } else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            currentCategory = 2;
        } else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            currentCategory = 3;
        } else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            currentCategory = 4;
        }

         //PlayerIndex
         else if (Input.GetKeyDown(KeyCode.V))
        {
            playerIndex = 0;
        } else if (Input.GetKeyDown(KeyCode.B))
        {
            playerIndex = 1;
        } else if (Input.GetKeyDown(KeyCode.N))
        {
            playerIndex = 2;
        } else if (Input.GetKeyDown(KeyCode.M))
        {
            playerIndex = 3;
        }




        if (Input.GetKeyDown(KeyCode.C))
        {
            GameManager.Instance.AssignCategory(currentCategory);
        } else if (Input.GetKeyDown(KeyCode.A))
        {
            BattleManager.Instance.Attack(playerIndex, currentValue, currentCategory);
        } else if (Input.GetKeyDown(KeyCode.D))
        {
            BattleManager.Instance.Defend(currentValue, currentCategory);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.Instance.StartNextTurn();
        }

        if (Input.GetMouseButtonDown(0))
        {
            FocusCastle();
        }
    }


    private void FocusCastle()
    {
        RaycastHit hit;
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
        try
        {
            if (hit.transform.GetComponent<Castle>() != null)
            {
                Castle rayCastedCastle = hit.transform.GetComponent<Castle>();
                if (rayCastedCastle.fromPlayer != GameManager.Instance.currentPlayer.playerNumber)
                {
                    BattleManager.Instance.ManualCastleFocus(hit.transform.GetComponent<Castle>().fromPlayer, hit.transform.GetComponent<Castle>());
                }
            }

        } catch (System.NullReferenceException)
        {
        }

    }
}

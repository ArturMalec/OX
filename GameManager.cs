using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Field> _Fields;
    [SerializeField] private Sprite _Ximage;
    [SerializeField] private Sprite _Oimage;
    [SerializeField] private Text _WinnerText;
    [SerializeField] private GameObject _RestartButton;

    private int emptyFields;
    private bool gameEnded = false;
    private string winner = "";

    public static GameManager Instance;
    public Action<int> onInteractEvent;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        onInteractEvent += DetectInteractionAndSetField;
        emptyFields = 9;
    }

    private IEnumerator WaitForPlayerInteraction(int interactField)
    {
        BlockAllInput();
        SetPlayerMarking(interactField);

        if (CheckForWin())
        {
            gameEnded = true;
            ShowWinner(false);
        }

        if (!CheckForWin() && emptyFields == 0)
        {
            ShowWinner(true);
        }

        yield return new WaitForSeconds(2f);

        if (!gameEnded)
        {
            SetAIMarking();
            
            if (CheckForWin())
            {
                gameEnded = true;
                ShowWinner(false);
            }
            else
            {
                UnblockInput();
            }

            if (!CheckForWin() && emptyFields == 0)
            {
                ShowWinner(true);
            }
        }
          
    }

    private void DetectInteractionAndSetField(int interactField)
    {
        if (emptyFields > 0)
        {
            StartCoroutine(WaitForPlayerInteraction(interactField));
        }
    }

    private void SetPlayerMarking(int fieldNum)
    {
        for (int i = 0; i < _Fields.Count; i++)
        {
            if (_Fields[i].ButtonNumber == fieldNum)
            {
                _Fields[i].SetFieldImage(_Ximage);
                _Fields[i].TextID = "X";
                emptyFields--;
            }
        }
    }

    private void SetAIMarking()
    {
        List<Field> emptyFieldsList = new List<Field>();

        for (int i = 0; i < _Fields.Count; i++)
        {
            if (string.IsNullOrEmpty(_Fields[i].TextID))
            {
                emptyFieldsList.Add(_Fields[i]);
            }
        }

        if (emptyFieldsList.Count > 0)
        {
            Field fieldToMark = emptyFieldsList[UnityEngine.Random.Range(0, emptyFieldsList.Count)];
            fieldToMark.SetFieldImage(_Oimage);
            fieldToMark.TextID = "O";
            emptyFields--;
        }
    }

    private void BlockAllInput()
    {
        for (int i = 0; i < _Fields.Count; i++)
        {
            _Fields[i].BlockInput(true);
        }
    }

    private void UnblockInput()
    {
        for (int i = 0; i < _Fields.Count; i++)
        {
            if (string.IsNullOrEmpty(_Fields[i].TextID))
            {
                _Fields[i].BlockInput(false);
            }
        }
    }

    private void ShowWinner(bool draw)
    {
        _WinnerText.gameObject.SetActive(true);
        _RestartButton.SetActive(true);

        if (!draw)
        {
            _WinnerText.text = winner + " Wins!";
        }
        else
        {
            _WinnerText.text = "DRAW!";
        }      
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    private bool CheckForWin()
    {
        // top row
        if (WinCaseOccured(_Fields[0], _Fields[1], _Fields[2]))
            return true;

        // middle row
        if (WinCaseOccured(_Fields[3], _Fields[4], _Fields[5]))
            return true;

        // bottom row
        if (WinCaseOccured(_Fields[6], _Fields[7], _Fields[8]))
            return true;

        // left column
        if (WinCaseOccured(_Fields[0], _Fields[3], _Fields[6]))
            return true;

        // middle column
        if (WinCaseOccured(_Fields[1], _Fields[4], _Fields[7]))
            return true;

        // right column
        if (WinCaseOccured(_Fields[2], _Fields[5], _Fields[8]))
            return true;

        // diag right
        if (WinCaseOccured(_Fields[0], _Fields[4], _Fields[8]))
            return true;

        // diag left
        if (WinCaseOccured(_Fields[2], _Fields[4], _Fields[6]))
            return true;

        return false;
    }

    private bool WinCaseOccured(Field one, Field two, Field three)
    {
        if (one.TextID == "X" && two.TextID == "X" && three.TextID == "X")
        {
            winner = "Krzyzyk";
            return true;
        }
        else if (one.TextID == "O" && two.TextID == "O" && three.TextID == "O")
        {
            winner = "Kolko";
            return true;
        }

        return false;
    }
}

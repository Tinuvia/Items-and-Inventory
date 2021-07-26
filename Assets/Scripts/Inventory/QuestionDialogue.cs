using System;
using UnityEngine;

public class QuestionDialogue : MonoBehaviour
{
    public event Action OnYesEvent;
    public event Action OnNoEvent;


    public void Show()
    {
        gameObject.SetActive(true);
        OnYesEvent = null;
        OnNoEvent = null;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void OnYesButtonClick()
    {
        OnYesEvent?.Invoke();
        Hide();
    }

    public void OnNoButtonClick()
    {
        OnNoEvent?.Invoke();
        Hide();
    }
}

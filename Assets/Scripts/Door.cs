using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour 
{
    public bool OpenOnAllEnemiesDestroyed;
    public bool IsOpen;

    public GameObject OpenState;
    public GameObject ClosedState;

	void Start () 
	{
        if (OpenOnAllEnemiesDestroyed)
        {
            GameManager.Instance.AllEnemiesDestroyed += OnAllEnemiesDestroyed;
        }

        ClosedState.SetActive(!IsOpen);
        OpenState.SetActive(IsOpen);
    }

    public void CleanUp()
    {
        GameManager.Instance.AllEnemiesDestroyed -= OnAllEnemiesDestroyed;
    }

    public void Toggle()
    {
        IsOpen = !IsOpen;

        ClosedState.SetActive(!IsOpen);
        OpenState.SetActive(IsOpen);
    }

    public void Open()
    {
        IsOpen = true;
        ClosedState.SetActive(false);
        OpenState.SetActive(true);
    }

    public void Close()
    {
        IsOpen = false;
        ClosedState.SetActive(true);
        OpenState.SetActive(false);
    }

    public void OnAllEnemiesDestroyed()
    {
        GameManager.Instance.AllEnemiesDestroyed -= OnAllEnemiesDestroyed;
        Open();
    }
}

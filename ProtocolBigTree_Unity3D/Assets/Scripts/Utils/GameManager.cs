using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private static InputManager inputManager;
    private void Awake()
    {
        _instance = this;
        inputManager = _instance.gameObject.AddComponent<InputManager>();
    }

    private void Start()
    {
        if (FindObjectsOfType<InputManager>().Length > 1) 
        {
            Debug.Log("Warning, more than 1 inputmanager on scene");
        }
    }

    public InputManager GetInputManager()
    {
        return inputManager;
    }


}

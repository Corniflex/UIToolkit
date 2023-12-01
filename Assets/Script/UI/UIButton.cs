using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIButton : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;

    private VisualElement rootElement;
    
    // Start is called before the first frame update
    void Start()
    {
        rootElement = uiDocument.rootVisualElement;
        var button1 = rootElement.Q<Button>("Button1");
        
        button1.RegisterCallback<ClickEvent, string>(Ping, "Ping");
    }

    private void Ping(ClickEvent e, string pingText)
    {
        Debug.Log(pingText);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DraftText : MonoBehaviour
{
    public GameObject skipButton;
    public TextMeshProUGUI timer;
    public TextMeshProUGUI instructions;
    public Player localPlayer;


    public void Activate(bool show) {
        gameObject.SetActive(show);
        skipButton.SetActive(show);
        UpdateCounter();
    }

    public void UpdateCounter() {
        instructions.text = "Choose a Card (" + (GameManager.Instance.draftRerolls - localPlayer.rerolls).ToString() + "/" + GameManager.Instance.draftRerolls + ")";

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsScreen : MonoBehaviour
{
    [SerializeField] GameObject startScreen;
    // Start is called before the first frame update

    public void CloseControlsSheet(){
        gameObject.SetActive(false);
        startScreen.SetActive(true);
    }
}

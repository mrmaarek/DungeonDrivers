using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    // Enumerations kunnen zowel binnen als buiten de class gemaakt worden!
    // Wanneer je ze buiten de class maakt, kun je ze ook buiten de class gebruiken!

    public enum Classes
    {
        NoneSelected,
        SandMage,
        Warrior
    }
    
    public Classes playerClass;


    void Start()
    {
        playerClass = Classes.NoneSelected;
        
    }

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Update()
    {
       
    }
   
}

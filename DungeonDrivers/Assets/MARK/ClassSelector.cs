using UnityEngine;
using System.Collections;

public class ClassSelector : MonoBehaviour
{

    public enum Classes
    {
        None_Selected,
        Sandmage,
        Warrior
    }

    public Classes playerClass;

    void Start()
    {
        this.playerClass = Classes.None_Selected;
    }

    void Update()
    {

    }

    public void ClassSelect(int pClass)
    {
        this.playerClass = (Classes)pClass;
    }
}

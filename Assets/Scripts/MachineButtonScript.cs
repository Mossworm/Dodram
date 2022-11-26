using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineButtonScript : MonoBehaviour
{
    public GameObject machine;

    public void MachineRun()
    {
        machine.GetComponent<SpineMachineScript>().CraftOn();
    }
}
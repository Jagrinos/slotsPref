using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsManager : MonoBehaviour
{
    SlotsMachine slotsMachine;

    private void Start()
    {
        slotsMachine = FindObjectOfType<SlotsMachine>();    
    }
    public void StartButton()
    {
        if (slotsMachine.isEnd)
        {
            slotsMachine.StartSpin = true;
            slotsMachine.StopSpin = false;
        }
    }

    public void StopButton()
    {
        if (slotsMachine.StartSpin)
        {
            slotsMachine.StartSpin = false;
            slotsMachine.StopSpin = true;
            slotsMachine.isEnd = false;
        }
    }
}

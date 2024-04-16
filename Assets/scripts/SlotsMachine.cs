using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UIElements;
public enum Slots
{
    potemkin,
    kittyProger,
    offendedKitten
}

public class SlotsMachine : MonoBehaviour
{
    [SerializeField] List<GameObject> _slots;
    [SerializeField] float _speedRotate;
    [SerializeField] TextMeshProUGUI winValue;
    float _stopSpeedRotate;
    [HideInInspector] public bool StartSpin = false;
    [HideInInspector] public bool StopSpin = false;

    GameObject nowStops;
    GameObject[] notStops;

    [HideInInspector] public bool isEnd = true;
    List<Slots> result;
    public List<Combinations> combinations;
    
    private void Start()
    {
        _stopSpeedRotate = _speedRotate;
        notStops = _slots.ToArray();
        result = new();
    }

    
    void Update()
    {
        if (StartSpin)
            startSpin();
        if (StopSpin)
            stopSpin();

        if (result.Count == 3) //Изменить в зависимости от количества слотов
            checkResult();
        
        
    }

    void startSpin()
    {
        winValue.gameObject.SetActive(false);
        _stopSpeedRotate = _speedRotate;

        notStops = _slots.ToArray();

        foreach (var slot in _slots)
        {
            slot.transform.position += _speedRotate * Time.deltaTime * Vector3.down;

            if (slot.transform.position.y <= -4.5)
                slot.transform.position = new Vector3(
                    slot.transform.position.x,
                    4.5f,
                    slot.transform.position.z
                    );
        }

    }

    void checkResult()
    {
       
        foreach (var comb in combinations) 
        { 
            if (result[0] == comb.firstSlot && result[1] == comb.secondSlot && result[2] == comb.thirdSlot)
            {
                winValue.gameObject.SetActive(true);    
                winValue.text = comb.WinValue.ToString();
                break;
            }
        }

        if (!winValue.gameObject.activeSelf)
        {
            winValue.gameObject.SetActive(true);
            winValue.text = "0";
        }
        isEnd = true;
        result.Clear();
    }
    
    void stopSpin()
    {
        //выбираем линию для остановки
        if (!nowStops)
        {
            if (notStops.Length > 0)
            {
                nowStops = notStops[Random.Range(0, notStops.Length)];
               
                notStops = notStops.Where(val => val != nowStops).ToArray();

            }
            else
            {
                StopSpin = false;
                return;
            }
        }

        //тормозим ее
        if (_stopSpeedRotate >= 0)
        {
            nowStops.transform.position += _stopSpeedRotate * Time.deltaTime * Vector3.down;
            _stopSpeedRotate -= Time.deltaTime;
            if (nowStops.transform.position.y <= -4.5)
                nowStops.transform.position = new Vector3(
                    nowStops.transform.position.x,
                    4.5f,
                    nowStops.transform.position.z
                    );
        }
        else
        {
            Vector3 stopPosition = nowStops.transform.position;
            //изменить в зависимости от линии
            if (stopPosition.y >= -4.5 && stopPosition.y <= -3 || stopPosition.y <= 4.5 && stopPosition.y >= 3.5)
            {
                nowStops.transform.position = new Vector3(
                    nowStops.transform.position.x,
                    4.5f,
                    nowStops.transform.position.z
                    );
                result.Add(Slots.potemkin);
            }

            if (stopPosition.y > -3 && stopPosition.y <= 0)
            {
                nowStops.transform.position = new Vector3(
                    nowStops.transform.position.x,
                    -1.5f,
                    nowStops.transform.position.z
                    );
                result.Add(Slots.offendedKitten);
            }


            if (stopPosition.y > 0 && stopPosition.y <= 3)
            {
                nowStops.transform.position = new Vector3(
                   nowStops.transform.position.x,
                   1.5f,
                   nowStops.transform.position.z
                   );
                result.Add(Slots.kittyProger);
            }

            //изменить в зависимости от линии

            nowStops = null;
            _stopSpeedRotate = _speedRotate;
        }

        //остальные продолжают движение
        foreach (var slot in notStops)
        {
            slot.transform.position += _speedRotate * Time.deltaTime * Vector3.down;

            if (slot.transform.position.y <= -4.5)
                slot.transform.position = new Vector3(
                    slot.transform.position.x,
                    4.5f,
                    slot.transform.position.z
                    );
        }
    }
    
}
[System.Serializable]
public class Combinations
{
    //добавить по необходимости (Не забыть про верхний)

    public Slots firstSlot; 
    public Slots secondSlot;
    public Slots thirdSlot;

    public int WinValue;
}
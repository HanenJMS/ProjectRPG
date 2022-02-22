using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : MonoBehaviour
{
    [SerializeField] private int hp;
    [SerializeField] private int mana;
    void Start()
    {
        hp = 100;
        mana = 20;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

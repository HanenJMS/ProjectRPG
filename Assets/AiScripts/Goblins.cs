using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblins : MonoBehaviour
{
    [SerializeField] private int hp;
    [SerializeField] private int mana;

    private void Start()
    {
        hp = 80;
        mana = 10;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

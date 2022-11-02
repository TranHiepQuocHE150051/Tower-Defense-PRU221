using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITowerFactory
{
    ITower CreateTower(int level, int type , int index);

    
   
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class TrashDamagedState : TrashState
{
    public TrashDamagedState(GameObject go) : base(go)
    {
    }

    public override void StartState()
    {
        //EnemyTrash trashState = go.GetComponent<EnemyTrash>();
        Debug.Log("Entro en damaged!");
        trashState.anim.SetBool("hit1", true);
    }

    public override void EndState()
    {
        //Exist from state
        Debug.Log("Salgo de damaged!");
        trashState.anim.SetBool("hit1", false);
    }

}

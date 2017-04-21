﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class TrashCombatMoveAround : TrashState
{
    private int attackProbability = 0;
    private int approachProbability = 0;
    private float startTime = 0;

    public TrashCombatMoveAround(GameObject go) : base(go, TrashStateTypes.COMBAT_MOVE_AROUND_STATE)
    {
    }

    public override void StartState()
    {
        numberOfFrames = 0;
        attackProbability = trashState.attackProbability;
        approachProbability = trashState.approachProbability;

        startTime = Time.time;
    }

    public override TrashStateTypes UpdateState()
    {
        if(Time.time - startTime >= 2)
        {
            int probability = UnityEngine.Random.Range(0, 100);
            if (trashState.attackProbability >= probability && trashState.nav.remainingDistance <= trashState.attackRange)
            {
                trashState.nav.Stop();
                return TrashStateTypes.ATTACK_STATE;
            }
            else if (trashState.approachProbability >= probability && trashState.nav.remainingDistance >= trashState.attackRange)
            {
                ++numberOfFrames;

                trashState.nav.SetDestination(trashState.target.position);
                trashState.nav.Resume();
                return TrashStateTypes.COMBAT_MOVE_FORWARD_STATE;
            }
            else
            {
                startTime = Time.time;
            }

        }
        trashState.nav.Stop();
        trashState.transform.RotateAround(trashState.target.transform.position, Vector3.up, trashState.combatAngularSpeed * Time.deltaTime);
        return type;
    }

    public override void EndState()
    {
        numberOfFrames = 0;
        attackProbability = trashState.attackProbability;
        approachProbability = trashState.approachProbability;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadTile : Tile
{
    public override void RunCommand(Ground ground, Coordinate pos)
    {
        SoundManager.Inst.PlayEffectSound(SOUND_NAME.FAILED_SOUND, 1f);
        GameManager.Inst.Fail();
    }
}

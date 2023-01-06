using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonBehavior<GameManager>
{
    private PlayerGround player;
    public PlayerGround Player => player;
}

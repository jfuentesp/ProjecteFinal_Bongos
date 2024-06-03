using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlaceableEnum
{
    BOMB, TEMPESTEXPLOSIVE, TRAPEXPLOSION, TRAPPOISON, TRAPWATER, INSTANTEXPLOSION
}

public enum ActivableEnum
{
    DAMAGE, POISON, SLOW
}

public enum ExplosionType
{
    EXPLOSION, PROTECTEDEXPLOSION, SLOW, POISON, INSTANTEXPLOSION
}

public enum StatType
{
    ATTACK, DEFENSE, SPEED
}

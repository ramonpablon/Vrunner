using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PieceType
{
    // tipos de obstaculos
    none = -1,
    gordura = 0,
    plaquetas = 1,
}

public class Piece : MonoBehaviour {

    public PieceType type;
    public int visualIndex;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PieceType
{
    none = -1,
    gordura = 0,
    plaqueta = 1,
}

public class Piece : MonoBehaviour {

    public PieceType type; // modelos de obstáculos
    public int visualIndex; // tipos de obstaculos (gordura 1, gordura2...)
}
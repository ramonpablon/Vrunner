using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSpawner : MonoBehaviour {

	public PieceType type;
    public int typeGraphic;
    private Piece CurrentPiece;

    public void Spawn()
    {
        CurrentPiece = LevelManager.Instance.GetPiece(type, typeGraphic);
        CurrentPiece.gameObject.SetActive(true);
        CurrentPiece.transform.SetParent(transform, false);
    }

    public void DeSpawn()
    {
        CurrentPiece.gameObject.SetActive(false);
    }
}

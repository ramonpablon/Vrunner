using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSpawner : MonoBehaviour {

    public PieceType type;
    public int typeGraphic;
    private Piece currentPiece;

    public void Spawn()
    {
        currentPiece = LevelManager.Instace.GetPieces(type, typeGraphic); // escolhe o tipo grafico do objeto (gordura1 ou gordura2....)
        currentPiece.gameObject.SetActive(true);
        currentPiece.transform.SetParent(transform, false); // insere o objeto grafico no objeto de colisão, "false" mantem a mesma posição que o pai

    }
    public void Despawn()
    {
        currentPiece.gameObject.SetActive(false);
    }
}

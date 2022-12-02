using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform lookAt;
    public Vector3 offset = new Vector3(0, -2, -8);

    private Vector3 lookAtPos;

    private void LateUpdate()
    {
        lookAt.transform.localPosition = Vector3.Lerp(lookAt.transform.localPosition, lookAtPos, Time.deltaTime / (BendMoviment.Instance.speedMultiplier * 150));
        transform.localPosition = Vector3.Lerp(transform.localPosition, offset, Time.deltaTime / (BendMoviment.Instance.speedMultiplier * 100));

        transform.LookAt (lookAt);

        switch (BendMoviment.Instance.curvatureMoviment)
        {
            case CurvatureMoviment.up:
                offset = new Vector3(0, -3.5f, -8);
                lookAtPos.y = -2;
                break;
            case CurvatureMoviment.rightUp:
                offset = new Vector3(-2.25f, -2.7f, -7);
                lookAtPos.y = -2;
                break;
            case CurvatureMoviment.right:
                offset = new Vector3(-3, 0, -8);
                lookAtPos.y = 0;
                break;
            case CurvatureMoviment.rightDown:
                offset = new Vector3(-2.5f, 2.5f, -7);
                lookAtPos.y = 0;
                break;
            case CurvatureMoviment.down:
                offset = new Vector3(0, 2, -8);
                lookAtPos.y = 0;
                break;
            case CurvatureMoviment.leftDown:
                offset = new Vector3(2.5f, 2.5f, -7);
                lookAtPos.y = 0;
                break;
            case CurvatureMoviment.left:
                offset = new Vector3(3, 0, -8);
                lookAtPos.y = 0;
                break;
            case CurvatureMoviment.leftUp:
                offset = new Vector3(3, -3.5f, -7);
                lookAtPos.y = -2;
                break;
            default:
                break;
        }
    }
}

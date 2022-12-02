using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Confrimação : MonoBehaviour
{
    public GameObject tela = null;

    private string cenna = null;

    public void IrParaConfirmacao(string cena)
    {
        cenna = cena;
        tela.SetActive(true);
    }
    public void Sim()
    {
        SceneManager.LoadScene(cenna);
    }

    public void Nao()
    {
        tela.SetActive(false);
    }

}

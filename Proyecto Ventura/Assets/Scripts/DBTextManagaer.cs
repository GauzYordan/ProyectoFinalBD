using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DBTextManagaer : MonoBehaviour
{

    public Text MunicionActual;
    public Text MunicionUtilizada;
    public Text MunicionTotal;
    public Text EnemigosDestruidos;

    private DBConnection dbConn;

    // Start is called before the first frame update
    void Start()
    {
        dbConn = FindObjectOfType<DBConnection>();
    }

    // Update is called once per frame
    void Update()
    {
        //float[] posPlayer = dbConn.LastRowPosicionPlayer(1);

        MunicionActual.text = "MUNICION ACTUAL: \n Swords = " + dbConn.IniAmmo("Sword") + "\n" + "Arrows = " + dbConn.IniAmmo("Arrow");
        MunicionUtilizada.text = "MUNICION UTILIZADA: \n Swords = " + dbConn.VistaMunicionUtilizada("Sword") + "\n" + "Arrows = " + dbConn.VistaMunicionUtilizada("Arrow");
        EnemigosDestruidos.text = "Total de enemigos destruidos: \n" + dbConn.VistaObjetosDestruidos(1);
        MunicionTotal.text = "MUNICION TOTAL = " + dbConn.totalMunicion(1); 
        //lastPosPlayer.text = "ULTIMA POSICION:\n  Posicion X = " + posPlayer[4] + "\n  Posicion Y = " + posPlayer[3];
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient; //notar que se incluyen las clases de este namespace
using UnityEngine;

//https://sqlchoice.azurewebsites.net/en-us/sql-server/developer-get-started/csharp/win/step/2.html
//SQLServerManager14.msc

public class DBConnection : MonoBehaviour
{

    string conn = " ";

    public GameObject[] tags;
    void Awake()
    {
       
        try
        {
            //código muy chafa, solo para que entiendan cómo se hacen las conexiones , uso de vistas y funciones
            //string conn = @"Server=localhost\SQLExpress;Database=DemoUsandoCS;Trusted_Connection=True;";
            //data source = 127.0.0.1
            conn = @"Data Source = 127.0.0.1;Database=DBUnity;User Id=UsuarioAdmin; Password = UsuarioAdmin";

            print("Connecting to SQL Server ... ");
            using (SqlConnection connection = new SqlConnection(conn))   //esta sentencia permite que connection se destruya al salir de bloque , no hay que usar connection.close() en otras palabras
            { //https://stackoverflow.com/questions/4717789/in-a-using-block-is-a-sqlconnection-closed-on-return-or-exception
                connection.Open();
                print("Done. SI ME PUDE CONECTAR");
            }
        }
        catch (SqlException e)
        {
            print(e.ToString());
        }

        tags = GameObject.FindGameObjectsWithTag("Destructible");

        foreach(GameObject destructible in tags){
            IniObjetosDestruibles(destructible.name, destructible.transform.localScale.x, destructible.transform.rotation.x);
        }
    }

    void Start(){
        tags = GameObject.FindGameObjectsWithTag("Destructible");

        foreach(GameObject destructible in tags){
            IniObjetosDestruibles(destructible.name, destructible.transform.localScale.x, destructible.transform.rotation.x);
        }
    }

    //VIEW MUnicionPlayer
    public int IniAmmo(string ammo){
        int currentAmmo = 0;
        
        try
        {
            using (SqlConnection connection = new SqlConnection(conn))   //esta sentencia permite que connection se destruya al salir de bloque , no hay que usar connection.close() en otras palabras
            {
                connection.Open();
                //Console.WriteLine("Done.");

                string sql = "SELECT NumMunicion FROM ViewMunicionPlayer WHERE Municion = @municion"; //ya se ve más bonito


                using (SqlCommand command = new SqlCommand(sql, connection))
                { //https://stackoverflow.com/questions/293311/whats-the-best-method-to-pass-parameters-to-sqlcommand
                    command.Parameters.Add(new SqlParameter("municion", ammo));
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //print(reader.GetInt32(0));
                            currentAmmo = reader.GetInt32(0);
                        }
                        
                        return currentAmmo;
                    }
                }
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.ToString());
            return currentAmmo;
        }
    }

    public int VistaMunicionUtilizada(string ammo){
        int currentAmmo = 0;
        
        try
        {
            using (SqlConnection connection = new SqlConnection(conn))   //esta sentencia permite que connection se destruya al salir de bloque , no hay que usar connection.close() en otras palabras
            {
                connection.Open();
                //Console.WriteLine("Done.");

                string sql = "SELECT MunicionesUsadas FROM ViewMunicionesUsadas WHERE Municion = @municion"; //ya se ve más bonito


                using (SqlCommand command = new SqlCommand(sql, connection))
                { //https://stackoverflow.com/questions/293311/whats-the-best-method-to-pass-parameters-to-sqlcommand
                    command.Parameters.Add(new SqlParameter("municion", ammo));
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //print(reader.GetInt32(0));
                            currentAmmo = reader.GetInt32(0);
                        }
                        
                        return currentAmmo;
                    }
                }
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.ToString());
            return currentAmmo;
        }
    }


    public int VistaObjetosDestruidos(int player){
        int enemigosDestruidos = 0;
        
        try
        {
            using (SqlConnection connection = new SqlConnection(conn))   //esta sentencia permite que connection se destruya al salir de bloque , no hay que usar connection.close() en otras palabras
            {
                connection.Open();
                //Console.WriteLine("Done.");

                string sql = "SELECT COUNT(playerID) FROM ViewObjetoDestruido WHERE PlayerID = @player"; //ya se ve más bonito


                using (SqlCommand command = new SqlCommand(sql, connection))
                { //https://stackoverflow.com/questions/293311/whats-the-best-method-to-pass-parameters-to-sqlcommand
                    command.Parameters.Add(new SqlParameter("player", player));
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //print(reader.GetInt32(0));
                            enemigosDestruidos = reader.GetInt32(0);
                        }
                        
                        return enemigosDestruidos;
                    }
                }
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.ToString());
            return enemigosDestruidos;
        }
    }

    public float[] LastRowPosicionPlayer(int player){
        float[] lastRow = {0, 0};
        try
        {
            using (SqlConnection connection = new SqlConnection(conn))   //esta sentencia permite que connection se destruya al salir de bloque , no hay que usar connection.close() en otras palabras
            {
                connection.Open();
                //Console.WriteLine("Done.");

                string sql = "SELECT TOP 1 PosicionX, PosicionY FROM ViewPlayerPosicion ORDER BY Fecha DESC"; //ya se ve más bonito


                using (SqlCommand command = new SqlCommand(sql, connection))
                { //https://stackoverflow.com/questions/293311/whats-the-best-method-to-pass-parameters-to-sqlcommand
                    command.Parameters.Add(new SqlParameter("player", player));
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //print(reader.GetInt32(0));
                            lastRow[0] = reader.GetFloat(0);
                            lastRow[1] = reader.GetFloat(1);
                            
                        }
                        
                        return lastRow;
                    }
                }
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.ToString());
            return lastRow;
        }
    }
    //SP municionPlayer, MunicionUsada y player
    public void RestaMunicionesActuales(int player, string ammoName){
        try
        {
            using (SqlConnection connection = new SqlConnection(conn))   //esta sentencia permite que connection se destruya al salir de bloque , no hay que usar connection.close() en otras palabras
            {
                connection.Open();
                print("Done.");

            
                using (SqlCommand command = new SqlCommand("dbo.RestaMunicionesActuales", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure; //si no le ponen esto no funciona
                    command.Parameters.AddWithValue("@player", player);
                    command.Parameters.AddWithValue("@ammoName", ammoName);

                    command.ExecuteNonQuery();
                    print("Se ha restado la municion actual");
                }
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.ToString());
        }        
    }

    //SP municionPlayer, MunicionUsada y player
    public void SumaMunicionesUsadas(int player, string ammoName){
        try
        {
            using (SqlConnection connection = new SqlConnection(conn))   //esta sentencia permite que connection se destruya al salir de bloque , no hay que usar connection.close() en otras palabras
            {
                connection.Open();
                print("Done.");

            
                using (SqlCommand command = new SqlCommand("dbo.SumaMunicionesUsadas", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure; //si no le ponen esto no funciona
                    command.Parameters.AddWithValue("@player", player);
                    command.Parameters.AddWithValue("@ammoName", ammoName);

                    command.ExecuteNonQuery();
                    print("Se ha sumado a la municion usada");
                }
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.ToString());
        }        
    }

    //SP municionPlayer
    public void UpdateAmmo(int player, string ammoName, int newAmmo){
        try
        {
            using (SqlConnection connection = new SqlConnection(conn))   //esta sentencia permite que connection se destruya al salir de bloque , no hay que usar connection.close() en otras palabras
            {
                connection.Open();
                print("Done.");

            
                using (SqlCommand command = new SqlCommand("dbo.UpdateAmmo", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure; //si no le ponen esto no funciona
                    command.Parameters.AddWithValue("@player", player);
                    command.Parameters.AddWithValue("@ammoName", ammoName);
                    command.Parameters.AddWithValue("@ammo", newAmmo);

                    command.ExecuteNonQuery();
                    print("Se ha actualizado la cantidad de municion");
                }
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.ToString());
        }        
    }

    //SP ObjetosDestruibles
    public void IniObjetosDestruibles(string objeto, float tamanio, float rotacion){
        try
        {
            using (SqlConnection connection = new SqlConnection(conn))   //esta sentencia permite que connection se destruya al salir de bloque , no hay que usar connection.close() en otras palabras
            {
                connection.Open();
                print("Done. objetos indestructibles");

            
                using (SqlCommand command = new SqlCommand("dbo.IniObjetoDestruible", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure; //si no le ponen esto no funciona
                    command.Parameters.AddWithValue("@nombre", objeto);
                    command.Parameters.AddWithValue("@tamanio", tamanio);
                    command.Parameters.AddWithValue("@rotacion", rotacion);

                    command.ExecuteNonQuery();
                    print("Se iniciaron los objetos destruibles");
                }
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.ToString());
        }        
    }

    //funcion escalar
    public int totalMunicion(int player){
        try
        {
            using (SqlConnection connection = new SqlConnection(conn))   //esta sentencia permite que connection se destruya al salir de bloque , no hay que usar connection.close() en otras palabras
            {
                connection.Open();
                
                using (SqlCommand command = new SqlCommand("SELECT dbo.SVTotalAmmo(@player)", connection))
                { //https://stackoverflow.com/questions/293311/whats-the-best-method-to-pass-parameters-to-sqlcommand
                    
                    command.Parameters.AddWithValue("@player", player);


                    int valor = (int)command.ExecuteScalar();
                    Console.WriteLine("Lista la funcion con {0}",valor);
                    return valor;
                }
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.ToString());
            return 0;
        }        
    }

    //SP player y municionplayer
    public void AmmoPickup(int player, string ammoName, int newAmmo, string item, float x, float y){
        try
        {
            using (SqlConnection connection = new SqlConnection(conn))   //esta sentencia permite que connection se destruya al salir de bloque , no hay que usar connection.close() en otras palabras
            {
                connection.Open();
                print("Done.");

            
                using (SqlCommand command = new SqlCommand("dbo.AmmoPickup", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure; //si no le ponen esto no funciona
                    command.Parameters.AddWithValue("@player", player);
                    command.Parameters.AddWithValue("@nombreAmmo", ammoName);
                    command.Parameters.AddWithValue("@ammo", newAmmo);
                    command.Parameters.AddWithValue("@nombreItem", item);
                    command.Parameters.AddWithValue("@x", x);
                    command.Parameters.AddWithValue("@y", y);

                    command.ExecuteNonQuery();
                    print("Se ha agarrado mas municion");
                }
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.ToString());
        }                
    }

    //SP ITEMTOCADO
    public void InsertObjetoTrigger(int player, string item, float x, float y){
        try
        {
            using (SqlConnection connection = new SqlConnection(conn))   //esta sentencia permite que connection se destruya al salir de bloque , no hay que usar connection.close() en otras palabras
            {
                connection.Open();
                print("Done.");

            
                using (SqlCommand command = new SqlCommand("dbo.InsertObjetoTocado", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure; //si no le ponen esto no funciona
                    command.Parameters.AddWithValue("@player", player);
                    command.Parameters.AddWithValue("@nombreItem", item);
                    command.Parameters.AddWithValue("@x", x);
                    command.Parameters.AddWithValue("@y", y);

                    command.ExecuteNonQuery();
                    print("Se ha actualizado la cantidad de municion");
                }
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.ToString());
        }                
    }

    public void InsertObjetoDestruido(int player, string objeto, string ammo, string fecha){
        try
        {
            using (SqlConnection connection = new SqlConnection(conn))   //esta sentencia permite que connection se destruya al salir de bloque , no hay que usar connection.close() en otras palabras
            {
                connection.Open();
                print("Done. Destruccion enemigo");

            
                using (SqlCommand command = new SqlCommand("dbo.SPObjetoDestruido", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure; //si no le ponen esto no funciona
                    command.Parameters.AddWithValue("@player", player);
                    command.Parameters.AddWithValue("@objeto", objeto);
                    command.Parameters.AddWithValue("@municion", ammo);
                    command.Parameters.AddWithValue("@fecha", fecha);

                    command.ExecuteNonQuery();
                    print("Se ha destruido un enemigo");
                }
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.ToString());
        }                
    }
    //SP PosicionPlayer
    public void PosicionPlayer(int player, string fecha, float x, float y){
        try
        {
            using (SqlConnection connection = new SqlConnection(conn))   //esta sentencia permite que connection se destruya al salir de bloque , no hay que usar connection.close() en otras palabras
            {
                connection.Open();
                print("Done.");

            
                using (SqlCommand command = new SqlCommand("dbo.SPPlayerPosicion", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure; //si no le ponen esto no funciona
                    command.Parameters.AddWithValue("@player", player);
                    command.Parameters.AddWithValue("@fecha", fecha);
                    command.Parameters.AddWithValue("@x", x);
                    command.Parameters.AddWithValue("@y", y);

                    command.ExecuteNonQuery();
                }
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.ToString());
        }     
    }      
/* 

        //Accediendo la info con vistas
        try
        {
            string conn = @"Server=localhost\SQLExpress;Database=DemoUsandoCS;Trusted_Connection=True;";

            using (SqlConnection connection = new SqlConnection(conn))   //esta sentencia permite que connection se destruya al salir de bloque , no hay que usar connection.close() en otras palabras
            { 
                connection.Open();
                Console.WriteLine("Done.");

                string sql = "SELECT * FROM ViewDeTablaDemo"; //ya se ve más bonito
                

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    //command.Parameters.Add()
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine("{0} {1}", reader.GetString(0), reader.GetString(1));
                        }
                    }
                }
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.ToString());
        }

        //Vistas con parámetros
        try
        {
            string conn = @"Server=localhost\SQLExpress;Database=DemoUsandoCS;Trusted_Connection=True;";

            using (SqlConnection connection = new SqlConnection(conn))   //esta sentencia permite que connection se destruya al salir de bloque , no hay que usar connection.close() en otras palabras
            {
                connection.Open();
                Console.WriteLine("Done.");

                string sql = "SELECT * FROM ViewDeTablaDemo WHERE Nombre = @nombre"; //ya se ve más bonito


                using (SqlCommand command = new SqlCommand(sql, connection))
                { //https://stackoverflow.com/questions/293311/whats-the-best-method-to-pass-parameters-to-sqlcommand
                    command.Parameters.Add(new SqlParameter("nombre", "Ventura"));
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine("{0} {1}", reader.GetString(0), reader.GetString(1));
                        }
                    }
                }
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.ToString());
        }

        //STORE PROCEDURES
        try
        {
            string conn = @"Server=localhost\SQLExpress;Database=DemoUsandoCS;Trusted_Connection=True;";

            using (SqlConnection connection = new SqlConnection(conn))   //esta sentencia permite que connection se destruya al salir de bloque , no hay que usar connection.close() en otras palabras
            {
                connection.Open();
                Console.WriteLine("Done.");

            
                using (SqlCommand command = new SqlCommand("dbo.InsertarDatos", connection))
                { //https://stackoverflow.com/questions/293311/whats-the-best-method-to-pass-parameters-to-sqlcommand
                    command.CommandType = System.Data.CommandType.StoredProcedure; //si no le ponen esto no funciona
                    command.Parameters.AddWithValue("@Nombre","Molina");
                    command.Parameters.AddWithValue("@Matricula", "2222222222");

                    command.ExecuteNonQuery();
                    Console.WriteLine("Lista escritura con un SP.");
                }
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.ToString());
        }


        //FUNCIONES (una escalar) (no olvidar que hay 3 tipos)
        try
        {
            string conn = @"Server=localhost\SQLExpress;Database=DemoUsandoCS;Trusted_Connection=True;";

            using (SqlConnection connection = new SqlConnection(conn))   //esta sentencia permite que connection se destruya al salir de bloque , no hay que usar connection.close() en otras palabras
            {
                connection.Open();
                
                using (SqlCommand command = new SqlCommand("SELECT dbo.ElCubo(@X)", connection))
                { //https://stackoverflow.com/questions/293311/whats-the-best-method-to-pass-parameters-to-sqlcommand
                    
                    command.Parameters.AddWithValue("@X", 2);


                    int valor = (int)command.ExecuteScalar();
                    Console.WriteLine("Lista la funcion con {0}",valor);
                }
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.ToString());
        }






        Console.WriteLine("All done. Press any key to finish...");
        Console.ReadKey(true);

        */
    
}


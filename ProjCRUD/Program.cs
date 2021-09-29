using System;
using static System.Console;
using System.Data;
using MySql.Data.MySqlClient;


namespace ProjCRUD
{
    
    class Program
    {
        static string conString = "Server=localhost;Port=3306;Database=schema_csharp;Uid=lucas;Pwd=lpm123456789";
        static MySqlConnection con = new MySqlConnection(conString);
        static bool ConnectDB()
        {
            try
            {
                con.Open();
                WriteLine("Connected!");
                return true;
            }
            catch (MySqlException ex)
            {
                WriteLine("Error connecting database - " + ex.Message + "\n");
                ReadLine();
                return false;
            }
        }

        static void CloseConnectionDB()
        {
            if (con.State == ConnectionState.Open) con.Clone();
        }

        static void CloseApplication()
        {
            CloseConnectionDB();
            WriteLine("\n\nPress Enter to Exit...");
            ReadLine();
        }


        static void Menu()
        {
            Clear();
            WriteLine("MENU: \n\n" +
                      "1 - Register Client \n" +
                      "2 - Delete Client \n" +
                      "3 - Alter Client \n" +
                      "4 - Client List \n" +
                      "5 - Exit");
        }

        static bool ExistsClient(string codClient)
        {
            MySqlTransaction transDB;
            transDB = con.BeginTransaction();

            //CREATES SQL COMMAND USING MYSQL
            MySqlCommand sqlcmd = con.CreateCommand();
            sqlcmd.Connection = con;
            sqlcmd.Transaction = transDB;

            try
            {
                sqlcmd.CommandType = CommandType.Text;
                sqlcmd.CommandText = "SELECT COUNT(1) FROM CLIENTE WHERE COD_CLIENTE = \"" + codClient + "\"";
                sqlcmd.ExecuteNonQuery();
                transDB.Commit();
                return true;
            }
            catch (MySqlException ex)
            {
                WriteLine("Error access database - " + ex.Message);
                ReadLine();
                transDB.Rollback();
                return false;
            }
            finally
            {
                CloseConnectionDB();
            }
        }


        static bool InsertClient(string codClient, string nameClient)
        {
            MySqlTransaction transDB;
            transDB = con.BeginTransaction();

            //CREATES SQL COMMAND USING MYSQL
            MySqlCommand sqlcmd = con.CreateCommand();
            sqlcmd.Connection = con;
            sqlcmd.Transaction = transDB;

            try
            {
                sqlcmd.CommandType = CommandType.Text;
                sqlcmd.CommandText = "INSERT CLIENTE VALUES ('"+ codClient + "', '" + nameClient + "')";
                sqlcmd.ExecuteNonQuery();
                transDB.Commit();
                return true;
            }
            catch (MySqlException ex)
            {
                WriteLine("Error inserting in database - " + ex.Message);
                ReadLine();
                transDB.Rollback();
                return false;
            }
            finally
            {
                CloseConnectionDB();
            }            
        }

        static bool DeleteClient(string codClient)
        {
            MySqlTransaction transDB;
            transDB = con.BeginTransaction();

            //CREATES SQL COMMAND USING MYSQL
            MySqlCommand sqlcmd = con.CreateCommand();
            sqlcmd.Connection = con;
            sqlcmd.Transaction = transDB;

            try
            {
                sqlcmd.CommandType = CommandType.Text;
                sqlcmd.CommandText = "DELETE FROM CLIENTE WHERE COD_CLIENTE = \""+ codClient + "\"";
                sqlcmd.ExecuteNonQuery();
                transDB.Commit();
                return true;
            }
            catch (MySqlException ex)
            {
                WriteLine("Error deleting from database - " + ex.Message);
                ReadLine();
                transDB.Rollback();
                return false;
            }
            finally
            {
                CloseConnectionDB();
            }
        }

        static void Main(string[] args)
        {           
            string vliOpcao = "";
            

            //Closes the application if there is no database connection
            bool isConnected = ConnectDB();
            if (!isConnected)
            {
                CloseApplication();
                Environment.Exit(0);
            }             

            
            do  {                
                Menu();
                vliOpcao = ReadLine();
                string clientID, clientName = "";
                switch (vliOpcao)
                {
                    case "1":
                        clientID = "";
                        clientName = "";
                        WriteLine("---REGISTER NEW CLIENT---");

                        Write("Enter the ID: ");
                        clientID = ReadLine();
                        if (clientID.Trim() == "")
                        {
                            WriteLine("Invalid argument! Please try again...");
                            ReadLine();
                            break;
                        }
                        Write("Client Name: ");
                        clientName = ReadLine();

                        if (!InsertClient(clientID.PadLeft(5, '0'), clientName)) WriteLine("Error registering new cliente. Please try again...");
                        else WriteLine("Success!");

                        ReadLine();
                        break;

                    case "2":                        
                        WriteLine("---DELETE CLIENT---");
                        Write("Enter the ID: ");
                        clientID = ReadLine();
                        if (clientID.Trim() == "")
                        {
                            WriteLine("Invalid argument! Please try again...");
                            ReadLine();
                            break;
                        }

                        if (!DeleteClient(clientID.PadLeft(5, '0'))) WriteLine("Error deleting cliente. Please try again...");
                        else WriteLine("Success!");

                        ReadLine();
                        break;

                    case "3":

                        WriteLine("Opção 3");

                        break;

                    case "4":

                        WriteLine("Opção 4");

                        break;

                    default:

                        if (vliOpcao != "5") WriteLine("Invalid Option - Try again!");

                        break;                    
                }
                
            } while (vliOpcao != "5");

                        
            CloseApplication();
        }

        
    }

    

}

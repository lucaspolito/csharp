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


        static bool InsertClient(int codClient, string nameClient)
        {
            MySqlTransaction transDB;
            transDB = con.BeginTransaction();

            //CRIA O COMANDO SQL
            MySqlCommand sqlInsert = con.CreateCommand();
            sqlInsert.Connection = con;
            sqlInsert.Transaction = transDB;

            try
            {
                sqlInsert.CommandType = CommandType.Text;
                sqlInsert.CommandText = "INSERT CLIENTE VALUES ('"+ codClient + "', '" + nameClient + "')";
                sqlInsert.ExecuteNonQuery();
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
                switch (vliOpcao)
                {
                    case "1":
                        int clientID = 0;
                        string clientName = "";

                        WriteLine("REGISTER NEW CLIENT");

                        Write("Enter the ID: ");
                        if (!Int32.TryParse(ReadLine(), out clientID))
                        {
                            WriteLine("Invalid number! Please try again...");
                            ReadLine();
                            break;
                        }
                        Write("Client Name: ");
                        clientName = ReadLine();

                        if (!InsertClient(clientID, clientName)) WriteLine("Error registering new cliente. Please try again...");
                        else WriteLine("Success!");

                        ReadLine();
                        break;
                    case "2":

                        WriteLine("Opção 2");

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

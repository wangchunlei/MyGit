using System;
using System.Collections.Generic; 

using System.Data.SqlClient;
using PokeIn;

namespace SQLSample
{
    public class SQLInterface
    {  
        //SQL Connection string
        private const string
            ConnStr = "User ID=USERNAME_HERE;Password=PASSWORD_HERE;"
                      +"Data Source=SERVER_HERE;Initial Catalog=DB_HERE;Persist Security Info=True;";

        private SqlConnection connection;

        private string _clientId;
        public SQLInterface(string clientId)
        {
            _clientId = clientId;
            connection = new SqlConnection(ConnStr);
        }

        //Because the definition for below function is "private", it won't be defined on the client side
        private void SetClientStatus(string mess)
        {
            PokeIn.Comet.CometWorker.SendToClient(_clientId, JSON.Method("SetStatus", mess));
        }

        public void GetRecords(string tableName, int recordCount)
        {
            //Let client user know what is going on
            SetClientStatus("Connecting To DB");

            //OPEN SQL Connection
            try
            {
                connection.Open();
            }
            catch
            {
                //if there is an error, show an alert on client side
                PokeIn.Comet.CometWorker.SendToClient(_clientId, "alert('Connection Error!');");
                SetClientStatus("Connection Error!");
                return;
            }

            SetClientStatus("Preparing Records");

            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT top "+ recordCount.ToString() +" * FROM " + tableName ;
            SqlDataReader reader = cmd.ExecuteReader();

            List<string> records = new List<string>();
            if (reader.Read())
            { 
                do
                { 
                    //grab all information within the row into our string parameter 
                    string record = "";
                    for (int i = 0, l = reader.FieldCount; i < l;i++ )
                    {
                        if (i != 0)
                            record += " - ";
                        record += reader[i].ToString();
                    }
                    records.Add(record);
                } while (reader.Read());
            }

            //Close Connections
            reader.Close();
            connection.Close();

            //Send our json to Client Side
            string json = JSON.Method("FillRecords", (object)records.ToArray());
            PokeIn.Comet.CometWorker.SendToClient(_clientId, json);
        }

        public void GetTables()
        {
            //Let client user know what is going on
            SetClientStatus("Connecting To DB");

            //OPEN SQL Connection
            try
            {
                connection.Open();
            }
            catch
            {
                //if there is an error, show an alert on client side
                PokeIn.Comet.CometWorker.SendToClient(_clientId, "alert('Connection Error!');");
                SetClientStatus("Connection Error!");
                return;
            }

            SetClientStatus("Preparing Table List");

            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM INFORMATION_SCHEMA.TABLES";
            SqlDataReader reader = cmd.ExecuteReader();
            
            //Prepare json string like ['MyTable', 'MySecondTable']
            List<string> tables = new List<string>();
            if (reader.Read())
            {
                do
                {
                    tables.Add(reader[2].ToString());
                } while (reader.Read());
            } 

            //Close Connections
            reader.Close();
            connection.Close();

            //Send our json to Client Side
            string json = PokeIn.JSON.Method("FillTable", (object)tables.ToArray());
            PokeIn.Comet.CometWorker.SendToClient(_clientId, json);
        }
    
    }
}
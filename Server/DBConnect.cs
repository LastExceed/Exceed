using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace Server
{
    class DBConnect {

        //TODO:
        //add global server-config-file handler
        //get loginRequest from bridge
        //  answer to bridge with hashed password
        //get registerRequest from bridge
        //  verify email address
        //  answer to bridge with status (success? failed? whatever?)
        //


        public MySqlConnection dbConnection = null;

        private string dbServer;
        private string dbUser;
        private string dbPass;
        private string dbName;
        private bool isConnected;
        private string serverConfig_filePath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName + @"\server.conf");

        public DBConnect() {
            this.Initialize();

        }

        private void Initialize() {
            if (!File.Exists(this.serverConfig_filePath)) {
                //TODO: Move to server startup - replace with proper error handling
                File.Create(this.serverConfig_filePath);
                TextWriter serverConfig_textWriter = new StreamWriter(this.serverConfig_filePath);
                serverConfig_textWriter.WriteLine("#This is the Server Configuration File!\n#!!IF YOU DON'T KNOW WHAT TO DO - READ THE INSTRUCTIONS!\n#===============\ndbServer=localhost\ndbName=Exceed\ndbUser=root\ndbPass=toor");
                serverConfig_textWriter.Close();

            } else {
                string line;
                string[] lineVar;

                // Read the file and display it line by line.  
                StreamReader serverConfig_streamReader = new StreamReader(this.serverConfig_filePath);
                while ((line = serverConfig_streamReader.ReadLine()) != null && line[0] != '#') {
                    lineVar = line.Split('=');
                    switch (lineVar[0]) {
                        case "dbServer":
                            this.dbServer = lineVar[1];
                            break;
                        case "dbName":
                            this.dbName = lineVar[1];
                            break;
                        case "dbUser":
                            this.dbUser = lineVar[1];
                            break;
                        case "dbPass":
                            this.dbPass = lineVar[1];
                            break;
                        default:
                            break;
                    }
                }
                serverConfig_streamReader.Close();

                string connectionString;
                connectionString = "SERVER=" + this.dbServer + ";" + "DATABASE=" +
                this.dbName + ";" + "UID=" + this.dbUser + ";" + "PASSWORD=" + this.dbPass + ";";
                this.dbConnection = new MySqlConnection(connectionString);
            }

        }

        //open connection to database
        private bool OpenConnection() {
            try {
                this.dbConnection.Open();
                return true;
            }
            catch (MySqlException ex) {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number) {
                    case 0:
                        MessageBox.Show("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        MessageBox.Show("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }

        //Close connection
        private bool CloseConnection() {
            try {
                this.dbConnection.Close();
                return true;
            }
            catch (MySqlException ex) {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        //Insert statement
        public void RegisterNewAccount() {

            //TODO: check if email does even exist
            bool isEmailValid = true;


            if (isEmailValid == true) {

                string query = "INSERT INTO users (email, nickname, hashedPassword) VALUES('mail@example.tld', 'John Smith', 'password123')";

                //open connection
                if (this.OpenConnection() == true) {
                    //create command and assign the query and connection from the constructor
                    MySqlCommand cmd = new MySqlCommand(query, this.dbConnection);

                    //Execute command
                    cmd.ExecuteNonQuery();

                    //close connection
                    this.CloseConnection();
                }
            }
        }

        public string getHashedPassword() {
            //Select statement
            string query = "SELECT pass FROM users WHERE(email IS 'mail@example.tld')";

            //Create a list to store the result
            string hashedPassword = "";
            //Open connection
            if (this.OpenConnection() == true) {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, this.dbConnection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read()) {
                    hashedPassword = dataReader["hashedPassword"] + "";
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return hashedPassword;
            } else {
               return hashedPassword;
            }
        }
    }

}

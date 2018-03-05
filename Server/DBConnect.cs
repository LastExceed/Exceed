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
        //get loginRequest from bridge
        //answer to bridge with hashed password
        //get registerRequest from bridge
        //verify email address
        //answer to bridge with status (success? failed? whatever?)

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
                File.WriteAllText(this.serverConfig_filePath, "#This is the Server Configuration File!\n#!!IF YOU DON'T KNOW WHAT TO DO - READ THE INSTRUCTIONS!\n#===============\ndbServer=localhost\ndbName=Exceed\ndbUser=root\ndbPass=toor");
            }
            else {
                string[] lines = File.ReadAllLines(this.serverConfig_filePath);
                foreach (var line in lines) {
                    if (line[0] != '#') continue;
                    string[] lineVar = line.Split('=');
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
                            throw new InvalidDataException();
                    }
                }
                
                string connectionString = string.Format("SERVER={0};DATABASE={1};UID={2};PASSWORD={3};",
                                                        this.dbServer,
                                                        this.dbName,
                                                        this.dbUser,
                                                        this.dbPass);
                this.dbConnection = new MySqlConnection(connectionString);
            }
        }

        private bool OpenConnection() {
            try {
                this.dbConnection.Open();
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

                    default:
                        MessageBox.Show("unknown db connection error");
                        break;
                }
                return false;
            }
            return true;
        }
        
        private bool CloseConnection() {
            try {
                this.dbConnection.Close();
            }
            catch (MySqlException ex) {
                MessageBox.Show(ex.Message);
                return false;
            }
            return true;
        }

        public void RegisterNewAccount() {
            //TODO: check if email does even exist
            bool isEmailValid = true;

            if (isEmailValid) {
                if (this.OpenConnection() == true) {
                    string query = "INSERT INTO users (email, nickname, hashedPassword) VALUES('mail@example.tld', 'John Smith', 'password123')";
                    MySqlCommand cmd = new MySqlCommand(query, this.dbConnection);
                    //TODO: catch exception on existing email
                    cmd.ExecuteNonQuery();
                    this.CloseConnection();
                }
            }
        }

        public string GetHashedPassword() {
            string query = "SELECT password FROM users WHERE(email IS 'mail@example.tld')";
            
            string hashedPassword = null;
            if (this.OpenConnection()) {
                MySqlCommand cmd = new MySqlCommand(query, this.dbConnection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                
                while (dataReader.Read()) {
                    hashedPassword = (string)dataReader["password"];
                }
                dataReader.Close();
                this.CloseConnection();
            }
            return hashedPassword;
        }
    }
}

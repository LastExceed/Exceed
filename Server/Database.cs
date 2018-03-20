using Resources;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace Server {
    static class Database {
        const string dbFileName = "db.sqlite";
        static SQLiteConnection dbConnection = new SQLiteConnection($"Data Source={dbFileName};Version=3;");

        public static void Setup() {
            if (!File.Exists(dbFileName)) {
                Console.WriteLine("no database found, creating from scratch");
                SQLiteConnection.CreateFile(dbFileName);

                dbConnection.Open();

                var tableStrings = new List<string> {
                    "CREATE TABLE users  (name        VARCHAR(11)  NOT NULL UNIQUE," +
                                         "password    VARCHAR(64)  NOT NULL," +
                                         "email       VARCHAR(64)  NOT NULL UNIQUE," +
                                         "permissions TINYINT)",
                                                                   
                    "CREATE TABLE clans  (name        VARCHAR(64)  NOT NULL UNIQUE," +
                                         "tag         VARCHAR(3)   NOT NULL UNIQUE)",
                                                                   
                    "CREATE TABLE logins (name        VARCHAR(11)  NOT NULL," +
                                         "ip          INT          NOT NULL," +
                                         "mac         CHAR(12)     NOT NULL)",//TODO: replace name with relation
                                                                   
                    "CREATE TABLE bans   (name        VARCHAR(11)  NOT NULL," +
                                         "ip          INT          NOT NULL," +
                                         "mac         CHAR(12)     NOT NULL," +
                                         "reason      VARCHAR(255) NOT NULL)",//same as logins, maybe use relation instead?
                };
                var cmd = new SQLiteCommand(dbConnection);
                foreach (string sql in tableStrings) {
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }

                cmd.CommandText = "INSERT INTO users (name, password, email) VALUES ('testuser1', 'apple', 'datboi@exceed.rocks')";
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            else {
                dbConnection.Open();
            }
            Console.WriteLine("database loaded");
        }

        public static RegisterResponse RegisterUser(string username, string email, string password) {
            using (var cmd = new SQLiteCommand(dbConnection)) {
                cmd.CommandText = "SELECT * FROM users WHERE name=@username";
                cmd.Parameters.AddWithValue("@username", username);
                using (var reader = cmd.ExecuteReader()) {
                    if (reader.Read()) {
                        return RegisterResponse.UsernameTaken;
                    }
                }
                cmd.CommandText = "SELECT * FROM users WHERE email=@email";
                cmd.Parameters.AddWithValue("@email", email);
                using (var reader = cmd.ExecuteReader()) {
                    if (reader.Read()) {
                        return RegisterResponse.EmailTaken;
                    }
                }
                cmd.CommandText = "INSERT INTO users (name, email, password) VALUES (@username, @email, @pw)";
                cmd.Parameters.AddWithValue("@pw", password);
                cmd.ExecuteNonQuery();
            }
            return RegisterResponse.Success;
        }

        public static AuthResponse AuthUser(string username, string password, int ip, string mac) {
            using (var cmd = new SQLiteCommand(dbConnection)) {
                cmd.CommandText = "SELECT password FROM users WHERE name=@username";
                cmd.Parameters.AddWithValue("@username", username);
                using (var reader = cmd.ExecuteReader()) {
                    if (!reader.Read()) {
                        return AuthResponse.UnknownUser;
                    }
                    else if ((string)reader["password"] != password) {
                        return AuthResponse.WrongPassword;
                    }
                }
                cmd.CommandText = "SELECT * FROM bans WHERE name=@username OR ip=@ip OR mac=@mac";
                cmd.Parameters.AddWithValue("@ip", ip);
                cmd.Parameters.AddWithValue("@mac", mac);
                using (var reader = cmd.ExecuteReader()) {
                    if (reader.Read()) {
                        return AuthResponse.Banned;
                    }
                }
                cmd.CommandText = "INSERT INTO logins (name, ip, mac) VALUES (@username, @ip, @mac)";
                cmd.ExecuteNonQuery();
            }
            return AuthResponse.Success;
        }

        public static void BanUser(string username, int ip, string mac, string reason) {
            using (var cmd = new SQLiteCommand(dbConnection)) {
                cmd.CommandText = "INSERT INTO bans (name, ip, mac, reason) VALUES (@username, @ip, @mac, @reason)";
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@ip", ip);
                cmd.Parameters.AddWithValue("@mac", mac);
                cmd.Parameters.AddWithValue("@reason", reason);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
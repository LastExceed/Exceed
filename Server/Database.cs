using Resources;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace Server {
    static class Database {
        const string dbFileName = "db.sqlite";
        static SQLiteConnection dbConnection = new SQLiteConnection(string.Format("Data Source={0};Version=3;", dbFileName));

        public static void Setup() {
            if (!File.Exists(dbFileName)) {
                Console.WriteLine("no database found, creating from scratch");
                SQLiteConnection.CreateFile(dbFileName);

                dbConnection.Open();

                var tableStrings = new List<string> {
                    "CREATE TABLE users (name VARCHAR(11) NOT NULL UNIQUE, password VARCHAR(255) NOT NULL, email VARCHAR(255) NOT NULL UNIQUE)",
                    "CREATE TABLE clans (name VARCHAR(255) NOT NULL UNIQUE, tag VARCHAR(3) NOT NULL UNIQUE)",
                    "CREATE TABLE logins (name VARCHAR(11) NOT NULL, ip INT NOT NULL, mac VARCHAR(12) NOT NULL)",//TODO: replace name with relation
                    "CREATE TABLE bans (name VARCHAR(11) NOT NULL, ip INT NOT NULL, mac VARCHAR(12) NOT NULL, reason VARCHAR(255))",//same as logins, maybe use relation instead?
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

        public static RegisterResponse RegisterUser(string username, string email, string pw) {
            var cmd = new SQLiteCommand(dbConnection) {
                CommandText = string.Format("SELECT * FROM users WHERE name='{0}'", username)
            };
            var reader = cmd.ExecuteReader();
            if (reader.Read()) return RegisterResponse.UsernameTaken;
            reader.Close();

            cmd.CommandText = string.Format("SELECT * FROM users WHERE email='{0}'", email);
            reader = cmd.ExecuteReader();
            if (reader.Read()) return RegisterResponse.EmailTaken;
            reader.Close();

            cmd.CommandText = string.Format("INSERT INTO users (name, email, password) VALUES ('{0}', '{1}', '{2}')", username, email, pw);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            return RegisterResponse.Success;
        }

        public static AuthResponse AuthUser(string username, string password, int ip, string mac) {
            var cmd = new SQLiteCommand(dbConnection) {
                CommandText = string.Format("SELECT password FROM users WHERE name='{0}'", username)
            };
            var reader = cmd.ExecuteReader();
            if (!reader.Read()) return AuthResponse.UnknownUser;
            if ((string)reader["password"] != password) return AuthResponse.WrongPassword;
            reader.Close();

            cmd.CommandText = string.Format("SELECT * FROM bans WHERE name='{0}' OR ip={1} OR mac='{2}'", username, ip, mac);
            reader = cmd.ExecuteReader();
            if (reader.Read()) return AuthResponse.Banned;
            reader.Close();

            cmd.CommandText = string.Format("INSERT INTO logins (name, ip, mac) VALUES ('{0}', {1}, '{2}')", username, ip, mac);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

            return AuthResponse.Success;
        }

        public static void BanUser(string username, int ip, string mac, string reason) {
            var cmd = new SQLiteCommand(dbConnection) {
                CommandText = string.Format("INSERT INTO bans (name, ip, mac, reason) VALUES ('{0}', {1}, '{2}', '{3}')", username, ip, mac, reason),
            };
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }
    }
}
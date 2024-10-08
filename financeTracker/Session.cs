using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace financeTracker
{
    internal class Session
    {
        public User? ActiveUser { get; set; }
        private string SessionFile;
        private string UsersFile;
        public Session() 
        {
            UsersFile = "users.json";
            SessionFile = "session.json";
            if (!File.Exists(SessionFile))
            {
                File.WriteAllText(SessionFile,"{}");
            }
            if (!File.Exists(UsersFile))
            {
                File.WriteAllText(UsersFile, "[]");
            }
            string jsonString = File.ReadAllText(SessionFile);
            
            try
            {
                User? user = JsonSerializer.Deserialize<User>(jsonString); 
                
                if (user == null || user.UserName == string.Empty)
                {
                    ActiveUser = null;
                }
                else
                {
                    ActiveUser = user;
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }            
        }

        public User? Registered(string userName)
        {
            string jsonUsers = File.ReadAllText(UsersFile);
            try
            {
                List<User> listUsers = JsonSerializer.Deserialize<List<User>>(jsonUsers) ?? new List<User>();
                foreach (User item in listUsers)
                {
                    if (item.UserName == userName)
                    {
                        return item;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            

            
            return null;
        }

        public void Register(string userName)
        {
            // method of activating a user session.
            // If the user is not registered, then the null value is returned to the user variable
            User? user = Registered(userName);
            if (user == null) 
            {
                //if the user is not registered, then we create a new user
                user = new User(userName);
                string jsonUsers = File.ReadAllText(UsersFile);
                List<User> listUsers = JsonSerializer.Deserialize<List<User>>(jsonUsers) ?? new List<User>();
                listUsers.Add(user);
                string jsonString = JsonSerializer.Serialize(listUsers);
                File.WriteAllText(UsersFile, jsonString);
            }
            // Determine the active user for the current session            
            ActiveUser = user;
            string json = JsonSerializer.Serialize(user);
            File.WriteAllText(SessionFile, json);
        }

        public void ChangePlan(string plan, IAccountList accountList)
        {
            if (ActiveUser == null)
            {
                throw new Exception("No authorized user");
            }
            if (plan == "basic" && ActiveUser.Plan == "gold")
            {
                // Verifying user compliance with the plan basic
                if (ActiveUser.Accounts.Count>2 || ActiveUser.TotalBalance(accountList) > 10000)
                {
                    throw new Exception("Please adjust your account first");
                }
            }

            // change plan in current session
            ActiveUser.Plan = plan;
            string json = JsonSerializer.Serialize(ActiveUser);
            File.WriteAllText(SessionFile, json);

            // change plan in file with Users
            string jsonUsers = File.ReadAllText(UsersFile);
            List<User> listUsers = JsonSerializer.Deserialize<List<User>>(jsonUsers) ?? new List<User>();
            foreach (User user in listUsers)
            {
                if (user.UserName == ActiveUser.UserName)
                {
                    user.Plan = plan;
                    break;
                }
            }
            json = JsonSerializer.Serialize(listUsers);
            File.WriteAllText(UsersFile,json);

        }

        

        public void AddLog(string json)
        {
            // adding event description to log
            string filePath = "log.json";

            List<dynamic> data = new List<dynamic>();
            if (File.Exists(filePath))
            {
                string jsonString = File.ReadAllText(filePath);
                data = JsonSerializer.Deserialize<List<dynamic>>(jsonString) ?? new List<dynamic>();
            }

            var obj = JsonSerializer.Deserialize<dynamic>(json);

            data.Add(obj);
            string updatedJson = JsonSerializer.Serialize(data);
            File.WriteAllText(filePath, updatedJson);

        }
        public void AddAccount(string name)
        {
            // adding an account for an active user in current session
            if (ActiveUser == null) return;

            // change list accounts for the active user (in session)
            ActiveUser.AddAccount(name);
            string json = JsonSerializer.Serialize(ActiveUser);
            File.WriteAllText(SessionFile, json);

            // Change list accounts for user in list Users 
            string jsonUsers = File.ReadAllText(UsersFile);

            List<User> listUsers = JsonSerializer.Deserialize<List<User>>(jsonUsers) ?? new List<User>();
            foreach(User user in listUsers)
            {
                if (user.UserName == ActiveUser.UserName)
                {
                    user.Accounts.Add(name);
                    break;
                }
            }
            string jsonString = JsonSerializer.Serialize(listUsers);
            File.WriteAllText(UsersFile, jsonString);
        }
    }
}

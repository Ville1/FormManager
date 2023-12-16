﻿using FormManager.Data;
using FormManager.Data.Managers;

namespace FormManager.Services
{
    public class DatabaseService
    {
        private readonly Database context;

        public DatabaseService(Database context)
        {
            this.context = context;
        }

        private UserManager? users;
        public UserManager Users
        {
            get {
                if(users == null) {
                    users = new UserManager(context);
                }
                return users;
            }
        }

        private LogManager? log;
        public LogManager Log
        {
            get {
                if (log == null) {
                    log = new LogManager(context);
                }
                return log;
            }
        }
    }
}

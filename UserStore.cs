using System.Collections.Generic;

namespace Inregistrare_BAC
{
    public static class UserStore
    {
        private static readonly Dictionary<string, (string Password, string CNP)> users = new Dictionary<string, (string, string)>();

        static UserStore()
        {
            AddUser("1", "admin", "2001234567899");

        }

        public static bool AddUser(string id, string password, string cnp)
        {
            if (string.IsNullOrWhiteSpace(id) || users.ContainsKey(id))
                return false;

            users[id] = (password, cnp);
            return true;
        }

        public static bool UserExists(string id)
        {
            return !string.IsNullOrWhiteSpace(id) && users.ContainsKey(id);
        }

        public static bool ValidateUser(string id, string password)
        {
            if (string.IsNullOrWhiteSpace(id) || password == null)
                return false;
            if (!users.TryGetValue(id, out var tuple))
                return false;
            return tuple.Password == password;
        }

        public static bool TryGetCnp(string id, out string cnp)
        {
            cnp = null;
            if (string.IsNullOrWhiteSpace(id))
                return false;
            if (!users.TryGetValue(id, out var tuple))
                return false;
            cnp = tuple.CNP;
            return true;
        }
    }
}

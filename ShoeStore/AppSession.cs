using ShoeStore.Models;

namespace ShoeStore
{
    public static class AppSession
    {
        public static UserInfo CurrentUser;

        public static bool IsGuest
        {
            get { return CurrentUser == null; }
        }

        public static bool IsAdmin
        {
            get
            {
                if (CurrentUser == null)
                    return false;
                if (CurrentUser.RoleName == "Администратор")
                    return true;
                return false;
            }
        }

        public static bool IsManager
        {
            get
            {
                if (CurrentUser == null)
                    return false;
                if (CurrentUser.RoleName == "Менеджер")
                    return true;
                return false;
            }
        }

        public static bool IsClient
        {
            get
            {
                if (CurrentUser == null)
                    return false;
                if (CurrentUser.RoleName == "Клиент")
                    return true;
                return false;
            }
        }

        public static void Clear()
        {
            CurrentUser = null;
        }
    }
}

namespace ConsoleServer.Models
{
    public class MessageType
    {
        public enum Type : byte
        {
            /// <summary>
            /// Нет команды
            /// </summary>
            none = 0,

            /// <summary>
            /// Сообщение о регистрации
            /// </summary>
            UserRegistration,

            /// <summary>
            /// Сообщение о авторизации
            /// </summary>
            UserAuthorization,

            /// <summary>
            /// Личное Сообщение КЛИЕНТУ
            /// </summary>
            UserMessage,

            /// <summary>
            /// Личное Сообщение ГРУППЕ
            /// </summary>
            GroupMessage,

            /// <summary>
            /// Личное Сообщение РАЙОНУ
            /// </summary>
            MessageChatRaion,

            /// <summary>
            /// Личное Сообщение ВСЕМ
            /// </summary>
            MessageChatAll,

            /// <summary>
            /// Запрос списка пользователей
            /// </summary>
            GetUsersList,

            /// <summary>
            /// Список пользователей
            /// </summary>
            UsersList,

            /// <summary>
            /// Запрос списка групп
            /// </summary>
            GetGroupsList,

            /// <summary>
            /// Список групп
            /// </summary>
            GroupsList,

            /// <summary>
            /// Запрос списка районов
            /// </summary>
            GetRaionsList,

            /// <summary>
            /// Список районов
            /// </summary>
            RaionsList,

            /// <summary>
            /// Твой профиль пользователя
            /// </summary>
            MyProfile,

            /// <summary>
            /// Профиль пользователя
            /// </summary>
            UserProfile,

            /// <summary>
            /// Пользователь отключился
            /// </summary>
            UserDisconnected,
        }
    }
}

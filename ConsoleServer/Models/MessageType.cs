using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleClient;
using ConsoleClient.Models;

namespace ConsoleClient.Models
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
            MessageChatUser,

            /// <summary>
            /// Личное Сообщение ГРУППЕ
            /// </summary>
            MessageChatGroup,

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

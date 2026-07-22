namespace Services.Texts
{
    public static class GameTexts
    {
        public static class Messages
        {
            public static class Start
            {
                public const string CREATING = "Начало выполнения создания игры";
                public const string UPDATING = "Начало выполнения обновления игры";
                public const string DELETING = "Начало выполнения удаления игры";
                public const string GETBY = "Начало выполнения получения игры по ID";
                public const string GETALL = "Начало выполнения получения списка игр";

                public const string DISPLAY_CREATING = "Начало отображения страницы создания игры";
                public const string DISPLAY_UPDATING = "Начало отображения страницы обновления игры";
                public const string DISPLAY_DELETING = "Начало отображения страницы удаления игры";


            }
            public static class Succses
            {
                public const string CREATING_COMPLETED = "Игра успешно создана";
                public const string UPDATING_COMPLETED = "Игра успешно обновлена";
                public const string DELETING_COMPLETED = "Игра успешно удалена";
                public const string GETBY_COMPLETED = "Игра успешно получена по ID";
                public const string GETALL_COMPLETED = "Список игр успешно получен";

                public const string DISPLAY_CREATING_COMPLETED = "Конец отображения страницы создания игры";
                public const string DISPLAY_UPDATING_COMPLETED = "Конец отображения страницы обновления игры";
                public const string DISPLAY_DELETING_COMPLETED = "Конец отображения страницы удаления игры";

            }
            public static class Error
            {

                public const string CREATING_ERROR = "Не удалось создать игру";
                public const string UPDATING_ERROR = "Не удалось обновить игру";
                public const string DELETING_ERROR = "Не удалось удалить игру";
                public const string GETBY_ERROR = "Не удалось получить игру по ID";
                public const string GETALL_ERROR = "Не удалось получить список игр";

                public const string DISPLAY_CREATING_ERROR = "Не удалось отобразить страницу создания игры";
                public const string DISPLAY_UPDATING_ERROR = "Не удалось отобразить страницу обновления игры";
                public const string DISPLAY_DELETING_ERROR = "Не удалось отобразить страницу удаления игры";

            }
            public static class Validation
            {
                public const string REQUAST_NULL = "Запрос не может быть пустым";
                public const string ID_NOT_VALID = "ID игры должен быть больше 0";
                public const string NAME_EMPTY = "Название игры не должно быть пустым";
                public const string NAME_ALREADY_EXISTS = "Игра с названием {0} уже существует";
                public const string NAME_EXCEEDS_LIMIT = "Имя игры не должно быть больше 50-ти символов";
                public const string NOT_FOUND_BY_ID = "Игра не найдена по ID {0}";

            }
        }
    }

}

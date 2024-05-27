using System;
using System.Globalization;
using Microsoft.Data.Sqlite;

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("1 - вывод данных из выбранной таблицы");
            Console.WriteLine("2 - вывод данных из представления");
            Console.WriteLine("3 - завершение работы");

            Console.WriteLine("Укажите ваш выбор: ");
            string choose = Console.ReadLine();

            if(string.IsNullOrWhiteSpace(choose))
            {
                Console.WriteLine("Вы не выбрали действие.");
                continue;
            }
            switch (choose)
            {
                case "1":
                    DataFromTable();
                    break;
                case "2":
                    DataFromView(); 
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Укажите существующий вариант");
                    break;
            }
        }
    }

    public static void DataFromTable()
    {
        Console.WriteLine("Укажите таблицу ");
        string name = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("ВЫ не указали название таблицы");
            return;
        }
        string sqlzapros = $"select * from {name};";
        using (var context = new SqliteConnection("DataSource=C:\\Program Files\\SQLite\\BD.db"))
        {
            context.Open();
            SqliteCommand command = new SqliteCommand(sqlzapros, context);
            try
            {
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows) // если есть данные
                    {
                        while (reader.Read())   // построчно считываем данные
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                Console.Write($"{reader.GetValue(i)} \t");
                            }
                            Console.WriteLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Таблица пуста");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message + ".");
            }
        }
    }
    public static void DataFromView()
    {
        string sqlzapros = $"select * from BookingDetails;";
        using (var context = new SqliteConnection("DataSource=C:\\Program Files\\SQLite\\BD.db"))
        {
            context.Open();
            SqliteCommand command = new SqliteCommand(sqlzapros, context);
            try
            {
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows) // если есть данные
                    {
                        while (reader.Read())   // построчно считываем данные
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                Console.Write($"{reader.GetValue(i)} \t");
                            }
                            Console.WriteLine();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Представление пустое");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message + ".");
            }
        }
    }
}

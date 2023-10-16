using HabitTracker.Habits;
using System.Data.SQLite;

namespace HabitTracker.Database
{
    public static class DatabaseHelper
    {
        private static readonly string connectionString = @"Data Source=..\..\..\Files\Habits.db;Version=3;";

        public static void InitializeDatabase()
        {
            if (!File.Exists(@"..\..\..\Files\Habits.db"))
            {
                SQLiteConnection.CreateFile(@"..\..\..\Files\Habits.db");

                using (SQLiteConnection connection = new(connectionString))
                {
                    connection.Open();

                    // create tables for your data
                    string createHabitsTableQuery = @"
                        CREATE TABLE IF NOT EXISTS habits(
                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                            habit TEXT NOT NULL,
                            times_done INTEGER NOT NULL,
                            habit_type TEXT NOT NULL
                        );";

                    using (SQLiteCommand command = new(connection))
                    {
                        command.CommandText = createHabitsTableQuery;
                        command.ExecuteNonQuery();
                    }
                }

                AddSampleHabitsToDB();
            }
        }

        public static void AddSampleHabitsToDB()
        {
            using (SQLiteConnection connection = new(connectionString))
            {

                connection.Open();
                string[] habits =
                {
                    "Building a database with SQLite",
                    "Trying to write the Theory Handbook",
                    "Failing to write the Theory Handbook"
                };
                int[] timesDone =
                {
                    1,
                    7,
                    7
                };
                string[] habitType =
                {
                    "Learning",
                    "Attempting",
                    "Failing"
                };

                using (SQLiteCommand command = new(connection))
                {
                    for (int i = 0; i < habits.Length; i++)
                    {
                        command.CommandText =
                            @"INSERT INTO habits (habit, times_done, habit_type)
                            VALUES (@habit, @times_done, @habit_type);";
                        command.Parameters.AddWithValue("@habit", habits[i]);
                        command.Parameters.AddWithValue("@times_done", timesDone[i]);
                        command.Parameters.AddWithValue("@habit_type", habitType[i]);

                        command.ExecuteNonQuery();
                        command.Parameters.Clear();
                    }
                }
            }
        }
        public static Habit GetSpecificHabitByIdFromDB(int id)
        {
            List<Habit> habits = new();

            using (SQLiteConnection connection = new(connectionString))
            {
                connection.Open();
                string getSpecificHabitByIdQuery = "SELECT * FROM habits WHERE id = @id";

                using (SQLiteCommand command = new(getSpecificHabitByIdQuery, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string habitName = reader.GetString(reader.GetOrdinal("habit"));
                            int timesDone = reader.GetInt32(reader.GetOrdinal("times_done"));
                            string habit_type = reader.GetString(reader.GetOrdinal("habit_type"));

                            if (Enum.TryParse(habit_type, true, out HabitType habitType))
                            {
                                habits.Add(new Habit(id, habitName, timesDone, habitType));
                            }
                            else
                            {
                                habits.Add(new Habit(id, habitName, timesDone, HabitType.NotSet));
                            }
                        }
                    }
                }
            }

            return habits[0];
        }
        public static List<Habit> GetAllHabitsFromDB()
        {
            List<Habit> habits = new();

            using (SQLiteConnection connection = new(connectionString))
            {
                connection.Open();

                string getAllHabitsQuery = "SELECT * FROM habits";


                using (SQLiteCommand command = new(getAllHabitsQuery, connection))
                {

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(reader.GetOrdinal("id"));
                            string habitName = reader.GetString(reader.GetOrdinal("habit"));
                            int timesDone = reader.GetInt32(reader.GetOrdinal("times_done"));
                            string habit_type = reader.GetString(reader.GetOrdinal("habit_type"));

                            if (Enum.TryParse(habit_type, true, out HabitType habitType))
                            {
                                habits.Add(new Habit(id, habitName, timesDone, habitType));
                            }
                            else
                            {
                                habits.Add(new Habit(id, habitName, timesDone, HabitType.NotSet));
                            }
                        }
                    }
                }
            }

            return habits;

        }
        public static void InsertNewHabit(Habit habit)
        {
            using (SQLiteConnection connection = new(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new(connection))
                {
                    command.CommandText =
                        @"INSERT INTO habits (habit, times_done, habit_type)
                        VALUES (@habit, @times_done, @habit_type);";

                    command.Parameters.AddWithValue("@habit", habit.HabitName);
                    command.Parameters.AddWithValue("@times_done", habit.TimesDone);
                    command.Parameters.AddWithValue("@habit_type", habit.HabitType);

                    command.ExecuteNonQuery();
                    command.Parameters.Clear();
                }
            }
        }
        public static void RemoveSpecificHabitByIdFromDB(int id)
        {
            using (SQLiteConnection connection = new(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new(connection))
                {
                    command.CommandText = @"DELETE FROM habits WHERE id = @id";
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                    command.Parameters.Clear();
                }
            }
        }
        public static void UpdateSpecificHabitById(Habit habit)
        {

            using (SQLiteConnection connection = new(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new(connection))
                {
                    command.CommandText = @"
                        UPDATE habits
                        SET habit = @habitName,
                            times_done = @timesDone,
                            habit_type = @habitType
                        WHERE id = @id
                        ";
                    command.Parameters.AddWithValue("@id", habit.Id);
                    command.Parameters.AddWithValue("@habitName", habit.HabitName);
                    command.Parameters.AddWithValue("@timesDone", habit.TimesDone);
                    command.Parameters.AddWithValue("@habitType", habit.HabitType);
                    command.ExecuteNonQuery();
                    command.Parameters.Clear();
                }
            }
        }








    }
}
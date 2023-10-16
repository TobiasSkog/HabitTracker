using HabitTracker.Communication;

namespace HabitTracker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Database.DatabaseHelper.InitializeDatabase();

            UserInteractions.HabitTracker();

        }
    }
}
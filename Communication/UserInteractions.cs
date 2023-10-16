using HabitTracker.Habits;

namespace HabitTracker.Communication
{
    public static class UserInteractions
    {
        private static string Menu = @"
MAIN MENU

Type 0 to Close Application.
Type 1 to View All Records.
Type 2 to View a Specific Record.
Type 3 to Insert Record.
Type 4 to Delete Record.
Type 5 to Update Record.";
        private static string rowLine = "=========================================";


        private static bool runApplication = true;

        private static int UserChoice()
        {
            int choice = InputValidation.GetIntegerRange("\nWhat would you like to do: ", 0, 5);
            return choice;
        }

        public static void HabitTracker()
        {
            while (runApplication)
            {
                Console.WriteLine(Menu);
                List<Habit> habits;
                Habit habit;
                int id, maxRange, choice = UserChoice();

                switch (choice)
                {
                    case 0:
                        runApplication = false;
                        break;
                    case 1:
                        // View All Records
                        habits = Database.DatabaseHelper.GetAllHabitsFromDB();
                        PresentHabitsToUser(habits);
                        habits.Clear();
                        break;
                    case 2:
                        Console.Clear();
                        maxRange = Database.DatabaseHelper.GetHighestId();
                        id = InputValidation.GetIntegerRange($"\nSpecify the ID of the Habit you would like to view (1 - {maxRange}): ", 1, maxRange);
                        habit = Database.DatabaseHelper.GetSpecificHabitByIdFromDB(id);
                        PresentSingularHabitToUser(habit);
                        break;
                    case 3:
                        // Insert Record
                        Console.Clear();
                        habit = Habit.CreateNewHabit();
                        Database.DatabaseHelper.InsertNewHabit(habit);
                        break;
                    case 4:
                        // Delete Record
                        Console.Clear();
                        maxRange = Database.DatabaseHelper.GetHighestId();
                        id = InputValidation.GetIntegerRange($"\nSpecify the ID of the Habit you would like to delete (1 - {maxRange}): ", 1, maxRange);
                        Database.DatabaseHelper.RemoveSpecificHabitByIdFromDB(id);
                        break;
                    case 5:
                        // Update Record
                        Console.Clear();
                        maxRange = Database.DatabaseHelper.GetHighestId();
                        //GET THE ID OF THE HABIT THE USER WANTS TO UPDATE
                        id = InputValidation.GetIntegerRange($"\nSpecify the ID of the Habit you would like to update (1 - {maxRange}): ", 1, maxRange);
                        //GET THE HABIT FROM DATABSE
                        habit = Database.DatabaseHelper.GetSpecificHabitByIdFromDB(id);
                        //MODIFY THE HABIT
                        Habit.UpdateHabit(habit);
                        //UPDATE THE HABIT IN THE DATABASE
                        Database.DatabaseHelper.UpdateSpecificHabitById(habit);
                        break;
                }
            }
        }

        private static void PresentHabitsToUser(List<Habit> habits)
        {
            Console.Clear();
            foreach (var habit in habits)
            {
                Console.WriteLine($"{rowLine}\n   {habit}");
            }
            Console.WriteLine(rowLine);
        }

        private static void PresentSingularHabitToUser(Habit habit)
        {
            Console.Clear();
            Console.WriteLine(rowLine);
            Console.WriteLine("   " + habit);
            Console.WriteLine(rowLine);
        }

    }
}

using HabitTracker.Communication;

namespace HabitTracker.Habits
{
    public class Habit
    {
        public int Id { get; private set; }
        public string HabitName { get; private set; }
        public int TimesDone { get; private set; }
        public HabitType HabitType { get; private set; }

        public Habit(int id, string habitName, int timesDone, HabitType habitType)
        {
            Id = id;
            HabitName = habitName;
            TimesDone = timesDone;
            HabitType = habitType;
        }
        public Habit(string habitName, int timesDone, HabitType habitType)
        {
            HabitName = habitName;
            TimesDone = timesDone;
            HabitType = habitType;
        }

        public static Habit CreateNewHabit()
        {
            string habitName = InputValidation.GetString("What is the name of the new habit: ");
            int timesDone = 0;
            HabitType habitType = InputValidation.ConvertToEnum("What type of habit is it: ");

            return new Habit(habitName, timesDone, habitType);
        }

        public static Habit UpdateHabit(Habit habit)
        {
            if (InputValidation.GetYesOrNo("Do you want to change the name of the habit: "))
            {
                habit.HabitName = InputValidation.GetString("What is the new name of this habit: ");
            }
            if (InputValidation.GetYesOrNo("Do you want to change the amount of times you have done the habit: "))
            {
                habit.TimesDone = InputValidation.GetIntegerRange("How many times have you done this habit: ", 0, 1000);
            }
            if (InputValidation.GetYesOrNo("Do you want to change the type of the habit: "))
            {
                habit.HabitType = InputValidation.ConvertToEnum("What is the new type of this habit: ");
            }
            return habit;
        }
        public override string ToString()
        {
            return $"{HabitName}\n\tID: {Id}\n\tTimes done: {TimesDone}\n\tType of habit: {HabitType.ToString()}";
        }

        //              habits
        //CREATE TABLE IF NOT EXISTS habits(
        //  id INTEGER PRIMARY KEY AUTOINCREMENT,
        //  habit TEXT NOT NULL,
        //  times_done INTEGER NOT NULL,
        //  habit_type TEXT NOT NULL
        //); ";
    }
}

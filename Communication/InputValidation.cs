using HabitTracker.Habits;
using System.Text;

namespace HabitTracker.Communication
{
    internal static class InputValidation
    {
        public static int GetIntegerRange(string prompt, int minRange, int maxRange, string errorMessage = $"Invalid input. Number must be an integer within the range ")
        {
            while (true)
            {
                Console.Write(prompt);

                if (int.TryParse(Console.ReadLine(), out int validInt))
                {
                    if (validInt >= minRange && validInt <= maxRange)
                    {
                        return validInt;
                    }
                }

                Console.WriteLine($"{errorMessage} ({minRange} - {maxRange}).");
            }
        }

        public static bool GetYesOrNo(string prompt)
        {
            Dictionary<string, bool> allowedAnswers = new()
            {
                { "Y", true },
                { "YE", true },
                { "YES", true },
                { "J", true },
                { "JA", true },
                { "K", true },
                { "OK", true },
                { "N", false },
                { "NO", false },
                { "NE", false },
                { "NEJ", false },
                { "NOP", false },
                { "NOPE", false },
                { "NOPER", false },
                { "NOPERS", false },
            };
            while (true)
            {
                Console.Write(prompt);

                string? userInput = Console.ReadLine().ToUpper();

                if (!string.IsNullOrWhiteSpace(userInput) && allowedAnswers.ContainsKey(userInput))
                {
                    return allowedAnswers[userInput];
                }

                Console.WriteLine("Please answer (Y)es or (N)o.");
            }
        }
        public static string GetString(string prompt)
        {

            while (true)
            {
                Console.Write(prompt);

                string? userInput = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(userInput))
                {
                    return userInput;
                }

                Console.WriteLine("Invalid input. You cannot use an empty text or only white spaces.");
            }
        }

        public static HabitType ConvertToEnum(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);

                if (Enum.TryParse(Console.ReadLine(), true, out HabitType validType))
                {
                    return validType;
                }

                Console.WriteLine($"This type of habit does not exist in the database");
            }
        }

        public static (List<string> ErrorMessages, string Username) UsernameValidation(
                string username,
                byte minLength = 5,
                byte maxLength = 113,
                bool avoidWhiteSpaces = true,
                bool avoidIllegalChars = true,
                char[]? illegalChars = null)
        {
            illegalChars ??= new char[] {
            '@', '#', '$', '%', '^', '&','*', '-',
            '_', '!', '+', '=','[', ']', '{', '}',
            '|', '\\',':', '\'', ',', '.', '?', '/',
            '`', '~', '"', '(', ')', ';','<', '>' };

            bool hasIllegal, hasWhiteSpace;
            List<string> errorMessages;
            while (true)
            {
                hasIllegal = false; hasWhiteSpace = false;
                errorMessages = new List<string>();
                foreach (char c in username)
                {
                    if (avoidIllegalChars && illegalChars != null && illegalChars.Contains(c))
                    {
                        hasIllegal = true;
                    }

                    if (avoidWhiteSpaces)
                    {
                        if (c == ' ')
                        {
                            hasWhiteSpace = true;
                        }
                    }
                }

                if (avoidWhiteSpaces && hasWhiteSpace)
                {
                    errorMessages.Add($"Username contains white spaces.");
                }
                if (username.Length < minLength || username.Length > maxLength)
                {
                    errorMessages.Add($"Username must be between {minLength} and {maxLength} characters.");
                }

                if (avoidIllegalChars && hasIllegal)
                {
                    errorMessages.Add("Username contains illegal characters.");
                }

                username = GetString(username);

                return (ErrorMessages: errorMessages, Username: username.Trim());
            }
        }


        public static string PasswordValidation(
                string prompt,
                byte minLength = 8,
                byte maxLength = 113,
                bool mustHaveSpecialCharacters = true,
                bool mustHaveNumbers = true,
                bool mustHaveLowerAndUpperCaseLetters = true,
                bool avoidIllegalChars = false,
                char[]? specialChars = null,
                char[]? illegalChars = null)
        {
            specialChars ??= new char[] {
            '@', '#', '$', '%', '^', '&','*', '-',
            '_', '!', '+', '=','[', ']', '{', '}',
            '|', '\\',':', '\'', ',', '.', '?', '/',
            '`', '~', '"', '(', ')', ';','<', '>' };
            bool hasDigit, hasLower, hasUpper, hasSpecial, hasIllegal, emptyVal;
            List<string> errorMessages;
            while (true)
            {
                string userInput = CharPasswordCreator(prompt, true);

                hasDigit = false; hasLower = false; hasUpper = false; hasSpecial = false; hasIllegal = false;

                errorMessages = new List<string>();
                foreach (char c in userInput)
                {
                    if (mustHaveNumbers && char.IsDigit(c))
                    {
                        hasDigit = true;
                    }

                    if (mustHaveLowerAndUpperCaseLetters)
                    {
                        if (char.IsLower(c))
                        {
                            hasLower = true;
                        }
                        if (char.IsUpper(c))
                        {
                            hasUpper = true;
                        }
                    }

                    if (mustHaveSpecialCharacters && specialChars.Contains(c))
                    {
                        hasSpecial = true;
                    }

                    if (avoidIllegalChars && illegalChars != null && illegalChars.Contains(c))
                    {
                        hasIllegal = true;
                    }
                }

                if (userInput.Length < minLength || userInput.Length > maxLength)
                {
                    errorMessages.Add($"Password must be between {minLength} and {maxLength} characters.");
                }

                if (mustHaveNumbers && !hasDigit)
                {
                    errorMessages.Add("Password must contain at least one digit.");
                }

                if (mustHaveLowerAndUpperCaseLetters && (!hasLower || !hasUpper))
                {
                    errorMessages.Add("Password must contain both lower and upper case letters.");
                }

                if (mustHaveSpecialCharacters && !hasSpecial)
                {
                    errorMessages.Add("Password must contain at least one special character.");
                }

                if (avoidIllegalChars && hasIllegal)
                {
                    errorMessages.Add("Password contains illegal characters.");
                }

                // If there are error messages, display them and ask the user to recreate the password
                if (errorMessages.Count > 0)
                {
                    Console.WriteLine("\nPassword validation failed. Please fix the following issues:");
                    foreach (var error in errorMessages)
                    {
                        Console.WriteLine(error);
                    }
                }
                else
                {
                    return userInput;
                }
            }
        }

        public static string CharPasswordCreator(string prompt, bool showHiddenOutput)
        {
            Console.OutputEncoding = Encoding.Unicode;
            string userInput = "";
            Console.Write(prompt);

            while (true)
            {

                ConsoleKeyInfo key = Console.ReadKey(true);

                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    userInput += key.KeyChar;
                    if (showHiddenOutput)
                    {
                        Console.Write((char)2534);
                    }
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && userInput.Length > 0)
                    {
                        userInput = userInput.Substring(0, (userInput.Length - 1));
                        if (showHiddenOutput)
                        {
                            Console.Write("\b \b");
                        }
                    }
                    else if (key.Key == ConsoleKey.Enter)
                    {
                        if (string.IsNullOrEmpty(userInput))
                        {
                            Console.WriteLine("\nEmpty value not allowed.");
                            userInput = "";
                        }
                        else
                        {
                            return userInput;
                        }
                    }
                }
            }







        }


    }
}
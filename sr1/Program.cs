using System;

namespace sr1
{
    static class Program
    {
        /// <summary>
        /// The type of the operation done
        /// with the numbers given by the user.
        /// </summary>
        private enum OperationType
        {
            Addition,
            Subtraction
        }


        /// <summary>
        /// Prints the message for user.
        /// </summary>
        private static void PrintHead()
        {
            Console.WriteLine("--------------");
            Console.WriteLine("Введите строковое мат.выражение из двух чисел в формате" +
                              " “а+b” или “a-b”, где a и b - целые числа от 0 до 100" +
                              " включительно, между которыми знак + или -.");
            Console.WriteLine("Вы также можете ввести \'exit\' для выхода");
        }

        /// <summary>
        /// Displays the given result on console.
        /// </summary>
        private static void PrintResult(int result)
        {
            Console.WriteLine("-------");
            Console.WriteLine($"Результат: {result}");
            Console.WriteLine();
        }

        /// <summary>
        /// Displays the error text on console.
        /// </summary>
        private static void PrintError(string errorText)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"!!! {errorText}");
            Console.ForegroundColor = ConsoleColor.Black;
        }

        /// <summary>
        /// Gets numbers and the operation from the user.
        /// </summary>
        private static void ReadData(out int numA,
            out int numB,
            out OperationType operationType)
        {
            PrintHead();

            // Reading data.
            var command = Console.ReadLine();

            if (command is null)
            {
                throw new InputException();
            }

            if (command == "exit")
            {
                throw new ExitException();
            }


            // Splitting the command.
            var members = command.Split('+', '-');
            if (members.Length != 2)
            {
                throw new InputException("Неправильный формат ввода. Введённая строка" +
                                         " не сответсвует требуемому шаблону");
            }

            // Trying to parse parts of command to numbers.
            if (!int.TryParse(members[0], out var num1) ||
                !int.TryParse(members[1], out var num2))
            {
                throw new InputException("Неправильный формат ввода." +
                                         " Проверьте правильность ввода чисел.");
            }

            // Checking if the numbers belong to the range needed.
            bool CheckNum(int num) => num is >= 0 and <= 100;
            if (!CheckNum(num1) || !CheckNum(num2))
            {
                throw new InputException("Неправильный формат ввода." +
                                         " Вводимые числа - целые от 0 до 100 включительно.");
            }

            numA = num1;
            numB = num2;
            operationType = command.Contains('+') ? OperationType.Addition : OperationType.Subtraction;
        }

        // Arithmetic operations
        private static int Add(int num1, int num2) => num1 + num2;
        private static int Subtract(int num1, int num2) => num1 - num2;

        /// <summary>
        /// Works with the user's input.
        /// </summary>
        private static void ManageCommands()
        {
            var exitRequired = false;
            while (!exitRequired)
            {
                try
                {
                    ReadData(out var numA, out var numB, out var operationType);

                    switch (operationType)
                    {
                        case OperationType.Addition:
                            PrintResult(Add(numA, numB));
                            break;

                        case OperationType.Subtraction:
                            PrintResult(Subtract(numA, numB));
                            break;
                    }
                }
                catch (ExitException)
                {
                    exitRequired = true;
                }
                catch (InputException e)
                {
                    PrintError(e.Message);
                }
                catch (Exception)
                {
                    PrintError("Что-то пошло не так... Попробуйте снова!");
                }
            }

            Console.WriteLine("До свидания!");
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Программа-калькулятор");
            ManageCommands();
        }
    }

    /// <summary>
    /// Occurs when the user prints exit command.
    /// </summary>
    class ExitException : ApplicationException
    {
        public ExitException() : this("Пользователь ввёл команду exit")
        {
        }

        public ExitException(string message) : base(message)
        {
        }
    }

    /// <summary>
    /// Occurs whenever the user provides data in wrong format.
    /// </summary>
    class InputException : ApplicationException
    {
        public InputException() : this("Неправильный ввод. Повторите попытку.")
        {
        }

        public InputException(string message) : base(message)
        {
        }
    }
}
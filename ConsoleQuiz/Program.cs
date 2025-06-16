namespace ConsoleQuizApp
{
    // Represents a question in the quiz. The text of the question, the options available, and the index of the correct answer.
    record Question(string questionText, string[] options, int correctAnswerIndex);

    class ConsoleQuiz
    {
        static int Main(string[] args)
        {
            // A list of questions for the quiz. Accessed via index.
            List<Question> questions = new List<Question>();

            Console.WriteLine("Welcome to the quiz!");
            Console.WriteLine("Please enter the path to the file containing the questions:");
            Console.Write("File path: ");
            
            string? filePath = Console.ReadLine(); // User may input null, so use nullable type.
            if (string.IsNullOrWhiteSpace(filePath))
            {
                Console.WriteLine("File path cannot be empty.");
                Environment.Exit(1);
            } else if (!File.Exists(filePath))
            {
                Console.WriteLine($"File not found: {filePath}");
                Environment.Exit(2);
            }

            Console.WriteLine(new string('-', 20)); // Formatting

            LoadQuestionsFromFile(filePath, questions);

            if (questions.Count == 0)
            {
                Console.WriteLine("No questions found in the file.");
                Environment.Exit(4);
            }

            foreach (Question question in questions)
            {
                Console.WriteLine(question.questionText);
                for (int i = 0; i < question.options.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {question.options[i]}");
                }

                while (true)
                {
                    Console.Write("Please enter the number of your answer: ");
                    string? input = Console.ReadLine();

                    if (int.TryParse(input, out int answerIndex) && answerIndex > 0 && answerIndex <= question.options.Length)
                    {
                        if (answerIndex - 1 == question.correctAnswerIndex)
                        {
                            Console.WriteLine("Correct!\n");
                        }
                        else
                        {
                            Console.WriteLine($"Incorrect. The correct answer was: {question.options[question.correctAnswerIndex]}\n");
                        }
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid option number.\n");
                    }
                }
            }

            // Default exit code
            return 0;
        }

        static void LoadQuestionsFromFile(string filePath, List<Question> questions)
        {
            // Read the whole file then parse each question and store in the questions list.
            string fileContents = File.ReadAllText(filePath);
            string[] questionLines = fileContents.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in questionLines)
            {
                string[] parts = line.Split('|');
                if (parts.Length < 3
                    || string.IsNullOrEmpty(parts[0].Trim()) // Question text must not be empty
                    || parts[1].Trim().Split(',').Select(o => o.Trim()).Where(o => !string.IsNullOrEmpty(o)).ToArray().Length < 2 // Must have at least two options
                    || !int.TryParse(parts[2].Trim(), out int answerIndex) // Answer index must be a valid integer
                 ) {
                    Console.WriteLine($"Invalid question format: {line}" + 
                        $"{Environment.NewLine}Must be \"question|option[, option, ...]|answerIndex\"");
                    Environment.Exit(3);
                }

                questions.Add(new Question(
                    parts[0].Trim(), // Question text
                    parts[1].Trim().Split(',').Select(o => o.Trim()).Where(o => !string.IsNullOrEmpty(o)).ToArray(), // Options
                    parts[2].Trim() switch
                    {
                        // Parse the answer index, ensuring it's a valid integer
                        var s when int.TryParse(s, out int index) && index >= 0 && index < parts[1].Split(',').Length => index,
                        _ => throw new FormatException($"Invalid answer index: {parts[2]}")
                    }
                ));
            }

        }
    }
}
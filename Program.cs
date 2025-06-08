using System.IO;

namespace ConsoleQuizApp
{
    /* Uses a record rather than a struct. A struct should represent a single logical
     * entity (e.g. point, up to 16B) whereas a record can be used to represent a more 
     * complex entity. A struct is passed by value (and allocated on the stack) while a record
     * is a reference type (and allocated on the heap). Records are immutable by default.
     * A struct may be more sutable for performance-critical code (e.g. arrays of points).
     * The alternative would be to use a class, but records provide a more concise syntax
     * but for a simple data structure like this, a record is more appropriate.
     */
    record Question(string questionText, string[] options, int correctAnswerIndex);

    class ConsoleQuiz
    {
        static int Main(string[] args)
        {
            // A list of questions for the quiz. Accessed via index.
            List<Question> questions = new List<Question>();

            Console.WriteLine("Welcome to the quiz!\n");
            Console.WriteLine("Please enter the path to the file containing the questions\n");
            string? filePath = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path cannot be empty.");
            else if (!File.Exists(filePath))
                throw new FileNotFoundException($"File not found: {filePath}");

            LoadQuestionsFromFile(filePath, questions);

            foreach (var q in questions)
            {
                Console.WriteLine($"Question: {q.questionText}");
                Console.WriteLine("Options:");
                for (int i = 0; i < q.options.Length; i++)
                {
                    Console.WriteLine($"  {i}: {q.options[i]}");
                }
                Console.WriteLine($"Answer Index: {q.correctAnswerIndex}");
                Console.WriteLine();
            }

            // Default exit code
            return 0;
        }

        static void LoadQuestionsFromFile(string filePath, List<Question> questions)
        {
            // Read the whole file then parse each question and store in the questions list.
            string fileContets = File.ReadAllText(filePath);
            string[] questionLines = fileContets.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in questionLines)
            {
                string[] parts = line.Split('|');
                if (parts.Length < 3) {
                    throw new FormatException($"Invalid question format: {line}" + 
                        $"{Environment.NewLine}Must be \"question|option[, option, ...]|answerIndex\"");
                }

                questions.Add(new Question(
                    parts[0].Trim(), // Question text
                    parts[1].Trim().Split(',').Select(o => o.Trim()).ToArray(), // Options
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
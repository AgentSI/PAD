using Newtonsoft.Json;

namespace Common
{
    public static class PayloadStorage
    {
        private static string filePath = @"C:\\Users\\Evghenia\\Desktop\\PAD\\message_storage.txt";

        public static Payload GetNext()
        {
            Payload payload = null;

            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        try
                        {
                            payload = JsonConvert.DeserializeObject<Payload>(line);
                            break;
                        }
                        catch(Exception e)
                        {

                        }
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error reading message file: {ex.Message}");
            }

            return payload;
        }

        public static bool IsEmpty()
        {
            return !File.Exists(filePath) || new FileInfo(filePath).Length == 0;
        }

        public static void SavePayloadToFile(string payloadString)
        {
            try
            {
                using (StreamWriter writer = File.AppendText(filePath))
                {
                    writer.WriteLine(payloadString);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error saving message to file: {e.Message}");
            }
        }

        public static void DeletePayloadFromFile(string messageToDelete)
        {
            try
            {
                string[] lines = File.ReadAllLines(filePath);

                // Folosim LINQ pentru a filtra liniile și a exclude mesajul pe care dorim să-l ștergem
                lines = lines.Where(line => !line.Contains(messageToDelete)).ToArray();

                // Rescriem fișierul cu liniile actualizate
                File.WriteAllLines(filePath, lines);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        }

        public static List<Payload> GetMessagesInQueue(string topic)
        {
            List<Payload> messagesInQueue = new List<Payload>();

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    try
                    {
                        Payload payload = JsonConvert.DeserializeObject<Payload>(line);

                        if (payload.Topic == topic)
                        {
                            messagesInQueue.Add(payload);
                        }
                    }
                    catch (JsonException ex)
                    {
                        Console.WriteLine($"Error deserializing message: {ex.Message}");
                    }
                }
            }

            return messagesInQueue;
        }
    }
}

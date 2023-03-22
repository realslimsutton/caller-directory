using CallerDirectory.Models;
using CallerDirectory.Worker;
using System.Globalization;

namespace CallerDirectory.Services
{
    public class DataUploadService : IDataUploadService
    {
        // Cost is in decipence (3dp)
        private const int COST_MULTIPLIER = 1000;

        private readonly IDataUploadQueue _queue;

        public DataUploadService(IDataUploadQueue queue)
        {
            this._queue = queue;
        }

        public async Task Import(Stream stream)
        {
            foreach (string[] fields in GetFields(stream))
            {
                this.ProcessFields(fields);
            }
        }

        private void ProcessFields(string[] fields)
        {
            DateTime endDateTime = DateTime.Parse($"{fields[2]} {fields[3]}");

            this._queue.Enqueue(new CallRecord
            {
                Caller = string.IsNullOrEmpty(fields[0]) ? null : long.Parse(fields[0]),
                Recipient = long.Parse(fields[1]),
                StartDateTime = endDateTime.AddSeconds(-1 * int.Parse(fields[4])),
                EndDateTime = endDateTime,
                Cost = float.Parse(fields[5]) * COST_MULTIPLIER,
                Reference = fields[6],
                Currency = fields[7]
            });
        }

        private IEnumerable<string[]> GetFields(Stream stream)
        {
            bool firstLine = true;

            using StreamReader reader = new (stream);

            while (!reader.EndOfStream)
            {
                string? line = reader.ReadLine();

                if (firstLine || string.IsNullOrWhiteSpace(line))
                {
                    firstLine = false;

                    continue;
                }

                yield return line.Split(',', StringSplitOptions.TrimEntries);
            }
        }
    }
}

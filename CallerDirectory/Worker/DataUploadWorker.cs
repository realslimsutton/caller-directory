using CallerDirectory.DataAccess;
using CallerDirectory.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace CallerDirectory.Worker
{
    public class DataUploadWorker : BackgroundService
    {
        private readonly IConfiguration _configuration;

        private readonly IDataUploadQueue _queue;

        private readonly ILogger<DataUploadWorker> _logger;

        private readonly List<CallRecord> _insertBuffer;

        public DataUploadWorker(IConfiguration configuration, IDataUploadQueue queue, ILogger<DataUploadWorker> logger)
        {
            this._configuration = configuration;
            this._queue = queue;
            this._insertBuffer = new(configuration.GetValue<int>("BulkInsertSize"));
            this._logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await Task.Factory.StartNew(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await this.ProcessQueueAsync(cancellationToken);
                }

                await this.ProcessQueueAsync(cancellationToken);
                await this.UpdateDatabaseAsync(cancellationToken);
            }, TaskCreationOptions.LongRunning);
        }

        private async Task ProcessQueueAsync(CancellationToken cancellationToken)
        {
            while (!this._queue.IsEmpty)
            {
                if (!this._queue.TryDequeue(out CallRecord? record) || record == null)
                {
                    continue;
                }

                this._insertBuffer.Add(record);

                if (this._insertBuffer.Count == this._insertBuffer.Capacity)
                {
                    await this.UpdateDatabaseAsync(cancellationToken);
                }
            }
        }

        private async Task UpdateDatabaseAsync(CancellationToken cancellationToken)
        {
            try
            {
                using (CallingContext context = new(this._configuration))
                {
                    await context.CallRecords.UpsertRange(this._insertBuffer).On(c => c.Reference).RunAsync(cancellationToken);
                }

                this._insertBuffer.Clear();
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Failed to import data uploads");
            }
        }
    }
}

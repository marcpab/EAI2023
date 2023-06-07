using EAI.Logging.Model;
using EAI.Logging.Writer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EAI.Logging.Test
{
    public class LoggerTest
    {
        [Fact]
        public async Task TestBasic01()
        {
            var service = "LogProviderTest";
            var transaction = "TestBasic01";
            var childTransaction = "Step1";
            var key = Guid.NewGuid().ToString()[..8];
            
            LogItemListWriter.Instance.Clear();
            StringBuilderWriter.Instance.Clear();

            var writers = new List<ILogWriter>
            {
                LogItemListWriter.Instance
            };

            var writerCollection = new DefaultLogWriterCollection(writers);
            

            var log = new Logger(writerCollection, LogStage.DEV, service, transaction, childTransaction, key);

            Assert.Equal(service, log.Service);
            Assert.Equal(transaction, log.Transaction);
            Assert.Equal(childTransaction, log.ChildTransaction);
            Assert.Equal(key, log.TransactionKey);

            var message = $"Test Record for {key}";
            await log.String<LevelDebug>(message);


            var items = LogItemListWriter.LogItems.ToList();
            Assert.Single(items);
            Assert.Equal(message, items[0].Description);
            Assert.Equal(service, items[0].Service);
            Assert.Equal(transaction, items[0].Transaction);
            Assert.Equal(transaction.GetHashCode(), items[0].TransactionHash);
            Assert.Equal(childTransaction, items[0].ChildTransaction);
            Assert.Equal(key, items[0].TransactionKey);
            Assert.Equal(new LevelDebug().Level, items[0].Level);
            Assert.Equal(LogStage.DEV.ToString(), items[0].Stage);            
        }

        [Fact]
        public async Task TestBasic02()
        {
            var service = "LogProviderTest";
            var transaction = "TestBasic02";
            var childTransaction = "Step2";
            var key = Guid.NewGuid().ToString()[..8];

            LogItemListWriter.Instance.Clear();
            StringBuilderWriter.Instance.Clear();

            var writers = new List<ILogWriter>
            {
                LogItemListWriter.Instance,
                StringBuilderWriter.Instance
            };

            var writerCollection = new DefaultLogWriterCollection(writers);


            var log = new Logger(writerCollection, LogStage.PROD, service, transaction, childTransaction, key);


            var message = $"Test Record for {key}";
            await log.String<LevelDebug>(message);


            var items = LogItemListWriter.LogItems.ToList();
            Assert.Single(items);
            Assert.Equal(message, items[0].Description);
            Assert.Equal(service, items[0].Service);
            Assert.Equal(transaction, items[0].Transaction);
            Assert.Equal(transaction.GetHashCode(), items[0].TransactionHash);
            Assert.Equal(childTransaction, items[0].ChildTransaction);
            Assert.Equal(key, items[0].TransactionKey);
            Assert.Equal(new LevelDebug().Level, items[0].Level);
            Assert.Equal(LogStage.PROD.ToString(), items[0].Stage);

            var sb = StringBuilderWriter.Logs.ToString().ReplaceLineEndings(string.Empty);
            var item = items[0].ToString();
            Assert.Equal(item, sb);
        }

        [Fact]
        public async Task TestBasic03()
        {
            var service = "LogProviderTest";
            var transaction = "TestBasic03";
            var childTransaction = "Step3";
            var key = Guid.NewGuid().ToString()[..8];
            
            LogItemListWriter.Instance.Clear();
            StringBuilderWriter.Instance.Clear();

            var writers = new List<ILogWriter>
            {
                LogItemListWriter.Instance,
                StringBuilderWriter.Instance
            };

            var writerCollection = new DefaultLogWriterCollection(writers);


            var log = new Logger(writerCollection, LogStage.UAT, service, transaction, childTransaction, key);


            var message = $"Test Record for {key}";
            await log.String<LevelCritical, LogItemListWriterId>(message);

            Assert.Single(LogItemListWriter.LogItems.ToList());

            var item = LogItemListWriter.LogItems.ToList()[0];
            Assert.Equal(message, item.Description);
            Assert.Equal(service, item.Service);
            Assert.Equal(transaction, item.Transaction);
            Assert.Equal(transaction.GetHashCode(), item.TransactionHash);
            Assert.Equal(childTransaction, item.ChildTransaction);
            Assert.Equal(key, item.TransactionKey);
            Assert.Equal(new LevelCritical().Level, item.Level);
            Assert.Equal(LogStage.UAT.ToString(), item.Stage);

            var sb = StringBuilderWriter.Logs.ToString().ReplaceLineEndings(string.Empty);
            
            Assert.Equal(string.Empty, sb);
        }

        [Fact]
        public async Task TestBasic04()
        {
            var service = "LogProviderTest";
            var transaction = "TestBasic04";
            var childTransaction = "Step4";
            var key = Guid.NewGuid().ToString()[..8];

            // requires Microsoft.Extensions.Logging.Debug 
            var serviceProvider = new ServiceCollection()
                .AddLogging(builder => builder.AddDebug())
                .BuildServiceProvider();

            var factory = serviceProvider.GetService<ILoggerFactory>();
            Assert.NotNull(factory);
            var logger = factory.CreateLogger<LoggerTest>();

            var log = new Logger(logger, LogStage.UAT, service, transaction, childTransaction, key);

            Assert.Equal(service, log.Service);
            Assert.Equal(transaction, log.Transaction);
            Assert.Equal(childTransaction, log.ChildTransaction);
            Assert.Equal(key, log.TransactionKey);

            var message = $"Test Record for {key}";
            await log.String<LevelDebug>(message);
        }

        [Fact]
        public async Task TestBasic05()
        {
            var service = "LogProviderTest";
            var transaction = "TestBasic05";
            var childTransaction = "Step5";
            var key = Guid.NewGuid().ToString()[..8];

            // requires Microsoft.Extensions.Logging.Debug 
            var serviceProvider = new ServiceCollection()
                .AddLogging(builder => builder.AddDebug())
                .BuildServiceProvider();

            var factory = serviceProvider.GetService<ILoggerFactory>();
            Assert.NotNull(factory);
            var logger = factory.CreateLogger<LoggerTest>();

            var log = new Logger(logger, LogStage.UAT, service, transaction, childTransaction, key);

            Assert.Equal(service, log.Service);
            Assert.Equal(transaction, log.Transaction);
            Assert.Equal(childTransaction, log.ChildTransaction);
            Assert.Equal(key, log.TransactionKey);

            var message = $"Test Record for {key}";
            var overrideStage = LogStage.SIT.ToString();
            var overrideStageId = (int)LogStage.SIT;
            await log.String<LevelDebug, DefaultWriterId>(overrideStage, overrideStageId, message);

            var message2 = $"Test Record 2 for {key}";
            await log.String<LevelDebug>(LogStage.UAT, message);
        }

        [Fact]
        public async Task TestBasic06()
        {
            var service = "LogProviderTest";
            var transaction = "TestBasic06";
            var childTransaction = "Step6";
            var key = Guid.NewGuid().ToString()[..8];

            LogItemListWriter.Instance.Clear();
            StringBuilderWriter.Instance.Clear();

            var writers = new List<ILogWriter>
            {
                LogItemListWriter.Instance
            };

            var writerCollection = new DefaultLogWriterCollection(writers);


            var log = new Logger(writerCollection, LogStage.DEV, service, transaction, childTransaction, key);

            Assert.Equal(service, log.Service);
            Assert.Equal(transaction, log.Transaction);
            Assert.Equal(childTransaction, log.ChildTransaction);
            Assert.Equal(key, log.TransactionKey);
            Assert.Equal(LogStage.DEV.ToString(), log.Stage);

            var message = $"Test Record for {key}";
            var overrideStage = LogStage.UAT.ToString();
            var overrideStageId = (int)LogStage.UAT;

            await log.String<LevelDebug>(overrideStage, overrideStageId, message);

            var items = LogItemListWriter.LogItems.ToList();
            Assert.Single(items);
            Assert.Equal(message, items[0].Description);
            Assert.Equal(service, items[0].Service);
            Assert.Equal(transaction, items[0].Transaction);
            Assert.Equal(transaction.GetHashCode(), items[0].TransactionHash);
            Assert.Equal(childTransaction, items[0].ChildTransaction);
            Assert.Equal(key, items[0].TransactionKey);
            Assert.Equal(new LevelDebug().Level, items[0].Level);
            Assert.Equal(overrideStage, items[0].Stage);
        }

        [Fact]
        public async Task TestBasic07()
        {
            var service = "LogProviderTest";
            var transaction = "TestBasic07";
            var childTransaction = "Step7";
            var key = Guid.NewGuid().ToString()[..8];

            LogItemListWriter.Instance.Clear();
            StringBuilderWriter.Instance.Clear();

            var writers = new List<ILogWriter>
            {
                LogItemListWriter.Instance
            };

            var writerCollection = new DefaultLogWriterCollection(writers);


            var log = new Logger(writerCollection, LogStage.DEV, service, transaction, childTransaction, key);

            Assert.Equal(service, log.Service);
            Assert.Equal(transaction, log.Transaction);
            Assert.Equal(childTransaction, log.ChildTransaction);
            Assert.Equal(key, log.TransactionKey);
            Assert.Equal(LogStage.DEV.ToString(), log.Stage);

            var message = $"Test Record for {key}";

            await log.String<LevelDebug>(LogStage.SIT, message);

            var items = LogItemListWriter.LogItems.ToList();
            Assert.Single(items);
            Assert.Equal(LogStage.SIT.ToString(), items[0].Stage);
        }
    }
}
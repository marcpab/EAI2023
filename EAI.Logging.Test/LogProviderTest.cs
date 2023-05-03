using EAI.Logging.Model;
using EAI.Logging.Writer;

namespace EAI.Logging.Test
{
    public class LogProviderTest
    {
        [Fact]
        public async Task TestBasic01()
        {
            var service = "LogProviderTest";
            var transaction = "TestBasic01";
            var childTransaction = "Step1";
            var key = Guid.NewGuid().ToString().Substring(0, 8);
            
            LogItemListWriter.Instance.Clear();
            StringBuilderWriter.Instance.Clear();

            var writers = new List<ILogWriter>();
            writers.Add(LogItemListWriter.Instance);

            var writerCollection = new DefaultLogWriterCollection(writers);
            

            var log = new LogProvider<StageDEV>(writerCollection, service, transaction, childTransaction, key);

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
            Assert.Equal(new StageDEV().Stage, items[0].Stage);            
        }

        [Fact]
        public async Task TestBasic02()
        {
            var service = "LogProviderTest";
            var transaction = "TestBasic02";
            var childTransaction = "Step2";
            var key = Guid.NewGuid().ToString().Substring(0, 8);

            LogItemListWriter.Instance.Clear();
            StringBuilderWriter.Instance.Clear();

            var writers = new List<ILogWriter>();
            writers.Add(LogItemListWriter.Instance);
            writers.Add(StringBuilderWriter.Instance);

            var writerCollection = new DefaultLogWriterCollection(writers);


            var log = new LogProvider<StagePROD>(writerCollection, service, transaction, childTransaction, key);


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
            Assert.Equal(new StagePROD().Stage, items[0].Stage);

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
            var key = Guid.NewGuid().ToString().Substring(0, 8);
            
            LogItemListWriter.Instance.Clear();
            StringBuilderWriter.Instance.Clear();

            var writers = new List<ILogWriter>();
            writers.Add(LogItemListWriter.Instance);
            writers.Add(StringBuilderWriter.Instance);

            var writerCollection = new DefaultLogWriterCollection(writers);


            var log = new LogProvider<StageUAT>(writerCollection, service, transaction, childTransaction, key);


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
            Assert.Equal(new StageUAT().Stage, item.Stage);

            var sb = StringBuilderWriter.Logs.ToString().ReplaceLineEndings(string.Empty);
            
            Assert.Equal(string.Empty, sb);
        }
    }
}
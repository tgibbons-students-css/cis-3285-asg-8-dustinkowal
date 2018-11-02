using Microsoft.VisualStudio.TestTools.UnitTesting;
using SingleResponsibilityPrinciple;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SingleResponsibilityPrinciple.Tests
{
    [TestClass()]
    public class TradeProcessorTests
    {
        private int CountDbRecords()
        {
            using (var connection = new System.Data.SqlClient.SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\dkowal\source\repos\cis-3285-asg-8-dustinkowal\tradesdatabase.mdf;Integrated Security=True;Connect Timeout=30"))
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                string myScalarQuery = "SELECT COUNT(*) FROM trade";
                SqlCommand myCommand = new SqlCommand(myScalarQuery, connection);
                //myCommand.Connection.Open();
                int count = (int)myCommand.ExecuteScalar();
                connection.Close();
                return count;
            }
        }
        [TestMethod()]
        public void TestNormalFile()
            //modify so it calls count before and after process trades
        {
            //Arrange
            var tradeStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SingleResponsibilityPrincipleTests.goodtrades.txt");
            var tradeProcessor = new TradeProcessor();

            //Act
            int countB = CountDbRecords();
            tradeProcessor.ProcessTrades(tradeStream);

            //Assert
            int countA = CountDbRecords();
            Assert.AreEqual(countB + 4, countA);
        }

        [TestMethod()]
        public void TestBadFile()
        //modify so it calls count before and after process trades
        {
            //Arrange
            var tradeStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SingleResponsibilityPrincipleTests.badtrades.txt");
            var tradeProcessor = new TradeProcessor();

            //Act
            int countB = CountDbRecords();
            tradeProcessor.ProcessTrades(tradeStream);

            //Assert
            int countA = CountDbRecords();
            Assert.AreEqual(countB, countA);
        }

        [TestMethod()]
        public void TestNegative()
        //modify so it calls count before and after process trades
        {
            //Arrange
            var tradeStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SingleResponsibilityPrincipleTests.negativetrades.txt");
            var tradeProcessor = new TradeProcessor();

            //Act
            int countB = CountDbRecords();
            tradeProcessor.ProcessTrades(tradeStream);

            //Assert
            int countA = CountDbRecords();
            Assert.AreEqual(countB, countA);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NoTestFile()
        {
            //Arrange
            var tradeStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("");
            var tradeProcessor = new TradeProcessor();

            //Act
            tradeProcessor.ProcessTrades(tradeStream);

            //Assert

        }
        [TestMethod]
        public void TestMultipleTradesInFile()
        {
            // Arrange
            var tradeStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("SingleResponsibilityPrincipleTests.trades4.txt");
            var tradeProcessor = new TradeProcessor();
            int startCount = CountDbRecords();
            // Act
            tradeProcessor.ProcessTrades(tradeStream);
            int endCount = CountDbRecords();
            // Assert
            Assert.AreEqual(endCount - startCount, 7);
        }
    }
}
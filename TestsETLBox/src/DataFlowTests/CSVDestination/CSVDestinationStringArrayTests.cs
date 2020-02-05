using ALE.ETLBox;
using ALE.ETLBox.ConnectionManager;
using ALE.ETLBox.ControlFlow;
using ALE.ETLBox.DataFlow;
using ALE.ETLBox.Helper;
using ALE.ETLBox.Logging;
using ALE.ETLBoxTests.Fixtures;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace ALE.ETLBoxTests.DataFlowTests
{
    [Collection("DataFlow")]
    public class CSVDestinationStringArrayTests : IDisposable
    {
        public SqlConnectionManager SqlConnection => Config.SqlConnection.ConnectionManager("DataFlow");
        public CSVDestinationStringArrayTests(DataFlowDatabaseFixture dbFixture)
        {
        }

        public void Dispose()
        {
        }


        [Fact]
        public void SimpleNonGeneric()
        {
            //Arrange
            TwoColumnsTableFixture s2C = new TwoColumnsTableFixture("CSVDestSimpleNonGeneric");
            s2C.InsertTestDataSet3();
            DBSource<string[]> source = new DBSource<string[]>(SqlConnection, "CSVDestSimpleNonGeneric");

            //Act
            CsvDestination<string[]> dest = new CsvDestination<string[]>("./SimpleNonGeneric.csv");
            source.LinkTo(dest);
            source.Execute();
            dest.Wait();

            //Assert
            //Assert
            Assert.Equal(File.ReadAllText("./SimpleNonGeneric.csv"),
                File.ReadAllText("res/CSVDestination/TwoColumnsSet3NoHeader.csv"));
        }


    }
}
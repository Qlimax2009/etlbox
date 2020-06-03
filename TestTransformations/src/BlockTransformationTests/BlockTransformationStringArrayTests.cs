using ETLBox.Connection;
using ETLBox.ControlFlow;
using ETLBox.ControlFlow.Tasks;
using ETLBox.DataFlow; using ETLBox.DataFlow.Connectors; using ETLBox.DataFlow.Transformations;
using ETLBoxTests.Fixtures;
using ETLBoxTests.Helper;
using Xunit;

namespace ETLBoxTests.DataFlowTests
{
    [Collection("DataFlow")]
    public class BlockTransformationStringArrayTests
    {
        public SqlConnectionManager SqlConnection => Config.SqlConnection.ConnectionManager("DataFlow");
        public BlockTransformationStringArrayTests(DataFlowDatabaseFixture dbFixture)
        {
        }

        [Fact]
        public void ModifyInputDataList()
        {
            //Arrange
            TwoColumnsTableFixture source2Columns = new TwoColumnsTableFixture("BlockTransSourceNonGeneric");
            source2Columns.InsertTestData();
            TwoColumnsTableFixture dest2Columns = new TwoColumnsTableFixture("BlockTransDestNonGeneric");

            DbSource<string[]> source = new DbSource<string[]>(SqlConnection, "BlockTransSourceNonGeneric");
            DbDestination<string[]> dest = new DbDestination<string[]>(SqlConnection, "BlockTransDestNonGeneric");

            //Act
            BlockTransformation<string[]> block = new BlockTransformation<string[]>(
                inputData =>
                {
                    inputData.RemoveRange(1, 2);
                    inputData.Add(new string[] { "4", "Test4" });
                    return inputData;
                });
            source.LinkTo(block);
            block.LinkTo(dest);
            source.Execute();
            dest.Wait();

            //Assert
            Assert.Equal(2, RowCountTask.Count(SqlConnection, "BlockTransDestNonGeneric"));
            Assert.Equal(1, RowCountTask.Count(SqlConnection, "BlockTransDestNonGeneric", "Col1 = 1 AND Col2='Test1'"));
            Assert.Equal(1, RowCountTask.Count(SqlConnection, "BlockTransDestNonGeneric", "Col1 = 4 AND Col2='Test4'"));
        }
    }
}

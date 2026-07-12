using DataBase.Data.Get;

namespace DataBaseUnitTest.DataGet
{
    public class GetInventoryAoTProductNameTest
    {
        [Fact]
        public async Task GetProductName_ShouldGet()
        {
            var db = await Helper.CreateDataBaseForTest(nameof(GetProductName_ShouldGet));
            var expected = await Helper.SetExampleDays(1, db);

            var aot = new GetInventoryAoT(db);

            var result = await aot.GetProductName(expected[0].Products[0].Name.Id);

            Assert.NotNull(result);
            Assert.Equal(expected[0].Products[0].Name, result);
        }


    }
}

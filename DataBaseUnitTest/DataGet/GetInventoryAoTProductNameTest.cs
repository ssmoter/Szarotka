using DataBase.Data.Get;

namespace DataBaseUnitTest.DataGet
{
    public class GetInventoryAoTProductNameTest
    {
        [Fact]
        public async Task GetProductName_ShouldGet()
        {
            var db = await Helper.CreateDataBaseForTest(nameof(GetProductName_ShouldGet));
            var excepted = await Helper.SetExampleDays(1, db);

            var aot = new GetInventoryAoT(db);

            var result = await aot.GetProductName(excepted[0].Products[0].Name.Id);

            Assert.NotNull(result);
            Assert.Equal(excepted[0].Products[0].Name, result);
        }


    }
}

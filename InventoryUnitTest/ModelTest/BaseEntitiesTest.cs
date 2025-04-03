using DataBase.Model;

using FluentAssertions;

namespace InventoryUnitTest.ModelTest
{
    public class BaseEntitiesTest
    {
        static readonly DateTime dateTime = DateTime.UtcNow;
        public static IEnumerable<object[]> TestDateTime =>
         [
              [dateTime.ToLocalTime(), dateTime.Ticks],
              [dateTime.ToLocalTime(), dateTime.Ticks],
          ];

        [Theory]
        [MemberData(nameof(TestDateTime))]
        public void TestUpdate(DateTime request, long result)
        {
            var entities = new BaseEntities<int>
            {
                UpdatedTicks = result
            };
            entities.Updated.Should().Be(request);
        }
        [Theory]
        [MemberData(nameof(TestDateTime))]
        public void TestCreated(DateTime request, long result)
        {
            var entities = new BaseEntities<int>
            {
                CreatedTicks = result
            };
            entities.Created.Should().Be(request);
        }
    }
}

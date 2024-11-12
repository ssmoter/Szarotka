using DriversRoutes.Data.GoogleApi;

using FluentAssertions;

using Xunit;

namespace DriversRoutesUnitTest
{
    public class GoogleApiAddressFromCoordinatesTest
    {
        private readonly AddressFromCoordinates _addressFromCoordinates;

        public GoogleApiAddressFromCoordinatesTest()
        {
            _addressFromCoordinates = new AddressFromCoordinates(new HttpClient(), DataBase.Key.GoogleApi.Key);
        }

        [Theory]
        [InlineData(49.736447681, 20.4015523482)]
        public async Task GetDataGoogleApi(double latitude, double longitude)
        {
            var obj = await _addressFromCoordinates.FindGoogleApiAddress(latitude, longitude);

            obj.Should().NotBeNull();
        }
        [Theory]
        [InlineData(49.736447681, 20.4015523482)]
        public async Task GetDataResidentialAddress(double latitude, double longitude)
        {
            var obj = await _addressFromCoordinates.FindResidentialAddress(latitude, longitude);

            obj.Should().NotBeNull();
        }
    }
}

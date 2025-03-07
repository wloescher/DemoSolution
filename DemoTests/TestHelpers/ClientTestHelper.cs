using DemoModels;

namespace DemoTests.TestHelpers
{
    internal static class ClientTestHelper
    {
        public static void Compare(ClientModel expected, ClientModel? actual)
        {
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.ClientId, actual.ClientId);
            Assert.AreEqual(expected.ClientGuid, actual.ClientGuid);
            Assert.AreEqual(expected.Type, actual.Type);
            Assert.AreEqual(expected.IsActive, actual.IsActive);
            Assert.AreEqual(expected.IsDeleted, actual.IsDeleted);
            Assert.AreEqual(expected.Name, actual.Name);
            Assert.AreEqual(expected.Address, actual.Address);
            Assert.AreEqual(expected.City, actual.City);
            Assert.AreEqual(expected.Region, actual.Region);
            Assert.AreEqual(expected.PostalCode, actual.PostalCode);
            Assert.AreEqual(expected.Country, actual.Country);
            Assert.AreEqual(expected.Url, actual.Url);
        }
    }
}

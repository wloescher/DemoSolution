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
            Assert.AreEqual(expected.AddressLine1, actual.AddressLine1);
            Assert.AreEqual(expected.AddressLine2, actual.AddressLine2);
            Assert.AreEqual(expected.City, actual.City);
            Assert.AreEqual(expected.Region, actual.Region);
            Assert.AreEqual(expected.PostalCode, actual.PostalCode);
            Assert.AreEqual(expected.Country, actual.Country);
            Assert.AreEqual(expected.PhoneNumber, actual.PhoneNumber);
            Assert.AreEqual(expected.Url, actual.Url);
        }

        public static void Compare(List<ClientModel> expected, List<ClientModel> actual)
        {
            Assert.AreEqual(expected.Count, actual.Count);
            for (int i = 0; i < expected.Count; i++)
            {
                Compare(expected[i], actual[i]);
            }
        }
    }
}

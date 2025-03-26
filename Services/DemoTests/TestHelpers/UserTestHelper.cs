using DemoModels;

namespace DemoTests.TestHelpers
{
    internal static class UserTestHelper
    {
        public static void Compare(UserModel expected, UserModel? actual)
        {
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.UserId, actual.UserId);
            Assert.AreEqual(expected.UserGuid, actual.UserGuid);
            Assert.AreEqual(expected.Type, actual.Type);
            Assert.AreEqual(expected.EmailAddress, actual.EmailAddress);
            Assert.AreEqual(expected.IsActive, actual.IsActive);
            Assert.AreEqual(expected.IsDeleted, actual.IsDeleted);
            Assert.AreEqual(expected.FirstName, actual.FirstName);
            Assert.AreEqual(expected.MiddleName, actual.MiddleName);
            Assert.AreEqual(expected.LastName, actual.LastName);
            Assert.AreEqual(expected.AddressLine1, actual.AddressLine1);
            Assert.AreEqual(expected.AddressLine2, actual.AddressLine2);
            Assert.AreEqual(expected.City, actual.City);
            Assert.AreEqual(expected.Region, actual.Region);
            Assert.AreEqual(expected.PostalCode, actual.PostalCode);
            Assert.AreEqual(expected.Country, actual.Country);
        }

        public static void Compare(List<UserModel> expected, List<UserModel> actual)
        {
            Assert.AreEqual(expected.Count, actual.Count);
            for (int i = 0; i < expected.Count; i++)
            {
                Compare(expected[i], actual[i]);
            }
        }
    }
}

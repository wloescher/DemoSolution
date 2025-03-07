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
            Assert.AreEqual(expected.Address, actual.Address);
            Assert.AreEqual(expected.City, actual.City);
            Assert.AreEqual(expected.Region, actual.Region);
            Assert.AreEqual(expected.PostalCode, actual.PostalCode);
            Assert.AreEqual(expected.Country, actual.Country);
        }
    }
}

using DemoModels;

namespace DemoTests.TestHelpers
{
    internal static class WorkItemTestHelper
    {
        public static void Compare(WorkItemModel expected, WorkItemModel? actual)
        {
            Assert.IsNotNull(actual);
            Assert.AreEqual(expected.WorkItemId, actual.WorkItemId);
            Assert.AreEqual(expected.WorkItemGuid, actual.WorkItemGuid);
            Assert.AreEqual(expected.ClientId, actual.ClientId);
            Assert.AreEqual(expected.Type, actual.Type);
            Assert.AreEqual(expected.IsActive, actual.IsActive);
            Assert.AreEqual(expected.IsDeleted, actual.IsDeleted);
            Assert.AreEqual(expected.Title, actual.Title);
            Assert.AreEqual(expected.SubTitle, actual.SubTitle);
            Assert.AreEqual(expected.Summary, actual.Summary);
            Assert.AreEqual(expected.Body, actual.Body);
        }

        public static void Compare(List<WorkItemModel> expected, List<WorkItemModel> actual)
        {
            Assert.AreEqual(expected.Count, actual.Count);
            for (int i = 0; i < expected.Count; i++)
            {
                Compare(expected[i], actual[i]);
            }
        }
    }
}

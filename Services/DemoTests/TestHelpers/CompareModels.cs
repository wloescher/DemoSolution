using DemoModels;

namespace DemoTests.TestHelpers
{
    public static partial class CompareModels
    {
        public static void Compare(List<GenericListItemModel> expected, List<GenericListItemModel> actual)
        {
            Assert.AreEqual(expected.Count, actual.Count);
            for (int i = 0; i < expected.Count; i++)
            {
                Compare(expected[i], actual[i]);
            }
        }

        public static void Compare(GenericListItemModel expected, GenericListItemModel actual)
        {
            Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Name, actual.Name);
        }
    }
}

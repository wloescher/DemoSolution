using DemoTests.BaseClasses;
using Microsoft.Extensions.DependencyInjection;

namespace DemoTests
{
    // https://medium.com/@lopezm.didac/how-to-use-dependency-injection-with-mstest-in-3-steps-8d705ea96411

    internal class TestMethodDependencyInjection : TestMethodAttribute
    {
        public override TestResult[] Execute(ITestMethod testMethod)
        {
            var nParameters = testMethod.ParameterTypes?.Length ?? 0;
            if (nParameters != 0)
            {
                object?[] injectedArgs = new object[nParameters];
                var serviceProvider = ServiceTestBase._serviceProvider;
                using (var scope = serviceProvider!.CreateScope())
                {
                    for (var i = 0; i < nParameters; i++)
                    {
                        injectedArgs[i] = scope.ServiceProvider.GetService(testMethod.ParameterTypes![i].ParameterType)!;
                    }
                }
                return [testMethod.Invoke(injectedArgs!)];
            }
            else
            {
                return base.Execute(testMethod);
            }
        }
    }
}

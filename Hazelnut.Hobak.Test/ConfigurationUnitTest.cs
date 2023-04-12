namespace Hazelnut.Hobak.Test;

[TestClass]
public class ConfigurationUnitTest
{
    [TestMethod]
    public void ParseTest1()
    {
        var configuration = Configuration.FromString(@"; Comment
TestKey1 = TestValue1
TestKey2 = TestValue2");
        
        Assert.IsTrue(configuration.Count == 1);
        Assert.IsTrue(configuration[Configuration.GlobalSectionName].Count == 2);
        Assert.IsTrue(configuration[Configuration.GlobalSectionName]["TestKey1"] == "TestValue1");
        Assert.IsTrue(configuration[Configuration.GlobalSectionName]["TestKey2"] == "TestValue2");
    }
    
    [TestMethod]
    public void ParseTest2()
    {
        var configuration = Configuration.FromString(@"[Section1]
TestKey1 = TestValue1
TestKey2 = TestValue2");
        
        Assert.IsTrue(configuration.Count == 2);
        Assert.IsTrue(configuration[Configuration.GlobalSectionName].Count == 0);
        Assert.IsTrue(configuration["Section1"].Count == 2);
        Assert.IsTrue(configuration["Section1"]["TestKey1"] == "TestValue1");
        Assert.IsTrue(configuration["Section1"]["TestKey2"] == "TestValue2");
    }
    
    [TestMethod]
    public void ParseTest3()
    {
        var configuration = Configuration.FromString(@"
TestKey1 = TestValue1 ; Comment
TestKey2 = TestValue2");
        
        Assert.IsTrue(configuration.Count == 1);
        Assert.IsTrue(configuration[Configuration.GlobalSectionName].Count == 2);
        Assert.IsTrue(configuration[Configuration.GlobalSectionName]["TestKey1"] == "TestValue1");
        Assert.IsTrue(configuration[Configuration.GlobalSectionName]["TestKey2"] == "TestValue2");
    }
}
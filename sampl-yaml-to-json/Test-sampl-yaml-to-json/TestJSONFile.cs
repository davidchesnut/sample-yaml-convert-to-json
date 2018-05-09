using System;
using System.Text;
using System.Collections.Generic;
using sampl_yaml_to_json;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test_sampl_yaml_to_json
{
    /// <summary>
    /// Summary description for TestJSONFile
    /// </summary>
    [TestClass]
    public class TestJSONFile
    {
        public TestJSONFile()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestMethod1()
        {
            var jsonFile = new JSONFile();
            jsonFile.ImportJSONFile(@"C:\projects\sample-yaml-convert-to-json\sampl-yaml-to-json\Test-sampl-yaml-to-json\test-savetofile-input.json");
            jsonFile.SaveToFile(@"C:\projects\sample-yaml-convert-to-json\sampl-yaml-to-json\Test-sampl-yaml-to-json\test-savetofile-result.json");
            return;
        }
    }
}

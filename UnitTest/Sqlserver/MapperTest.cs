using Accounting.Sqlserver;
using System;
using System.Xml;
using Xunit;
using System.IO;

namespace UnitTest.Sqlserver
{
    public class MapperTest
    {
        public MapperTest()
        {

        }
        [Fact]
        public void Create_buyer_item_success()
        {
            //Arrange    
            XmlDocument dom = new XmlDocument();
            var xml = File.ReadAllText("d:/xml3.txt");
            dom.LoadXml(xml);
            var json = dom.MapToJsonString();

            //Act 


            //Assert
            Assert.NotNull(null);
        }
    }
}

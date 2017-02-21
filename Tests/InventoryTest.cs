using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Inventory
{
  public class InventoryTest : IDisposable
  {
    public InventoryTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=inventory_test;Integrated Security=SSPI;";
    }

    public void Dispose()
    {
      InventoryItem.DeleteAll();
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      //Arrange, Act
      int result = InventoryItem.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }
    [Fact]
    public void Test_Equal_ReturnsTrueIfDescriptionsAreTheSame()
    {
      //Arrange, Act
      InventoryItem firstInventoryItem = new InventoryItem("Stickers");
      InventoryItem secondInventoryItem = new InventoryItem("Stickers");

      //Assert
      Assert.Equal(firstInventoryItem, secondInventoryItem);
    }
    [Fact]
    public void Test_Save_SavesToDatabase()
    {
      //Arrange
      InventoryItem testInventoryItem = new InventoryItem("Stickers");

      //Act
      testInventoryItem.Save();
      List<InventoryItem> result = InventoryItem.GetAll();
      List<InventoryItem> testList = new List<InventoryItem>{testInventoryItem};

      //Assert
      Assert.Equal(testList, result);
    }
    [Fact]
    public void Test_Save_AssignsIdToObject()
    {
      //Arrange
      InventoryItem testInventoryItem = new InventoryItem("Stickers");

      //Act
      testInventoryItem.Save();
      InventoryItem savedInventoryItem = InventoryItem.GetAll()[0];

      int result = savedInventoryItem.GetId();
      int testId = testInventoryItem.GetId();

      //Assert
      Assert.Equal(testId, result);
    }
    [Fact]
    public void Test_Find_FindsInventoryItemInDatabase()
    {
      //Arrange
      InventoryItem item1 = new InventoryItem("Item1");
      item1.Save();
      InventoryItem item2 = new InventoryItem("Item2");
      item2.Save();

      //Act
      List<InventoryItem> foundInventoryItems = InventoryItem.Find("Item1");

      //Assert
      Assert.Equal(item1, foundInventoryItems[0]);
    }
  }
}

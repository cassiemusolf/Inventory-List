using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Inventory
{
  public class InventoryItem
  {
    private int _id;
    private string _description;

    public InventoryItem(string Description, int Id = 0)
    {
      _id = Id;
      _description = Description;
    }

    public int GetId()
    {
      return _id;
    }
    public string GetDescription()
    {
      return _description;
    }
    public void SetDescription(string newDescription)
    {
      _description = newDescription;
    }

    public override bool Equals(System.Object otherInventoryItem)
    {
      if (!(otherInventoryItem is InventoryItem))
      {
        return false;
      }
      else
      {
        InventoryItem newInventoryItem = (InventoryItem) otherInventoryItem;
        bool idEquality = (this.GetId() == newInventoryItem.GetId());
        bool descriptionEquality = (this.GetDescription() == newInventoryItem.GetDescription());
        Console.WriteLine(this.GetId());
        Console.WriteLine(newInventoryItem.GetId());
        return (idEquality && descriptionEquality);
      }
    }

    public override int GetHashCode()
    {
         return this.GetDescription().GetHashCode();
    }

    public static List<InventoryItem> GetAll()
    {
      List<InventoryItem> allInventoryItems = new List<InventoryItem>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM inventory;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int itemId = rdr.GetInt32(0);
        string itemDescription = rdr.GetString(1);
        InventoryItem newInventoryItem = new InventoryItem(itemDescription, itemId);
        allInventoryItems.Add(newInventoryItem);
      }

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allInventoryItems;
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO inventory (description) OUTPUT INSERTED.id VALUES (@InventoryItemDescription);", conn);

      SqlParameter descriptionParameter = new SqlParameter();
      descriptionParameter.ParameterName = "@InventoryItemDescription";
      descriptionParameter.Value = this.GetDescription();
      cmd.Parameters.Add(descriptionParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }
    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM inventory;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public static List<InventoryItem> Find(string itemDescriptionParameter)
    {
      List<InventoryItem> allInventoryItems = new List<InventoryItem>{};

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM inventory WHERE description = @InventoryItemDescription;", conn);
      SqlParameter newParameter = new SqlParameter();
      newParameter.ParameterName = "@InventoryItemDescription";
      newParameter.Value = itemDescriptionParameter;
      cmd.Parameters.Add(newParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundInventoryItemId = 0;
      string foundInventoryItemDescription = null;
      while(rdr.Read())
      {
        foundInventoryItemId = rdr.GetInt32(0);
        foundInventoryItemDescription = rdr.GetString(1);
        InventoryItem newInventoryItem = new InventoryItem(foundInventoryItemDescription, foundInventoryItemId);
        allInventoryItems.Add(newInventoryItem);
      }
      // InventoryItem foundInventoryItem = new InventoryItem(foundInventoryItemDescription, foundInventoryItemId);
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allInventoryItems;
    }
  }
}

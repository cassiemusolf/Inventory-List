using Nancy;
using Inventory;
using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace InventoryList
{
  public class HomeModule : NancyModule
  {
    public HomeModule()
    {
      Get["/"] = _ => {
        return View["index.cshtml"];
      };
      Get["/items"] = _ => {
        List<InventoryItem> result = InventoryItem.GetAll();
        return View["items.cshtml", result];
      };
      Post["/items"] = _ => {
        InventoryItem newItem = new InventoryItem(Request.Form["item"]);
        Console.WriteLine(newItem.GetDescription());
        newItem.Save();
        List<InventoryItem> result = InventoryItem.GetAll();
        return View["items.cshtml", result];
      };
      Post["/clear"] = _ => {
        InventoryItem.DeleteAll();
        return View["cleared.cshtml"];
      };
    }
  }
}

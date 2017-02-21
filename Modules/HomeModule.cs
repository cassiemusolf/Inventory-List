using Nancy;
using Inventory;
using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace Inventory
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
        newItem.Save();
        List<InventoryItem> result = InventoryItem.GetAll();
        return View["items.cshtml", result];
      };
      Get["/searcheditems"] = _ => {
        List<InventoryItem> result = InventoryItem.Find(Request.Form["searched-items"]);
        return View["searcheditems.cshtml", result];
      };
      Post["/searcheditem"] = _ => {
        // InventoryItem newItem = new InventoryItem(Request.Form["searched-items"]);
        // Console.WriteLine(newItem.GetDescription());
        // newItem.Save();
        List<InventoryItem> result = InventoryItem.Find(Request.Form["searched-item"]);


        return View["searcheditems.cshtml", result];
      };
      Post["/clear"] = _ => {
        InventoryItem.DeleteAll();
        return View["cleared.cshtml"];
      };
    }
  }
}

<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SimpleCQRS.ReadModel.InventoryItems>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Home Page
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>All items:</h2>
    <ul><% foreach (var record in Model.Items)
        {%><li>
            <%: Html.ActionLink("Name: " + record.Value,"Details",new{Id=record.Key}) %>
        </li>
    <%} %></ul>
    <%: Html.ActionLink("Add","Add") %>
</asp:Content>

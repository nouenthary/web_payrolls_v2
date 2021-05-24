<%@ Page Title="" Language="C#" MasterPageFile="~/View/Layout.Master" AutoEventWireup="true" CodeBehind="Transfer.aspx.cs" Inherits="web_payrolls.View.Transfers.Transfer" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">

    <asp:GridView runat="server">

    </asp:GridView>

  <div class="table-responsive ">
    <table class="table table-bordered bg-white">
      <thead>
      <tr>
        <td>#</td>
        <td>First</td>
        <td>Last</td>
        <td>Handle</td>
        <td></td>
      </tr>
      </thead>
      <tbody>
      <tr>
        <td >1</td>
        <td>Mark</td>
        <td>Otto</td>
        <td>@mdo</td>
      </tr>
      </tbody>
    </table>
  </div>

  <asp:TextBox runat="server" ID="demo"/>

</asp:Content>

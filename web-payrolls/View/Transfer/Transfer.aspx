<%@ Page Title="" Language="C#" MasterPageFile="~/View/Master.Master" AutoEventWireup="true" CodeBehind="Transfer.aspx.cs" Inherits="web_payrolls.View.Transfer.Transfer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
  <div class="box box-primary">
    <div class="box-body">


      <input runat="server" id="test"/>

      <asp:LinkButton runat="server">Demo</asp:LinkButton>

      <input runat="server" id="Text1"/>


      <table class="table table-bordered">
        <thead>
        <tr>
          <th scope="col">#</th>
          <th scope="col">First</th>
          <th scope="col">Last</th>
          <th scope="col">Handle</th>
          <th scope="col">#</th>
          <th scope="col">First</th>
          <th scope="col">Last</th>
          <th scope="col">Handle</th>
        </tr>
        </thead>
        <tbody>
        <tr>
          <th scope="row">1</th>
          <td>Mark</td>
          <td>Otto</td>
          <td>@mdo</td>
          <th scope="row">2</th>
          <td>Jacob</td>
          <td>Thornton</td>
          <td>@fat</td>
        </tr>
        <tr>
          <th scope="row">2</th>
          <td>Jacob</td>
          <td>Thornton</td>
          <td>@fat</td>
          <th scope="row">2</th>
          <td>Jacob</td>
          <td>Thornton</td>
          <td>@fat</td>
        </tr>
        <tr>
          <th scope="row">3</th>
          <td colspan="2">Larry the Bird</td>
          <td>@twitter</td>
          <th scope="row">3</th>
          <td colspan="2">Larry the Bird</td>
          <td>@twitter</td>
        </tr>
        </tbody>
      </table>
    </div>
  </div>


  <div class="col-sm-12">

    <asp:GridView ID="DGView" OnSelectedIndexChanged="DGView_SelectedIndexChanged" runat="server" AutoGenerateColumns="False" PageSize="50" BorderStyle="Solid"
                  DataKeyNames="PK_Dept_Id" CellPadding="4" ForeColor="#333333" GridLines="Horizontal" Width="100%" BorderWidth="1"
                  AllowPaging="true" BorderColor="#718FC8" CssClass="box-display-GridView Grid table-responsive text-nowrap"

                  style="margin-left: -14px;">
      <FooterStyle BackColor="#a4b8dd" Font-Bold="True" ForeColor="White"/>
      <AlternatingRowStyle BackColor="White" ForeColor="#333333" CssClass="alternatHover"/>
      <Columns>
        <asp:TemplateField HeaderText="ID">
          <ItemTemplate>

            <asp:Label ID="lblNo" runat="server" Text=' <%# Container.DataItemIndex + 1 %>'/>
          </ItemTemplate>
          <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
          <ItemStyle Width="10" HorizontalAlign="Center"></ItemStyle>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Company">
          <ItemTemplate>
            <asp:Label ID="lblComp_Name" runat="server" Text='<%# Eval("Time_Update") %>'/>
          </ItemTemplate>
          <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
          <ItemStyle Width="" HorizontalAlign="Center"></ItemStyle>
        </asp:TemplateField>

      <%--  <asp:TemplateField HeaderText="Department">
          <ItemTemplate>
            <asp:Label ID="lblDept_Name" runat="server" Text='<%# Eval("Dept_Name") %>'/>
          </ItemTemplate>
          <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
          <ItemStyle Width="" HorizontalAlign="Center"></ItemStyle>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="User Updated">
          <ItemTemplate>
            <asp:Label ID="lblUserUpdate" runat="server" Text='<%# Eval("UserUpdate") %>'></asp:Label>
          </ItemTemplate>
          <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
          <ItemStyle Width="" HorizontalAlign="Center"></ItemStyle>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Date Updated">
          <ItemTemplate>
            <asp:Label ID="lblDate_Update" runat="server" Text='<%# Eval("Date_Upate") %>'></asp:Label>
          </ItemTemplate>
          <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
          <ItemStyle Width="" HorizontalAlign="Center"></ItemStyle>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Time Updated">
          <ItemTemplate>
            <asp:Label ID="lblTime_Update" runat="server" Text='<%# Eval("Time_Update") %>'></asp:Label>
          </ItemTemplate>
          <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
          <ItemStyle Width="" HorizontalAlign="Center"></ItemStyle>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Description">
          <ItemTemplate>
            <asp:Label ID="lblDescr" runat="server" Text='<%# Eval("Descr") %>'/>
          </ItemTemplate>
          <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
          <ItemStyle Width="" HorizontalAlign="Center"></ItemStyle>
        </asp:TemplateField>--%>

      </Columns>
      <PagerStyle BackColor="#8299c5" ForeColor="White" HorizontalAlign="Center"/>
      <EditRowStyle BackColor="#999999"/>
      <FooterStyle Font-Bold="True" HorizontalAlign="Right" ForeColor="#000"/>
      <HeaderStyle BackColor="#018301" Font-Bold="True" ForeColor="White" Height="30px"/>
      <PagerStyle HorizontalAlign="Center" Width="1" VerticalAlign="Bottom" BackColor="#718FC8"
                  ForeColor="White" CssClass="pager_gridview"/>
      <RowStyle BackColor="gainsboro" ForeColor="#333333"/>
      <SelectedRowStyle BackColor="#A1DCF2" Font-Bold="True" ForeColor="#333333"/>
      <SortedAscendingCellStyle BackColor="#E9E7E2"/>
      <SortedAscendingHeaderStyle BackColor="#506C8C"/>
      <SortedDescendingCellStyle BackColor="#FFFDF8"/>
      <SortedDescendingHeaderStyle BackColor="#6F8DAE"/>
    </asp:GridView>

  </div>
</asp:Content>

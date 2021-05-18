<%@ Page Language="C#" 
    AutoEventWireup="true" 
    CodeBehind="Product.aspx.cs" 
    Inherits="web_payrolls.ViewWebForm.Product.Product"
    MasterPageFile="~/ViewWebForm/MasterPage.Master"
    ViewStateMode="Disabled" EnableViewState="false"
    %>

<asp:Content ID="Content2" ContentPlaceHolderID="MasterContent" runat="server">
    
    <asp:UpdatePanel runat="server">
        
        <ContentTemplate>
              <asp:TextBox runat="server" ID="txt"/>
                <asp:Button runat="server" ID="btn" Text="Login" OnClick="OnClick"/>
                <asp:Label ID="lbl" runat="server">
                    Show
                </asp:Label>
        </ContentTemplate>
        
        <Triggers>
            <asp:PostBackTrigger ControlID="btn"/>
        </Triggers>

    </asp:UpdatePanel>
</asp:Content>



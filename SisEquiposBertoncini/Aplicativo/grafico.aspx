<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="grafico.aspx.cs" Inherits="SisEquiposBertoncini.Aplicativo.grafico" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_style" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cph_menu" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="cph_body" runat="server">
    <asp:Chart ID="Chart1" runat="server">
        <Series>
            <asp:Series Name="Series1" ChartType="Spline">
                <Points>
                    <asp:DataPoint AxisLabel="Enero" YValues="1.3" />
                    <asp:DataPoint AxisLabel="Febrero" YValues="1.3" />
                    <asp:DataPoint AxisLabel="Marzo" YValues="1.4" />
                    <asp:DataPoint AxisLabel="Abril" YValues="2" />
                    <asp:DataPoint AxisLabel="Mayo" YValues="5" />
                    <asp:DataPoint AxisLabel="Junio" YValues="15" />
                    <asp:DataPoint AxisLabel="Julio" YValues="7" />
                    <asp:DataPoint AxisLabel="Agosto" YValues="3" />
                    <asp:DataPoint AxisLabel="Septiembre" YValues="1.3" />
                    <asp:DataPoint AxisLabel="Octubre" YValues="1.0" />
                    <asp:DataPoint AxisLabel="Noviembre" YValues="0.5" />
                    <asp:DataPoint AxisLabel="Diciembre" YValues="0.1" />
                </Points>
            </asp:Series>
        </Series>
        <ChartAreas>
            <asp:ChartArea Name="ChartArea1"></asp:ChartArea>
        </ChartAreas>
    </asp:Chart>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="cph_scripts" runat="server">
</asp:Content>

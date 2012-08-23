<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Send.aspx.cs" Inherits="PokeInServerWatch.Send" %>
<%
//Check UserLogin etc
PokeIn.Comet.CometWorker.Send(this); 
%>

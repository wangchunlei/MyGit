<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Listen.aspx.cs" Inherits="PokeInServerWatch.Listen" %>
<% 
    //Check UserLogin etc
    PokeIn.Comet.CometWorker.Listen(this);
%>

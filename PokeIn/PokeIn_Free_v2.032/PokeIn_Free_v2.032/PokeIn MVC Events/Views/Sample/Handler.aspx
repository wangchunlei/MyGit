<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" EnableSessionState="ReadOnly"  %>

<% 
    //Check UserLogin etc
    PokeIn.Comet.CometWorker.Handle();
%>

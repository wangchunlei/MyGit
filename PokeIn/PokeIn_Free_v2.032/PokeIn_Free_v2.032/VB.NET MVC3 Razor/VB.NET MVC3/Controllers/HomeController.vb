Public Class HomeController
    Inherits System.Web.Mvc.Controller

    Function Index() As ActionResult
        ViewData("Message") = "Welcome to ASP.NET MVC! (PokeIn Sample)"

        Return View()
    End Function

    Function About() As ActionResult
        Return View()
    End Function

    'PokeIn: Handler page is defined here
    Function Handler() As ActionResult
        Return View()
    End Function
End Class

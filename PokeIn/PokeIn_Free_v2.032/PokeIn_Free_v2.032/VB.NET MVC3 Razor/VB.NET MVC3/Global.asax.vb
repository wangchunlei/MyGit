' Note: For instructions on enabling IIS6 or IIS7 classic mode, 
' visit http://go.microsoft.com/?LinkId=9394802

Imports PokeIn
Imports PokeIn.Comet

Public Class MvcApplication
    Inherits System.Web.HttpApplication

    Shared Sub RegisterGlobalFilters(ByVal filters As GlobalFilterCollection)
        filters.Add(New HandleErrorAttribute())
    End Sub

    Shared Sub RegisterRoutes(ByVal routes As RouteCollection) 
        routes.IgnoreRoute("{resource}.axd/{*pathInfo}")

        ' MapRoute takes the following parameters, in order:
        ' (1) Route name
        ' (2) URL with parameters
        ' (3) Parameter defaults
        routes.MapRoute( _
            "Default", _
            "{controller}/{action}/{id}", _
            New With {.controller = "Home", .action = "Index", .id = UrlParameter.Optional} _
        )

    End Sub

    'PokeIn OnClientConnected Event
    Shared Sub OnClientConnected(ByVal details As ConnectionDetails, ByRef list As Dictionary(Of String, Object)) 
        list.Add("Sample", New SampleClass(details.ClientId))
    End Sub

    Sub Application_Start()
        'Ignore the route for PokeIn AdvancedHandler
        RouteTable.Routes.IgnoreRoute("{*allpokein}", New With {.allpokein = ".*\.pokein(/.*)?"})

        AreaRegistration.RegisterAllAreas()

        RegisterGlobalFilters(GlobalFilters.Filters)
        RegisterRoutes(RouteTable.Routes)


        'PokeIn Event Definition
        AddHandler CometWorker.OnClientConnected, New PokeIn.Comet.DefineClassObjects(AddressOf OnClientConnected)
    End Sub
End Class

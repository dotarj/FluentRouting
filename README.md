# ASP.NET MVC Fluent Routing 

ASP.NET MVC Fluent Routing is a thin wrapper around the ASP.NET MVC attribute routing engine. With Fluent Routing you can define your routes using a fluent interface, but with the full power of the attribute routing engine (inline route constraints, optional URI parameters and default values), for example:

```csharp
routes.For<HomeController>()
    .CreateRoute("").WithName("my route name").To(controller => controller.Index())
        .WithConstraints().HttpMethod(HttpMethod.Get);
```

## Where can I get it?

Open the Visual Studio Package Manager Console and run the following command:

``
Install-Package FluentRouting.Mvc
``

## How do I get started?

Check out [this blog post](http://arjenpost.nl/blog/asp-net-mvc-fluentrouting/) for an introduction to ASP.NET MVC Fluent Routing.

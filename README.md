
# Folked from https://github.com/isc30/blazor-analytics
Because I would like to add some more features what would break the orginal ideas of isc30

Blazor extensions for Analytics: Google Analytics, GTAG, ...

Blazor Version: 3.0.0-preview6.19307.2

# NuGet Package
https://www.nuget.org/packages/Blazor-Google-Analytics

# NuGet Package (Original package - Ivan Sanz Carasa (isc30))
https://nuget.org/packages/Blazor-Analytics

### Server Side Rendering
The Google Analytics and Google Tag Manager will be added at Server Side Rendering time so that there is no latency time 

# Configuration

### Google Analytics, GTAG

Add the `GoogleAnalytics` component below your Router in `App.razor`.<br/>
The tracker listens to every navigation change while it's rendered on a page.

```
<Router AppAssembly="typeof(App).Assembly" />
<Blazor.Analytics.GoogleAnalytics.GoogleAnalyticsComponent TrackingId="UA-XXXXXXXXX-X" ContainerId="GTM-XXXXXXX" />
```
Remove `ContainerId` attribute if you don't use Google Tag Manager

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.RenderTree;

namespace Blazor.Analytics.GoogleAnalytics
{
    public class GoogleAnalyticsComponent : ComponentBase, IDisposable
    {
        public const string Configure = "GoogleAnalyticsInterop.configure";
        public const string Navigate = "GoogleAnalyticsInterop.navigate";
        public const string GoogleTagManager = "GoogleAnalyticsInterop.googleTagManager";
        public const string GA = "GoogleAnalyticsInterop.callGA";

        /// <summary>
        /// UA-XXXXXXXXX-X
        /// </summary>
        [Parameter]
        protected string TrackingId { get; set; }

        /// <summary>
        /// GTM-XXXXXXX
        /// </summary>
        [Parameter]
        protected string ContainerId { get; set; }

        [Inject]
        protected IUriHelper UriHelper { get; set; }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        protected override async Task OnInitAsync()
        {
            base.OnInit();

            UriHelper.OnLocationChanged += OnLocationChangedAsync;

        }

        //protected override async Task OnAfterRenderAsync()
        //{
        //    await JSRuntime.InvokeAsync<string>(Configure,
        //        TrackingId);

        //    if (ContainerId != null)
        //    {
        //        await JSRuntime.InvokeAsync<string>(GoogleTagManager, ContainerId);
        //    }

        //    base.OnAfterRenderAsync();
        //}

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            base.BuildRenderTree(builder);

            int sequence = 0;

            var googleAnalyticsScript = @"
                   <script>
                    (function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){
                    (i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),
                    m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
                    })(window,document,'script','https://www.google-analytics.com/analytics.js','ga');

                    ga('create', '" + this.TrackingId + @"', 'auto');
                    ga('send', 'pageview');
                    </script>
                ";
            builder.AddMarkupContent(sequence++, googleAnalyticsScript);

            if (string.IsNullOrEmpty(this.ContainerId) == false)
            {

                var headerScript = @"
                    <script>(function(w,d,s,l,i){w[l]=w[l]||[];w[l].push({'gtm.start':
                    new Date().getTime(),event:'gtm.js'});var f=d.getElementsByTagName(s)[0],
                    j=d.createElement(s),dl=l!='dataLayer'?'&l='+l:'';j.async=true;j.src=
                    'https://www.googletagmanager.com/gtm.js?id='+i+dl;f.parentNode.insertBefore(j,f);
                    })(window,document,'script','dataLayer','" + ContainerId + @"');</script>
                ";

                var bodyScript = @"
                    <noscript><iframe src=""https://www.googletagmanager.com/ns.html?id=" + ContainerId + @"""
                    height=""0"" width=""0"" style=""display:none;visibility:hidden""></iframe></noscript>
                ";

                builder.AddMarkupContent(sequence++, headerScript);
                builder.AddMarkupContent(sequence++, bodyScript);
            }

            builder.AddMarkupContent(sequence++, @"<script src=""_content/Blazor-Google-Analytics/interop.js""></script>");
        }

        public void Dispose()
        {
            UriHelper.OnLocationChanged -= OnLocationChangedAsync;
        }

        private async void OnLocationChangedAsync(object sender, LocationChangedEventArgs e)
        {
            var relativeUri = new Uri(e.Location).PathAndQuery;

            await JSRuntime.InvokeAsync<string>(GA, "send", "pageview", relativeUri);
           

            //await JSRuntime.InvokeAsync<string>(Navigate,
            //    TrackingId, relativeUri);

            //if (this.ContainerId != null)
            //{
            //    await JSRuntime.InvokeAsync<string>(GoogleAnalyticsInterop.GoogleTagManager, ContainerId);
            //}
        }

    }
}

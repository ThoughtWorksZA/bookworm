using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using BookWorm.Models.indices;
using Raven.Client.Document;
using Raven.Client.Indexes;
using WebMatrix.WebData;

namespace BookWorm
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        private DocumentStore _store;
        public DocumentStore Store
        {
            get 
            { 
                if (_store == null || _store.WasDisposed)
                {
                    _store = new DocumentStore
                    {
                        ConnectionStringName = "RavenDB"
                    };
                    _store.Initialize();
                }
                return _store;
            }
        }
        protected void Application_Start()
        {
            //WebSecurity.Initialized = true;
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            IndexCreation.CreateIndexes(typeof(Book_AllProperties).Assembly, Store);
        }

        public override void Dispose()
        {
            Store.Dispose();
            base.Dispose();
        }
    }
}
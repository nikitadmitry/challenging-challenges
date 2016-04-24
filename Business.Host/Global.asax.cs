namespace Business.Host
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            DependencyRegistration.ConfigureContainer();
        }
    }
}

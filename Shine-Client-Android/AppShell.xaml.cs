using Shine_Client_Android.Features.Register;
using Shine_Client_Android.Features.ShoppingList;

namespace Shine_Client_Android
{
    public partial class AppShell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("RegisterPage", typeof(RegisterPage));
            Routing.RegisterRoute("ShoppingListPage", typeof(ShoppingListPage));
            Routing.RegisterRoute("ShoppingListEditPage", typeof(ShoppingListEditPage));
        }
    }
}
﻿namespace Shine_Client_Android
{
    public partial class AppShell : Shell
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
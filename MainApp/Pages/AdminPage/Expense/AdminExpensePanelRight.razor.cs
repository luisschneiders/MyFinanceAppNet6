using Microsoft.AspNetCore.Components;

namespace MainApp.Pages.AdminPage.Expense;

public partial class AdminExpensePanelRight : ComponentBase
{
    private string _staticMapImage { get; set; } = string.Empty;
    public AdminExpensePanelRight()
    {
        _staticMapImage = "https://maps.googleapis.com/maps/api/staticmap?markers=color:red|-37.813628,144.963058|-37.862225,144.747939|-37.974917,145.258188&key=AIzaSyAhgTY2LNVdT_Zs0inRSg0mh3eRrOl4qpA&size=400x250&scale=2&map_id=3144ac18e4b7ff64";
    }
}

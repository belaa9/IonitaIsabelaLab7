using IonitaIsabelaLab7.Models;

namespace IonitaIsabelaLab7;

public partial class ListPage : ContentPage
{
    public ListPage()
    {
        InitializeComponent();
    }
    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var slist = (ShopList)BindingContext;
        slist.Date = DateTime.UtcNow;
        await App.Database.SaveShopListAsync(slist);
        await Navigation.PopAsync();
    }
    async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var slist = (ShopList)BindingContext;
        await App.Database.DeleteShopListAsync(slist);
        await Navigation.PopAsync();
    }
    async void OnDeleteItemButtonClicked(object sender, EventArgs e)
    {
        var selectedProduct = listView.SelectedItem as Product;
        if (selectedProduct != null)
        {
            bool confirm = await DisplayAlert("Confirm Delete",
                            $"Are you sure you want to delete {selectedProduct.Description}?",
                            "Yes", "No");
            if (confirm)
            {
                await App.Database.DeleteProductAsync(selectedProduct);
                listView.ItemsSource = await App.Database.GetListProductsAsync(((ShopList)BindingContext).ID);
            }
        }
        else
        {
            await DisplayAlert("Error", "Please select an item to delete.", "OK");
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var shopl = (ShopList)BindingContext;

        listView.ItemsSource = await App.Database.GetListProductsAsync(shopl.ID);
    }
    async void OnChooseButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProductPage((ShopList)
       this.BindingContext)
        {
            BindingContext = new Product()
        });
    }
}
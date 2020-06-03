using Microsoft.AspNetCore.Mvc;
using WebStore.Interfaces.Services;

namespace WebStore.ViewComponents
{
    public class CartViewComponent: ViewComponent
    {
        private readonly ICartService _CartService;

        public CartViewComponent(ICartService CartService) => _CartService = CartService;

        public IViewComponentResult Invoke() => View(_CartService.TransformCartToViewModel());
    }
}

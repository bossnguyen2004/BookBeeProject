using BookBee.Services.AddressService;
using BookBee.Services.AuthorService;
using BookBee.Services.AuthService;
using BookBee.Services.BookService;
using BookBee.Services.CacheService;
using BookBee.Services.CartService;
using BookBee.Services.CategoryService;
using BookBee.Services.FileStorageService;
using BookBee.Services.MailService;
using BookBee.Services.OrderService;
using BookBee.Services.PublisherService;
using BookBee.Services.StatisticalService;
using BookBee.Services.TokenService;
using BookBee.Services.UserService;
using BookBee.Services.VNPayService;
using BookBee.Utilities;


namespace BookBee.Extensions
{
    public static class ServiceExtension
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();
            services.AddScoped<IAuthService, AuthService>();
            services.AddSingleton<ICacheService, InMemoryCacheService>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPublisherService, PublisherService>();
            services.AddScoped<IStatisticalService, StatisticalService>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IVNPayService, VNPayService>();
            //services.AddScoped<IFileStorageService, FileStorageService>();
            services.AddScoped<IMailService, MailService>();
        }

        public static void AddUtilities(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<UserAccessor>();
        }
    }
}

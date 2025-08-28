using Azure.Storage.Files.Shares;
using Azure.Storage.Queues;
using Azure.Data.Tables;
using Azure.Storage.Blobs;
using ABCRetailPOE.Services;

namespace ABCRetailPOE
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            
            builder.Services.AddControllersWithViews();

            builder.Services.AddSingleton(new QueueService(
            new QueueServiceClient(builder.Configuration.GetConnectionString("AzureStorage"))
            ));
            
            var connectionString = builder.Configuration.GetConnectionString("AzureStorage")
                 ;

            //Azure storage clients
            builder.Services.AddSingleton(new TableServiceClient(connectionString));
            builder.Services.AddSingleton(new FileStorageService(new ShareServiceClient(connectionString)));
            builder.Services.AddSingleton(new BlobServiceClient(connectionString));
            builder.Services.AddSingleton(new QueueServiceClient(connectionString));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}

/*References:
 * Mozilla Developer Network (MDN). (2024). CSS Reference. Retrieved from: https://developer.mozilla.org/en-US/docs/Web/CSS/Reference
 * OpenAI. (2024). ChatGPT (version GPT-5). Retrieved from: https://chat.openai.com/
 ChatGPT was used as a supportive AI tool to assist with:
-Providing insights for Azure Storage integration.
-Suggesting styling improvements for Razor Views using Bootstrap and CSS best practices.

*IIEVC School of Computer Science . (n.d.) YouTube playlist: CLDV6212 [playlist online]. YouTube. Available at: https://youtube.com/playlist?list=PL480DYS-b_kcZiyuCyHolh6Nad8J_Xnk7
 (Accessed: 16 August 2025).
*Marcelle Govender (2025) POE PART 1 - GUIDE [Unpublished course material]. VCDN, CLDV6212.

 */
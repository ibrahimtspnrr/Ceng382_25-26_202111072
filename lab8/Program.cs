var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddSession(); // Ekle

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Ekle
app.UseRouting();

app.UseSession(); // Ekle
app.UseAuthorization();

app.MapRazorPages();

app.Run();

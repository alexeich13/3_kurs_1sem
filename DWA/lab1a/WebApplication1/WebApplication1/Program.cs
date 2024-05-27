internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        app.MapGet("/", () => "Get/Handler");

        app.MapGet("/DAI", (HttpContext context) => {

            string parmA = context.Request.Query["ParmA"];
            string parmB = context.Request.Query["ParmB"];
            string responseText = $"GET-Http-DAI:ParmA = {parmA},ParmB = {parmB}";
            context.Response.ContentType = "text/plain";
            return context.Response.WriteAsync(responseText);
        });

        app.MapPost("/DAI", (HttpContext context) => {
            string parmA = context.Request.Query["ParmA"];
            string parmB = context.Request.Query["ParmB"];
            string responseText = $"POST-Http-DAI:ParmA = {parmA},ParmB = {parmB}";
            context.Response.ContentType = "text/plain";
            return context.Response.WriteAsync(responseText);
        });

        app.MapPut("/DAI", (HttpContext context) => {
            string parmA = context.Request.Query["ParmA"];
            string parmB = context.Request.Query["ParmB"];
            string responseText = $"PUT-Http-DAI:ParmA = {parmA},ParmB = {parmB}";
            context.Response.ContentType = "text/plain";
            return context.Response.WriteAsync(responseText);
        });

        app.MapPost("/sum", (HttpContext context) => {
            int x = int.Parse(context.Request.Query["X"]);
            int y = int.Parse(context.Request.Query["Y"]);
            int sum = x + y;
            context.Response.ContentType = "text/plain";
            return context.Response.WriteAsync(sum.ToString());
        });

        app.MapGet("/calculator", (HttpContext context) => {
            string html = "<html><body><form method='post' action='/multiply'>" +
                          "X: <input type='number' name='X'><p>" +
                          "Y: <input type='number' name='Y'><p>" +
                          "<input type='submit' value='submit'></form></body></html>";
            context.Response.ContentType = "text/html";

            return context.Response.WriteAsync(html);
        });

        app.MapPost("/multiply", (HttpContext context) => {
            int x = int.Parse(context.Request.Form["X"]);
            int y = int.Parse(context.Request.Form["Y"]);
            int product = x * y;
            context.Response.ContentType = "text/plain";
            return context.Response.WriteAsync(product.ToString());
        });

        app.Run();
    }
}
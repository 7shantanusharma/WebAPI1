namespace WebAPI1.Helpers
{
    public class CustomMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/custom"))
            {
                await Display(context, "Start");

                await _next(context);

                await Display(context, "End");
            }
            else
            {
                await _next(context);
            }
        }

        public async Task Display(HttpContext context, string text)
        {
            context.Response.ContentType = "text/plain";

            await context.Response.WriteAsync("Hello from custom middleware "+text);
        }
    }
}

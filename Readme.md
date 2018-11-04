# .NET Core Angular Spa

Created using dotnet new angular

Used to analyse the operation of the Spa middleware

Removed "ASPNETCORE_ENVIRONMENT": "Development" entry from Properties/launchSettings.json to allow running in production mode by using env vars.

Commented out app.UseStaticFiles(); as not really relevant to the Spa on its own.

Added some logging to see what's happening in the middleware pipeline

To run in Development mode

```sh
ASPNETCORE_ENVIRONMENT=Development dotnet run
```

To run in Production mode first build the Angular app in prod mode:

```sh
cd ClientApp
npm run build -- --prod
cd ..
ASPNETCORE_ENVIRONMENT=Production dotnet run
```

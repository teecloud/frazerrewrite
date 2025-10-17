# Use the full .NET SDK image (includes runtime and build tools)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the entire solution
COPY . .

# Restore dependencies for all projects
RUN dotnet restore

# Install Entity Framework Core CLI globally
RUN dotnet tool install --global dotnet-ef

# Add .NET global tools to PATH
ENV PATH="$PATH:/root/.dotnet/tools"

# Build the project
RUN dotnet build -c Release --no-restore

# Publish the app to the /out folder
RUN dotnet publish src/Api -c Release -o /out

# --- Runtime image ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy published files from the build stage
COPY --from=build /out .

# Expose the default port
EXPOSE 8080

# Start the application
ENTRYPOINT ["dotnet", "Api.dll"]

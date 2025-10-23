FROM mcr.microsoft.com/playwright/dotnet:v1.48.0-focal

WORKDIR /app

COPY . .

RUN dotnet restore
RUN dotnet build

CMD ["dotnet", "test"]
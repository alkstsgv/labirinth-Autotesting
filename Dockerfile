FROM mcr.microsoft.com/playwright/dotnet:v1.55.0-noble 

WORKDIR /app

# Копируем остальной код
COPY . .

RUN dotnet restore
RUN dotnet build --no-restore

ENTRYPOINT ["dotnet"]
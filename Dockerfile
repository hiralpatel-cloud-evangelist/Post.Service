#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Post.Service/Post.Service.csproj", "Post.Service/"]
COPY ["Post.Service.Base/Post.Service.Base.csproj", "Post.Service.Base/"]
COPY ["Post.Service.Models/Post.Service.Models.csproj", "Post.Service.Models/"]
COPY ["Post.Service.Dto/Post.Service.DTO.csproj", "Post.Service.Dto/"]
COPY ["Post.Service.Services/Post.Service.Services.csproj", "Post.Service.Services/"]
RUN dotnet restore "Post.Service/Post.Service.csproj"
COPY . .
WORKDIR "/src/Post.Service"
RUN dotnet build "Post.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Post.Service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Post.Service.dll"]
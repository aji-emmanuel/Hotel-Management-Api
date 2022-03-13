
#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY *.sln .

COPY ["hotel-booking-api/hotel-booking-api.csproj", "hotel-booking-api/"]
COPY ["hotel-booking-data/hotel-booking-data.csproj", "hotel-booking-data/"]
COPY ["hotel-booking-models/hotel-booking-models.csproj", "hotel-booking-models/"]
COPY ["hotel-booking-core/hotel-booking-core.csproj", "hotel-booking-core/"]
COPY ["hotel-booking-test/hotel-booking-test.csproj", "hotel-booking-test/"]
COPY ["hotel-booking-utilities/hotel-booking-utilities.csproj", "hotel-booking-utilities/"]
COPY ["hotel-booking-dto/hotel-booking-dtos.csproj", "hotel-booking-dto/"]
RUN dotnet restore "hotel-booking-api/hotel-booking-api.csproj"
COPY . .

WORKDIR /src/hotel-booking-api
RUN dotnet build

FROM build AS publish
WORKDIR /src/hotel-booking-api
RUN dotnet publish  -c Release -o /src/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /src/publish .

COPY --from=publish /src/hotel-booking-api/Json/Amenities.json ./
COPY --from=publish /src/hotel-booking-api/Json/bookings.json ./
COPY --from=publish /src/hotel-booking-api/Json/Hotel.json ./
COPY --from=publish /src/hotel-booking-api/Json/users.json ./
COPY --from=publish /src/hotel-booking-api/Json/wishlists.json ./
COPY --from=publish /src/hotel-booking-api/ErrorLoggingCertificate.pfx ./
COPY --from=publish /src/hotel-booking-api/StaticFiles/Html/ConfirmEmail.html ./
COPY --from=publish /src/hotel-booking-api/StaticFiles/Html/ForgotPassword.html ./
COPY --from=publish /src/hotel-booking-api/StaticFiles/Html/ManagerInvite.html ./

#ENTRYPOINT ["dotnet", "hotel-booking-api.dll"]
CMD ASPNETCORE_URLS=http://*:$PORT dotnet hotel-booking-api.dll
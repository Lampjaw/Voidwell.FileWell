FROM microsoft/dotnet:2.1-sdk AS build-env
WORKDIR /app

# Copy and restore as distinct layers
COPY *.sln ./
COPY ./src/Voidwell.FileWell/*.csproj ./src/Voidwell.FileWell/
COPY ./src/Voidwell.Cache/*.csproj ./src/Voidwell.Cache/
COPY ./src/Voidwell.FileWell.Data/*.csproj ./src/Voidwell.FileWell.Data/

RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o /app/out

# Build runtime image
FROM microsoft/dotnet:2.1-aspnetcore-runtime

# Copy the app
WORKDIR /app
COPY --from=build-env /app/out .

ENV ASPNETCORE_URLS http://*:5000
EXPOSE 5000

# Start the app
ENTRYPOINT dotnet Voidwell.FileWell.dll
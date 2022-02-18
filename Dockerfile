FROM mcr.microsoft.com/dotnet/sdk:6.0 as base
WORKDIR /code

COPY . .
RUN dotnet restore && \
    dotnet publish --configuration Release --output /output

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app

COPY --from=base /output .

EXPOSE 80
EXPOSE 443

CMD [ "dotnet", "FileUpload.dll" ]
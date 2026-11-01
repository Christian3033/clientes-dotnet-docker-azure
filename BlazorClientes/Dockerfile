FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY BlazorClientes.csproj ./
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /app/publish

FROM nginx:stable-alpine
RUN apk add --no-cache ca-certificates && update-ca-certificates
ENV API_UPSTREAM=http://api:8080
ENV NGINX_ENVSUBST_FILTER=API_UPSTREAM
COPY nginx.conf.template /etc/nginx/templates/default.conf.template
COPY --from=build /app/publish/wwwroot /usr/share/nginx/html
EXPOSE 8080


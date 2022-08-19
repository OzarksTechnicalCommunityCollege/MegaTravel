FROM mcr.microsoft.com/dotnet/sdk:6.0
WORKDIR /home/app
EXPOSE 80
EXPOSE 443
COPY ./*.sln ./
COPY ./*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p ./${file%.*}/ && mv $file ./${file%.*}/; done
RUN dotnet restore
COPY . .
RUN dotnet publish ./MegaTravelAPI/MegaTravelAPI.csproj -o /publish/
WORKDIR /publish
ENTRYPOINT ["dotnet", "MegaTravelAPI.dll"]
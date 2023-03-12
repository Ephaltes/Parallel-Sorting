FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ParallelSorting.Benchmark/ParallelSorting.Benchmark.csproj", "ParallelSorting.Benchmark/"]
RUN dotnet restore "ParallelSorting.Benchmark/ParallelSorting.Benchmark.csproj"
COPY . .
WORKDIR "/src/ParallelSorting.Benchmark"
RUN dotnet build "ParallelSorting.Benchmark.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ParallelSorting.Benchmark.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ParallelSorting.Benchmark.dll"]

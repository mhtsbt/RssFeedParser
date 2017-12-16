ApiKey=$1
Source=$2

dotnet pack ./RssFeedParser/RssFeedParser.csproj -c Release
dotnet nuget push ./RssFeedParser/bin/Release/RssFeedParser.1.0.3.nupkg -k $ApiKey -s $Source
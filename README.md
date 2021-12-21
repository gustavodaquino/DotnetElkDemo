# Dotnet Elk Demo

Projeto de exemplo com .NET 6.0 logging, usando o Serilog integrado ao ASP.NET, e o sink do Elasticsearch.

> O pool de conexões configurado é o `CloudConnectionPool` para o [Elastic Cloud](https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/connection-pooling.html#cloud-connection-pool).

* .NET 6.0;
* NEST (Elasticsearch SDK) 7.16.0;
* Serilog 2.10.0;
* Serilog Sinks Elasticsearch 8.4.1.

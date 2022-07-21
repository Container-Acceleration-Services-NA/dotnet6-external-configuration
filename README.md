# dotnet6-external-configuration

#BUILD

podman build -t sampleapp:v1-alpha .

# RUN with local config

podman run --rm=true -it -p 7111:7111 sampleapp:v1-alpha

# RUN with mounted config

podman run --rm=true -it -p 7111:7111 -v path/to/appsettings.external.json:/app/appsettings.json:z  sampleapp:v1-alpha

# Access swagger UI to test the API

From a browser access the following URL

http://localhost:7111/swagger/index.html

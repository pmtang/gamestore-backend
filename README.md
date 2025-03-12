local:
dotnet publish -o ./publish 

scp ./publish /var/www/app

remote:
install: 
dotnet-sdk-8.0
aspnetcore-runtime-8.0
nginx

sudo nano /etc/nginx/sites-available/default with setting at the end->
## for new settings, not tried: sudo ln -s /etc/nginx/sites-available/myapi /etc/nginx/sites-enabled/
sudo systemctl reload nginx
sudo nginx -t

sudo ufw allow http
sudo ufw allow https
sudo ufw enable

run:
sudo dotnet app.dll




server {
        listen 80 default_server;
        listen [::]:80 default_server;

        root /var/www/app/;

        # Add index.php to the list if you are using PHP
        index index.html index.htm index.nginx-debian.html;

        server_name _;

        location / {
                proxy_pass http://127.0.0.1:5000/;  # .NET app port
                proxy_http_version 1.1;
                proxy_set_header Upgrade $http_upgrade;
                proxy_set_header Connection keep-alive;
                proxy_set_header Host $host;
                proxy_set_header X-Real-IP $remote_addr;
                proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
                proxy_set_header X-Forwarded-Proto $scheme;
        }
}

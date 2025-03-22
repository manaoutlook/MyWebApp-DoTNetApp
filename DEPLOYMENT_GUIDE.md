# Ubuntu Server Deployment Guide

This guide provides step-by-step instructions for deploying the application on an Ubuntu server.

## Prerequisites

- Ubuntu 20.04 LTS or higher
- SSH access to the server
- Sudo privileges
- Domain name (optional for production)

## Step 1: Server Setup

1. Update system packages:
   ```bash
   sudo apt update && sudo apt upgrade -y
   ```

2. Install required dependencies:
   ```bash
   sudo apt install -y curl nginx
   ```

## Step 2: Install .NET Runtime

1. Add Microsoft package repository:
   ```bash
   wget https://packages.microsoft.com/config/ubuntu/$(lsb_release -rs)/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
   sudo dpkg -i packages-microsoft-prod.deb
   rm packages-microsoft-prod.deb
   ```

2. Install .NET runtime:
   ```bash
   sudo apt update
   sudo apt install -y aspnetcore-runtime-8.0
   ```

3. Verify installation:
   ```bash
   dotnet --list-runtimes
   ```

## Step 3: Deploy Application

1. Create application directory:
   ```bash
   sudo mkdir -p /var/www/mywebapp
   sudo chown $USER:$USER /var/www/mywebapp
   ```

2. Copy application files to server (from your development machine):
   ```bash
   scp -r ./publish/* user@your-server:/var/www/mywebapp
   ```

3. Set correct permissions:
   ```bash
   sudo chown -R www-data:www-data /var/www/mywebapp
   ```

## Step 4: Configure Systemd Service

1. Create service file:
   ```bash
   sudo nano /etc/systemd/system/mywebapp.service
   ```

2. Add the following content:
   ```ini
   [Unit]
   Description=MyWebApp Service
   After=network.target

   [Service]
   WorkingDirectory=/var/www/mywebapp
   ExecStart=/usr/bin/dotnet /var/www/mywebapp/MyWebApp.dll
   Restart=always
   RestartSec=10
   SyslogIdentifier=mywebapp
   User=www-data
   Environment=ASPNETCORE_ENVIRONMENT=Production
   Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false

   [Install]
   WantedBy=multi-user.target
   ```

3. Enable and start the service:
   ```bash
   sudo systemctl enable mywebapp
   sudo systemctl start mywebapp
   ```

## Step 5: Configure Nginx Reverse Proxy

1. Create Nginx configuration:
   ```bash
   sudo nano /etc/nginx/sites-available/mywebapp
   ```

2. Add the following configuration:
   ```nginx
   server {
       listen 80;
       server_name your-domain.com;

       location / {
           proxy_pass         http://localhost:5000;
           proxy_http_version 1.1;
           proxy_set_header   Upgrade $http_upgrade;
           proxy_set_header   Connection keep-alive;
           proxy_set_header   Host $host;
           proxy_cache_bypass $http_upgrade;
           proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;
           proxy_set_header   X-Forwarded-Proto $scheme;
       }
   }
   ```

3. Enable the configuration:
   ```bash
   sudo ln -s /etc/nginx/sites-available/mywebapp /etc/nginx/sites-enabled/
   sudo nginx -t
   sudo systemctl reload nginx
   ```

## Step 6: Configure SSL (Optional but Recommended)

1. Install Certbot:
   ```bash
   sudo apt install certbot python3-certbot-nginx
   ```

2. Obtain SSL certificate:
   ```bash
   sudo certbot --nginx -d your-domain.com
   ```

3. Set up automatic renewal:
   ```bash
   sudo certbot renew --dry-run
   ```

## Step 7: Verify Deployment

1. Check service status:
   ```bash
   sudo systemctl status mywebapp
   ```

2. Check Nginx status:
   ```bash
   sudo systemctl status nginx
   ```

3. Visit your domain or server IP in a web browser to verify the application is running.

## Maintenance

- View application logs:
  ```bash
  sudo journalctl -u mywebapp
  ```

- Restart application:
  ```bash
  sudo systemctl restart mywebapp
  ```

- Update application:
  1. Stop service
  2. Deploy new files
  3. Restart service

# 🚀 **HƯỚNG DẪN DEPLOY LÊN SERVER**

## 📋 **TỔNG QUAN**

Deploy TrainingB.Web lên server để:
- ✅ Access từ mọi nơi (Android, iOS, Desktop)
- ✅ Không cần Windows PC
- ✅ Không cần install app
- ✅ PWA install như native app

---

## 🏗️ **KIẾN TRÚC DEPLOYMENT**

```
[Android Phone]
      ↓ HTTPS
[Internet]
      ↓
[Your Server - VPS]
      ├─→ Nginx (Reverse Proxy + SSL)
      └─→ TrainingB.Web (ASP.NET Core)
            └─→ Chrome/Chromium (Selenium)
```

---

## 💻 **YÊU CẦU SERVER**

### **Tối thiểu:**
- **OS:** Ubuntu 22.04 / Debian 11 / Windows Server
- **RAM:** 2GB (1GB cũng OK)
- **CPU:** 1 vCore
- **Disk:** 10GB
- **Bandwidth:** Unlimited hoặc >100GB/month
- **Cost:** ~$5-10/month

### **Recommended VPS Providers:**
- **DigitalOcean** - $6/month (1GB RAM)
- **Vultr** - $5/month (1GB RAM)
- **Linode** - $5/month (1GB RAM)
- **Hetzner** - €4.5/month (2GB RAM)

---

## 📝 **BƯỚC 1: CHUẨN BỊ SERVER**

### **1.1. Tạo VPS (VD: DigitalOcean)**

1. Đăng ký tài khoản
2. Create Droplet
3. Chọn Ubuntu 22.04
4. Size: $6/month (1GB RAM)
5. Location: Singapore (gần Việt Nam)
6. SSH key hoặc password
7. Create!

### **1.2. SSH vào server**

```bash
ssh root@your-server-ip
```

### **1.3. Install .NET 8.0**

```bash
# Download Microsoft packages
wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb

# Install .NET SDK & Runtime
sudo apt update
sudo apt install -y dotnet-sdk-8.0
sudo apt install -y aspnetcore-runtime-8.0

# Verify
dotnet --version
# Output: 8.0.x
```

### **1.4. Install Chrome/Chromium (cho Selenium)**

```bash
# Install Chromium
sudo apt install -y chromium-browser chromium-chromedriver

# Verify
chromium-browser --version
chromedriver --version
```

### **1.5. Install Nginx**

```bash
sudo apt install -y nginx
sudo systemctl start nginx
sudo systemctl enable nginx

# Test
curl http://localhost
# Should see "Welcome to nginx!"
```

---

## 📦 **BƯỚC 2: BUILD & UPLOAD**

### **2.1. Build trên Windows**

```powershell
# Trên máy Windows của bạn
cd D:\TrainingB\TrainingB.Web

# Build for Linux
dotnet publish -c Release -r linux-x64 --self-contained false -o publish

# Tạo file zip
Compress-Archive -Path publish\* -DestinationPath TrainingB.Web.zip
```

### **2.2. Upload lên server**

```powershell
# Upload qua SCP
scp TrainingB.Web.zip root@your-server-ip:/var/www/
```

### **2.3. Extract trên server**

```bash
# SSH vào server
ssh root@your-server-ip

# Tạo thư mục
sudo mkdir -p /var/www/trainingb
cd /var/www

# Unzip
sudo apt install -y unzip
sudo unzip TrainingB.Web.zip -d trainingb/

# Set permissions
sudo chmod +x /var/www/trainingb/TrainingB.Web
```

---

## ⚙️ **BƯỚC 3: CONFIG SYSTEMD SERVICE**

### **3.1. Tạo service file**

```bash
sudo nano /etc/systemd/system/trainingb.service
```

**Nội dung:**
```ini
[Unit]
Description=TrainingB Web API
After=network.target

[Service]
Type=notify
WorkingDirectory=/var/www/trainingb
ExecStart=/usr/bin/dotnet /var/www/trainingb/TrainingB.Web.dll --urls "http://localhost:5000"
Restart=always
RestartSec=10
SyslogIdentifier=trainingb
User=www-data
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false

[Install]
WantedBy=multi-user.target
```

### **3.2. Enable & Start service**

```bash
# Reload systemd
sudo systemctl daemon-reload

# Enable auto-start
sudo systemctl enable trainingb

# Start service
sudo systemctl start trainingb

# Check status
sudo systemctl status trainingb

# Should see: "Active: active (running)"
```

### **3.3. Test**

```bash
curl http://localhost:5000/api/scraper/status
# Should return JSON
```

---

## 🌐 **BƯỚC 4: SETUP NGINX REVERSE PROXY**

### **4.1. Config Nginx**

```bash
sudo nano /etc/nginx/sites-available/trainingb
```

**Nội dung:**
```nginx
server {
    listen 80;
    server_name your-domain.com;  # Thay bằng domain của bạn

    location / {
        proxy_pass http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }
}
```

### **4.2. Enable site**

```bash
# Link to sites-enabled
sudo ln -s /etc/nginx/sites-available/trainingb /etc/nginx/sites-enabled/

# Test config
sudo nginx -t

# Reload Nginx
sudo systemctl reload nginx
```

---

## 🔒 **BƯỚC 5: SETUP SSL (HTTPS) - FREE**

### **5.1. Install Certbot**

```bash
sudo apt install -y certbot python3-certbot-nginx
```

### **5.2. Get SSL Certificate**

```bash
sudo certbot --nginx -d your-domain.com

# Follow prompts:
# - Enter email
# - Agree to terms
# - Auto redirect HTTP to HTTPS? Yes
```

### **5.3. Test**

```bash
# Test SSL
curl https://your-domain.com/api/scraper/status
```

**Done! SSL được gia hạn tự động!** ✅

---

## 📱 **BƯỚC 6: TEST TRÊN ANDROID**

### **6.1. Mở browser**
- Chrome/Firefox trên Android
- Truy cập: `https://your-domain.com`

### **6.2. Install PWA**
- Click 3 dots → "Add to Home screen"
- Icon xuất hiện trên màn hình
- Mở như native app!

---

## 🔧 **QUẢN LÝ SERVER**

### **Xem logs:**
```bash
# Realtime logs
sudo journalctl -u trainingb -f

# Recent logs
sudo journalctl -u trainingb -n 100
```

### **Restart service:**
```bash
sudo systemctl restart trainingb
```

### **Stop service:**
```bash
sudo systemctl stop trainingb
```

### **Update app:**
```bash
# 1. Build mới trên Windows
# 2. Upload lên server
# 3. Extract
# 4. Restart service
sudo systemctl restart trainingb
```

---

## 💰 **CHI PHÍ**

| Item | Cost/month | Note |
|------|------------|------|
| VPS | $5-10 | DigitalOcean/Vultr |
| Domain | ~$1 | .com/.net |
| SSL | FREE | Let's Encrypt |
| **Total** | **$6-11** | |

---

## 🎯 **CHECKLIST HOÀN THÀNH**

- [ ] Tạo VPS
- [ ] Install .NET 8.0
- [ ] Install Chrome/Chromium
- [ ] Install Nginx
- [ ] Build & Upload app
- [ ] Create systemd service
- [ ] Config Nginx reverse proxy
- [ ] Setup SSL certificate
- [ ] Test trên Android
- [ ] Install PWA

---

## ❓ **TROUBLESHOOTING**

### **Service không start:**
```bash
sudo journalctl -u trainingb -n 50
# Check errors
```

### **Selenium không chạy:**
```bash
# Install dependencies
sudo apt install -y libnss3 libatk-bridge2.0-0 libdrm2 libgbm1
```

### **Port 80/443 bị block:**
```bash
# Check firewall
sudo ufw status
sudo ufw allow 80
sudo ufw allow 443
```

---

**DONE! BẠN ĐÃ DEPLOY THÀNH CÔNG!** 🎉

**Access:** https://your-domain.com  
**From Android:** Install PWA → Dùng như native app!

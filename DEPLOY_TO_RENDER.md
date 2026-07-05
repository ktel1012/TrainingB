# 🚀 HƯỚNG DẪN DEPLOY LÊN RENDER.COM

## ✅ ĐIỀU KIỆN:
- Code đã push lên GitHub ✅
- Repository là PUBLIC ✅

---

## 📋 BƯỚC 5: DEPLOY LÊN RENDER

### **1. Đăng ký Render.com**

1. Mở: https://render.com
2. Click: **"Get Started"** hoặc **"Sign Up"**
3. Chọn: **"Sign up with GitHub"** (khuyên dùng)
4. Authorize Render to access GitHub
5. Done! ✅

---

### **2. Tạo Web Service**

1. **Dashboard:** https://dashboard.render.com
2. Click: **"New +"** (góc trên bên phải)
3. Chọn: **"Web Service"**

---

### **3. Connect Repository**

**Option 1: Nếu thấy repository trong list**
- Click vào repository `TrainingB`

**Option 2: Nếu KHÔNG thấy**
- Click: **"Configure account"** (link ở cuối list)
- Chọn repositories to allow
- Tick: `TrainingB`
- Save
- Quay lại Render → Repository sẽ xuất hiện

---

### **4. Configure Service**

**Name:**
```
trainingb
```
(Hoặc tên khác, không dấu, không space)

**Region:**
```
Singapore
```
(Gần Việt Nam nhất)

**Branch:**
```
main
```

**Runtime:**
```
Docker
```
⚠️ QUAN TRỌNG: Phải chọn **Docker**, không phải Node/Python!

**Instance Type:**
```
Free
```

---

### **5. Environment Variables (Optional)**

Click: **"Advanced"** → **"Add Environment Variable"**

**Không cần thêm gì, mặc định OK!**

_(Nếu muốn custom port, có thể thêm `PORT=8080` nhưng không bắt buộc)_

---

### **6. Deploy!**

1. Scroll xuống cuối
2. Click: **"Create Web Service"**
3. Render sẽ bắt đầu build!

**Timeline:**
- ⏱️ 0-2 phút: Cloning repository
- ⏱️ 2-5 phút: Building Docker image
- ⏱️ 5-7 phút: Starting service
- ✅ ~7 phút: **LIVE!**

---

### **7. Monitor Deployment**

**Bạn sẽ thấy logs real-time:**

```
==> Cloning from https://github.com/your-username/TrainingB...
==> Building...
==> Step 1/12: FROM mcr.microsoft.com/dotnet/sdk:8.0
==> Step 2/12: WORKDIR /src
...
==> Build successful!
==> Starting service...
==> Your service is live 🎉
```

---

### **8. Lấy URL**

**Sau khi deploy xong:**

URL sẽ là:
```
https://trainingb.onrender.com
```

Hoặc:
```
https://trainingb-xxxx.onrender.com
```

**Copy URL này!** 🔗

---

### **9. Test Website**

1. **Mở URL** trong browser
2. **Đợi ~30s** (lần đầu wake up)
3. Sẽ thấy TrainingB UI! 🎉

**Test:**
- Click "Đảo Số" → Should work ✅
- Click scraper buttons → Should work ✅
- Install PWA → Should work ✅

---

## 📱 SỬ DỤNG TRÊN MOBILE

### **Trên Android/iPhone:**

1. Mở browser (Chrome/Safari)
2. Truy cập: `https://trainingb.onrender.com`
3. Click: **"Cài đặt App"**
4. Add to Home Screen
5. **Done!** Icon xuất hiện như app thật! 📱

---

## ⚙️ SETTINGS & MANAGEMENT

### **View Logs:**
1. Vào Dashboard → Your service
2. Tab: **"Logs"**
3. See real-time logs

### **Environment Variables:**
1. Tab: **"Environment"**
2. Add/Edit variables
3. Save → Auto redeploy

### **Manual Deploy:**
1. Tab: **"Manual Deploy"**
2. Click: **"Deploy latest commit"**
3. Or: **"Clear build cache & deploy"**

---

## 🔄 UPDATE CODE (SAU NÀY)

**Khi bạn thay đổi code:**

```powershell
# Trong folder D:\TrainingB
git add .
git commit -m "Updated features"
git push
```

**Render sẽ AUTO DEPLOY!** ✅

**Timeline:**
- Detect push → ~30s
- Build → ~5 phút
- Deploy → ~1 phút
- **LIVE!** 🎉

---

## 💰 GIỚI HẠN FREE TIER

### **Render Free Plan:**

✅ **Miễn phí:**
- 750 giờ/tháng
- 512 MB RAM
- Shared CPU
- HTTPS tự động
- Custom domain

⚠️ **Giới hạn:**
- Sleep sau **15 phút** không dùng
- Wake up mất ~30s
- 1 web service (có thể tạo nhiều nhưng share 750h)

### **Nếu muốn 24/7:**
- Upgrade: **$7/month**
- Không sleep
- 512 MB RAM
- Better performance

---

## 🚨 TROUBLESHOOTING

### **Lỗi: "Build failed"**
→ Check logs, có thể thiếu files
→ Verify `Dockerfile` tồn tại

### **Lỗi: "Service unavailable"**
→ Đợi ~30s (đang wake up)
→ Refresh page

### **Lỗi: "Cannot find repository"**
→ Repository phải **PUBLIC**
→ Hoặc config GitHub permissions

### **Scrapers không chạy:**
→ Có thể do free tier RAM limit
→ Thử upgrade plan

### **Website chậm:**
→ Free tier có CPU limit
→ Bình thường cho free hosting

---

## ✅ CHECKLIST HOÀN THÀNH

- [ ] Code đã push lên GitHub
- [ ] Đăng ký Render.com
- [ ] Connect GitHub repository
- [ ] Chọn Docker runtime
- [ ] Deploy successful
- [ ] Website LIVE
- [ ] Test trên browser
- [ ] Test trên mobile
- [ ] Install PWA

---

## 🎉 XỨC!

**Website của bạn đã LIVE trên internet!**

**URL:** `https://trainingb.onrender.com`

**Share với bạn bè, dùng trên mọi thiết bị!** 🌐📱💻

---

**Nếu gặp vấn đề, báo lỗi cho tôi nhé!** 😊

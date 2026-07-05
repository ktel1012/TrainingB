# ⚡ DEPLOY TRAININGB - QUICK START

**Từ PC của bạn → Website LIVE trên internet trong 15 phút!**

---

## 📋 CHECKLIST

### **✅ Đã hoàn thành:**
- [x] Repository trên GitHub đã tạo

### **⏳ Cần làm tiếp:**
- [ ] Push code lên GitHub (5 phút)
- [ ] Deploy lên Render (5 phút)
- [ ] Test website (2 phút)

---

## 🚀 3 BƯỚC ĐƠN GIẢN

### **BƯỚC 1: PUSH CODE LÊN GITHUB** (5 phút)

**Mở PowerShell trong folder `D:\TrainingB`:**

```powershell
# 1. Initialize Git
git init

# 2. Config user (lần đầu tiên)
git config --global user.name "Your Name"
git config --global user.email "your-email@example.com"

# 3. Add all files
git add .

# 4. Commit
git commit -m "Initial commit"

# 5. Add remote (THAY YOUR_USERNAME!)
git remote add origin https://github.com/YOUR_USERNAME/TrainingB.git

# 6. Rename branch
git branch -M main

# 7. Push!
git push -u origin main
```

**⚠️ Khi hỏi password:**
- Dùng **Personal Access Token** (không phải password GitHub!)
- Tạo token tại: https://github.com/settings/tokens

**✅ Verify:** Vào GitHub repository → Sẽ thấy tất cả files!

---

### **BƯỚC 2: DEPLOY LÊN RENDER** (5 phút)

1. **Đăng ký:** https://render.com
   - Chọn "Sign up with GitHub"

2. **Tạo service:**
   - Click: "New +" → "Web Service"
   - Connect repository: `TrainingB`

3. **Configure:**
   - Name: `trainingb`
   - Region: `Singapore`
   - Branch: `main`
   - Runtime: **Docker** ⚠️ Quan trọng!
   - Instance Type: `Free`

4. **Deploy:**
   - Click: "Create Web Service"
   - Đợi ~7 phút

5. **Done!**
   - URL: `https://trainingb.onrender.com`

---

### **BƯỚC 3: TEST WEBSITE** (2 phút)

1. **Mở URL** trong browser
2. **Đợi ~30s** (lần đầu wake up)
3. **Test features:**
   - ✅ Click scraper buttons
   - ✅ Mở Đảo Số
   - ✅ Install PWA trên mobile

---

## 📱 SỬ DỤNG TRÊN ĐIỆN THOẠI

**Android/iPhone:**
1. Mở browser
2. Vào: `https://trainingb.onrender.com`
3. Click: "Cài đặt App"
4. Add to Home Screen
5. **Dùng như app thật!** 🎉

---

## 🔄 UPDATE CODE SAU NÀY

**Khi muốn thay đổi code:**

```powershell
# Trong folder D:\TrainingB
git add .
git commit -m "Updated features"
git push
```

**→ Render sẽ tự động deploy! Đợi ~5 phút!** ✅

---

## 📚 TÀI LIỆU CHI TIẾT

1. **DEPLOY_TO_GITHUB.md** - Hướng dẫn chi tiết push GitHub
2. **DEPLOY_TO_RENDER.md** - Hướng dẫn chi tiết deploy Render
3. **README.md** - Tổng quan project

---

## 🆘 CẦN GIÚP?

### **Lỗi thường gặp:**

**1. "git is not recognized"**
→ Cài Git: https://git-scm.com/download/win

**2. "Authentication failed"**
→ Dùng Personal Access Token thay vì password
→ Tạo tại: https://github.com/settings/tokens

**3. "Build failed" trên Render**
→ Check chọn đúng "Docker" runtime
→ Verify `Dockerfile` tồn tại trong repo

**4. Website chậm**
→ Free tier có giới hạn
→ Bình thường, đợi ~30s lần đầu

---

## 💰 CHI PHÍ

### **FREE (Đủ dùng):**
- ✅ 750 giờ/tháng
- ✅ HTTPS tự động
- ✅ Custom domain
- ⚠️ Sleep sau 15 phút idle

### **Paid (nếu cần 24/7):**
- $7/month - Không sleep
- Better performance

---

## ✅ SUMMARY

**Input:** Code trong folder `D:\TrainingB`

**Process:**
1. Push → GitHub (5 phút)
2. Deploy → Render (5 phút)
3. Test (2 phút)

**Output:** Website LIVE tại `https://trainingb.onrender.com` 🎉

---

## 🎯 BƯỚC TIẾP THEO

**BÂY GIỜ:**
1. **Mở PowerShell** trong `D:\TrainingB`
2. **Copy-paste** commands từ BƯỚC 1 ở trên
3. **Nhớ thay** `YOUR_USERNAME` bằng GitHub username thật
4. **Run** từng command

**SAU ĐÓ:**
1. Verify code trên GitHub
2. Deploy lên Render
3. Share URL với mọi người!

---

**Sẵn sàng bắt đầu? Hãy chạy BƯỚC 1!** 🚀

**Nếu gặp lỗi, báo cho tôi ngay!** 😊
